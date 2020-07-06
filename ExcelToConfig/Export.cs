using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


class Export : IExportSave, IExportSetting
{
    public Export()
    { }
    public Export(TableInfo tableInfo)
    {
        // IsExport = GetIsExport();
    }
    public bool IsExport { get; set; }
    public string ExportPath { get; set; }
    public bool IsExportKeepDirectoryStructure { get; set; } = false;
    public string ExcelOldPath { get; set; }
    public string ExportName { get; set; }
    public string ExcelNameSplitString { get; set; }
    public string ExportExtension { get; set; }
    public string ExportNameBeforeAdd { get; set; }
    public string ExportNameAfterLanguageMark { get; set; }
    public string ExportContent { get; set; }
    public bool IsExportNullNumber { get; set; }
    public bool IsExportNullString { get; set; }
    public bool IsExportNullJson { get; set; }
    public bool IsExportNullArray { get; set; }
    public bool IsExportNullDict { get; set; }
    public bool IsExportNullBool { get; set; }
    public bool IsExportNullDate { get; set; }
    public bool IsExportNullTime { get; set; }
    public bool IsExportNullLang { get; set; }
    public bool IsExportFormat { get; set; }
    public bool IsExportFieldComment { get; set; }
    public string ExportTopWords { get; set; }
    public string ExportIndentationString { get; set; }
    public string ExportSpaceString { get; set; }
    public string ExportLineString { set; get; }
    public bool IsExportJsonArrayFormat { get; set; }
    public bool IsExportJsonMapIncludeKeyColumnValue { set; get; }
    public bool IsArrayFieldName { set; get; }
    public bool IsTableNameStart { set; get; }
    public bool IsAddKeyToLuaTable { set; get; }
    public string DateToExportFormat { set; get; }
    public string TimeToExportFormat { set; get; }
    public string SpecialExport { set; get; }

    public void GetParamValue() { }
    public void GetValue(TableInfo tableInfo,ExcelConfigSetting excel, BatExportSetting bat, BatExportPublicSetting bPubilc)
    {
        IsExport = GetBoolValue(tableInfo, excel.IsExportParam, excel.IsExport, bat.IsExport, bPubilc.IsExport);
        ExportPath = GetExportPath(GetStringValue(excel.ExportPath, bat.ExportPath, bPubilc.ExportPath));
        IsExportKeepDirectoryStructure = GetBoolValue(tableInfo, excel.IsExportKeepDirectoryStructureParam, excel.IsExportKeepDirectoryStructure, bat.IsExportKeepDirectoryStructure, bPubilc.IsExportKeepDirectoryStructure);
        ExcelOldPath = tableInfo.ExcelFilePath;// GetStringValue(excel.ExcelOldPath, bat.ExcelOldPath, bPubilc.ExcelOldPath);
        ExportName = GetExportName(tableInfo, excel.ExportName);
        ExcelNameSplitString = GetStringValue(excel.ExcelNameSplitString, bat.ExcelNameSplitString, bPubilc.ExcelNameSplitString);
        ExportExtension = GetStringValue(excel.ExportExtension, bat.ExportExtension, bPubilc.ExportExtension);
        ExportNameBeforeAdd = GetStringValue(excel.ExportNameBeforeAdd, bat.ExportNameBeforeAdd, bPubilc.ExportNameBeforeAdd);
        ExportNameAfterLanguageMark = GetStringValue(excel.ExportNameAfterLanguageMark, bat.ExportNameAfterLanguageMark, bPubilc.ExportNameAfterLanguageMark);
        //ExportContent =  
        IsExportNullNumber = GetBoolValue(tableInfo, excel.IsExportNullNumberParam, excel.IsExportNullNumber, bat.IsExportNullNumber, bPubilc.IsExportNullNumber);
        IsExportNullString = GetBoolValue(tableInfo, excel.IsExportNullStringParam, excel.IsExportNullString, bat.IsExportNullString, bPubilc.IsExportNullString);
        IsExportNullJson = GetBoolValue(tableInfo, excel.IsExportNullJsonParam, excel.IsExportNullJson, bat.IsExportNullJson, bPubilc.IsExportNullJson);
        IsExportNullArray = GetBoolValue(tableInfo, excel.IsExportNullArrayParam, excel.IsExportNullArray, bat.IsExportNullArray, bPubilc.IsExportNullArray);
        IsExportNullDict = GetBoolValue(tableInfo, excel.IsExportNullDictParam, excel.IsExportNullDict, bat.IsExportNullDict, bPubilc.IsExportNullDict);
        IsExportNullBool = GetBoolValue(tableInfo, excel.IsExportNullBoolParam, excel.IsExportNullBool, bat.IsExportNullBool, bPubilc.IsExportNullBool);
        IsExportNullDate = GetBoolValue(tableInfo, excel.IsExportNullDateParam, excel.IsExportNullDate, bat.IsExportNullDate, bPubilc.IsExportNullDate);
        IsExportNullTime = GetBoolValue(tableInfo, excel.IsExportNullTimeParam, excel.IsExportNullTime, bat.IsExportNullTime, bPubilc.IsExportNullTime);
        IsExportNullLang = GetBoolValue(tableInfo, excel.IsExportNullLangParam, excel.IsExportNullLang, bat.IsExportNullLang, bPubilc.IsExportNullLang);
        IsExportFormat = GetBoolValue(tableInfo, excel.IsExportFormatParam, excel.IsExportFormat, bat.IsExportFormat, bPubilc.IsExportFormat);
        IsExportFieldComment = GetBoolValue(tableInfo, excel.IsExportFieldCommentParam, excel.IsExportFieldComment, bat.IsExportFieldComment, bPubilc.IsExportFieldComment);
        ExportTopWords = GetStringValue(excel.ExportTopWords, bat.ExportTopWords, bPubilc.ExportTopWords);
        ExportIndentationString = GetStringValue(excel.ExportIndentationString, bat.ExportIndentationString, bPubilc.ExportIndentationString);
        ExportSpaceString = GetStringValue(excel.ExportSpaceString, bat.ExportSpaceString, bPubilc.ExportSpaceString);
        ExportLineString = GetStringValue(excel.ExportLineString, bat.ExportLineString, bPubilc.ExportLineString);
        IsExportJsonArrayFormat = GetBoolValue(tableInfo, excel.IsExportJsonArrayFormatParam, excel.IsExportJsonArrayFormat, bat.IsExportJsonArrayFormat, bPubilc.IsExportJsonArrayFormat);
        IsExportJsonMapIncludeKeyColumnValue = GetBoolValue(tableInfo, excel.IsExportJsonMapIncludeKeyColumnValueParam, excel.IsExportJsonMapIncludeKeyColumnValue, bat.IsExportJsonMapIncludeKeyColumnValue, bPubilc.IsExportJsonMapIncludeKeyColumnValue);
        IsArrayFieldName = GetBoolValue(tableInfo, excel.IsArrayFieldNameParam, excel.IsArrayFieldName, bat.IsArrayFieldName, bPubilc.IsArrayFieldName);
        IsTableNameStart = GetBoolValue(tableInfo, excel.IsTableNameStartParam, excel.IsTableNameStart, bat.IsTableNameStart, bPubilc.IsTableNameStart);
        IsAddKeyToLuaTable = GetBoolValue(tableInfo, excel.IsAddKeyToLuaTableParam, excel.IsAddKeyToLuaTable, bat.IsAddKeyToLuaTable, bPubilc.IsAddKeyToLuaTable);
        DateToExportFormat = GetStringValue(excel.DateToExportFormat, bat.DateToExportFormat, bPubilc.DateToExportFormat);
        TimeToExportFormat = GetStringValue(excel.TimeToExportFormat, bat.TimeToExportFormat, bPubilc.TimeToExportFormat);
        SpecialExport = GetStringValue(excel.SpecialExport, bat.SpecialExport, bPubilc.SpecialExport);
    }
    private string GetStringValue(string excelParam, string batParam, string bPubilcParam)
    {
        if (excelParam != null)
            return excelParam;
        else if (batParam != null)
            return batParam;
        else if (bPubilcParam != null)
            return bPubilcParam;
        else
            return "";
    }

    private bool GetBoolValue(TableInfo tableInfo,string excelParam,bool excelValue, bool batValue, bool bPubilcValue)
    {
        if (tableInfo.TableConfigData == null)
        {
            if (batValue)
                return batValue;
            else if (bPubilcValue)
                return bPubilcValue;
            else
                return false;
        }

        if (!tableInfo.TableConfigData.ContainsKey(excelParam))
        {
            if (batValue)
                return batValue;
            else if (bPubilcValue)
                return bPubilcValue;
            else
                return false;
        }

        return excelValue;

    }
    private string GetExportName(TableInfo tableInfo,string excelParam)
    {
        if (excelParam != null && excelParam != "")
            ExportName = excelParam;
        else
            ExportName = tableInfo.TableName;

        return ExportName;
    }
    private string GetExportPath(string self)
    {
        if (self==null || self == "")
            return ExcelFolder.ExcelPath;
        else
            return self;
    }
    private void GetExportName(string excelSetting, string ExcelName, string ExcelNameSplitString)
    {
        if (excelSetting != null && excelSetting != "")
            ExportName = excelSetting;
        else
        {
            if (ExcelNameSplitString != null && ExcelNameSplitString != "")
            {
                if (ExcelName.Contains(ExcelNameSplitString))
                {
                    string[] fileNames = null;
                    fileNames = ExcelName.Split(new string[] { ExcelNameSplitString }, StringSplitOptions.RemoveEmptyEntries);
                    ExcelName = fileNames[0];
                }

                if(ExcelName.EndsWith(ExcelFolder.TheLanguage))
                    ExcelName = ExcelName.Substring(0, ExcelFolder.TheLanguage.Length);

                ExportName = ExcelName;
            }
            else
            {
                if (ExcelName.EndsWith(ExcelFolder.TheLanguage))
                    ExcelName = ExcelName.Substring(0, ExcelFolder.TheLanguage.Length);

                ExportName = ExcelName;
            }
        }
    }
    /// <summary>
    /// 获取最终导出的完整文件路径
    /// </summary>
    /// <param name="ExcelName">excel名，如item-道具</param>
    /// <returns></returns>
    private string GetPath(string ExcelName)
    {
        // 获取表格相对于Excel文件夹的相对路径
        string excelFolderPath = ExcelFolder.ExcelPath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        if (!excelFolderPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            excelFolderPath = excelFolderPath + Path.DirectorySeparatorChar;

        Uri excelFolderUri = new Uri(excelFolderPath);
        Uri fileUri = new Uri(ExcelFolder.ExportTables[ExcelName]);
        Uri relativeUri = excelFolderUri.MakeRelativeUri(fileUri);
        // 注意：Uri转为的字符串中会将中文转义为%xx，需要恢复为非转义形式
        string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        if (ExcelNameSplitString != null)
        {
            string[] SplitRelativePath = relativePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string[] tempSplitRelativePath;
            relativePath = "";
            for (int i = 0; i < SplitRelativePath.Length - 1; i++)
            {
                tempSplitRelativePath = SplitRelativePath[i].Split(new string[] { ExcelNameSplitString }, StringSplitOptions.RemoveEmptyEntries);
                relativePath = relativePath + tempSplitRelativePath[0] + "/";
            }
            relativePath = relativePath + SplitRelativePath[SplitRelativePath.Length - 1];
        }

        return Path.GetDirectoryName(FileModule.CombinePath(ExportPath, relativePath));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ExcelName">excel名，如item-道具</param>
    /// <returns></returns>
    public bool SaveFile(string tableName)
    {
        try
        {
            if (IsExportKeepDirectoryStructure == true)
            {
                ExportPath = FileModule.GetExportDirectoryPath(tableName, ExportPath, IsExportKeepDirectoryStructure, true);
            }
            //如果文件夹不存在就创建
            if (Directory.Exists(ExportPath) == false)
                Directory.CreateDirectory(ExportPath);

            string fileName2 = string.Concat(ExportNameBeforeAdd + ExportName + ExportNameAfterLanguageMark, ".", ExportExtension);
            string savePath = FileModule.CombinePath(ExportPath, fileName2);
            StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
            writer.Write(ExportContent);
            writer.Flush();
            writer.Close();
            return true;
        }
        catch(Exception e)
        {
            AppLog.LogErrorAndExit(string.Format("导出失败:{0}",e.ToString()));
            return false;
        }
    }
    public bool SaveFile(string tableName,string excelName,string sheetName)
    {
        try
        {
            if (IsExportKeepDirectoryStructure == true)
            {
                ExportPath = FileModule.GetExportDirectoryPath(tableName, ExportPath, IsExportKeepDirectoryStructure, false);
            }
            //如果文件夹不存在就创建
            if (Directory.Exists(ExportPath) == false)
                Directory.CreateDirectory(ExportPath);

            string s="";
            if (excelName.Contains("-"))
                s = excelName.Split('-')[1];

            string fileName2 = string.Concat(excelName + "-"+sheetName, ".", ExportExtension);
            string savePath = FileModule.CombinePath(ExportPath, fileName2);
            StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
            writer.Write(ExportContent);
            writer.Flush();
            writer.Close();
            return true;
        }
        catch (Exception e)
        {
            AppLog.LogErrorAndExit(string.Format("导出失败:{0}", e.ToString()));
            return false;
        }
    }
    public void GetParamValue(TableInfo tableInfo)
    {

    }
}


