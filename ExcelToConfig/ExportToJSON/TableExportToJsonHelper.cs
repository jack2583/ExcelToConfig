using System;
using System.Collections.Generic;
using System.Text;

public partial class TableExportToJsonHelper
{
    public static bool ExportTableToJson(TableInfo tableInfo, out string errorString)
    {
        StringBuilder content = new StringBuilder();

        // 若生成为各行数据对应的json object包含在一个json array的形式
        if (JsonStruct.ExportJsonIsExportJsonArrayFormat == true)
        {
            // 生成json字符串开头，每行数据为一个json object，作为整张表json array的元素
            content.Append("[");

            // 逐行读取表格内容生成json
            List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
            int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
            int fieldCount = allField.Count;
            for (int row = 0; row < dataCount; ++row)
            {
                // 生成一行数据json object的开头
                content.Append("{");

                for (int column = 0; column < fieldCount; ++column)
                {
                    string oneFieldString = _GetOneField(allField[column], row, out errorString);
                    if (errorString != null)
                    {
                        errorString = string.Format("导出表格{0}为json文件失败，", tableInfo.TableName) + errorString;
                        return false;
                    }
                    else
                    {
                        content.Append(oneFieldString);
                    }
                }

                // 去掉本行最后一个字段后多余的英文逗号，json语法不像lua那样最后一个字段后的逗号可有可无
                content.Remove(content.Length - 1, 1);
                // 生成一行数据json object的结尾
                content.Append("}");
                // 每行的json object后加英文逗号
                content.Append(",");
            }

            // 去掉最后一行后多余的英文逗号，此处要特殊处理当表格中没有任何数据行时的情况
            if (content.Length > 1)
                content.Remove(content.Length - 1, 1);
            // 生成json字符串结尾
            content.Append("]");
        }
        else
        {
            // 生成json字符串开头，每行数据以表格主键列为key，各字段信息组成的json object为value，作为整张表json object的元素
            content.Append("{");

            // 逐行读取表格内容生成json
            List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
            FieldInfo keyColumnInfo = tableInfo.GetKeyColumnFieldInfo();
            int dataCount = keyColumnInfo.Data.Count;
            int fieldCount = allField.Count;
            for (int row = 0; row < dataCount; ++row)
            {
                // 将主键列的值作为key
                string keyString = null;
                StringBuilder contentkey = new StringBuilder();
                if (keyColumnInfo.DataType == DataType.String)
                {
                    keyString = _GetStringValue(keyColumnInfo, row);
                    contentkey.Append(keyString);
                }
                else if (keyColumnInfo.DataType == DataType.Int || keyColumnInfo.DataType == DataType.Long)
                {
                    keyString = _GetNumberValue(keyColumnInfo, row);
                    contentkey.Append("\"").Append(keyString).Append("\"");
                }
                else
                {
                    errorString = string.Format("ExportTableToJson函数中未定义{0}类型的主键数值导出至json文件的形式", keyColumnInfo.DataType);
                    AppLog.LogErrorAndExit(errorString);
                    return false;
                }

                StringBuilder contenvalue = new StringBuilder();
                int startColumn = (JsonStruct.ExportJsonIsExportJsonMapIncludeKeyColumnValue == true ? 0 : 1);
                for (int column = startColumn; column < fieldCount; ++column)
                {
                    string oneFieldString = _GetOneField(allField[column], row, out errorString);
                    if (errorString != null)
                    {
                        errorString = string.Format("额外导出表格{0}为json文件失败，", tableInfo.TableName) + errorString;
                        return false;
                    }
                    else
                    {
                        contenvalue.Append(oneFieldString);
                    }
                }
                string str = contenvalue.ToString();
                if (JsonStruct.ExportJsonIsExportJsonMapIncludeKeyColumnValue == true)
                {
                    // 生成一行数据json object的开头
                    content.Append(contentkey);
                    content.Append(":{");

                    content.Append(contenvalue);

                    // 去掉本行最后一个字段后多余的英文逗号，json语法不像lua那样最后一个字段后的逗号可有可无
                    content.Remove(content.Length - 1, 1);
                    // 生成一行数据json object的结尾
                    content.Append("}");
                    // 每行的json object后加英文逗号
                    content.Append(",");
                }
                else if (str != "")
                {
                    // 生成一行数据json object的开头
                    content.Append(contentkey);
                    content.Append(":{");

                    content.Append(contenvalue);

                    // 去掉本行最后一个字段后多余的英文逗号，json语法不像lua那样最后一个字段后的逗号可有可无
                    content.Remove(content.Length - 1, 1);
                    // 生成一行数据json object的结尾
                    content.Append("}");
                    // 每行的json object后加英文逗号
                    content.Append(",");
                }
            }

            // 去掉最后一行后多余的英文逗号，此处要特殊处理当表格中没有任何数据行时的情况
            if (content.Length > 1)
                content.Remove(content.Length - 1, 1);
            // 生成json字符串结尾
            content.Append("}");
        }

        string exportString = content.ToString();
       

        // 如果声明了要整理为带缩进格式的形式
        if (JsonStruct.ExportJsonIsFormat == true)
            exportString = _FormatJson(exportString);

        // 保存为json文件
        if (SaveJson.SaveJsonFile(tableInfo.ExcelName, ExcelMethods.GetSaveTableName(tableInfo.TableName), exportString) == true)
        {
            errorString = null;
            try
            {
                LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(exportString);
            }
            catch (LitJson.JsonException exception)
            {
                errorString = "错误：导出json出现异常，请检查导出的json及Excel\n";
            }

            return true;
        }
        else
        {
            errorString = "保存为json文件失败\n";
            return false;
        }
    }

    /// <summary>
    /// 将紧凑型的json字符串整理为带缩进和换行的形式，需注意string型值中允许含有括号和\"
    /// </summary>
    private static string _FormatJson(string json)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int level = 0;
        bool isInQuotationMarks = false;
        for (int i = 0; i < json.Length; ++i)
        {
            char c = json[i];

            if (c == '[' || c == '{')
            {
                stringBuilder.Append(c);
                if (isInQuotationMarks == false)
                {
                    stringBuilder.AppendLine();
                    ++level;
                    stringBuilder.Append(_GetJsonIndentation(level));
                }
            }
            else if (c == ']' || c == '}')
            {
                if (isInQuotationMarks == false)
                {
                    stringBuilder.AppendLine();
                    --level;
                    stringBuilder.Append(_GetJsonIndentation(level));
                }
                stringBuilder.Append(c);
            }
            else if (c == ',')
            {
                stringBuilder.Append(c);
                if (isInQuotationMarks == false)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append(_GetJsonIndentation(level));
                }
            }
            else if (c == '"')
            {
                stringBuilder.Append('"');
                if (i > 0 && json[i - 1] != '\\')
                    isInQuotationMarks = !isInQuotationMarks;
            }
            else
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将紧凑型的json字符串整理为带缩进和换行的形式，需注意string型值中允许含有括号和\"
    /// </summary>
    private static string _FormatJson2(string json)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int level = 0;
        bool isInQuotationMarks = false;
        for (int i = 0; i < json.Length; ++i)
        {
            char c = json[i];

            if (c == '[' || c == '{')
            {
                stringBuilder.Append(c);
                if (isInQuotationMarks == false)
                {
                    stringBuilder.AppendLine();
                    ++level;
                    stringBuilder.Append(_GetJsonIndentation(level));
                }
            }
            else if (c == ']' || c == '}')
            {
                if (isInQuotationMarks == false)
                {
                    stringBuilder.AppendLine();
                    --level;
                    stringBuilder.Append(_GetJsonIndentation(level));
                }
                stringBuilder.Append(c);
            }
            else if (c == ',')
            {
                stringBuilder.Append(c);
                if (isInQuotationMarks == false)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append(_GetJsonIndentation(level));
                }
            }
            else if (c == '"')
            {
                stringBuilder.Append('"');
                if (i > 0 && json[i - 1] != '\\')
                    isInQuotationMarks = !isInQuotationMarks;
            }
            else if (c == ':')
            {
                stringBuilder.Append(" : ");
                //if (i > 0 && json[i - 1] != '\\')
                //    isInQuotationMarks = !isInQuotationMarks;
            }
            else
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// json缩进处理
    /// </summary>
    /// <param name="level">缩进数量</param>
    /// <returns></returns>
    private static string _GetJsonIndentation(int level)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < level; ++i)
            stringBuilder.Append("\t"); // 用于缩进json的字符串

        return stringBuilder.ToString();
    }
}