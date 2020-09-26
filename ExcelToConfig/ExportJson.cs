using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

class ExportJson : Export
{
    private static string _DateToExportFormatKey = "dateToLuaFormat";
    private static string _TimeToExportFormatKey = "timeToLuaFormat";

    public static void ExportToJson()
    {
        string ExportType = "Json";
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
        batExportSetting.ExportTypeParam =  "ExportJson";
        batExportSetting.ExportPath = "";
        batExportSetting.IsExport = false;
        batExportSetting.IsExportKeepDirectoryStructure = false;
        batExportSetting.ExportExtension = "json";
        batExportSetting.ExportNameBeforeAdd = "";
        batExportSetting.IsExportNullNumber = false;
        batExportSetting.IsExportNullString = false;
        batExportSetting.IsExportNullJson = false;
        batExportSetting.IsExportNullArray = false;
        batExportSetting.IsExportNullDict = false;
        batExportSetting.IsExportNullBool = false;
        batExportSetting.IsExportNullDate = false;
        batExportSetting.IsExportNullTime = false;
        batExportSetting.IsExportNullLang = false;
        batExportSetting.IsExportFormat = false;
        batExportSetting.IsExportFieldComment = false;
        batExportSetting.ExportTopWords = "";
        batExportSetting.ExportIndentationString = "    ";//缩进符
        batExportSetting.ExportSpaceString = " ";//间隔符
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
            excelConfigSetting.IsExportParam = "Export"+ ExportType;// ExportType + "IsExport";
            excelConfigSetting.ExportPathParam = ExportType + "ExportPath";
            excelConfigSetting.IsExportKeepDirectoryStructureParam = ExportType + "IsExportKeepDirectoryStructure";
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
            excelConfigSetting.IsExportJsonArrayFormatParam = ExportType + "IsExportJsonArrayFormat";
            excelConfigSetting.IsExportJsonMapIncludeKeyColumnValueParam = ExportType + "IsExportJsonMapIncludeKeyColumnValue";
            excelConfigSetting.IsArrayFieldNameParam = ExportType + "IsArrayFieldName";
            excelConfigSetting.IsTableNameStartParam = ExportType + "IsTableNameStart";
            excelConfigSetting.IsAddKeyToLuaTableParam = ExportType + "IsAddKeyToLuaTable";
            excelConfigSetting.DateToExportFormatParam = ExportType + "DateToExportFormat";
            excelConfigSetting.TimeToExportFormatParam = ExportType + "TimeToExportFormat";
            excelConfigSetting.SpecialExportParam ="SpecialExport"+ ExportType;

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
            if (AppValues.ConfigData.ContainsKey("SpecialExport"+ExportType))
                excelConfigSetting.TimeToExportFormatParam = AppValues.ConfigData["SpecialExport" + ExportType].Trim();

            excelConfigSetting.GetParamValue(tableInfo);

            Export export = new Export();
            export.GetValue(tableInfo,excelConfigSetting, batExportSetting, batExportPublicSetting);
            string exportName = export.ExportName;


            string m = "";
            if (AppValues.MergeTableList != null && AppValues.MergeTableList.ContainsKey(kvp.Key) && batExportSetting.IsExport == true)
            {
                export.IsExport = true;
                export.ExportName = kvp.Key;
                m = "[合并]";
            }

            if (export.IsExport == true)
            {
                AppLog.Log(string.Format("\n开始{0}导出{1}：", m ,ExportType), ConsoleColor.Green, false);
                AppLog.Log(string.Format("{0}", tableInfo.ExcelNameTips), ConsoleColor.Green);

                if (export.IsExportJsonArrayFormat == true)
                {
                    if (!ExportJsonOneTableArray(tableInfo, export, out errorString))
                    {
                        if (errorString != null)
                            AppLog.LogErrorAndExit(errorString);
                        else
                            AppLog.LogErrorAndExit("导出遇到错误");
                    }
                }
                else
                {
                    if (!ExportOneTable(tableInfo, export, out errorString))
                    {
                        if (errorString != null)
                            AppLog.LogErrorAndExit(errorString);
                        else
                            AppLog.LogErrorAndExit("导出遇到错误");
                    }
                }
            }

            m = "";
            if (AppValues.MergeTableList != null && AppValues.MergeTableList.ContainsKey(kvp.Key))
            {
                tableInfo.TableConfigData = AppValues.MergeTableList[kvp.Key][0].TableConfigData;
                m = "[合并]";
            }
            if (export.SpecialExport != null && export.SpecialExport != "" && batExportSetting.IsExport == true)
            {
                List<string> exportRuleList;
                if (export.SpecialExport.Contains(new string(new char[] { '|' })))
                {
                    string[] exportRuleArr = export.SpecialExport.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    exportRuleList = new List<string>(exportRuleArr);
                }
                else
                {
                    exportRuleList = new List<string>();
                    exportRuleList.Add(export.SpecialExport);
                }
                // 特殊嵌套导出
                foreach (string param in exportRuleList)
                {
                    AppLog.Log(string.Format("\n开始{0}特殊导出{1}：", m, ExportType), ConsoleColor.Green, false);
                    AppLog.Log(string.Format("{0}", tableInfo.ExcelNameTips), ConsoleColor.Green);
                    AppLog.Log(string.Format("规则：\"{0}\"", param), ConsoleColor.Yellow);

                    SpecialExportTableToJson(tableInfo, export, param, out errorString);
                    if (errorString != null)
                        AppLog.LogErrorAndExit(string.Format("{0}导出特殊失败：\n{1}\n",m, errorString));
                }
            }

            string ECP = excelConfigSetting.SpecialExportParam + "Object";
            if (tableInfo.TableConfigData.ContainsKey(ECP))
            {
                if(!string.IsNullOrEmpty(tableInfo.TableConfigData[ECP]))
                {
                    AppLog.Log(string.Format("\n开始特殊导出{0}Object：", ExportType), ConsoleColor.Green, false);
                    SpecialExportTableToJsonObject(tableInfo, export, tableInfo.TableConfigData[ECP], out errorString);
                    if (errorString != null)
                        AppLog.LogErrorAndExit(string.Format("导出特殊失败：\n{0}\n",  errorString));
                }
            }

        }

    }

    // 生成json字符串开头，每行数据以表格主键列为key，各字段信息组成的json object为value，作为整张表json object的元素
    private static bool ExportOneTable(TableInfo tableInfo, Export export, out string errorString)
    {
        try
        {
            errorString = null;
            StringBuilder content = new StringBuilder();
            StringBuilder contentOneRow;

            content.Append("{");

            // 逐行读取表格内容生成json
            List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
            FieldInfo keyColumnInfo = tableInfo.GetKeyColumnFieldInfo();
            if (keyColumnInfo.FieldName.StartsWith(AppValues.AutoFieldNamePrefix))
            {
                AppLog.Log("主键未设置，已忽略导出！", ConsoleColor.Yellow);
                errorString = null;
                return true;
            }
            int dataCount = keyColumnInfo.Data.Count;
            int fieldCount = allField.Count;
            for (int row = 0; row < dataCount; ++row)

            {
                contentOneRow = new StringBuilder();
                // 将主键列的值作为key
                string keyString = null;
                StringBuilder contentkey = new StringBuilder();
                if (keyColumnInfo.DataType == DataType.String)
                {
                    keyString = _GetStringValue(keyColumnInfo, export, row);
                    contentkey.Append(keyString);
                }
                else if (keyColumnInfo.DataType == DataType.Int || keyColumnInfo.DataType == DataType.Long)
                {
                    keyString = _GetNumberValue(keyColumnInfo, export, row);
                    contentkey.Append("\"").Append(keyString).Append("\"");
                }
                else
                {
                    errorString = string.Format("ExportTableToJson函数中未定义{0}类型的主键数值导出至json文件的形式", keyColumnInfo.DataType);
                    AppLog.LogErrorAndExit(errorString);
                    return false;
                }

                StringBuilder contenvalue = new StringBuilder();
                int startColumn = (export.IsExportJsonMapIncludeKeyColumnValue == true ? 0 : 1);
                for (int column = startColumn; column < fieldCount; ++column)
                {
                    string oneFieldString = _GetOneField(allField[column], export, row, out errorString);
                    if (errorString != null)
                    {
                        errorString = string.Format("导出表格{0}为json文件失败，", tableInfo.TableName) + errorString;
                        return false;
                    }
                    else
                    {
                        if (oneFieldString == null)
                            continue;

                        StringBuilder contentTemp = new StringBuilder();
                        // 变量名，注意array下属的子元素在json中不含key的声明
                        if (!(allField[column].ParentField != null && allField[column].ParentField.DataType == DataType.Array))
                        {
                            contentTemp.Append("\"").Append(allField[column].FieldName).Append("\"");
                            contentTemp.Append(":");
                        }
                        contentTemp.Append(oneFieldString);
                        // 一个字段结尾加逗号
                        contentTemp.Append(",");

                        contenvalue.Append(contentTemp.ToString());
                    }
                }
                string str = contenvalue.ToString();
                if (export.IsExportJsonMapIncludeKeyColumnValue == true)
                {
                    // 生成一行数据json object的开头
                    contentOneRow.Append(contentkey);
                    contentOneRow.Append(":{");

                    contentOneRow.Append(contenvalue);

                    // 去掉本行最后一个字段后多余的英文逗号，json语法不像lua那样最后一个字段后的逗号可有可无
                    contentOneRow.Remove(contentOneRow.Length - 1, 1);
                    // 生成一行数据json object的结尾
                    contentOneRow.Append("}");
                    // 每行的json object后加英文逗号
                    contentOneRow.Append(",");
                }
                else if (str != "")
                {
                    // 生成一行数据json object的开头
                    contentOneRow.Append(contentkey);
                    contentOneRow.Append(":{");

                    contentOneRow.Append(contenvalue);

                    // 去掉本行最后一个字段后多余的英文逗号，json语法不像lua那样最后一个字段后的逗号可有可无
                    contentOneRow.Remove(contentOneRow.Length - 1, 1);
                    // 生成一行数据json object的结尾
                    contentOneRow.Append("}");

                    // 每行的json object后加英文逗号
                    if(row < dataCount)
                        contentOneRow.Append(",");
                }
                content.Append(contentOneRow.ToString());
            }

            // 去掉最后一行后多余的英文逗号，此处要特殊处理当表格中没有任何数据行时的情况
            if (content.Length > 1)
                content.Remove(content.Length - 1, 1);
            // 生成json字符串结尾
            content.Append("}");


            string exportString = content.ToString();


            // 如果声明了要整理为带缩进格式的形式
            if (export.IsExportFormat == true)
                exportString = _FormatJson(exportString);

            export.ExportContent = exportString;
            string s = ExcelMethods.GetTableName(tableInfo.ExcelName,"-",ExcelFolder.TheLanguage);
            export.SaveFile(s);
            AppLog.Log(string.Format("成功导出：{0}{1}{2}.{3}", export.ExportNameBeforeAdd, export.ExportName, export.ExportNameAfterLanguageMark, export.ExportExtension));
            errorString = null;
            return true;
        }
        catch (Exception e)
        {
#if DEBUG
            errorString = e.ToString();
            return false;
#endif
            errorString = "遇到错误";
            return false;
        }
    }
    // 若生成为各行数据对应的json object包含在一个json array的形式
    private static bool ExportJsonOneTableArray(TableInfo tableInfo, Export export, out string errorString)
    {
        try
        {
            errorString = null;
            StringBuilder content = new StringBuilder();

            // 生成json字符串开头，每行数据为一个json object，作为整张表json array的元素
            content.Append("[");

            // 逐行读取表格内容生成json
            List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
            FieldInfo keyColumnInfo = tableInfo.GetKeyColumnFieldInfo();
            if (keyColumnInfo.FieldName.StartsWith(AppValues.AutoFieldNamePrefix))
            {
                AppLog.Log("主键未设置，已忽略导出！", ConsoleColor.Yellow);
                errorString = null;
                return true;
            }

            int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
            int fieldCount = allField.Count;
            for (int row = 0; row < dataCount; ++row)
            {
                // 生成一行数据json object的开头
                content.Append("{");

                for (int column = 0; column < fieldCount; ++column)
                {
                    string oneFieldString = _GetOneField(allField[column], export, row, out errorString);
                    if (errorString != null)
                    {
                        errorString = string.Format("导出表格{0}为json文件失败，", tableInfo.TableName) + errorString;
                        return false;
                    }
                    else
                    {
                        if (oneFieldString == null)
                            continue;

                        StringBuilder contentTemp = new StringBuilder();
                        // 变量名，注意array下属的子元素在json中不含key的声明
                        if (!(allField[column].ParentField != null && allField[column].ParentField.DataType == DataType.Array))
                        {
                            contentTemp.Append("\"").Append(allField[column].FieldName).Append("\"");
                            contentTemp.Append(":");
                        }
                        contentTemp.Append(oneFieldString);
                        // 一个字段结尾加逗号
                        contentTemp.Append(",");

                        content.Append(contentTemp.ToString());
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


            string exportString = content.ToString();


            // 如果声明了要整理为带缩进格式的形式
            if (export.IsExportFormat == true)
                exportString = _FormatJson(exportString);

            export.ExportContent = exportString;

            export.SaveFile(tableInfo.ExcelName);
            AppLog.Log(string.Format("成功导出：{0}{1}{2}.{3}", export.ExportNameBeforeAdd, export.ExportName, export.ExportNameAfterLanguageMark, export.ExportExtension));
            errorString = null;
            return true;
        }
        catch (Exception e)
        {
#if DEBUG
            errorString = e.ToString();
#endif
            errorString = "遇到错误";
            return false;
        }
    }
    /// <summary>
    /// 按配置的特殊索引导出方式输出json文件（如果声明了在生成的json文件开头以注释形式展示列信息，将生成更直观的嵌套字段信息，而不同于普通导出规则的列信息展示）
    /// </summary>
    public static bool SpecialExportTableToJson(TableInfo tableInfo, Export export, string exportRule, out string errorString)
    {
        try
        {
            exportRule = exportRule.Trim();
            // 解析按这种方式导出后的json文件名
            int colonIndex = exportRule.IndexOf(':');

            // 解析table value中要输出的字段名
            List<FieldInfo> tableValueField = new List<FieldInfo>();
            // 解析完依次作为索引的字段以及table value中包含的字段后，按索引要求组成相应的嵌套数据结构
            Dictionary<object, object> data = new Dictionary<object, object>();
            //自定义导出规则检查
            TableCheckHelper.CheckSpecialExportRule(tableInfo, exportRule, out tableValueField, out data, out errorString);

            if (errorString != null)
            {
                errorString = string.Format("错误：对表格{0}按\"{1}\"规则进行特殊索引导出时发现以下错误，导出被迫停止，请修正错误后重试：\n{2}\n", tableInfo.TableName, exportRule, errorString);
                return false;
            }

            // 生成导出的文件内容
            StringBuilder content = new StringBuilder();

            // 生成数据内容开头
            content.Append("{");//content.AppendLine("{");

            // 当前缩进量
            int currentLevel = 1;

            // 逐层按嵌套结构输出数据
            _GetIndexFieldData(content, export, data, tableValueField, ref currentLevel, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("错误：对表格{0}按\"{1}\"规则进行特殊索引导出时发现以下错误，导出被迫停止，请修正错误后重试：\n{2}\n", tableInfo.TableName, exportRule, errorString);
                return false;
            }

            // 去掉最后一个子元素后多余的英文逗号
            content.Remove(content.Length - 1, 1);
            // 生成数据内容结尾
            content.AppendLine("}");

            string exportString = content.ToString();

            // 如果声明了要整理为带缩进格式的形式
            if (export.IsExportFormat == true)
            {
                exportString = _FormatJson(exportString);
            }

            // 保存为json文件
            export.ExportName = exportRule.Substring(0, colonIndex).Trim();
            export.ExportContent = exportString;

            export.SaveFile(tableInfo.ExcelName);
            AppLog.Log(string.Format("成功特殊导出：{0}{1}{2}.{3}", export.ExportNameBeforeAdd, export.ExportName, export.ExportNameAfterLanguageMark, export.ExportExtension));
            errorString = null;
            return true;
        }
        catch (ArgumentOutOfRangeException e)
        {
#if DEBUG
            errorString = e.ToString();
#endif
            errorString = "遇到错误";
            return false;
        }
    }
    /// <summary>
    /// 按配置导出特殊的Object类型
    /// </summary>
    public static bool SpecialExportTableToJsonObject(TableInfo tableInfo, Export export, string exportRule, out string errorString)
    {
        try
        {
            exportRule = exportRule.Trim();
            // 解析按这种方式导出后的json文件名
            int colonIndex = exportRule.IndexOf(':');

            // 解析table value中要输出的字段名
            List<FieldInfo> tableValueField = new List<FieldInfo>();
            // 解析完依次作为索引的字段以及table value中包含的字段后，按索引要求组成相应的嵌套数据结构
            Dictionary<object, object> data = new Dictionary<object, object>();
            //自定义导出规则检查
            TableCheckHelper.CheckSpecialExportRuleObject(tableInfo, exportRule, out tableValueField,  out errorString);

            if (errorString != null)
            {
                errorString = string.Format("错误：对表格{0}按\"{1}\"规则进行特殊索引导出时发现以下错误，导出被迫停止，请修正错误后重试：\n{2}\n", tableInfo.TableName, exportRule, errorString);
                return false;
            }

            // 生成导出的文件内容
            StringBuilder content = new StringBuilder();

            // 生成数据内容开头
            content.Append("{");//content.AppendLine("{");

            string oneTableValueFieldData = null;
            int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
            
            foreach (FieldInfo fieldInfo in tableValueField)
            {
                if (fieldInfo.DataType == DataType.Int || fieldInfo.DataType == DataType.Long || fieldInfo.DataType == DataType.Float || fieldInfo.DataType == DataType.String || fieldInfo.DataType == DataType.Json || fieldInfo.DataType == DataType.MapString)
                {
                    content.Append("\"").Append(fieldInfo.FieldName).Append("\"").Append(":[");

                    for (int row = 0; row < dataCount; ++row)
                    {
                        oneTableValueFieldData = _GetOneField(fieldInfo, export, row, out errorString);//
                        if (errorString != null)
                        {
                            errorString = string.Format("第{0}行的字段\"{1}\"（列号：{2}）导出数据错误：{3}", row + ExcelTableSetting.DataFieldDataStartRowIndex + 1, fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), errorString);
                            return false;
                        }
                        else
                        {
                            if ( string.IsNullOrEmpty(oneTableValueFieldData))
                                continue;
                        }

                        if (fieldInfo.DataType == DataType.String)
                        {
                            content.Append(oneTableValueFieldData).Append(",");
                        }
                        else
                        {
                            content.Append(oneTableValueFieldData).Append(",");
                        }
                    }
                }
                else
                {
                    errorString = string.Format("SpecialExportTableToJsonObject中出现非法类型的索引列类型{0}", fieldInfo.DataType);
                    AppLog.LogErrorAndExit(errorString);
                    return false;
                }
                // 去掉最后一个子元素后多余的英文逗号
                content.Remove(content.Length - 1, 1);
                content.Append("],");
            }


            // 去掉最后一个子元素后多余的英文逗号
            content.Remove(content.Length - 1, 1);
            // 生成数据内容结尾
            content.AppendLine("}");

            string exportString = content.ToString();

            // 如果声明了要整理为带缩进格式的形式
            if (export.IsExportFormat == true)
            {
                exportString = _FormatJson(exportString);
            }

            // 保存为json文件
            export.ExportName = exportRule.Substring(0, colonIndex).Trim();
            export.ExportContent = exportString;

            export.SaveFile(tableInfo.ExcelName);
            AppLog.Log(string.Format("成功特殊导出：{0}{1}{2}.{3}", export.ExportNameBeforeAdd, export.ExportName, export.ExportNameAfterLanguageMark, export.ExportExtension));
            errorString = null;
            return true;
        }
        catch (ArgumentOutOfRangeException e)
        {

            errorString = e.ToString();

            errorString = "遇到错误";
            return false;
        }
    }

    /// <summary>
    /// 按指定索引方式导出数据时,通过此函数递归生成层次结构,当递归到最内层时输出指定table value中的数据
    /// </summary>
    private static void _GetIndexFieldData(StringBuilder content, Export export, Dictionary<object, object> parentDict, List<FieldInfo> tableValueField, ref int currentLevel, out string errorString)
    {
        string oneTableValueFieldData = null;
        try
        {
            foreach (var key in parentDict.Keys)
            {
                if (key.GetType() == typeof(int) || key.GetType() == typeof(long) || key.GetType() == typeof(float))
                    content.Append("\"").Append(key).Append("\"");
                else if (key.GetType() == typeof(string))
                {
                    //// 检查作为key值的变量名是否合法
                    //TableCheckHelper.CheckFieldName(key.ToString(), out errorString);
                    //if (errorString != null)
                    //{
                    //    errorString = string.Format("作为第{0}层索引的key值不是合法的变量名，你填写的为\"{1}\"", currentLevel - 1, key.ToString());
                    //    return;
                    //}
                    //content.Append(key);

                    content.Append("\"").Append(key).Append("\"");
                }
                else
                {
                    errorString = string.Format("SpecialExportTableToJson中出现非法类型的索引列类型{0}", key.GetType());
                    AppLog.LogErrorAndExit(errorString);
                    return;
                }

                content.Append(":{");// content.AppendLine(":{");
                ++currentLevel;

                // 如果已是最内层，输出指定table value中的数据
                if (parentDict[key].GetType() == typeof(int))
                {
                    foreach (FieldInfo fieldInfo in tableValueField)
                    {
                        int rowIndex = (int)parentDict[key];
                        oneTableValueFieldData = _GetOneField(fieldInfo, export, rowIndex, out errorString);//
                        if (errorString != null)
                        {
                            errorString = string.Format("第{0}行的字段\"{1}\"（列号：{2}）导出数据错误：{3}", rowIndex + ExcelTableSetting.DataFieldDataStartRowIndex + 1, fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), errorString);
                            return;
                        }
                        else
                        {
                            if (oneTableValueFieldData == null)
                                continue;

                            StringBuilder contentTemp = new StringBuilder();
                            // 变量名，注意array下属的子元素在json中不含key的声明
                            if (!(fieldInfo.ParentField != null && fieldInfo.ParentField.DataType == DataType.Array))
                            {
                                contentTemp.Append("\"").Append(fieldInfo.FieldName).Append("\"");
                                contentTemp.Append(":");
                            }
                            contentTemp.Append(oneTableValueFieldData);
                            // 一个字段结尾加逗号
                            contentTemp.Append(",");
                            oneTableValueFieldData = contentTemp.ToString();

                            if (oneTableValueFieldData == null)
                            {
                                errorString = string.Format("第{0}行的字段\"{1}\"（列号：{2}）导出数据错误：{3}", rowIndex + ExcelTableSetting.DataFieldDataStartRowIndex + 1, fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), "嵌套导出的值不能为空");
                                //AppLog.LogErrorAndExit(errorString);
                                //return;
                                content.Append("\"").Append(fieldInfo.FieldName).Append("\"");
                                content.Append(":");
                                switch (fieldInfo.DataType)
                                {
                                    case DataType.Int:
                                    case DataType.Long:
                                    case DataType.Float:
                                        {
                                            content.Append("0").Append(",");
                                            break;
                                        }
                                    case DataType.String:
                                        {
                                            content.Append("\"\"").Append(",");
                                            break;
                                        }
                                    case DataType.Bool:
                                        {
                                            content.Append("false").Append(",");
                                            break;
                                        }
                                    case DataType.Lang:
                                    case DataType.Date:
                                    case DataType.Time:
                                    default:
                                        {
                                            content.Append("\"\"").Append(",");
                                            break;
                                        }
                                }
                            }
                            else
                                content.Append(oneTableValueFieldData);
                        }
                    }
                    // 去掉最后一个子元素后多余的英文逗号
                    if (content.ToString().EndsWith(","))
                        content.Remove(content.Length - 1, 1);
                }
                // 否则继续递归生成索引key
                else
                {
                    // if (content.ToString().EndsWith("}"))
                    // content.Remove(content.Length - 1, 1).Append(",");

                    _GetIndexFieldData(content, export, (Dictionary<object, object>)(parentDict[key]), tableValueField, ref currentLevel, out errorString);
                    if (errorString != null)
                        return;

                    if (content.ToString().EndsWith(","))
                        content.Remove(content.Length - 1, 1);
                    //content.Append(",");
                }

                --currentLevel;
                // content.Append(_GetJsonIndentation(currentLevel));
                content.Append("},"); // content.AppendLine("},");
                
                //AppLog.Log(content2.Length.ToString());
            }
            errorString = null;
        }
        catch(ArgumentOutOfRangeException e)
        {
            errorString = e.ToString();
        }

       
    }

    private static string _GetOneField(FieldInfo fieldInfo, Export export, int row, out string errorString)
    {
        errorString = null;
        //if (fieldInfo.ParentField == null && fieldInfo.DatabaseFieldName == null)
        //{
        //    return null;
        //}
        // 变量名前的缩进
        // content.Append(_GetJsonIndentation(level));

        // 对应数据值
        string value = null;
        switch (fieldInfo.DataType)
        {
            case DataType.Int:
            case DataType.Long:
            case DataType.Float:
                {
                    value = _GetNumberValue(fieldInfo, export, row);
                    break;
                }
            case DataType.String:
                {
                    value = _GetStringValue(fieldInfo, export, row);
                    break;
                }
            case DataType.Bool:
                {
                    value = _GetBoolValue(fieldInfo, export, row);
                    break;
                }
            case DataType.Lang:
                {
                    value = _GetLangValue(fieldInfo, export, row);
                    break;
                }
            case DataType.Date:
                {
                    value = _GetDateValue(fieldInfo, export, row);
                    break;
                }
            case DataType.Time:
                {
                    value = _GetTimeValue(fieldInfo, export, row);
                    break;
                }
            case DataType.Json:
                {
                    value = _GetJsonValue(fieldInfo, export, row);
                    break;
                }
            case DataType.TableString:
                {
                    value = _GetTableStringValue(fieldInfo, export, row, out errorString);
                    break;
                }
            case DataType.MapString:
                {
                    value = _GetMapStringValue(fieldInfo, export, row);
                    break;
                }
            case DataType.Dict:
                {
                    value = _GetDictValue(fieldInfo, export, row, out errorString);
                    break;
                }
            case DataType.Array:
                {
                    value = _GetArrayValue(fieldInfo, export, row, out errorString);
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

        if (value == null)
            return null;
        else
            return value;

        //StringBuilder content = new StringBuilder();
        //// 变量名，注意array下属的子元素在json中不含key的声明
        //if (!(fieldInfo.ParentField != null && fieldInfo.ParentField.DataType == DataType.Array))
        //{
        //    content.Append("\"").Append(fieldInfo.FieldName).Append("\"");
        //    content.Append(":");
        //}
        //content.Append(value);
        //// 一个字段结尾加逗号
        //content.Append(",");

        //return content.ToString();


        //if (export.IsExportNullJson==true)
        //{
        //    // 变量名，注意array下属的子元素在json中不含key的声明
        //    if (!(fieldInfo.ParentField != null && fieldInfo.ParentField.DataType == DataType.Array))
        //    {
        //        content.Append("\"").Append(fieldInfo.FieldName).Append("\"");
        //        content.Append(":");
        //    }
        //    content.Append(value);
        //    // 一个字段结尾加逗号
        //    content.Append(",");

        //    return content.ToString();
        //}
        //else
        //{
        //    if (value != "null")
        //    {
        //        // 变量名，注意array下属的子元素在json中不含key的声明
        //        if (!(fieldInfo.ParentField != null && fieldInfo.ParentField.DataType == DataType.Array))
        //        {
        //            content.Append("\"").Append(fieldInfo.FieldName).Append("\"");
        //            content.Append(":");
        //        }
        //        content.Append(value);
        //        // 一个字段结尾加逗号
        //        content.Append(",");

        //        return content.ToString();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }

    private static string _GetNumberValue(FieldInfo fieldInfo, Export export, int row)
    {
        if (fieldInfo.Data[row] == null)
        {
            if (export.IsExportNullNumber)
                return "0";
            else
                return null;
        }
        else
            return fieldInfo.Data[row].ToString();
    }

    private static string _GetStringValue(FieldInfo fieldInfo, Export export, int row)
    {
        if (fieldInfo.Data[row] == null)
        {
            if (export.IsExportNullString)
                return "\"\"";
            else
                return null;
        }

        string str = fieldInfo.Data[row].ToString();
        if (str == "")
        {
            if (export.IsExportNullString)
                return "\"\"";
            else
                return null;
        }
        StringBuilder content = new StringBuilder();

        content.Append("\"");
        content.Append(str.Replace("\n", "\\n"));
        content.Append("\"");

        return content.ToString();
    }

    private static string _GetBoolValue(FieldInfo fieldInfo, Export export, int row)
    {
        if (fieldInfo.Data[row] == null)
        {
            if (export.IsExportNullBool)
                return "false";
            else
                return null;
        }
        if ((bool)fieldInfo.Data[row] == true)
            return "true";
        else
            return "false";
    }

    private static string _GetLangValue(FieldInfo fieldInfo, Export export, int row)
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
            if (AppLang.IsLangNull == true)
                content.Append("\"\"");
            else
                content.Append("null");
        }

        return content.ToString();
    }

    private static string _GetDateValue(FieldInfo fieldInfo, Export export, int row)
    {
        StringBuilder content = new StringBuilder();
        string slua = DateTimeTypeKey.toJson.ToString();
        DateFormatType dateFormatType = DateTimeValue.GetDateFormatType(fieldInfo.ExtraParam[DateTimeTypeKey.toJson.ToString()].ToString());
        string exportFormatString = null;
        // 若date型声明toLua的格式为dateTable，则按input格式进行导出
        if (dateFormatType == DateFormatType.DataTable)
        {
            dateFormatType = DateTimeValue.GetDateFormatType(fieldInfo.ExtraParam[DateTimeTypeKey.input.ToString()].ToString());
            exportFormatString = fieldInfo.ExtraParam[DateTimeTypeKey.input.ToString()].ToString();
        }
        else
            exportFormatString = fieldInfo.ExtraParam[DateTimeTypeKey.toJson.ToString()].ToString();

        switch (dateFormatType)
        {
            case DateFormatType.FormatString:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append("\"").Append(((DateTime)(fieldInfo.Data[row])).ToString(exportFormatString)).Append("\"");

                    break;
                }
            case DateFormatType.ReferenceDateSec:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE_LOCAL).TotalSeconds);

                    break;
                }
            case DateFormatType.ReferenceDateMsec:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE_LOCAL).TotalMilliseconds);

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

    private static string _GetTimeValue(FieldInfo fieldInfo, Export export, int row)
    {
        StringBuilder content = new StringBuilder();

        TimeFormatType timeFormatType = DateTimeValue.GetTimeFormatType(fieldInfo.ExtraParam[DateTimeTypeKey.toJson.ToString()].ToString());
        switch (timeFormatType)
        {
            case TimeFormatType.FormatString:
                {
                    if (fieldInfo.Data[row] == null)
                        content.Append("null");
                    else
                        content.Append("\"").Append(((DateTime)(fieldInfo.Data[row])).ToString(fieldInfo.ExtraParam[DateTimeTypeKey.toJson.ToString()].ToString())).Append("\"");

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
                    AppLog.LogErrorAndExit("错误：用_GetTimeValue函数导出json文件的time型的TimeFormatType非法");
                    break;
                }
        }

        return content.ToString();
    }

    private static string _GetJsonValue(FieldInfo fieldInfo, Export export, int row)
    {
        if (fieldInfo.Data[row] == null)
        {
            if (export.IsExportNullJson == true)
                return "null";
            else
                return null;
        }
        else
        {
            return JsonMapper.ToJson(fieldInfo.Data[row]);
        }
    }

    private static string _GetMapStringValue(FieldInfo fieldInfo, Export export, int row)
    {
        if (fieldInfo.Data[row] == null)
            return "null";
        else
            return JsonMapper.ToJson(fieldInfo.Data[row]);
    }

    private static string _GetTableStringValue(FieldInfo fieldInfo, Export export, int row, out string errorString)
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

        // 若tableString的key为#seq，则生成json array，否则生成json object
        if (fieldInfo.TableStringFormatDefine.KeyDefine.KeyType == TableStringKeyType.Seq)
            content.Append("[");
        else
            content.Append("{");

        // 每组数据间用英文分号分隔
        string[] allDataString = inputData.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        // 记录每组数据中的key值（转为字符串后的），不允许出现相同的key（key：每组数据中的key值， value：第几组数据，从0开始记）
        Dictionary<string, int> stringKeys = new Dictionary<string, int>();
        for (int i = 0; i < allDataString.Length; ++i)
        {
            // 根据key的格式定义生成key
            switch (fieldInfo.TableStringFormatDefine.KeyDefine.KeyType)
            {
                case TableStringKeyType.Seq:
                    break;

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
                                        content.AppendFormat("\"{0}\"", value);
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

                        content.Append(":");

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
                        content.Append("{");

                        // 依次输出table中定义的子元素
                        foreach (TableElementDefine elementDefine in fieldInfo.TableStringFormatDefine.ValueDefine.TableValueDefineList)
                        {
                            content.AppendFormat("\"{0}\"", elementDefine.KeyName);
                            content.Append(":");
                            string value = _GetDataInIndexType(elementDefine.DataInIndexDefine, allDataString[i], out errorString);
                            if (errorString == null)
                            {
                                if (elementDefine.DataInIndexDefine.DataType == DataType.String || elementDefine.DataInIndexDefine.DataType == DataType.Lang)
                                    content.AppendFormat("\"{0}\"", value);
                                else
                                    content.Append(value);
                            }
                            content.Append(",");
                        }

                        // 去掉最后一个子元素后多余的英文逗号
                        content.Remove(content.Length - 1, 1);
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

            // 每组数据生成完毕后加逗号
            content.Append(",");
        }

        // 去掉最后一组后多余的英文逗号
        content.Remove(content.Length - 1, 1);
        if (fieldInfo.TableStringFormatDefine.KeyDefine.KeyType == TableStringKeyType.Seq)
            content.Append("]");
        else
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

    private static string _GetDictValue(FieldInfo fieldInfo, Export export, int row, out string errorString)
    {

        errorString = null;
        StringBuilder content = new StringBuilder();

        // 如果该dict数据用-1标为无效，则赋值为null
        if ((bool)fieldInfo.Data[row] == false)
        {
            if (export.IsExportNullDict)
                content.Append("null");
            else
                return null;
        }
        else
        {
            // dict生成json object
            content.Append("{");

            // 逐个对子元素进行生成

            foreach (FieldInfo childField in fieldInfo.ChildField)
            {
                string oneFieldString = _GetOneField(childField, export, row, out errorString);
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
                        contentTemp.Append("\"").Append(childField.FieldName).Append("\"");
                        contentTemp.Append(":");
                    }
                    contentTemp.Append(oneFieldString);
                    // 一个字段结尾加逗号
                    contentTemp.Append(",");

                    content.Append(contentTemp.ToString()) ;
                }
                    
            }

            // 去掉最后一个子元素末尾多余的英文逗号
            if (content.ToString().EndsWith(","))
                content.Remove(content.Length - 1, 1);

            content.Append("}");

            LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(content.ToString());
            if (jsonData.Count == 0)
                AppLog.LogWarning(string.Format("警告：名为{0}类型为{1}的字段，第{2}行的值未设置为-1，但所有值都为空，请确认是否为这样", fieldInfo.FieldName, fieldInfo.DataTypeString, row + ExcelTableSetting.DataFieldDataStartRowIndex + 1), ConsoleColor.Yellow);

        }

        return content.ToString();
    }

    private static string _GetArrayValue(FieldInfo fieldInfo, Export export, int row, out string errorString)
    {
        errorString = null;
        StringBuilder content = new StringBuilder();

        // 如果该array数据用-1标为无效，则赋值为null
        if ((bool)fieldInfo.Data[row] == false)
            if (export.IsExportNullArray)
                content.Append("[]");
            else
                return null;
        else
        {
            // array生成json array
            content.Append("[");

            // 逐个对子元素进行生成
            bool hasValidChild = false;
            foreach (FieldInfo childField in fieldInfo.ChildField)
            {
                string oneFieldString = _GetOneField(childField, export, row, out errorString);
                if (errorString != null)
                    return null;

                // json array中不允许null元素
                if (oneFieldString != null && (!"null".Equals(oneFieldString)))
                {
                    StringBuilder contentTemp = new StringBuilder();
                    // 变量名，注意array下属的子元素在json中不含key的声明
                    if (!(childField.ParentField != null && childField.ParentField.DataType == DataType.Array))
                    {
                        contentTemp.Append("\"").Append(childField.FieldName).Append("\"");
                        contentTemp.Append(":");
                    }
                    contentTemp.Append(oneFieldString);
                    // 一个字段结尾加逗号
                    contentTemp.Append(",");

                    content.Append(contentTemp.ToString());
                    hasValidChild = true;
                }
            }

            // 去掉最后一个子元素末尾多余的英文逗号
            if (hasValidChild == true)
                content.Remove(content.Length - 1, 1);

            content.Append("]");
        }

        return content.ToString();
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
