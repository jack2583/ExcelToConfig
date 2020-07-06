using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 导出保存公共接口
/// </summary>
interface IExportSave
{
    /// <summary>
    /// 获取导出时的表名
    /// </summary>
    /// <param name="excelSetting">Excel中config中设置的表名</param>
    /// <param name="ExcelName">Excel文件的名字</param>
    /// <param name="ExcelNameSplitString">Excel文件的名字中分割符，然后取第1部分为导出表名</param>
    /// <returns></returns>
   // void GetExportName(string excelSetting, string ExcelName,string ExcelNameSplitString);
    void GetParamValue(TableInfo tableInfo);
    /// <summary>
    /// 导出文本
    /// </summary>
    string ExportContent { set; get; }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <returns></returns>
    bool SaveFile(string ExcelName);
}



interface IBatExportMoreLanguageSetting
{
    /// <summary>
    /// 参数名，如：MoreLanguage
    /// </summary>
    string ExportMoreLanguageP { set; get; }
}
interface IBatExportLangSetting
{
    /// <summary>
    /// 参数名，如：Lang
    /// </summary>
    string ExportLangP { set; get; }
}
