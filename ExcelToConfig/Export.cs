using System;
using System.Collections.Generic;
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
    public string ExportName { get; set; }
    public string ExportExtension { get; set; } = "txt";
    public string ExportNameBeforeAdd { get; set; }
    public string ExportNameAfterLanguageMark { get; set; }
    public string ExportContent { get; set; }
    public bool ExportNullNumber { get; set; }
    public bool ExportNullString { get; set; }
    public bool ExportNullJson { get; set; }
    public bool ExportNullArray { get; set; }
    public bool ExportNullDict { get; set; }
    public bool ExportFormat { get; set; }
    public bool ExportFieldComment { get; set; }
    public string ExportTopWords { get; set; }
    public string ExportIndentationString { get; set; }
    public string ExportSpaceString { get; set; }

    public bool GetExportFieldComment(bool ExportFieldCommentECV, bool ExportFieldCommentBCV, bool ExportFieldCommentPBCV)
    {
        if (ExportFieldCommentECV)
            return ExportFieldCommentECV;
        else if (ExportFieldCommentBCV)
            return ExportFieldCommentBCV;
        else if (ExportFieldCommentPBCV)
            return ExportFieldCommentPBCV;
        else
            return false;
    }

    public bool GetExportFormat(bool ExportFormatECV, bool ExportFormatBCV, bool ExportFormatPBCV)
    {
        if (ExportFormatECV)
            return ExportFormatECV;
        else if (ExportFormatBCV)
            return ExportFormatBCV;
        else if (ExportFormatPBCV)
            return ExportFormatPBCV;
        else
            return false;
    }

    public string GetExportIndentationString(string ExportIndentationStringECV, string ExportIndentationStringBCV, string ExportIndentationStringPBCV)
    {
        if (ExportIndentationStringECV != null && ExportIndentationStringECV != "")
            return ExportIndentationStringECV;
        else if (ExportIndentationStringBCV != null && ExportIndentationStringBCV != "")
            return ExportIndentationStringBCV;
        else if (ExportIndentationStringPBCV != null && ExportIndentationStringPBCV != "")
            return ExportIndentationStringPBCV;
        else
            return "";
    }

    public string GetExportName(string ExportNameECV, string ExcelName, string ExcelNameSplitString = "-")
    {
        if (ExportNameECV != null && ExportNameECV != "")
            return ExportNameECV;
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
                return ExcelName;
            }
            else
            {
                return ExcelName;
            }
        }
    }

    public string GetExportNameAfterLanguageMark(string ExportNameAfterLanguageMarkECV, string ExportNameAfterLanguageMarkBCV, string ExportNameAfterLanguageMarkPBCV)
    {
        if (ExportNameAfterLanguageMarkECV != null && ExportNameAfterLanguageMarkECV != "")
            return ExportNameAfterLanguageMarkECV;
        else if (ExportNameAfterLanguageMarkBCV != null && ExportNameAfterLanguageMarkBCV != "")
            return ExportNameAfterLanguageMarkBCV;
        else if (ExportNameAfterLanguageMarkPBCV != null && ExportNameAfterLanguageMarkPBCV != "")
            return ExportNameAfterLanguageMarkPBCV;
        else
            return "";
    }

    public string GetExportNameBeforeAdd(string ExportNameBeforeAddECV, string ExportNameBeforeAddBCV, string ExportNameBeforeAddPBCV)
    {
        if (ExportNameBeforeAddECV != null && ExportNameBeforeAddECV != "")
            return ExportNameBeforeAddECV;
        else if (ExportNameBeforeAddBCV != null && ExportNameBeforeAddBCV != "")
            return ExportNameBeforeAddBCV;
        else if (ExportNameBeforeAddPBCV != null && ExportNameBeforeAddPBCV != "")
            return ExportNameBeforeAddPBCV;
        else
            return "";
    }

    public bool GetExportNullArray(bool ExportNullAraayECV, bool ExportNullAraayBCV, bool ExportNullAraayPBCV)
    {
        if (ExportNullAraayECV)
            return ExportNullAraayECV;
        else if (ExportNullAraayBCV)
            return ExportNullAraayBCV;
        else if (ExportNullAraayPBCV)
            return ExportNullAraayPBCV;
        else
            return false;
    }

    public bool GetExportNullDict(bool ExportNullDictECV, bool ExportNullDictBCV, bool ExportNullDictPBCV)
    {
        if (ExportNullDictECV)
            return ExportNullDictECV;
        else if (ExportNullDictBCV)
            return ExportNullDictBCV;
        else if (ExportNullDictPBCV)
            return ExportNullDictPBCV;
        else
            return false;
    }

    public bool GetExportNullJson(bool ExportNullJsonECV, bool ExportNullJsonBCV, bool ExportNullJsonPBCV)
    {
        if (ExportNullJsonECV)
            return ExportNullJsonECV;
        else if (ExportNullJsonBCV)
            return ExportNullJsonBCV;
        else if (ExportNullJsonPBCV)
            return ExportNullJsonPBCV;
        else
            return false;
    }

    public bool GetExportNullNumber(bool ExportNullNumberECV, bool ExportNullNumberBCV, bool ExportNullNumberPBCV)
    {
        if (ExportNullNumberECV)
            return ExportNullNumberECV;
        else if (ExportNullNumberBCV)
            return ExportNullNumberBCV;
        else if (ExportNullNumberPBCV)
            return ExportNullNumberPBCV;
        else
            return false;
    }

    public bool GetExportNullString(bool ExportNullStringECV, bool ExportNullStringBCV, bool ExportNullStringPBCV)
    {
        if (ExportNullStringECV)
            return ExportNullStringECV;
        else if (ExportNullStringBCV)
            return ExportNullStringBCV;
        else if (ExportNullStringPBCV)
            return ExportNullStringPBCV;
        else
            return false;
    }

    public string GetExportPath(string ExportPathECV, string ExportPathBCV, string ExportPathPBCV)
    {
        if (ExportPathECV !=null && ExportPathECV!="")
            return ExportPathECV;
        else if (ExportPathBCV != null && ExportPathBCV != "")
            return ExportPathBCV;
        else if (ExportPathPBCV != null && ExportPathPBCV != "")
            return ExportPathPBCV;
        else
            return "";
    }

    public string GetExportSpaceString(string ExportSpaceStringECV, string ExportSpaceStringBCV, string ExportSpaceStringPBCV)
    {
        if (ExportSpaceStringECV != null && ExportSpaceStringECV != "")
            return ExportSpaceStringECV;
        else if (ExportSpaceStringBCV != null && ExportSpaceStringBCV != "")
            return ExportSpaceStringBCV;
        else if (ExportSpaceStringPBCV != null && ExportSpaceStringPBCV != "")
            return ExportSpaceStringPBCV;
        else
            return "";
    }

    public string GetExportTopWords(string ExportTopWordsECV, string ExportTopWordsBCV, string ExportTopWordsPBCV)
    {
        if (ExportTopWordsECV != null && ExportTopWordsECV != "")
            return ExportTopWordsECV;
        else if (ExportTopWordsBCV != null && ExportTopWordsBCV != "")
            return ExportTopWordsBCV;
        else if (ExportTopWordsPBCV != null && ExportTopWordsPBCV != "")
            return ExportTopWordsPBCV;
        else
            return "";
    }

    public bool GetIsExport(bool isExporECV, bool isExporBCV, bool isExporPBCV)
    {
        if (isExporECV)
            return isExporECV;
        else if (isExporBCV)
            return isExporBCV;
        else if (isExporPBCV)
            return isExporPBCV;
        else
            return false;
    }

    public bool GetIsExportKeepDirectoryStructure(bool ExportKeepDirectoryStructureECV, bool ExportKeepDirectoryStructureBCV, bool ExportKeepDirectoryStructurePBCV)
    {
        if (ExportKeepDirectoryStructureECV)
            return ExportKeepDirectoryStructureECV;
        else if (ExportKeepDirectoryStructureBCV)
            return ExportKeepDirectoryStructureBCV;
        else if (ExportKeepDirectoryStructurePBCV)
            return ExportKeepDirectoryStructurePBCV;
        else
            return false;
    }
}

