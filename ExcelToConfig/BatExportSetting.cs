using System;
using System.Collections.Generic;
using System.Text;

class BatExportSetting : IExportSetting, IExportSettingParam
{
    public string ExportTypeParam { get; set; }
    public string IsExportParam { get; set; } = "IsExport";
    public bool IsExport { get; set; }
    public string ExportPathParam { get; set; } = "ExportPath";
    public string ExportPath { get; set; }
    public string IsExportKeepDirectoryStructureParam { get; set; } = "ExportKeepDirectoryStructure";
    public bool IsExportKeepDirectoryStructure { get; set; }
    public string ExcelOldPathParam { get; set; } = "ExcelOldPath";
    public string ExcelOldPath { get; set; }
    public string ExportNameParam { get; set; } = "ExportName";
    public string ExportName { get; set; }
    public string ExcelNameSplitStringParam { get; set; } = "ExcelNameSplitString";
    public string ExcelNameSplitString { get; set; }
    public string ExportExtensionParam { get; set; } = "ExportExtension";
    public string ExportExtension { get; set; }
    public string ExportNameBeforeAddParam { get; set; } = "ExportNameBeforeAdd";
    public string ExportNameBeforeAdd { get; set; }
    public string ExportNameAfterLanguageMarkParam { get; set; } = "ExportNameAfterLanguageMark";
    public string ExportNameAfterLanguageMark { get; set; }
    public string IsExportNullNumberParam { get; set; } = "ExportNullNumber";
    public bool IsExportNullNumber { get; set; }
    public string IsExportNullStringParam { get; set; } = "ExportNullString";
    public bool IsExportNullString { get; set; }
    public string IsExportNullJsonParam { get; set; } = "ExportNullJson";
    public bool IsExportNullJson { get; set; }
    public string IsExportNullArrayParam { get; set; } = "ExportNullAraay";
    public bool IsExportNullArray { get; set; }
    public string IsExportNullDictParam { get; set; } = "ExportNullDict";
    public bool IsExportNullDict { get; set; }
    public string IsExportNullBoolParam { get; set; } = "ExportNullBool";
    public bool IsExportNullBool { get; set; }
    public string IsExportNullDateParam { get; set; } = "ExportNullDate";
    public bool IsExportNullDate { get; set; }
    public string IsExportNullTimeParam { get; set; } = "ExportNullTime";
    public bool IsExportNullTime { get; set; }
    public string IsExportNullLangParam { get; set; } = "ExportNullLang";
    public bool IsExportNullLang { get; set; }
    public string IsExportFormatParam { get; set; } = "IsFormat";
    public bool IsExportFormat { get; set; }
    public string IsExportFieldCommentParam { get; set; } = "ExportFieldComment";
    public bool IsExportFieldComment { get; set; }
    public string ExportTopWordsParam { get; set; } = "ExportTopWords";
    public string ExportTopWords { get; set; }
    public string ExportIndentationStringParam { get; set; } = "ExportIndentationString";
    public string ExportIndentationString { get; set; }
    public string ExportSpaceStringParam { get; set; } = "ExportSpaceString";
    public string ExportSpaceString { get; set; }
    public string ExportLineStringParam { set; get; } = "ExportSplitChar";
    public string ExportLineString { set; get; }
    public string IsExportJsonArrayFormatParam { set; get; } = "IsExportJsonArrayFormat";
    public bool IsExportJsonArrayFormat { set; get; }
    public string IsExportJsonMapIncludeKeyColumnValueParam { set; get; } = "IsKeyColumnValue";
    public bool IsExportJsonMapIncludeKeyColumnValue { set; get; }
    public string IsArrayFieldNameParam { set; get; } = "IsArrayFieldName";
    public bool IsArrayFieldName { set; get; }
    public string IsTableNameStartParam { set; get; } = "IsTableNameStart";
    public bool IsTableNameStart { set; get; }
    public string IsAddKeyToLuaTableParam { set; get; } = "IsAddKeyToLuaTable";
    public bool IsAddKeyToLuaTable { set; get; }
    public string DateToExportFormatParam { set; get; } = "DateToExportFormat";
    public string DateToExportFormat { set; get; }
    public string TimeToExportFormatParam { set; get; } = "TimeToExportFormat";
    public string TimeToExportFormat { set; get; }
    public string SpecialExportParam { set; get; } = "SpecialExport";
    public string SpecialExport { set; get; }
    public void GetParamValue()
    {
        if (AppValues.BatParamInfo.ContainsKey(ExportTypeParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[ExportTypeParam].ChildParam;
            IsExport = BatMethods.GetBoolValue(IsExport,ChildParam, IsExportParam);
            ExportPath = BatMethods.GetStringValue(ExportPath,ChildParam, ExportPathParam);
            IsExportKeepDirectoryStructure = BatMethods.GetBoolValue(IsExportKeepDirectoryStructure,ChildParam, IsExportKeepDirectoryStructureParam);
            ExportName = BatMethods.GetStringValue(ExportName,ChildParam, ExportNameParam);
            ExcelNameSplitString = BatMethods.GetStringValue(ExcelNameSplitString,ChildParam, ExcelNameSplitStringParam);
            ExportExtension = BatMethods.GetStringValue(ExportExtension, ChildParam, ExportExtensionParam);
            ExportNameBeforeAdd = BatMethods.GetStringValue(ExportNameBeforeAdd,ChildParam, ExportNameBeforeAddParam);
            ExportNameAfterLanguageMark = BatMethods.GetStringValue(ExportNameAfterLanguageMark,ChildParam, ExportNameAfterLanguageMarkParam);
            IsExportNullNumber = BatMethods.GetBoolValue(IsExportNullNumber, ChildParam, IsExportNullNumberParam);
            IsExportNullString = BatMethods.GetBoolValue(IsExportNullString, ChildParam, IsExportNullStringParam);
            IsExportNullJson = BatMethods.GetBoolValue(IsExportNullJson,ChildParam, IsExportNullJsonParam);
            IsExportNullArray = BatMethods.GetBoolValue(IsExportNullArray,ChildParam, IsExportNullArrayParam);
            IsExportNullDict = BatMethods.GetBoolValue(IsExportNullDict,ChildParam, IsExportNullDictParam);
            IsExportNullBool = BatMethods.GetBoolValue(IsExportNullBool, ChildParam, IsExportNullBoolParam);
            IsExportNullDate = BatMethods.GetBoolValue(IsExportNullDate, ChildParam, IsExportNullDateParam);
            IsExportNullTime = BatMethods.GetBoolValue(IsExportNullTime, ChildParam, IsExportNullTimeParam);
            IsExportNullLang = BatMethods.GetBoolValue(IsExportNullLang, ChildParam, IsExportNullLangParam);
            IsExportFormat = BatMethods.GetBoolValue(IsExportFormat,ChildParam, IsExportFormatParam);
            IsExportFieldComment = BatMethods.GetBoolValue(IsExportFieldComment,ChildParam, IsExportFieldCommentParam);
            ExportTopWords = BatMethods.GetStringValue(ExportTopWords,ChildParam, ExportTopWordsParam);
            ExportIndentationString = BatMethods.GetStringValue(ExportIndentationString,ChildParam, ExportIndentationStringParam);
            ExportSpaceString = BatMethods.GetStringValue(ExportSpaceString,ChildParam, ExportSpaceStringParam);
            ExportLineString = BatMethods.GetStringValue(ExportLineString, ChildParam, ExportLineStringParam);
            IsExportJsonArrayFormat = BatMethods.GetBoolValue(IsExportJsonArrayFormat, ChildParam, IsExportJsonArrayFormatParam);
            IsExportJsonMapIncludeKeyColumnValue = BatMethods.GetBoolValue(IsExportJsonMapIncludeKeyColumnValue, ChildParam, IsExportJsonMapIncludeKeyColumnValueParam);
            IsArrayFieldName = BatMethods.GetBoolValue(IsArrayFieldName, ChildParam, IsArrayFieldNameParam);
            IsTableNameStart = BatMethods.GetBoolValue(IsTableNameStart, ChildParam, IsTableNameStartParam);
            IsAddKeyToLuaTable = BatMethods.GetBoolValue(IsAddKeyToLuaTable, ChildParam, IsAddKeyToLuaTableParam);
            DateToExportFormat = BatMethods.GetStringValue(DateToExportFormat, ChildParam, DateToExportFormatParam);
            TimeToExportFormat = BatMethods.GetStringValue(TimeToExportFormat, ChildParam, TimeToExportFormatParam);
            SpecialExport = BatMethods.GetStringValue(SpecialExport, ChildParam, SpecialExportParam);
        }
    }
}

class BatMethods
{
    public static bool GetBoolValue(bool self,Dictionary<string, BatChildParam> ChildParam, string BCP)
    {
        if (ChildParam != null)
        {
            if (ChildParam.ContainsKey(BCP))
            {
                if ("true".Equals(ChildParam[BCP].ChildParamValue, StringComparison.CurrentCultureIgnoreCase))
                    return true;
                else
                    return false;
            }
            else
                return self;
        }
        else
            return self;
    }
    public static string GetStringValue(string self,Dictionary<string, BatChildParam> ChildParam, string BCP)
    {
        if (ChildParam != null)
        {
            if (ChildParam.ContainsKey(BCP))
            {
                string v = ChildParam[BCP].ChildParamValue;
                if (v == null)
                    return self;
                else if (v.Length == 0)
                    return self;
                else
                    return v;

            }
            else
                return self;
        }
        else
            return self;
    }
    public static Dictionary<string, string[]> GetStringArrValue(Dictionary<string, BatChildParam> ChildParam, string ExportMergeTableP)
    {
        if (ChildParam != null)
        {
            Dictionary<string, string[]> MergeTabble = new Dictionary<string, string[]>();
            foreach (KeyValuePair<string,BatChildParam> kvp in ChildParam)
            {
                if (kvp.Key == ExportMergeTableP)
                    continue;
                else
                {
                    MergeTabble.Add(kvp.Key, kvp.Value.ChildParamValueList);
                }
            }
            return MergeTabble;
        }
        else
            return null;
    }
}