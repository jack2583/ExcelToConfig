using System;
using System.Collections.Generic;
using System.Text;
using LitJson;


class ExportErlang : Export
{
    public static void ExportToErlang()
    {
        string ExportType = "Erlang";
        if (AppValues.ConfigData.ContainsKey("AllExport" + ExportType))
        {
            if (string.Equals("false", AppValues.ConfigData["AllExport" + ExportType].Trim().ToLower()))
                return;
        }
        else
            return;

        string errorString = null;
        BatExportPublicSetting batExportPublicSetting = new BatExportPublicSetting();
        batExportPublicSetting.IsExport = false;
        batExportPublicSetting.ExcelNameSplitString = "-";
        batExportPublicSetting.ExportNameAfterLanguageMark = "";
        batExportPublicSetting.GetParamValue();

        BatExportSetting batExportSetting = new BatExportSetting();
        batExportSetting.ExportTypeParam = "ExportErlang";
        batExportSetting.ExportPath = "";
        batExportSetting.IsExport = false;
        batExportSetting.IsExportKeepDirectoryStructure = false;
        batExportSetting.ExportExtension = "erl";
        batExportSetting.ExportNameBeforeAdd = "";
        batExportSetting.IsExportNullNumber = true;
        batExportSetting.IsExportNullString = true;
        batExportSetting.IsExportNullJson = false;
        batExportSetting.IsExportNullArray = true;
        batExportSetting.IsExportNullDict = true;
        batExportSetting.IsExportNullBool = false;
        batExportSetting.IsExportNullDate = false;
        batExportSetting.IsExportNullTime = false;
        batExportSetting.IsExportNullLang = false;
        batExportSetting.IsExportFormat = true;
        batExportSetting.IsExportFieldComment = false;
        batExportSetting.ExportTopWords = "%%--- coding:utf-8 ---";
        batExportSetting.ExportIndentationString = "    ";//缩进符
        batExportSetting.ExportSpaceString = " ";//间隔符
        batExportSetting.ExportLineString = "\r\n";//换行符
        batExportSetting.IsExportJsonArrayFormat = false;
        batExportSetting.IsExportJsonMapIncludeKeyColumnValue = false;
        batExportSetting.IsArrayFieldName = false;
        batExportSetting.IsTableNameStart = false;
        batExportSetting.IsAddKeyToLuaTable = false;
        batExportSetting.GetParamValue();


        foreach (KeyValuePair<string, TableInfo> kvp in AppValues.TableInfo)
        {
            TableInfo tableInfo = kvp.Value;
            errorString = null;
            ExcelConfigSetting excelConfigSetting = new ExcelConfigSetting();
            excelConfigSetting.IsExportParam = "Export" + ExportType;// ExportType + "IsExport";
            excelConfigSetting.ExportPathParam = ExportType + "ExportPath";
            excelConfigSetting.IsExportKeepDirectoryStructureParam = ExportType + "ExportPath";
            excelConfigSetting.ExportNameParam = ExportType + "ExportName";
            excelConfigSetting.ExcelNameSplitStringParam = ExportType + "ExcelNameSplitString";
            excelConfigSetting.ExportExtensionParam = ExportType + "ExportExtension";
            excelConfigSetting.ExportNameBeforeAddParam = ExportType + "ExportNameBeforeAdd";
            excelConfigSetting.ExportNameAfterLanguageMarkParam = ExportType + "ExportNameAfterLanguageMark";
            excelConfigSetting.IsExportNullNumberParam = ExportType + "IsExportNullNumber";
            excelConfigSetting.IsExportNullStringParam = ExportType + "IsExportNullString";
            excelConfigSetting.IsExportNullJsonParam = ExportType + "IsExportNullJson";
            excelConfigSetting.IsExportNullArrayParam = ExportType + "IsExportNullArray";
            excelConfigSetting.IsExportNullDictParam = ExportType + "IsExportNullDict";
            excelConfigSetting.IsExportNullBoolParam = ExportType + "IsExportNullBool";
            excelConfigSetting.IsExportNullDateParam = ExportType + "IsExportNullDate";
            excelConfigSetting.IsExportNullTimeParam = ExportType + "IsExportNullTime";
            excelConfigSetting.IsExportNullLangParam = ExportType + "IsExportNullLang";
            excelConfigSetting.IsExportFormatParam = ExportType + "IsExportFormat";
            excelConfigSetting.IsExportFieldCommentParam = ExportType + "ExportFieldComment";
            excelConfigSetting.ExportTopWordsParam = ExportType + "ExportTopWords";
            excelConfigSetting.ExportIndentationStringParam = ExportType + "ExportIndentationString";
            excelConfigSetting.ExportSpaceStringParam = ExportType + "ExportSpaceString";
            excelConfigSetting.ExportLineStringParam = ExportType + "ExportLineString";
            excelConfigSetting.IsExportJsonArrayFormatParam = ExportType + "IsExportJsonArrayFormat";
            excelConfigSetting.IsExportJsonMapIncludeKeyColumnValueParam = ExportType + "IsExportJsonMapIncludeKeyColumnValue";
            excelConfigSetting.IsArrayFieldNameParam = ExportType + "IsArrayFieldName";
            excelConfigSetting.IsTableNameStartParam = ExportType + "IsTableNameStart";
            excelConfigSetting.IsAddKeyToLuaTableParam = ExportType + "IsAddKeyToLuaTable";
            excelConfigSetting.DateToExportFormatParam = ExportType + "DateToExportFormat";
            excelConfigSetting.TimeToExportFormatParam = ExportType + "TimeToExportFormat";

            if (AppValues.ConfigData.ContainsKey("Export" + ExportType))
                excelConfigSetting.IsExportParam = AppValues.ConfigData["Export" + ExportType].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportPath"))
                excelConfigSetting.ExportPathParam = AppValues.ConfigData[ExportType + "ExportPath"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportKeepDirectoryStructure"))
                excelConfigSetting.IsExportKeepDirectoryStructureParam = AppValues.ConfigData[ExportType + "IsExportKeepDirectoryStructure"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportName"))
                excelConfigSetting.ExportNameParam = AppValues.ConfigData[ExportType + "ExportName"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExcelNameSplitString"))
                excelConfigSetting.ExcelNameSplitStringParam = AppValues.ConfigData[ExportType + "ExcelNameSplitString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportExtension"))
                excelConfigSetting.ExportExtensionParam = AppValues.ConfigData[ExportType + "ExportExtension"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportNameBeforeAdd"))
                excelConfigSetting.ExportNameBeforeAddParam = AppValues.ConfigData[ExportType + "ExportNameBeforeAdd"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportNameAfterLanguageMark"))
                excelConfigSetting.ExportNameAfterLanguageMarkParam = AppValues.ConfigData[ExportType + "ExportNameAfterLanguageMark"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullNumber"))
                excelConfigSetting.IsExportNullNumberParam = AppValues.ConfigData[ExportType + "IsExportNullNumber"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullString"))
                excelConfigSetting.IsExportNullStringParam = AppValues.ConfigData[ExportType + "IsExportNullString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullJson"))
                excelConfigSetting.IsExportNullJsonParam = AppValues.ConfigData[ExportType + "IsExportNullJson"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullArray"))
                excelConfigSetting.IsExportNullArrayParam = AppValues.ConfigData[ExportType + "IsExportNullArray"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullDict"))
                excelConfigSetting.IsExportNullDictParam = AppValues.ConfigData[ExportType + "IsExportNullDict"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullBool"))
                excelConfigSetting.IsExportNullBoolParam = AppValues.ConfigData[ExportType + "IsExportNullBool"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullDate"))
                excelConfigSetting.IsExportNullDateParam = AppValues.ConfigData[ExportType + "IsExportNullDate"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullTime"))
                excelConfigSetting.IsExportNullTimeParam = AppValues.ConfigData[ExportType + "IsExportNullTime"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullLang"))
                excelConfigSetting.IsExportNullLangParam = AppValues.ConfigData[ExportType + "IsExportNullLang"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportFormat"))
                excelConfigSetting.IsExportFormatParam = AppValues.ConfigData[ExportType + "IsExportFormat"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportFieldComment"))
                excelConfigSetting.IsExportFieldCommentParam = AppValues.ConfigData[ExportType + "ExportFieldComment"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportTopWords"))
                excelConfigSetting.ExportTopWordsParam = AppValues.ConfigData[ExportType + "ExportTopWords"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportIndentationString"))
                excelConfigSetting.ExportIndentationStringParam = AppValues.ConfigData[ExportType + "ExportIndentationString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportSpaceString"))
                excelConfigSetting.ExportSpaceStringParam = AppValues.ConfigData[ExportType + "ExportSpaceString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportLineString"))
                excelConfigSetting.ExportLineStringParam = AppValues.ConfigData[ExportType + "ExportLineString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportJsonArrayFormat"))
                excelConfigSetting.IsExportJsonArrayFormatParam = AppValues.ConfigData[ExportType + "IsExportJsonArrayFormat"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportJsonMapIncludeKeyColumnValue"))
                excelConfigSetting.IsExportJsonMapIncludeKeyColumnValueParam = AppValues.ConfigData[ExportType + "IsExportJsonMapIncludeKeyColumnValue"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsArrayFieldName"))
                excelConfigSetting.IsArrayFieldNameParam = AppValues.ConfigData[ExportType + "IsArrayFieldName"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsTableNameStart"))
                excelConfigSetting.IsTableNameStartParam = AppValues.ConfigData[ExportType + "IsTableNameStart"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsAddKeyToLuaTable"))
                excelConfigSetting.IsAddKeyToLuaTableParam = AppValues.ConfigData[ExportType + "IsAddKeyToLuaTable"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "DateToExportFormat"))
                excelConfigSetting.DateToExportFormatParam = AppValues.ConfigData[ExportType + "DateToExportFormat"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "TimeToExportFormat"))
                excelConfigSetting.TimeToExportFormatParam = AppValues.ConfigData[ExportType + "TimeToExportFormat"].Trim();

            excelConfigSetting.GetParamValue(tableInfo);

            Export export = new Export();
            export.GetValue(tableInfo,excelConfigSetting, batExportSetting, batExportPublicSetting);
          //  export.GetExportName(excelConfigSetting.ExportName, tableInfo.ExcelName, export.ExcelNameSplitString);

            string m = "";
            if (AppValues.MergeTableList != null && AppValues.MergeTableList.ContainsKey(kvp.Key) && batExportSetting.IsExport == true)
            {
                export.IsExport = true;
                export.ExportName = kvp.Key;
                m = "[合并]";
            }

            if (export.IsExport == false)
                continue;

            AppLog.Log(string.Format("\n开始{0}导出{1}：", m, ExportType), ConsoleColor.Green, false);
            AppLog.Log(string.Format("{0}", tableInfo.ExcelNameTips), ConsoleColor.Green);

            if (!ExportOneTable(tableInfo, export, out errorString))
            {
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.LogErrorAndExit("导出遇到错误");
            }
            
        }

    }
    private static bool ExportOneTable(TableInfo tableInfo, Export export, out string errorString)
    {
        try
        {
            errorString = null;
            StringBuilder content = new StringBuilder();

            // 当前缩进量
            int currentLevel = 1;

            // 逐行读取表格内容生成erlang table
            List<FieldInfo> allField = tableInfo.GetAllFieldInfo();//获取所有字段，第2行没有定义也获取

            // 将主键列作为key生成
            FieldInfo keyColumnField = tableInfo.GetKeyColumnFieldInfo();
            if (keyColumnField.DatabaseFieldName == null)
            {
                AppLog.Log("主键未设置，已忽略导出！", ConsoleColor.Yellow);
                errorString = null;
                return true;
            }

            int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
            for (int row = 0; row < dataCount; ++row)
            {
                if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
                    content.Append("get(").Append(keyColumnField.Data[row]).Append(")->");
                // 注意：像“1_2”这样的字符串作为table的key必须加[""]否则lua认为是语法错误
                else if (keyColumnField.DataType == DataType.String)
                {
                    string FieldString = keyColumnField.Data[row].ToString();
                    content.Append("get(").Append(FieldString.ToLower()).Append(")->");
                }
                else
                {
                    errorString = "用ExportTableToErlang导出不支持的主键列数据类型";
                   // AppLog.LogErrorAndExit(errorString);
                    return false;
                }

                content.Append(export.ExportSpaceString + "#{").Append(export.ExportLineString);

                // 将其他列依次作为value生成
                int i = 0;
                for (int column = 1; column < allField.Count; ++column)
                {
                    string oneFieldString = _GetOneField(allField[column], export, row, currentLevel, out errorString);

                    if (errorString != null)
                    {
                        errorString = string.Format("导出表格{0}失败，", tableInfo.ExcelName) + errorString;
                        return false;
                    }
                    else
                    {
                        if (oneFieldString == null)
                            continue;
                        else
                            i++;

                        if (export.IsExportFormat == true)
                            content.Append(_GetErlangIndentation(currentLevel));


                        if (i > 1)
                            content.Append(",");

                        if (export.IsExportFormat == true)
                            content.Append("'").Append(allField[column].DatabaseFieldName.ToLower()).Append("' => ").Append(oneFieldString).Append(export.ExportLineString);
                        else
                            content.Append("'").Append(allField[column].DatabaseFieldName.ToLower()).Append("' => ").Append(oneFieldString);

                    }
                }
                content.Append("};").Append(export.ExportLineString);
            }

            string exportString2 = content.ToString();

            StringBuilder stringBuilder = new StringBuilder();
            // 生成数据内容开头
            string erlangTableName = tableInfo.TableName;

            stringBuilder.Append(export.ExportTopWords).Append(export.ExportLineString);
            stringBuilder.Append("-module(").Append(export.ExportNameBeforeAdd + export.ExportName).Append(").").Append(export.ExportLineString);
            stringBuilder.Append(@"-export([get/1,get_list/0]).").Append(export.ExportLineString);
            stringBuilder.Append(exportString2);

            // 生成数据内容结尾
            stringBuilder.Append("get(_N) -> false.").Append(export.ExportLineString);
            stringBuilder.Append("get_list() ->").Append(export.ExportLineString);
            stringBuilder.Append("\t[");
            for (int i = 0; i < dataCount; i++)
            {
                string FieldString = allField[0].Data[i].ToString();// _GetOneField(allField[0], i, currentLevel, out errorString);
                stringBuilder.Append(FieldString.ToLower()).Append(",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append("].").Append(export.ExportLineString);

            export.ExportContent = stringBuilder.ToString();

            string s = ExcelMethods.GetTableName(tableInfo.ExcelName, "-", ExcelFolder.TheLanguage);
            export.SaveFile(s);
            AppLog.Log(string.Format("成功导出：{0}{1}{2}.{3}", export.ExportNameBeforeAdd, export.ExportName, export.ExportNameAfterLanguageMark, export.ExportExtension));
            errorString = null;
            return true;
        }
        catch(Exception e)
        {
#if DEBUG
            errorString = e.ToString();
#endif
            errorString = "遇到错误";
            return false;
        }
        
    }
    private static string _GetOneField(FieldInfo fieldInfo, Export export, int row, int level, out string errorString)
    {
        errorString = null;
        if (fieldInfo.ParentField == null && fieldInfo.DatabaseFieldName == null)
        {
            return null;
        }
        // 对应数据值
        string value = null;
        switch (fieldInfo.DataType)
        {
            case DataType.Int:
            case DataType.Long:
                {
                    value = _GetNumberValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.Float:
                {
                    value = _GetFloatValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.String:
                {
                    value = _GetStringValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.Bool:
                {
                    value = _GetBoolValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.Lang:
                {
                    value = _GetLangValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.Date:
                {
                    value = _GetDateValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.Time:
                {
                    value = _GetTimeValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.Json:
                {
                    value = _GetJsonValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.TableString:
                {
                    value = _GetTableStringValue(fieldInfo, export, row, level, out errorString);
                    break;
                }
            case DataType.MapString:
                {
                    value = _GetMapStringValue(fieldInfo, export, row, level);
                    break;
                }
            case DataType.Dict:
                {
                    value = _GetDictValue(fieldInfo, export, row, level, out errorString);
                    break;
                }
            case DataType.Array:
                {
                    value = _GetSetValue(fieldInfo, export, row, level, out errorString);
                    break;
                }
            default:
                {
                    errorString = string.Format("_GetOneField函数中未定义{0}类型数据导出至erlang文件的形式", fieldInfo.DataType);
                    AppLog.LogErrorAndExit(errorString);
                    return null;
                }
        }

        if (errorString != null)
        {
            errorString = string.Format("第{0}行第{1}列的数据存在错误无法导出，", row + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1)) + errorString;
            return null;
        }
        if (value == null)
            return null;
        else
            return value;
    }

    private static string _GetNumberValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullNumber == true)
                return "0";
            else
                return null;
        else
            return fieldInfo.Data[row].ToString();
    }
    private static string _GetFloatValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullNumber == true)
                return "0.0";
            else
                return null;
        else
        {
            string t1 = fieldInfo.Data[row].ToString();
            int ti;
            if (!int.TryParse(t1, out ti))//如果转换失败（为false）时输出括号内容
            {
                return t1;
            }

            else
            {
                return ti + ".0"; //成功时输出数字+.0
            }
        }

    }
    private static string _GetStringValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullString == true)
                return @"<<"""">>";
            else
                return null; 

        StringBuilder content = new StringBuilder();
        if (fieldInfo.Data[row].ToString().Length == 0)
            if (export.IsExportNullString == true)
                return @"<<"""">>"; 
            else
                return null;

        if (fieldInfo.ExportTable == FieldInfo.ExportTableType.ToErlang)
        {
            // 将单元格中填写的英文引号进行转义，使得单元格中填写123"456时，最终生成的lua文件中为xx = "123\"456"
            // 将单元格中手工按下的回车变成"\n"输出到lua文件中，单元格中输入的"\n"等原样导出到lua文件中使其能被lua转义处理。之前做法为Replace("\\", "\\\\")，即将单元格中输入内容均视为普通字符，忽略转义的处理
            string str = fieldInfo.Data[row].ToString();//.Replace("\n", "\\n").Replace("\"", "\\\"");
            content.Append(str);
        }
        else
        {
            string str2 = fieldInfo.Data[row].ToString().Replace("\n", "\\n");
            content.Append("<<\"");
            // 将单元格中填写的英文引号进行转义，使得单元格中填写123"456时，最终生成的lua文件中为xx = "123\"456"
            // 将单元格中手工按下的回车变成"\n"输出到lua文件中，单元格中输入的"\n"等原样导出到lua文件中使其能被lua转义处理。之前做法为Replace("\\", "\\\\")，即将单元格中输入内容均视为普通字符，忽略转义的处理
            string str = fieldInfo.Data[row].ToString().Replace("\n", "\\n").Replace("\\\"", "\"");//.Replace("\n", "\\n").Replace("\"", "\"");

            //int ti;
            //if (!int.TryParse(str, out ti))//如果转换失败（为false）时输出括号内容
            //{
            //    content.Append(str);
            //}
            //else
            //{
            //    content.Append(str + ".0"); ; //成功时输出数字+.0
            //}

            content.Append(str);

            if (str.Length == 0)
                content.Append("\">>");
            else
                content.Append("\"/utf8>>");
        }

        return content.ToString();
    }

    private static string _GetBoolValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullBool == true)
                return "false";
            else
                return null;

        if ((bool)fieldInfo.Data[row] == true)
            return "true";
        else
            return "false";
    }

    private static string _GetLangValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
            if (fieldInfo.Data[row] == null)
                if (export.IsExportNullLang == true)
                    return @"<< """" >>";
                else
                    return null;
        StringBuilder content = new StringBuilder();

        if (fieldInfo.Data[row] != null)
        {
            int intValue;
            string inputData = fieldInfo.Data[row].ToString();
            bool isValid = int.TryParse(inputData, out intValue);
            if (isValid)
                content.Append(intValue);
            else
            {
                content.Append("<<\"");
                content.Append(inputData.Replace("\n", "\\n").Replace("\\\"", "\""));
                content.Append("\"/utf8>>");
            }
        }
        else
        {
            if (AppLang.IsLangNull == true)
                content.Append(@"<< """" >>");
            else
                content.Append("null");
        }

        return content.ToString();
    }

    private static string _GetDateValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        StringBuilder content = new StringBuilder();

        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullDate == true)
                return "{{1970,1,1}, {0,0,0}}";
            else
                return null;
        else
        {
            DateTime dt = (DateTime)(fieldInfo.Data[row]);
            content.Append("{{").Append(dt.Year).Append(",").Append(dt.Month).Append(",").Append(dt.Day).Append("}, {0,0,0}}");
        }
        return content.ToString();
    }

    private static string _GetTimeValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        StringBuilder content = new StringBuilder();
        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullTime == true)
                return "{{1970,1,1}, {0,0,0}}";
            else
                return null;
        else
        {
            DateTime dt = (DateTime)(fieldInfo.Data[row]);
            content.Append("{").Append(dt.Hour).Append(",").Append(dt.Minute).Append(",").Append(dt.Second).Append("}");
        }

        return content.ToString();
    }

    private static string _GetDictValue(FieldInfo fieldInfo, Export export, int row, int level, out string errorString)
    {
        errorString = null;

        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullDict == true)
                return "[]";
            else
                return null;

        StringBuilder content = new StringBuilder();
        // 如果该dict数据用-1标为无效，则赋值为null
        if ((bool)fieldInfo.Data[row] == false)
            if (export.IsExportNullDict == true)
                content.Append("[]");
            else
                return null;
        else
        {
            // dict生成json object
            content.Append("#{");

            // 逐个对子元素进行生成

            foreach (FieldInfo childField in fieldInfo.ChildField)
            {
                string oneFieldString = _GetOneField(childField,export, row, level, out errorString);
                if (errorString != null)
                    return null;
                else
                {
                    if (oneFieldString == null)
                        continue;

                    StringBuilder contentTemp = new StringBuilder();
                    // 变量名，注意array下属的子元素在json中不含key的声明
                    if (!(childField.ParentField != null && childField.ParentField.DataType == DataType.Array))
                    {
                        content.AppendFormat("{0}", childField.DatabaseFieldName);
                        content.Append("=> ");
                    }
                    contentTemp.Append(oneFieldString);
                    // 一个字段结尾加逗号
                    contentTemp.Append(",");

                    content.Append(contentTemp.ToString());
                }
            }

            // 去掉最后一个子元素末尾多余的英文逗号

            content.Remove(content.Length - 1, 1);

            content.Append("}");
        }

        errorString = null;
        return content.ToString();
    }

    private static string _GetSetValue(FieldInfo fieldInfo, Export export, int row, int level, out string errorString)
    {
        errorString = null;
        if (fieldInfo.Data[row] == null)
            if (export.IsExportNullArray == true)
                return "[]";
            else
                return null;

        StringBuilder content = new StringBuilder();

        // 如果该dict或array数据用-1标为无效，则赋值为nil
        if ((bool)fieldInfo.Data[row] == false)
            if (export.IsExportNullArray == true)
                return "[]";
            else
                return null;
        else
        {
            // 包裹dict或array所生成table的左括号
            content.Append("[");
            // ++level;
            // 逐个对子元素进行生成
            foreach (FieldInfo childField in fieldInfo.ChildField)
            {
                string oneFieldString = _GetOneField(childField, export, row, level, out errorString);
                if (errorString != null)
                    return null;
                else
                    content.Append(oneFieldString).Append(", ");
            }
            content.Remove(content.Length - 2, 2);
            // 包裹dict或array所生成table的右括号
            //--level;
            //content.Append(_GetErlangIndentation(level));
            content.Append("]");
        }

        errorString = null;
        return content.ToString();
    }

    private static string _GetJsonValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
        {
            if (export.IsExportNullJson == true)
                return "[]";
            else
            {
                if (fieldInfo.JsonString[row] == "[]")//兼容旧的
                    return "[]";
                else
                return null;
            }
        }

        JsonData jsonData = fieldInfo.Data[row] as JsonData;
        StringBuilder content = new StringBuilder();
        _AnalyzeJsonData(content, export, jsonData, level);
        return content.ToString();
      
    }

    private static void _AnalyzeJsonData(StringBuilder content, Export export, JsonData jsonData, int level)
    {
        if (jsonData == null)
        {
            // 处理键值对中的值为null的情况
            if (export.IsExportNullJson == true)
                content.Append("[]");
            else
                content.Append("");//content.Append("null");
        }
        else if (jsonData.IsObject == true)
        {
            content.Append("#{");

            ++level;

            List<string> childKeyNames = new List<string>(jsonData.Keys);
            int childCount = jsonData.Count;
            for (int i = 0; i < childCount; ++i)
            {
                //// content.Append(_GetErlangIndentation(level));
                //if(ErlangStruct.IsArrayFieldName==true)
                //{
                // 如果键名为数字，需要加方括号和引号
                string keyName = childKeyNames[i];
                double temp;
                if (double.TryParse(keyName, out temp) == true)
                    content.AppendFormat("{0}", keyName);
                else
                    content.AppendFormat("{0}", keyName);

                content.Append("=>");
                //}
                _AnalyzeJsonData(content, export, jsonData[i], level);
                content.Append(",");
            }
            content.Remove(content.Length - 1, 1);
            --level;
            //// content.Append(_GetErlangIndentation(level));
            content.Append("}");
        }
        else if (jsonData.IsArray == true)
        {
            int childCount = jsonData.Count;
            if (childCount == 0)
            {
                if (export.IsExportNullJson == true)
                    content.Append("[]");
                else
                    content.Append("");//content.Append("null");
            }
            else
            {
                content.Append("[");
                ++level;

                for (int i = 0; i < childCount; ++i)
                {
                    //// content.Append(_GetErlangIndentation(level));

                    //if (ErlangStruct.IsArrayFieldName == true)
                    //    content.AppendFormat("[{0}] = ", i + 1);

                    _AnalyzeJsonData(content, export, jsonData[i], level);
                    content.Append(", ");
                }
                content.Remove(content.Length - 2, 2);
                --level;
                //// content.Append(_GetErlangIndentation(level));
                content.Append("]");
            }
        }
        else if (jsonData.IsString == true)
        {
            if (jsonData.ToString() == "")
            {
                content.Append(@"<<"">>");
            }
            else
            {
                //string s = jsonData.ToString();
                //s = s.Replace("\"", "\\\"");
                content.Append("<<\"");
                // 将单元格中填写的英文引号进行转义，使得单元格中填写123"456时，最终生成的lua文件中为xx = "123\"456"
                // 将单元格中手工按下的回车变成"\n"输出到lua文件中，单元格中输入的"\n"等原样导出到lua文件中使其能被lua转义处理
                content.AppendFormat("{0}", jsonData.ToString().Replace("\n", "\\n").Replace("\"", "\""));
                content.Append("\"/utf8>>");
            }
        }
        else if (jsonData.IsBoolean == true)
            content.AppendFormat(jsonData.ToString().ToLower());
        else if (jsonData.IsInt == true || jsonData.IsLong == true || jsonData.IsDouble == true)
            content.AppendFormat(jsonData.ToString());
        else
            AppLog.LogErrorAndExit("用_AnalyzeJsonData解析了未知的JsonData类型");
    }

    private static string _GetMapStringValue(FieldInfo fieldInfo, Export export, int row, int level)
    {
        if (fieldInfo.Data[row] == null)
            return null;

        JsonData jsonData = fieldInfo.Data[row] as JsonData;
        if (jsonData == null)
            return null;
        else
        {
            StringBuilder content = new StringBuilder();
            _AnalyzeJsonData(content, export, jsonData, level);
            return content.ToString();
        }
    }

    private static string _GetTableStringValue(FieldInfo fieldInfo, Export export, int row, int level, out string errorString)
    {
        errorString = null;
        if (fieldInfo.Data[row] == null)
            return null;

        StringBuilder content = new StringBuilder();
        string inputData = fieldInfo.Data[row].ToString();

        // tableString字符串中不允许出现英文引号、斜杠
        if (inputData.Contains("\"") || inputData.Contains("\\") || inputData.Contains("/"))
        {
            errorString = "tableString字符串中不允许出现英文引号、斜杠";
            return null;
        }

        // 包裹tableString所生成table的左括号
        content.Append("{").Append(export.ExportLineString);
        ++level;

        // 每组数据间用英文分号分隔，最终每组数据会生成一个lua table
        string[] allDataString = inputData.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        // 记录每组数据中的key值（转为字符串后的），不允许出现相同的key（key：每组数据中的key值， value：第几组数据，从0开始记）
        Dictionary<string, int> stringKeys = new Dictionary<string, int>();
        for (int i = 0; i < allDataString.Length; ++i)
        {
            content.Append(_GetErlangIndentation(level));

            // 根据key的格式定义生成key
            switch (fieldInfo.TableStringFormatDefine.KeyDefine.KeyType)
            {
                case TableStringKeyType.Seq:
                    {
                        content.AppendFormat("[{0}]", i + 1);
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
                                    content.AppendFormat("[{0}]", value);
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

            content.Append(" = ");

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
                        content.Append("{").Append(export.ExportLineString);
                        ++level;

                        // 依次输出table中定义的子元素
                        foreach (TableElementDefine elementDefine in fieldInfo.TableStringFormatDefine.ValueDefine.TableValueDefineList)
                        {
                            content.Append(_GetErlangIndentation(level));
                            content.Append(elementDefine.KeyName);
                            content.Append(" = ");
                            string value = _GetDataInIndexType(elementDefine.DataInIndexDefine, allDataString[i], out errorString);
                            if (errorString == null)
                            {
                                if (elementDefine.DataInIndexDefine.DataType == DataType.String || elementDefine.DataInIndexDefine.DataType == DataType.Lang)
                                    content.AppendFormat("\"{0}\"", value);
                                else
                                    content.Append(value);
                            }
                            content.Append(",").Append(export.ExportLineString);
                        }
                        --level;
                        content.Append(_GetErlangIndentation(level));
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
            content.Append(",").Append(export.ExportLineString);
        }

        // 包裹tableString所生成table的右括号
        --level;
        content.Append(_GetErlangIndentation(level));
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
                    if (AppLang.LangData.ContainsKey(inputData))
                    {
                        string langValue = AppLang.LangData[inputData];
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

    private static string _GetErlangIndentation(int level)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < level; ++i)
            stringBuilder.Append("    ");// 用于缩进字符串

        return stringBuilder.ToString();
    }

}
