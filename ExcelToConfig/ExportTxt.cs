﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

class ExportTxt : Export
{
    public static void ExportToTxt()
    {
        string errorString = null;
        BatExportPublicSetting batExportPublicSetting = new BatExportPublicSetting();
        batExportPublicSetting.IsExport = true;
        batExportPublicSetting.ExcelNameSplitString = "-";
        batExportPublicSetting.ExportNameAfterLanguageMark = "";

        BatExportSetting batExportSetting = new BatExportSetting();
        batExportSetting.ExportTypeParam = "ExportTxt";
        batExportSetting.ExportPath = ExcelFolder.ExcelPath;// FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ExportTxt");
        batExportSetting.IsExport = true;
        batExportSetting.IsExportKeepDirectoryStructure = true;
        batExportSetting.ExportExtension = "txt";
        batExportSetting.ExportNameBeforeAdd = "";
        batExportPublicSetting.ExportNameAfterLanguageMark= "-data";
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
        batExportSetting.ExportTopWords = "";
        batExportSetting.ExportIndentationString = "";//缩进符 
        batExportSetting.ExportSpaceString = "\t";//间隔符
        batExportSetting.ExportLineString = "\n";//换行符
        batExportSetting.IsExportJsonArrayFormat = false;
        batExportSetting.IsExportJsonMapIncludeKeyColumnValue = false;
        batExportSetting.IsArrayFieldName = false;
        batExportSetting.IsTableNameStart = false;
        batExportSetting.IsAddKeyToLuaTable = false;
        batExportSetting.GetParamValue();
        
        string ExportType = "Txt";
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
            excelConfigSetting.IsExportJsonArrayFormatParam = ExportType + "IsExportJsonArrayFormat";
            excelConfigSetting.IsExportJsonMapIncludeKeyColumnValueParam = ExportType + "IsExportJsonMapIncludeKeyColumnValue";
            excelConfigSetting.IsArrayFieldNameParam = ExportType + "IsArrayFieldName";
            excelConfigSetting.IsTableNameStartParam = ExportType + "IsTableNameStart";
            excelConfigSetting.IsAddKeyToLuaTableParam = ExportType + "IsAddKeyToLuaTable";
            excelConfigSetting.DateToExportFormatParam = ExportType + "DateToExportFormat";
            excelConfigSetting.TimeToExportFormatParam = ExportType + "TimeToExportFormat";
            excelConfigSetting.GetParamValue(tableInfo);

            Export export = new Export();
            export.GetValue(tableInfo, excelConfigSetting, batExportSetting, batExportPublicSetting);

            if (export.IsExport == false)
                continue;

            if (AppValues.MergeTableList !=null && AppValues.MergeTableList.ContainsKey(tableInfo.TableName))
            {

                AppLog.Log(string.Format("\n开始导出{0}：", ExportType), ConsoleColor.Green, false);
                AppLog.Log(string.Format("{0}", tableInfo.TableName), ConsoleColor.Green);

                ExportOneMergeTable(tableInfo, export, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                
            }
            else
            {
                AppLog.Log(string.Format("\n开始导出{0}：", ExportType), ConsoleColor.Green, false);
                AppLog.Log(string.Format("{0}", tableInfo.ExcelNameTips), ConsoleColor.Green);

                ExportOneTable(tableInfo, export, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
            }
        }

    }
    private static bool ExportOneTable(TableInfo tableInfo, Export export, out string errorString)
    {
        try
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (DataTable dt in AppValues.ExcelDataSet[tableInfo.ExcelFilePath].Tables)
            {
                if (dt.TableName == "config$")
                    continue;

                StringBuilder content = new StringBuilder();
                for (int row = 0; row < dt.Rows.Count; ++row)
                {
                    List<string> strList = new List<string>();
                    string str = null;
                    for (int column = 0; column < dt.Columns.Count; ++column)
                    {
                        str = str + "{" + column + "}" + export.ExportSpaceString;
                        strList.Add(dt.Rows[row][column].ToString());
                    }

                    string[] str2 = strList.ToArray();
                    content.Append(string.Format(str, str2)).Append(export.ExportLineString);
                }
                string exportString = content.ToString();

                if (exportString.Length < 2)
                    continue;


                export.ExportContent = content.ToString();

                // 保存为txt文件
                if (export.ExportPath == null || export.ExportPath == "")
                    export.ExportPath = Path.GetDirectoryName(tableInfo.ExcelFilePath);

                string sheetName = dt.TableName;
                if (sheetName.StartsWith("'"))
                    sheetName = sheetName.Substring(1);
                if (sheetName.EndsWith("'"))
                    sheetName = sheetName.Substring(0, sheetName.Length - 1);
                if (sheetName.EndsWith("$"))
                    sheetName = sheetName.Substring(0, sheetName.Length - 1);

                //string s = ExcelMethods.GetTableName(tableInfo.ExcelName, "-", ExcelFolder.TheLanguage);
                //export.SaveFile(s, tableInfo.ExcelName, sheetName);

                string fileName2 = string.Concat(tableInfo.ExcelName + "-" + sheetName, ".", export.ExportExtension);
                string savePath = FileModule.CombinePath(Path.GetDirectoryName(tableInfo.ExcelFilePath), fileName2);
                StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
                writer.Write(export.ExportContent);
                writer.Flush();
                writer.Close();
                AppLog.Log(string.Format("成功导出：{0}{1}{2}.{3}", tableInfo.ExcelName, "-", sheetName, export.ExportExtension));
                errorString = null;
                return true;

            }
            if (stringBuilder == null)
            {
                errorString = null;
                return true;
            }
            else
            {
                errorString = stringBuilder.ToString();
                if (errorString.Length > 0)
                {
                    return false;
                }
                else
                {
                    errorString = null;
                    return true;
                }
            }
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
    private static bool ExportOneMergeTable(TableInfo tableInfo, Export export, out string errorString)
    {
        try
        {
            errorString = null;

        // 存储每一行数据生成的txt文件内容
        List<StringBuilder> rowContentList = new List<StringBuilder>();

        // 生成主键列的同时，对每行的StringBuilder进行初始化，主键列只能为int、long或string型，且值不允许为空，直接转为字符串即可
        FieldInfo keyColumnFieldInfo = tableInfo.GetKeyColumnFieldInfo();
        int rowCount = keyColumnFieldInfo.Data.Count;
        for (int row = 0; row < rowCount; ++row)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(keyColumnFieldInfo.Data[row]);
            rowContentList.Add(stringBuilder);
        }
        // 生成其他列的内容（将array、dict这样的集合类型下属字段作为独立字段处理）
        List<FieldInfo> allFieldInfoIgnoreSetDataStructure = tableInfo.GetAllClientFieldInfoIgnoreSetDataStructure();
        for (int i = 1; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
            _GetOneFieldTxtContent(allFieldInfoIgnoreSetDataStructure[i], export, rowContentList);

        // 要在txt中显示 导出至数据库名字及类型
        StringBuilder tempStringBuilder = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder.Append(export.ExportSpaceString);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder.Append(fieldInfo.DatabaseInfoString);
        }
        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder.Remove(0, 1));

        // 要在txt中显示 声明字段检查字符串
        StringBuilder tempStringBuilder2 = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder2.Append(export.ExportSpaceString);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder2.Append(fieldInfo.CheckRule);
        }

        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder2.Remove(0, 1));

        // 要在txt中显示 Lua等客户端数据类型
        StringBuilder tempStringBuilder3 = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder3.Append(export.ExportSpaceString);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder3.Append(fieldInfo.DataType);
        }

        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder3.Remove(0, 1));


        StringBuilder tempStringBuilder4 = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder4.Append(export.ExportSpaceString);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            // 如果是array下属的子元素，字段名生成格式为“array字段名[从1开始的下标序号]”。dict下属的子元素，生成格式为“dict字段名.下属字段名”
            if (fieldInfo.ParentField != null)
            {
                String fieldName = fieldInfo.FieldName;
                FieldInfo tempField = fieldInfo;
                while (tempField.ParentField != null)
                {
                    if (tempField.ParentField.DataType == DataType.Array)
                        fieldName = string.Concat(tempField.ParentField.FieldName, fieldName);
                    else if (tempField.ParentField.DataType == DataType.Dict)
                        fieldName = string.Concat(tempField.ParentField.FieldName, ".", fieldName);

                    tempField = tempField.ParentField;
                }

                tempStringBuilder4.Append(fieldName);
            }
            else
                tempStringBuilder4.Append(fieldInfo.FieldName);
        }

        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder4.Remove(0, 1));


        // 要在txt中显示 中文字段名
        StringBuilder tempStringBuilder5 = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder5.Append(export.ExportSpaceString);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder5.Append(fieldInfo.Desc.Replace("\n", "\\n"));
        }

        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder5.Remove(0, 1));

        StringBuilder content = new StringBuilder();
        foreach (StringBuilder stringBuilder in rowContentList)
            content.AppendLine(stringBuilder.ToString());


        //if (export.ExportPath == null || export.ExportPath == "")
        //    export.ExportPath = Path.GetDirectoryName(tableInfo.ExcelFilePath);


        //    string s = AppValues.MergeTableList[tableInfo.TableName][0].TableName;
        //    string exportDirectoryPath = FileModule.GetExportDirectoryPath(s, export.ExportPath, export.IsExportKeepDirectoryStructure, false);
        //    //如果文件夹不存在就创建
        //    if (Directory.Exists(exportDirectoryPath) == false)
        //        Directory.CreateDirectory(exportDirectoryPath);


            string fileName2 = string.Concat(tableInfo.TableName + ExcelFolder.TheLanguage, ".", export.ExportExtension);
            string savePath = FileModule.CombinePath(Path.GetDirectoryName(tableInfo.ExcelFilePath), fileName2);
            StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
            writer.Write(content);
            writer.Flush();
            writer.Close();

            AppLog.Log(string.Format("成功导出合并TXT：{0}", fileName2));
            errorString = null;
            return true;
        }
        catch
        {
            errorString = string.Format("[合并]表{0}的{1}保存为txt文件失败\n", tableInfo.ExcelName, tableInfo.TableConfigData2);
            return false;
        }

    }
    private static void _GetOneFieldTxtContent(FieldInfo fieldInfo, Export export, List<StringBuilder> rowContentList)
    {
        int rowCount = fieldInfo.Data.Count;

        switch (fieldInfo.DataType)
        {
            case DataType.Int:
            case DataType.Long:
            case DataType.Float:
            case DataType.String:
                {
                    for (int row = 0; row < rowCount; ++row)
                    {
                        StringBuilder stringBuilder = rowContentList[row];
                        // 先增加与上一字段间的分隔符
                        stringBuilder.Append(export.ExportSpaceString);
                        // 再生成本行对应的内容
                        if (fieldInfo.Data[row] != null)

                            stringBuilder.Append(fieldInfo.Data[row].ToString().Replace("\n", "\\n"));
                    }
                    break;
                }
            case DataType.Lang:
            case DataType.TableString:
                {
                    for (int row = 0; row < rowCount; ++row)
                    {
                        StringBuilder stringBuilder = rowContentList[row];
                        // 先增加与上一字段间的分隔符
                        stringBuilder.Append(export.ExportSpaceString);
                        // 再生成本行对应的内容
                        if (fieldInfo.Data[row] != null)

                            stringBuilder.Append(fieldInfo.Data[row].ToString().Replace("\n", "\\n"));
                    }
                    break;
                }
            case DataType.Bool:
                {
                    for (int row = 0; row < rowCount; ++row)
                    {
                        StringBuilder stringBuilder = rowContentList[row];
                        stringBuilder.Append(export.ExportSpaceString);
                        if (fieldInfo.Data[row] != null)
                        {
                            if ((bool)fieldInfo.Data[row] == true)
                                stringBuilder.Append("true");
                            else
                                stringBuilder.Append("false");
                        }
                    }
                    break;
                }
            case DataType.Json:
                {
                    for (int row = 0; row < rowCount; ++row)
                    {
                        StringBuilder stringBuilder = rowContentList[row];
                        stringBuilder.Append(export.ExportSpaceString);
                        if (fieldInfo.Data[row] != null)
                            stringBuilder.Append(fieldInfo.JsonString[row]);
                    }
                    break;
                }
            case DataType.Date:
                {
                    DateFormatType dateFormatType = TableAnalyzeHelper.GetDateFormatType(fieldInfo.ExtraParam[DateTimeValue.DateInputFormat].ToString());
                    string exportFormatString = null;
                    // 若date型声明toLua的格式为dateTable，则按input格式进行导出
                    if (dateFormatType == DateFormatType.DataTable)
                    {
                        dateFormatType = TableAnalyzeHelper.GetDateFormatType(fieldInfo.ExtraParam[DateTimeValue.DateInputFormat].ToString());
                        exportFormatString = fieldInfo.ExtraParam[DateTimeValue.DateInputFormat].ToString();
                    }
                    else
                        exportFormatString = fieldInfo.ExtraParam[DateTimeValue.DateInputFormat].ToString();

                    switch (dateFormatType)
                    {
                        case DateFormatType.FormatString:
                            {
                                for (int row = 0; row < rowCount; ++row)
                                {
                                    StringBuilder stringBuilder = rowContentList[row];
                                    stringBuilder.Append(export.ExportSpaceString);
                                    if (fieldInfo.Data[row] != null)
                                        stringBuilder.Append(((DateTime)(fieldInfo.Data[row])).ToString(exportFormatString));
                                }
                                break;
                            }
                        case DateFormatType.ReferenceDateSec:
                            {
                                for (int row = 0; row < rowCount; ++row)
                                {
                                    StringBuilder stringBuilder = rowContentList[row];
                                    stringBuilder.Append(export.ExportSpaceString);
                                    if (fieldInfo.Data[row] != null)
                                        stringBuilder.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE).TotalSeconds);
                                }
                                break;
                            }
                        case DateFormatType.ReferenceDateMsec:
                            {
                                for (int row = 0; row < rowCount; ++row)
                                {
                                    StringBuilder stringBuilder = rowContentList[row];
                                    stringBuilder.Append(export.ExportSpaceString);
                                    if (fieldInfo.Data[row] != null)
                                        stringBuilder.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE).TotalMilliseconds);
                                }
                                break;
                            }
                        default:
                            {
                                AppLog.LogErrorAndExit("用_GetOneFieldTxtContent函数导出txt文件的date型的DateFormatType非法");
                                break;
                            }
                    }
                    break;
                }
            case DataType.Time:
                {
                    TimeFormatType timeFormatType = TableAnalyzeHelper.GetTimeFormatType(fieldInfo.ExtraParam[DateTimeValue.TimeInputFormat].ToString());
                    switch (timeFormatType)
                    {
                        case TimeFormatType.FormatString:
                            {
                                for (int row = 0; row < rowCount; ++row)
                                {
                                    StringBuilder stringBuilder = rowContentList[row];
                                    stringBuilder.Append(export.ExportSpaceString);
                                    if (fieldInfo.Data[row] != null)
                                        stringBuilder.Append(((DateTime)(fieldInfo.Data[row])).ToString(fieldInfo.ExtraParam[DateTimeValue.TimeInputFormat].ToString()));
                                }
                                break;
                            }
                        case TimeFormatType.ReferenceTimeSec:
                            {
                                for (int row = 0; row < rowCount; ++row)
                                {
                                    StringBuilder stringBuilder = rowContentList[row];
                                    stringBuilder.Append(export.ExportSpaceString);
                                    if (fieldInfo.Data[row] != null)
                                        stringBuilder.Append(((DateTime)(fieldInfo.Data[row]) - DateTimeValue.REFERENCE_DATE).TotalSeconds);
                                }
                                break;
                            }
                        default:
                            {
                                AppLog.LogErrorAndExit("错误：用_GetOneFieldTxtContent函数导出txt文件的time型的TimeFormatType非法");
                                break;
                            }
                    }
                    break;
                }
            case DataType.Array:
            case DataType.Dict:
                {
                    for (int row = 0; row < rowCount; ++row)
                    {
                        StringBuilder stringBuilder = rowContentList[row];
                        stringBuilder.Append(export.ExportSpaceString);
                        if ((bool)fieldInfo.Data[row] == false)
                            stringBuilder.Append("-1");
                    }
                    break;
                }
            default:
                {
                    AppLog.LogErrorAndExit(string.Format("_GetOneFieldTxtContent函数中未定义{0}类型数据导出至txt文件的形式", fieldInfo.DataType));
                    break;
                }
        }
    }
    /// <summary>
    /// 将某张Excel表格转换为lua table内容保存到文件
    /// </summary>
    public static bool SaveTxtFile(Export export, string excelName, string sheetName)
    {
        try
        {
            string exportDirectoryPath = FileModule.GetExportDirectoryPath(excelName, export.ExportPath, export.IsExportKeepDirectoryStructure, false);
            //如果文件夹不存在就创建
            if (Directory.Exists(exportDirectoryPath) == false)
                Directory.CreateDirectory(exportDirectoryPath);

            if (sheetName.StartsWith("'"))
                sheetName = sheetName.Substring(1);
            if (sheetName.EndsWith("'"))
                sheetName = sheetName.Substring(0, sheetName.Length - 1);
            if (sheetName.EndsWith("$"))
                sheetName = sheetName.Substring(0, sheetName.Length - 1);

            string fileName2 = string.Concat(excelName + "-" + sheetName, ".", export.ExportExtension);
            string savePath = FileModule.CombinePath(exportDirectoryPath, fileName2);
            StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
            writer.Write(export.ExportContent);
            writer.Flush();
            writer.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
