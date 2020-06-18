using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 导出设置
/// </summary>
interface IExportSetting
{
    /// <summary>
    /// int long float为空时是否导出
    /// </summary>
    bool ExportNullNumber { set; get; }
    bool GetExportNullNumber(bool ExportNullNumberECV, bool ExportNullNumberBCV, bool ExportNullNumberPBCV);
    /// <summary>
    /// string为空时是否导出
    /// </summary>
    bool ExportNullString { set; get; }
    bool GetExportNullString(bool ExportNullStringECV, bool ExportNullStringBCV, bool ExportNullStringPBCV);
    /// <summary>
    /// json为空时是否导出
    /// </summary>
    bool ExportNullJson { set; get; }
    bool GetExportNullJson(bool ExportNullJsonECV, bool ExportNullJsonBCV, bool ExportNullJsonPBCV);
    /// <summary>
    /// array类型为空（-1）时是否导出
    /// </summary>
    bool ExportNullArray { set; get; }
    bool GetExportNullArray(bool ExportNullAraayECV, bool ExportNullAraayBCV, bool ExportNullAraayPBCV);
    /// <summary>
    /// dict为空（-1）时是否导出
    /// </summary>
    bool ExportNullDict { set; get; }
    bool GetExportNullDict(bool ExportNullDictECV, bool ExportNullDictBCV, bool ExportNullDictPBCV);
    /// <summary>
    /// 导出时是否需要格式化
    /// </summary>
    bool ExportFormat { set; get; }
    bool GetExportFormat(bool ExportFormatECV, bool ExportFormatBCV, bool ExportFormatPBCV);
    /// <summary>
    /// 导出时在文件中是否需要生成字段注释信息
    /// </summary>
    bool ExportFieldComment { set; get; }
    bool GetExportFieldComment(bool ExportFieldCommentECV, bool ExportFieldCommentBCV, bool ExportFieldCommentPBCV);
    /// <summary>
    /// 导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWords { set; get; }
    string GetExportTopWords(string ExportTopWordsECV, string ExportTopWordsBCV, string ExportTopWordsPBCV);
    /// <summary>
    /// 导出时的缩进符
    /// </summary>
    string ExportIndentationString { set; get; }
    string GetExportIndentationString(string ExportIndentationStringECV, string ExportIndentationStringBCV, string ExportIndentationStringPBCV);
    /// <summary>
    /// 导出时的间隔符
    /// </summary>
    string ExportSpaceString { set; get; }
    string GetExportSpaceString(string ExportSpaceStringECV, string ExportSpaceStringBCV, string ExportSpaceStringPBCV);

}
