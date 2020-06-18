using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Text;

public partial class TableExportToTxtHelper
{
    public static bool ExportTableToTxt(TableInfo tableInfo, out string errorString)
    {
        StringBuilder stringBuilder = new StringBuilder();

        // DataTable dt = AppValues.ExcelDataSet[tableInfo.ExcelFilePath].Tables[ExcelTableSetting.ExcelDataSheetName];
        foreach (DataTable dt in AppValues.ExcelDataSet[tableInfo.ExcelFilePath].Tables)
        {
            if (dt.TableName == "config$" && TxtStruct.IsExportConfig==false)
                continue;

            StringBuilder content = new StringBuilder();
            for (int row = 0; row < dt.Rows.Count; ++row)
            {
                List<string> strList = new List<string>();
                string str = null;
                for (int column = 0; column < dt.Columns.Count; ++column)
                {
                    str = str + "{" + column + "}" + TxtStruct.ExportTxtSplitChar;
                    strList.Add(dt.Rows[row][column].ToString());
                }

                //for (int column = 0; column < dt.Columns.Count; ++column)
                //{
                //    //str2 = str2 +" \""+ dt.Rows[row][column].ToString()+"\" ," ;
                //}
                string[] str2 = strList.ToArray();

                //str2 = str2.Remove(str2.Length - 1);
                //  str = str.TrimEnd(TxtStruct.ExportTxtSplitChar);
                // content.Append(str).Append(TxtStruct.ExportTxtLineChar);
                content.Append(string.Format(str, str2)).Append(TxtStruct.ExportTxtLineChar);
                //content.AppendFormat(str, str2).Append(TxtStruct.ExportTxtLineChar);
            }
            string exportString = content.ToString();

            if (exportString.Length < 2)
                continue;

            if (TxtStruct.SavePath == null)
            {
                TxtStruct.SavePath = tableInfo.ExcelDirectoryName;
                TxtStruct.IsExportKeepDirectoryStructure = false;
            }
            // 保存为txt文件
            if (SaveTxt.SaveTxtFile(tableInfo.ExcelName, dt.TableName, exportString) == true)
            {
                errorString = null;
                // return true;
            }
            else
            {
                errorString = string.Format("{0}的数据表{1}保存为txt文件失败\n", tableInfo.ExcelName, tableInfo.TableConfigData2);
                stringBuilder.Append(errorString);
                //return false;
            }
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
    public static bool ExportTableToTxt2(TableInfo tableInfo, out string errorString)
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
            _GetOneFieldTxtContent(allFieldInfoIgnoreSetDataStructure[i], rowContentList);

        // 要在txt中显示 导出至数据库名字及类型
        StringBuilder tempStringBuilder = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder.Append(TxtStruct.ExportTxtSplitChar);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder.Append(fieldInfo.DatabaseInfoString);
        }
        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder.Remove(0, 1));

        // 要在txt中显示 声明字段检查字符串
        StringBuilder tempStringBuilder2 = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder2.Append(TxtStruct.ExportTxtSplitChar);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder2.Append(fieldInfo.CheckRule);
        }

        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder2.Remove(0, 1));

        // 要在txt中显示 Lua等客户端数据类型
        StringBuilder tempStringBuilder3 = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder3.Append(TxtStruct.ExportTxtSplitChar);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder3.Append(fieldInfo.DataType);
        }

        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder3.Remove(0, 1));


        StringBuilder tempStringBuilder4 = new StringBuilder();
        for (int i = 0; i < allFieldInfoIgnoreSetDataStructure.Count; ++i)
        {
            tempStringBuilder4.Append(TxtStruct.ExportTxtSplitChar);
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
            tempStringBuilder5.Append(TxtStruct.ExportTxtSplitChar);
            FieldInfo fieldInfo = allFieldInfoIgnoreSetDataStructure[i];
            tempStringBuilder5.Append(fieldInfo.Desc.Replace("\n", "\\n"));
        }

        // 去掉开头多加的一个分隔符
        rowContentList.Insert(0, tempStringBuilder5.Remove(0, 1));

        StringBuilder content = new StringBuilder();
        foreach (StringBuilder stringBuilder in rowContentList)
            content.AppendLine(stringBuilder.ToString());

        if (TxtStruct.SavePath == null)
        {
            TxtStruct.SavePath = tableInfo.ExcelDirectoryName;
            TxtStruct.IsExportKeepDirectoryStructure = false;
        }

        try
        {
            string exportDirectoryPath = FileModule.GetExportDirectoryPath(tableInfo.ExcelName, TxtStruct.SavePath, TxtStruct.IsExportKeepDirectoryStructure, false);
            //如果文件夹不存在就创建
            if (Directory.Exists(exportDirectoryPath) == false)
                Directory.CreateDirectory(exportDirectoryPath);

            string fileName2 = string.Concat(tableInfo.TableName, ".", TxtStruct.SaveExtension);
            string savePath = FileModule.CombinePath(exportDirectoryPath, fileName2);
            StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
            writer.Write(content);
            writer.Flush();
            writer.Close();

            return true;
        }
        catch
        {
            errorString = string.Format("{0}的数据表{1}保存为txt文件失败\n", tableInfo.ExcelName, tableInfo.TableConfigData2);
            return false;
        }

    }
    private static void _GetOneFieldTxtContent(FieldInfo fieldInfo, List<StringBuilder> rowContentList)
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
                        stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                        stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                        stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                        stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                                    stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                                    stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                                    stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                                    stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                                    stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
                        stringBuilder.Append(TxtStruct.ExportTxtSplitChar);
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
        /// 按配置的特殊索引导出方式输出lua文件（如果声明了在生成的lua文件开头以注释形式展示列信息，将生成更直观的嵌套字段信息，而不同于普通导出规则的列信息展示）
        /// </summary>
        public static bool SpecialExportTableToLua(TableInfo tableInfo, string exportRule, out string errorString)
    {
        StringBuilder content = new StringBuilder();
        string exportString = content.ToString();

        exportRule = exportRule.Trim();
        // 解析按这种方式导出后的lua文件名
        int colonIndex = exportRule.IndexOf(':');
        if (colonIndex == -1)
        {
            errorString = string.Format("导出配置\"{0}\"定义错误，必须在开头声明导出txt文件名\n", exportRule);
            return false;
        }
        string fileName = exportRule.Substring(0, colonIndex).Trim();
        // 保存为txt文件
        if (SaveTxt.SaveTxtFile(tableInfo.TableName, fileName, exportString) == true)
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = "保存为txt文件失败\n";
            return false;
        }
    }
}