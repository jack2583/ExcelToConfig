using System;
using System.Collections.Generic;
using System.Text;

class ExcelConfigSetting : IExcelConfigSetting
{
    public string IsExportECP { get; set; }
    public bool isExporECV { get; set; }
    public string ExportPathECP { get; set; }
    public string ExportPathECV { get; set; }
    public string ExportKeepDirectoryStructureECP { get; set; }
    public bool ExportKeepDirectoryStructureECV { get; set; }
    public string ExportNameECP { get; set; }
    public string ExportNameECV { get; set; }
    public string ExportNameBeforeAddECP { get; set; }
    public string ExportNameBeforeAddECV { get; set; }
    public string ExportNameAfterLanguageMarkECP { get; set; }
    public string ExportNameAfterLanguageMarkECV { get; set; }
    public string ExportNullNumberECP { get; set; }
    public bool ExportNullNumberECV { get; set; }
    public string ExportNullStringECP { get; set; }
    public bool ExportNullStringECV { get; set; }
    public string ExportNullJsonECP { get; set; }
    public bool ExportNullJsonECV { get; set; }
    public string ExportNullAraayECP { get; set; }
    public bool ExportNullAraayECV { get; set; }
    public string ExportNullDictECP { get; set; }
    public bool ExportNullDictECV { get; set; }
    public string ExportFormatECP { get; set; }
    public bool ExportFormatECV { get; set; }
    public string ExportFieldCommentECP { get; set; }
    public bool ExportFieldCommentECV { get; set; }
    public string ExportTopWordsECP { get; set; }
    public string ExportTopWordsECV { get; set; }
    public string ExportIndentationStringECP { get; set; }
    public string ExportIndentationStringECV { get; set; }
    public string ExportSpaceStringECP { get; set; }
    public string ExportSpaceStringECV { get; set; }
    public string ExportMergeToTableECP { get; set; }
    public string ExportMergeToTableECV { get; set; }

    public void GetExcelConfigValue(TableInfo tableInfo)
    {
        isExporECV = GetExcelConfigBoolValue(tableInfo, IsExportECP);
        ExportPathECV = GetExcelConfigStringValue(tableInfo, ExportPathECP);
        ExportKeepDirectoryStructureECV = GetExcelConfigBoolValue(tableInfo, ExportKeepDirectoryStructureECP);
        ExportNameECV = GetExcelConfigStringValue(tableInfo, ExportNameECP);
        ExportNameBeforeAddECV = GetExcelConfigStringValue(tableInfo, ExportNameBeforeAddECP);
        ExportNameAfterLanguageMarkECV = GetExcelConfigStringValue(tableInfo, ExportNameAfterLanguageMarkECP);
        ExportNullNumberECV = GetExcelConfigBoolValue(tableInfo, ExportNullNumberECP);
        ExportNullStringECV = GetExcelConfigBoolValue(tableInfo, ExportNullStringECP);
        ExportNullJsonECV = GetExcelConfigBoolValue(tableInfo, ExportNullJsonECP);
        ExportNullAraayECV = GetExcelConfigBoolValue(tableInfo, ExportNullAraayECP);
        ExportNullDictECV = GetExcelConfigBoolValue(tableInfo, ExportNullDictECP);
        ExportFormatECV = GetExcelConfigBoolValue(tableInfo, ExportFormatECP);
        ExportFieldCommentECV = GetExcelConfigBoolValue(tableInfo, ExportFieldCommentECP);
        ExportTopWordsECV = GetExcelConfigStringValue(tableInfo, ExportTopWordsECP);
        ExportIndentationStringECP = GetExcelConfigStringValue(tableInfo, ExportIndentationStringECP);
        ExportSpaceStringECV = GetExcelConfigStringValue(tableInfo, ExportSpaceStringECP);
        ExportMergeToTableECV = GetExcelConfigStringValue(tableInfo, ExportMergeToTableECP);
    }

    private bool GetExcelConfigBoolValue(TableInfo tableInfo, string ECP)
    {
        bool ECV;
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

    private string GetExcelConfigStringValue(TableInfo tableInfo, string ECP)
    {
        string ECV=null;
        if (tableInfo.TableConfigData.ContainsKey(ECP))
        {
            if (tableInfo.TableConfigData[ECP]==null)
                ECV = null;
            else if (tableInfo.TableConfigData[ECP] == "")
                ECV = "";
        }
        else
            ECV = null;

        return ECV;
    }
}
