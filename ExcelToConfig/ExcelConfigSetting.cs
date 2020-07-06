using System;
using System.Collections.Generic;
using System.Text;

class ExcelConfigSetting : IExportSetting, IExportSettingParam
{
    public ExcelConfigSetting()
    {
    }
    public ExcelConfigSetting(TableInfo tableInfo)
    {
        GetParamValue(tableInfo);
    }
    public string IsExportParam { get; set; }
    public bool IsExport { get; set; }
    public string ExportPathParam { get; set; }
    public string ExportPath { get; set; }
    public string IsExportKeepDirectoryStructureParam { get; set; }
    public bool IsExportKeepDirectoryStructure { get; set; }
    public string ExcelOldPathParam { get; set; }
    public string ExcelOldPath { get; set; }
    public string ExportNameParam { get; set; }
    public string ExportName { get; set; }
    public string ExcelNameSplitStringParam { get; set; }
    public string ExcelNameSplitString { get; set; }
    public string ExportExtensionParam { get; set; }
    public string ExportExtension { get; set; }
    public string ExportNameBeforeAddParam { get; set; }
    public string ExportNameBeforeAdd { get; set; }
    public string ExportNameAfterLanguageMarkParam { get; set; }
    public string ExportNameAfterLanguageMark { get; set; }
    public string IsExportNullNumberParam { get; set; }
    public bool IsExportNullNumber { get; set; }
    public string IsExportNullStringParam { get; set; }
    public bool IsExportNullString { get; set; }
    public string IsExportNullJsonParam { get; set; }
    public bool IsExportNullJson { get; set; }
    public string IsExportNullArrayParam { get; set; }
    public bool IsExportNullArray { get; set; }
    public string IsExportNullDictParam { get; set; }
    public bool IsExportNullDict { get; set; }
    public string IsExportNullBoolParam { get; set; }
    public bool IsExportNullBool { get; set; }
    public string IsExportNullDateParam { get; set; }
    public bool IsExportNullDate { get; set; }
    public string IsExportNullTimeParam { get; set; }
    public bool IsExportNullTime { get; set; }
    public string IsExportNullLangParam { get; set; }
    public bool IsExportNullLang { get; set; }
    public string IsExportFormatParam { get; set; }
    public bool IsExportFormat { get; set; }
    public string IsExportFieldCommentParam { get; set; }
    public bool IsExportFieldComment { get; set; }
    public string ExportTopWordsParam { get; set; }
    public string ExportTopWords { get; set; }
    public string ExportIndentationStringParam { get; set; }
    public string ExportIndentationString { get; set; }
    public string ExportSpaceStringParam { get; set; }
    public string ExportSpaceString { get; set; }
    public string ExportLineStringParam { set; get; }
    public string ExportLineString { set; get; }
    public string IsExportJsonArrayFormatParam { set; get; }
    public bool IsExportJsonArrayFormat { set; get; }
    public string IsExportJsonMapIncludeKeyColumnValueParam { set; get; }
    public bool IsExportJsonMapIncludeKeyColumnValue { set; get; }
    public string IsArrayFieldNameParam { set; get; }
    public bool IsArrayFieldName { set; get; }
    public string IsTableNameStartParam { set; get; }
    public bool IsTableNameStart { set; get; }
    public string IsAddKeyToLuaTableParam { set; get; }
    public bool IsAddKeyToLuaTable { set; get; }
    public string DateToExportFormatParam { set; get; }
    public string DateToExportFormat { set; get; }
    public string TimeToExportFormatParam { set; get; }
    public string TimeToExportFormat { set; get; }
    public string SpecialExportParam { set; get; }
    public string SpecialExport { set; get; }
    //public string ExportMergeToTableParam { get; set; }
    // public string ExportMergeToTable { get; set; }
    public void GetParamValue() { }
    public void GetParamValue(TableInfo tableInfo)
    {
        ExcelOldPath = tableInfo.ExcelFilePath;
        if (tableInfo.TableConfigData == null)
            return ;

        IsExport = GetBoolValue(tableInfo, IsExportParam);
        ExportPath = GetStringValue(tableInfo, ExportPathParam);
        IsExportKeepDirectoryStructure = GetBoolValue(tableInfo, IsExportKeepDirectoryStructureParam);
       
        ExportName = GetStringValue(tableInfo, ExportNameParam);
        ExcelNameSplitString = GetStringValue(tableInfo, ExcelNameSplitStringParam);
        ExportExtension = GetStringValue(tableInfo, ExportExtensionParam);
        ExportNameBeforeAdd = GetStringValue(tableInfo, ExportNameBeforeAddParam);
        ExportNameAfterLanguageMark = GetStringValue(tableInfo, ExportNameAfterLanguageMarkParam);
        IsExportNullNumber = GetBoolValue(tableInfo, IsExportNullNumberParam);
        IsExportNullString = GetBoolValue(tableInfo, IsExportNullStringParam);
        IsExportNullJson = GetBoolValue(tableInfo, IsExportNullJsonParam);
        IsExportNullArray = GetBoolValue(tableInfo, IsExportNullArrayParam);
        IsExportNullDict = GetBoolValue(tableInfo, IsExportNullDictParam);
        IsExportNullBool = GetBoolValue(tableInfo, IsExportNullBoolParam);
        IsExportNullDate = GetBoolValue(tableInfo, IsExportNullDateParam);
        IsExportNullTime = GetBoolValue(tableInfo, IsExportNullTimeParam);
        IsExportNullLang = GetBoolValue(tableInfo, IsExportNullLangParam);
        IsExportFormat = GetBoolValue(tableInfo, IsExportFormatParam);
        IsExportFieldComment = GetBoolValue(tableInfo, IsExportFieldCommentParam);
        ExportTopWords = GetStringValue(tableInfo, ExportTopWordsParam);
        ExportIndentationStringParam = GetStringValue(tableInfo, ExportIndentationStringParam);
        ExportSpaceString = GetStringValue(tableInfo, ExportSpaceStringParam);
        ExportLineString = GetStringValue(tableInfo, ExportLineStringParam);
        IsExportJsonArrayFormat = GetBoolValue(tableInfo, IsExportJsonArrayFormatParam);
        IsExportJsonMapIncludeKeyColumnValue = GetBoolValue(tableInfo, IsExportJsonMapIncludeKeyColumnValueParam);
        IsArrayFieldName = GetBoolValue(tableInfo, IsArrayFieldNameParam);
        IsTableNameStart = GetBoolValue(tableInfo, IsTableNameStartParam);
        IsAddKeyToLuaTable = GetBoolValue(tableInfo, IsAddKeyToLuaTableParam);
        DateToExportFormat = GetStringValue(tableInfo, DateToExportFormatParam);
        TimeToExportFormat = GetStringValue(tableInfo, TimeToExportFormatParam);
        SpecialExport = GetStringValue(tableInfo, SpecialExportParam);
        // ExportMergeToTable = GetStringValue(tableInfo, ExportMergeToTableParam);
    }

    private bool GetBoolValue(TableInfo tableInfo, string ECP)
    {
        bool ECV = false;
        if (ECP == null)
            return ECV;

        if (tableInfo.TableConfigData.ContainsKey(ECP))
        {
            if ("true".Equals(tableInfo.TableConfigData[ECP], StringComparison.CurrentCultureIgnoreCase))
                ECV = true;
            else
                ECV = false;

        }
        else
            ECV = false;

        return ECV;
    }

    private string GetStringValue(TableInfo tableInfo, string ECP)
    {
        string ECV=null;
        if (ECP == null)
            return ECV;

        if (tableInfo.TableConfigData.ContainsKey(ECP))
        {
            if (tableInfo.TableConfigData[ECP] == null)
                ECV = null;
            else if (tableInfo.TableConfigData[ECP] == "")
                ECV = "";
            else
                ECV = tableInfo.TableConfigData[ECP];
        }
        else
            ECV = null;

        return ECV;
    }
}
