using LitJson;
using System;
using System.Collections.Generic;
using System.Text;

public partial class TableExportToJsonHelper
{
    // 用于缩进Json table的字符串
    private static string _JSON_TABLE_INDENTATION_STRING = "\t";

    // 生成json文件上方字段描述的配置
    // 每行开头的json注释声明
    private static string _COMMENT_OUT_STRING = "-- ";
    // 变量名、数据类型、描述声明之间的间隔字符串
    private static string _DEFINE_INDENTATION_STRING = "   ";
    // dict子元素相对于父dict变量名声明的缩进字符串
    private static string _DICT_CHILD_INDENTATION_STRING = "   ";
    // 变量名声明所占的最少字符数
    private static int _FIELD_NAME_MIN_LENGTH = 30;
    // 数据类型声明所占的最少字符数
    private static int _FIELD_DATA_TYPE_MIN_LENGTH = 30;

    public static bool ExportTableToJson(TableInfo tableInfo, out string errorString)
    {
        StringBuilder content = new StringBuilder();

        // 生成数据内容开头
        content.AppendLine("{");

        // 当前缩进量
        int currentLevel = 1;

        // 判断是否设置要将主键列的值作为导出的table中的元素
        bool isAddKeyToJsonTable = tableInfo.TableConfig != null && tableInfo.TableConfig.ContainsKey(JsonStruct.CONFIG_NAME_ADD_KEY_TO_JSON_TABLE) && tableInfo.TableConfig[JsonStruct.CONFIG_NAME_ADD_KEY_TO_JSON_TABLE].Count > 0 && "true".Equals(tableInfo.TableConfig[JsonStruct.CONFIG_NAME_ADD_KEY_TO_JSON_TABLE][0], StringComparison.CurrentCultureIgnoreCase);

        // 逐行读取表格内容生成json table
        List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
        int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        for (int row = 0; row < dataCount; ++row)
        {
            // 将主键列作为key生成
            content.Append(_GetJsonIndentation(currentLevel));
            FieldInfo keyColumnField = allField[0];
            if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
                content.AppendFormat("\"{0}\":", keyColumnField.Data[row]);
            // 注意：像“1_2”这样的字符串作为table的key必须加[""]否则json认为是语法错误
            else if (keyColumnField.DataType == DataType.String)
                content.AppendFormat("\"{0}\":", keyColumnField.Data[row]);
            else
            {
                errorString = "用ExportTableTojson导出不支持的主键列数据类型";
                AppLog.LogErrorAndExit(errorString);
                return false;
            }

            content.AppendLine("{");
            ++currentLevel;

            // 如果设置了要将主键列的值作为导出的table中的元素
            if (isAddKeyToJsonTable == true)
            {
                content.Append(_GetJsonIndentation(currentLevel));
                content.Append("\"").Append(keyColumnField.FieldName);
                content.Append("\":");
                if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
                    content.Append(keyColumnField.Data[row]);
                else if (keyColumnField.DataType == DataType.String)
                    content.AppendFormat("\"{0}\"", keyColumnField.Data[row]);

                content.AppendLine(",");
            }

            // 将其他列依次作为value生成
            for (int column = 1; column < allField.Count; ++column)
            {
                string oneFieldString = _GetOneField(allField[column], row, currentLevel, out errorString);
                if (errorString != null)
                {
                    errorString = string.Format("导出表格{0}失败，", tableInfo.TableName) + errorString;
                    return false;
                }
                else
                    content.Append(oneFieldString);
            }

            // 一行数据生成完毕后添加右括号结尾等
            --currentLevel;
            content.Append(_GetJsonIndentation(currentLevel));
            content.AppendLine("},");
        }

        // 生成数据内容结尾
        content.AppendLine("}");

        string exportString = content.ToString();
        //if (JsonStruct.IsNeedColumnInfo == true)
        //    exportString = _GetColumnInfo(tableInfo) + exportString;

        // 保存为json文件
        if (SaveJson.SaveJsonFile(tableInfo.TableName, tableInfo.TableName, exportString) == true)
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = "保存为json文件失败\n";
            return false;
        }
    }


    /// <summary>
    /// 生成要在json文件最上方以注释形式展示的列信息
    /// </summary>
    private static string _GetColumnInfo(TableInfo tableInfo)
    {
        // 变量名前的缩进量
        int level = 0;

        StringBuilder content = new StringBuilder();
        foreach (FieldInfo fieldInfo in tableInfo.GetAllClientFieldInfo())
            content.Append(_GetOneFieldColumnInfo(fieldInfo, level));

        content.Append(System.Environment.NewLine);
        return content.ToString();
    }

    private static string _GetOneFieldColumnInfo(FieldInfo fieldInfo, int level)
    {
        StringBuilder content = new StringBuilder();
        content.AppendFormat("{0}{1, -" + _FIELD_NAME_MIN_LENGTH + "}{2}{3, -" + _FIELD_DATA_TYPE_MIN_LENGTH + "}{4}{5}\n", _COMMENT_OUT_STRING, _GetFieldNameIndentation(level) + fieldInfo.FieldName, _DEFINE_INDENTATION_STRING, fieldInfo.DataTypeString, _DEFINE_INDENTATION_STRING, fieldInfo.Desc);
        if (fieldInfo.DataType == DataType.Dict || fieldInfo.DataType == DataType.Array)
        {
            ++level;
            foreach (FieldInfo childFieldInfo in fieldInfo.ChildField)
                content.Append(_GetOneFieldColumnInfo(childFieldInfo, level));

            --level;
        }

        return content.ToString();
    }

    /// <summary>
    /// 生成columnInfo变量名前的缩进字符串
    /// </summary>
    private static string _GetFieldNameIndentation(int level)
    {
        StringBuilder indentationStringBuilder = new StringBuilder();
        for (int i = 0; i < level; ++i)
            indentationStringBuilder.Append(_DICT_CHILD_INDENTATION_STRING);

        return indentationStringBuilder.ToString();
    }

    private static string _GetOneField(FieldInfo fieldInfo, int row, int level, out string errorString)
    {
        StringBuilder content = new StringBuilder();
        errorString = null;

        // 变量名前的缩进
        content.Append(_GetJsonIndentation(level));
        // 变量名
        if (!(fieldInfo.ParentField != null && fieldInfo.ParentField.DataType == DataType.Array))
        {
            content.Append("\"").Append(fieldInfo.FieldName).Append("\"");
            content.Append(":");
        }
        // 对应数据值
        string value = null;
        switch (fieldInfo.DataType)
        {
            case DataType.Int:
            case DataType.Long:
            case DataType.Float:
                {
                    value = _GetNumberValue(fieldInfo, row, level);
                    break;
                }
            case DataType.String:
                {
                    value = _GetStringValue(fieldInfo, row, level);
                    break;
                }
            case DataType.Bool:
                {
                    value = _GetBoolValue(fieldInfo, row, level);
                    break;
                }
            case DataType.Lang:
                {
                    value = _GetLangValue(fieldInfo, row, level);
                    break;
                }
            case DataType.Date:
                {
                    value = _GetDateValue(fieldInfo, row, level);
                    break;
                }
            case DataType.Time:
                {
                    value = _GetTimeValue(fieldInfo, row, level);
                    break;
                }
            case DataType.Json:
                {
                    value = _GetJsonValue(fieldInfo, row, level);
                    break;
                }
            case DataType.TableString:
                {
                    value = _GetTableStringValue(fieldInfo, row, level, out errorString);
                    break;
                }
            case DataType.MapString:
                {
                    value = _GetMapStringValue(fieldInfo, row, level);
                    break;
                }
            case DataType.Dict:
            case DataType.Array:
                {
                    value = _GetSetValue(fieldInfo, row, level, out errorString);
                    break;
                }
            default:
                {
                    errorString = string.Format("_GetOneField函数中未定义{0}类型数据导出至json文件的形式", fieldInfo.DataType);
                    AppLog.LogErrorAndExit(errorString);
                    return null;
                }
        }

        if (errorString != null)
        {
            errorString = string.Format("第{0}行第{1}列的数据存在错误无法导出，", row + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1)) + errorString;
            return null;
        }

        content.Append(value);
        // 一个字段结尾加逗号并换行
        content.AppendLine(",");

        return content.ToString();
    }

    private static string _GetNumberValue(FieldInfo fieldInfo, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
            return "null";
        else
            return fieldInfo.Data[row].ToString();
    }

    private static string _GetStringValue(FieldInfo fieldInfo, int row, int level)
    {
        StringBuilder content = new StringBuilder();

        content.Append("\"");
        // 将单元格中填写的英文引号进行转义，使得单元格中填写123"456时，最终生成的json文件中为xx = "123\"456"
        // 将单元格中手工按下的回车变成"\n"输出到json文件中，单元格中输入的"\n"等原样导出到json文件中使其能被json转义处理。之前做法为Replace("\\", "\\\\")，即将单元格中输入内容均视为普通字符，忽略转义的处理
        content.Append(fieldInfo.Data[row].ToString().Replace("\n", "\\n").Replace("\"", "\\\""));
        content.Append("\"");

        return content.ToString();
    }

    private static string _GetBoolValue(FieldInfo fieldInfo, int row, int level)
    {
        if ((bool)fieldInfo.Data[row] == true)
            return "true";
        else
            return "false";
    }

    private static string _GetLangValue(FieldInfo fieldInfo, int row, int level)
    {
        StringBuilder content = new StringBuilder();

        if (fieldInfo.Data[row] != null)
        {
            content.Append("\"");
            content.Append(fieldInfo.Data[row].ToString().Replace("\n", "\\n").Replace("\"", "\\\""));
            content.Append("\"");
        }
        else
        {
            if (LuaStruct.IsPrintEmptyStringWhenLangNotMatching == true)
                content.Append("\"\"");
            else
                content.Append("null");
        }

        return content.ToString();
    }

    private static string _GetDateValue(FieldInfo fieldInfo, int row, int level)
    {
        StringBuilder content = new StringBuilder();

        DateFormatType dateFormatType = TableAnalyzeHelper.GetDateFormatType(fieldInfo.ExtraParam[DateTimeValue.TABLE_INFO_EXTRA_PARAM_KEY_DATE_TO_LUA_FORMAT].ToString());
        switch (dateFormatType)
        {
            case DateFormatType.FormatString:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append("\"").Append(((DateTime)(fieldInfo.Data[row])).ToString(fieldInfo.ExtraParam[DateTimeValue.TABLE_INFO_EXTRA_PARAM_KEY_DATE_TO_LUA_FORMAT].ToString())).Append("\"");

                    break;
                }
            case DateFormatType.ReferenceDateSec:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE).TotalSeconds);

                    break;
                }
            case DateFormatType.ReferenceDateMsec:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE).TotalMilliseconds);

                    break;
                }
            case DateFormatType.DataTable:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                    {
                        double totalSeconds = ((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE).TotalSeconds;
                        content.Append("os.date(\"!*t\", ").Append(totalSeconds).Append(")");
                    }

                    break;
                }
            default:
                {
                    AppLog.LogErrorAndExit("错误：用_GetDateValue函数导出json文件的date型的DateFormatType非法");
                    break;
                }
        }

        return content.ToString();
    }

    private static string _GetTimeValue(FieldInfo fieldInfo, int row, int level)
    {
        StringBuilder content = new StringBuilder();

        TimeFormatType timeFormatType = TableAnalyzeHelper.GetTimeFormatType(fieldInfo.ExtraParam[DateTimeValue.TABLE_INFO_EXTRA_PARAM_KEY_TIME_TO_LUA_FORMAT].ToString());
        switch (timeFormatType)
        {
            case TimeFormatType.FormatString:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append("\"").Append(((DateTime)(fieldInfo.Data[row])).ToString(fieldInfo.ExtraParam[DateTimeValue.TABLE_INFO_EXTRA_PARAM_KEY_TIME_TO_LUA_FORMAT].ToString())).Append("\"");

                    break;
                }
            case TimeFormatType.ReferenceTimeSec:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE).TotalSeconds);

                    break;
                }
            default:
                {
                    AppLog.LogErrorAndExit("错误：用_GetTimeValue函数导出lua文件的time型的TimeFormatType非法");
                    break;
                }
        }

        return content.ToString();
    }

    private static string _GetSetValue(FieldInfo fieldInfo, int row, int level, out string errorString)
    {
        StringBuilder content = new StringBuilder();

        // 如果该dict或array数据用-1标为无效，则赋值为null
        if ((bool)fieldInfo.Data[row] == false)
            content.Append("null");
        else
        {
            // 包裹dict或array所生成table的左括号
            content.AppendLine("{");
            ++level;
            // 逐个对子元素进行生成
            foreach (FieldInfo childField in fieldInfo.ChildField)
            {
                string oneFieldString = _GetOneField(childField, row, level, out errorString);
                if (errorString != null)
                    return null;
                else
                    content.Append(oneFieldString);
            }
            // 包裹dict或array所生成table的右括号
            --level;
            content.Append(_GetJsonIndentation(level));
            content.Append("}");
        }

        errorString = null;
        return content.ToString();
    }

    private static string _GetJsonValue(FieldInfo fieldInfo, int row, int level)
    {
        JsonData jsonData = fieldInfo.Data[row] as JsonData;
        if (jsonData == null)
            return "null";
        else
        {
            StringBuilder content = new StringBuilder();
            _AnalyzeJsonData(content, jsonData, level);
            return content.ToString();
        }
    }

    private static void _AnalyzeJsonData(StringBuilder content, JsonData jsonData, int level)
    {
        if (jsonData == null)
        {
            // 处理键值对中的值为null的情况
            content.Append("null");
        }
        else if (jsonData.IsObject == true)
        {
            content.AppendLine("{");
            ++level;

            List<string> childKeyNames = new List<string>(jsonData.Keys);
            int childCount = jsonData.Count;
            for (int i = 0; i < childCount; ++i)
            {
                content.Append(_GetJsonIndentation(level));
                // 如果键名为数字，需要加方括号和引号
                string keyName = childKeyNames[i];
                double temp;
                if (double.TryParse(keyName, out temp) == true)
                    content.AppendFormat("\"{0}\"", keyName);
                else
                    content.Append(childKeyNames[i]);

                content.Append(":");
                _AnalyzeJsonData(content, jsonData[i], level);
                content.AppendLine(",");
            }

            --level;
            content.Append(_GetJsonIndentation(level));
            content.Append("}");
        }
        else if (jsonData.IsArray == true)
        {
            content.AppendLine("[");
            ++level;

            int childCount = jsonData.Count;
            for (int i = 0; i < childCount; ++i)
            {
                content.Append(_GetJsonIndentation(level));
                content.AppendFormat("\"{0}\":", i + 1);
                _AnalyzeJsonData(content, jsonData[i], level);
                content.AppendLine(",");
            }

            --level;
            content.Append(_GetJsonIndentation(level));
            content.Append("]");
        }
        else if (jsonData.IsString == true)
        {
            // 将单元格中填写的英文引号进行转义，使得单元格中填写123"456时，最终生成的json文件中为xx = "123\"456"
            // 将单元格中手工按下的回车变成"\n"输出到json文件中，单元格中输入的"\n"等原样导出到json文件中使其能被json转义处理
            content.AppendFormat("\"{0}\"", jsonData.ToString().Replace("\n", "\\n").Replace("\"", "\\\""));
        }
        else if (jsonData.IsBoolean == true)
            content.AppendFormat(jsonData.ToString().ToLower());
        else if (jsonData.IsInt == true || jsonData.IsLong == true || jsonData.IsDouble == true)
            content.AppendFormat(jsonData.ToString());
        else
            AppLog.LogErrorAndExit("用_AnalyzeJsonData解析了未知的JsonData类型");
    }

    private static string _GetMapStringValue(FieldInfo fieldInfo, int row, int level)
    {
        JsonData jsonData = fieldInfo.Data[row] as JsonData;
        if (jsonData == null)
            return "null";
        else
        {
            StringBuilder content = new StringBuilder();
            _AnalyzeJsonData(content, jsonData, level);
            return content.ToString();
        }
    }

    private static string _GetTableStringValue(FieldInfo fieldInfo, int row, int level, out string errorString)
    {
        errorString = null;
        if (fieldInfo.Data[row] == null)
            return "null";

        StringBuilder content = new StringBuilder();
        string inputData = fieldInfo.Data[row].ToString();

        // tableString字符串中不允许出现英文引号、斜杠
        if (inputData.Contains("\"") || inputData.Contains("\\") || inputData.Contains("/"))
        {
            errorString = "tableString字符串中不允许出现英文引号、斜杠";
            return null;
        }

        // 包裹tableString所生成table的左括号
        content.AppendLine("{");
        ++level;

        // 每组数据间用英文分号分隔，最终每组数据会生成一个json table
        string[] allDataString = inputData.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        // 记录每组数据中的key值（转为字符串后的），不允许出现相同的key（key：每组数据中的key值， value：第几组数据，从0开始记）
        Dictionary<string, int> stringKeys = new Dictionary<string, int>();
        for (int i = 0; i < allDataString.Length; ++i)
        {
            content.Append(_GetJsonIndentation(level));

            // 根据key的格式定义生成key
            switch (fieldInfo.TableStringFormatDefine.KeyDefine.KeyType)
            {
                case TableStringKeyType.Seq:
                    {
                        content.AppendFormat("\"{0}\"", i + 1);
                        break;
                    }
                case TableStringKeyType.DataInIndex:
                    {
                        string value = _GetDataInIndexType(fieldInfo.TableStringFormatDefine.KeyDefine.DataInIndexDefine, allDataString[i], out errorString);
                        if (errorString == null)
                        {
                            if (fieldInfo.TableStringFormatDefine.KeyDefine.DataInIndexDefine.DataType == DataType.Int || fieldInfo.TableStringFormatDefine.KeyDefine.DataInIndexDefine.DataType == DataType.Long)
                            {
                                // 检查key是否在该组数据中重复
                                if (stringKeys.ContainsKey(value))
                                    errorString = string.Format("第{0}组数据与第{1}组数据均为相同的key（{2}）", stringKeys[value] + 1, i + 1, value);
                                else
                                {
                                    stringKeys.Add(value, i);
                                    content.AppendFormat("\"{0}\"", value);
                                }
                            }
                            else if (fieldInfo.TableStringFormatDefine.KeyDefine.DataInIndexDefine.DataType == DataType.String)
                            {
                                // string型的key不允许为空或纯空格且必须符合变量名的规范
                                value = value.Trim();
                                if (TableCheckHelper.CheckFieldName(value, out errorString))
                                {
                                    // 检查key是否在该组数据中重复
                                    if (stringKeys.ContainsKey(value))
                                        errorString = string.Format("第{0}组数据与第{1}组数据均为相同的key（{2}）", stringKeys[value] + 1, i + 1, value);
                                    else
                                    {
                                        stringKeys.Add(value, i);
                                        content.Append(value);
                                    }
                                }
                                else
                                    errorString = "string型的key不符合变量名定义规范，" + errorString;
                            }
                            else
                            {
                                AppLog.LogErrorAndExit("错误：用_GetTableStringValue函数导出非int、long或string型的key值");
                                return null;
                            }
                        }

                        break;
                    }
                default:
                    {
                        AppLog.LogErrorAndExit("错误：用_GetTableStringValue函数导出未知类型的key");
                        return null;
                    }
            }
            if (errorString != null)
            {
                errorString = string.Format("tableString中第{0}组数据（{1}）的key数据存在错误，", i + 1, allDataString[i]) + errorString;
                return null;
            }

            content.Append(":");

            // 根据value的格式定义生成value
            switch (fieldInfo.TableStringFormatDefine.ValueDefine.ValueType)
            {
                case TableStringValueType.True:
                    {
                        content.Append("true");
                        break;
                    }
                case TableStringValueType.DataInIndex:
                    {
                        string value = _GetDataInIndexType(fieldInfo.TableStringFormatDefine.ValueDefine.DataInIndexDefine, allDataString[i], out errorString);
                        if (errorString == null)
                        {
                            DataType dataType = fieldInfo.TableStringFormatDefine.ValueDefine.DataInIndexDefine.DataType;
                            if (dataType == DataType.String || dataType == DataType.Lang)
                                content.AppendFormat("\"{0}\"", value);
                            else
                                content.Append(value);
                        }

                        break;
                    }
                case TableStringValueType.Table:
                    {
                        content.AppendLine("{");
                        ++level;

                        // 依次输出table中定义的子元素
                        foreach (TableElementDefine elementDefine in fieldInfo.TableStringFormatDefine.ValueDefine.TableValueDefineList)
                        {
                            content.Append(_GetJsonIndentation(level));
                            content.Append(elementDefine.KeyName);
                            content.Append(":");
                            string value = _GetDataInIndexType(elementDefine.DataInIndexDefine, allDataString[i], out errorString);
                            if (errorString == null)
                            {
                                if (elementDefine.DataInIndexDefine.DataType == DataType.String || elementDefine.DataInIndexDefine.DataType == DataType.Lang)
                                    content.AppendFormat("\"{0}\"", value);
                                else
                                    content.Append(value);
                            }
                            content.AppendLine(",");
                        }
                        --level;
                        content.Append(_GetJsonIndentation(level));
                        content.Append("}");

                        break;
                    }
                default:
                    {
                        AppLog.LogErrorAndExit("错误：用_GetTableStringValue函数导出未知类型的value");
                        return null;
                    }
            }
            if (errorString != null)
            {
                errorString = string.Format("tableString中第{0}组数据（{1}）的value数据存在错误，", i + 1, allDataString[i]) + errorString;
                return null;
            }

            // 每组数据生成完毕后加逗号并换行
            content.AppendLine(",");
        }

        // 包裹tableString所生成table的右括号
        --level;
        content.Append(_GetJsonIndentation(level));
        content.Append("}");

        return content.ToString();
    }

    /// <summary>
    /// 将形如#1(int)的数据定义解析为要输出的字符串
    /// </summary>
    private static string _GetDataInIndexType(DataInIndexDefine define, string oneDataString, out string errorString)
    {
        // 一组数据中的子元素用英文逗号分隔
        string[] allElementString = oneDataString.Trim().Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        // 检查是否存在指定序号的数据
        if (allElementString.Length < define.DataIndex)
        {
            errorString = string.Format("解析#{0}({1})类型的数据错误，输入的数据中只有{2}个子元素", define.DataIndex, define.DataType.ToString(), allElementString.Length);
            return null;
        }
        // 检查是否为指定类型的合法数据
        string inputString = allElementString[define.DataIndex - 1];
        if (define.DataType != DataType.String)
            inputString = inputString.Trim();

        string value = _GetDataStringInTableString(inputString, define.DataType, out errorString);
        if (errorString != null)
        {
            errorString = string.Format("解析#{0}({1})类型的数据错误，", define.DataIndex, define.DataType.ToString()) + errorString;
            return null;
        }
        else
            return value;
    }

    /// <summary>
    /// 将tableString类型数据字符串中的某个所填数据转为需要输出的字符串
    /// </summary>
    private static string _GetDataStringInTableString(string inputData, DataType dataType, out string errorString)
    {
        string result = null;
        errorString = null;

        switch (dataType)
        {
            case DataType.Bool:
                {
                    if ("1".Equals(inputData) || "true".Equals(inputData, StringComparison.CurrentCultureIgnoreCase))
                        result = "true";
                    else if ("0".Equals(inputData) || "false".Equals(inputData, StringComparison.CurrentCultureIgnoreCase))
                        result = "false";
                    else
                        errorString = string.Format("输入的\"{0}\"不是合法的bool值，正确填写bool值方式为填1或true代表真，0或false代表假", inputData);

                    break;
                }
            case DataType.Int:
            case DataType.Long:
                {
                    long longValue;
                    bool isValid = long.TryParse(inputData, out longValue);
                    if (isValid)
                        result = longValue.ToString();
                    else
                        errorString = string.Format("输入的\"{0}\"不是合法的{1}类型的值", inputData, dataType);

                    break;
                }
            case DataType.Float:
                {
                    float floatValue;
                    bool isValid = float.TryParse(inputData, out floatValue);
                    if (isValid)
                        result = floatValue.ToString();
                    else
                        errorString = string.Format("输入的\"{0}\"不是合法的float类型的值", inputData);

                    break;
                }
            case DataType.String:
                {
                    result = inputData;
                    break;
                }
            case DataType.Lang:
                {
                    if (AppValues.LangData.ContainsKey(inputData))
                    {
                        string langValue = AppValues.LangData[inputData];
                        if (langValue.Contains("\"") || langValue.Contains("\\") || langValue.Contains("/") || langValue.Contains(",") || langValue.Contains(";"))
                            errorString = string.Format("tableString中的lang型数据中不允许出现英文引号、斜杠、逗号、分号，你输入的key（{0}）对应在lang文件中的值为\"{1}\"", inputData, langValue);
                        else
                            result = langValue;
                    }
                    else
                        errorString = string.Format("输入的lang型数据的key（{0}）在lang文件中找不到对应的value", inputData);

                    break;
                }
            default:
                {
                    AppLog.LogErrorAndExit(string.Format("错误：用_GetDataInTableString函数解析了tableString中不支持的数据类型{0}", dataType));
                    break;
                }
        }

        return result;
    }

    private static string _GetJsonIndentation(int level)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < level; ++i)
            stringBuilder.Append(_JSON_TABLE_INDENTATION_STRING);

        return stringBuilder.ToString();
    }
}
