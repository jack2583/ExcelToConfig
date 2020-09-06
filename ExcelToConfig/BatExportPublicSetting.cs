using System;
using System.Collections.Generic;
using System.Text;

public class BatExportMergeSetting : IBatExportMergeSetting
{
    public BatExportMergeSetting()
    {
        StartMerge();
    }
    public BatExportMergeSetting(string exportMergeParam,string isMergeParam)
    {
        ExportMergeParam = exportMergeParam;
        IsMergeParam = isMergeParam;
        StartMerge();
    }
    public string ExportMergeParam { get; set; } = "MergeTable";
    public string IsMergeParam { get; set; } = "IsMerge";
    public bool IsMerge { get; set; }
    private Dictionary<string, string[]> MergeTabble { get; set; }
    /// <summary>
    /// 存储合并表信息，在执行StartMerge()时赋值
    /// </summary>
    public Dictionary<string, List<TableInfo>> MergeTableList { get; set; }

    private void StartMerge()
    {
        //须先ExportMergeTableParam,IsMergeTableParam赋值
        if (AppValues.BatParamInfo.ContainsKey(ExportMergeParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[ExportMergeParam].ChildParam;
            IsMerge = BatMethods.GetBoolValue(IsMerge, ChildParam, IsMergeParam);
            MergeTabble = BatMethods.GetStringArrValue(ChildParam, ExportMergeParam);
        }

        
        if (IsMerge == true)
        {
            string errorString;
            Dictionary<string, List<TableInfo>> MergeTableListTemp = new Dictionary<string, List<TableInfo>>();
            foreach (KeyValuePair<string, string[]> kvp in MergeTabble)
            {
                if (kvp.Key == IsMergeParam)
                    continue;


                if (!MergeTableListTemp.ContainsKey(kvp.Key))
                    MergeTableListTemp.Add(kvp.Key, new List<TableInfo>());

                foreach (string s in kvp.Value)
                {
                    if (AppValues.TableInfo.ContainsKey(s))
                    {
                        if (!MergeTableListTemp[kvp.Key].Contains(AppValues.TableInfo[s]))
                        {
                            MergeTableListTemp[kvp.Key].Add(AppValues.TableInfo[s]);
                        }
                    }
                }
            }
            MergeTableList = MergeTableListTemp;
            foreach (KeyValuePair<string, List<TableInfo>> kvp in MergeTableListTemp)
            {
                if (kvp.Value.Count > 0)
                {
                    TableInfo tableInfo = TableInfo.Merge(kvp.Key, kvp.Value, out errorString);
                    AppValues.TableInfo.Add(kvp.Key, tableInfo);
                }
            }
            
        }
    }
}
interface IBatExportMergeSetting
{
    /// <summary>
    /// 参数名，如：MergeTable
    /// </summary>
    string ExportMergeParam { set; get; }
    /// <summary>
    /// 参数名：是否需要合并
    /// </summary>
    string IsMergeParam { set; get; }
    /// <summary>
    /// 参数值：是否需要导出
    /// </summary>
    bool IsMerge { set; get; }
    /// <summary>
    /// 存储所有合并表信息
    /// </summary>
    Dictionary<string, List<TableInfo>> MergeTableList { set; get; }
}
class BatExportPublicSetting : IExportSetting, IExportSettingParam
{
    public BatExportPublicSetting()
    {

    }
    public string ExportTypeParam { get; set; } = "PublicSetting";
    public string IsExportParam { get; set; } = "IsExport";
    public bool IsExport { get; set; }
    public string ExportPathParam { get; set; } = "ExportPath";
    public string ExportPath { get; set; }
    public string ExportNameParam { get; set; } = "ExportName";
    public string ExportName { get; set; }
    public string ExcelNameSplitStringParam { get; set; } = "ExcelNameSplitString";
    public string ExcelNameSplitString { get; set; } = "-";//如item-道具
    public string ExportExtensionParam { get; set; } = "ExportExtension";
    public string ExportExtension { get; set; }
    public string IsExportKeepDirectoryStructureParam { get; set; } = "ExportKeepDirectoryStructure";
    public bool IsExportKeepDirectoryStructure { get; set; }
    public string ExcelOldPathParam { get; set; } = "ExcelOldPath";
    public string ExcelOldPath { get; set; }
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
    public string IsExportFormatParam { get; set; } = "ExportFormat";
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
    public string IsExportJsonMapIncludeKeyColumnValueParam { set; get; } = "IsExportJsonMapIncludeKeyColumnValue";
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
        //须先赋值
        if (AppValues.BatParamInfo.ContainsKey(ExportTypeParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[ExportTypeParam].ChildParam;
            IsExport = BatMethods.GetBoolValue(IsExport, ChildParam, IsExportParam);
            ExportPath = BatMethods.GetStringValue(ExportPath, ChildParam, ExportPathParam);
            ExportName = BatMethods.GetStringValue(ExportName, ChildParam, ExportNameParam);
            IsExportKeepDirectoryStructure = BatMethods.GetBoolValue(IsExportKeepDirectoryStructure, ChildParam, IsExportKeepDirectoryStructureParam);
            ExcelNameSplitString = BatMethods.GetStringValue(ExcelNameSplitString, ChildParam, ExcelNameSplitStringParam);
            ExportExtension = BatMethods.GetStringValue(ExportExtension, ChildParam, ExportExtensionParam);
            ExportNameBeforeAdd = BatMethods.GetStringValue(ExportNameBeforeAdd, ChildParam, ExportNameBeforeAddParam);
            ExportNameAfterLanguageMark = BatMethods.GetStringValue(ExportNameAfterLanguageMark, ChildParam, ExportNameAfterLanguageMarkParam);
            IsExportNullNumber = BatMethods.GetBoolValue(IsExportNullNumber, ChildParam, IsExportNullNumberParam);
            IsExportNullString = BatMethods.GetBoolValue(IsExportNullString, ChildParam, IsExportNullStringParam);
            IsExportNullJson = BatMethods.GetBoolValue(IsExportNullJson, ChildParam, IsExportNullJsonParam);
            IsExportNullArray = BatMethods.GetBoolValue(IsExportNullArray, ChildParam, IsExportNullArrayParam);
            IsExportNullDict = BatMethods.GetBoolValue(IsExportNullDict, ChildParam, IsExportNullDictParam);
            IsExportNullBool = BatMethods.GetBoolValue(IsExportNullBool, ChildParam, IsExportNullBoolParam);
            IsExportNullDate = BatMethods.GetBoolValue(IsExportNullDate, ChildParam, IsExportNullDateParam);
            IsExportNullTime = BatMethods.GetBoolValue(IsExportNullTime, ChildParam, IsExportNullTimeParam);
            IsExportNullLang = BatMethods.GetBoolValue(IsExportNullLang, ChildParam, IsExportNullLangParam);
            IsExportFormat = BatMethods.GetBoolValue(IsExportFormat, ChildParam, IsExportFormatParam);
            IsExportFieldComment = BatMethods.GetBoolValue(IsExportFieldComment, ChildParam, IsExportFieldCommentParam);
            ExportTopWords = BatMethods.GetStringValue(ExportTopWords, ChildParam, ExportTopWordsParam);
            ExportIndentationString = BatMethods.GetStringValue(ExportIndentationString, ChildParam, ExportIndentationStringParam);
            ExportSpaceString = BatMethods.GetStringValue(ExportSpaceString, ChildParam, ExportSpaceStringParam);
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
