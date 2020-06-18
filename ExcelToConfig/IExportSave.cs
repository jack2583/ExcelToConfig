using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 导出保存公共接口
/// </summary>
interface IExportSave
{
    /// <summary>
    /// 是否需要导出
    /// </summary>
    bool IsExport { set; get; }
    /// <summary>
    /// 获取某个表最终是否需要导出
    /// </summary>
    /// <param name="isExporECV">Excel中config设置取得的值（最高级判断）</param>
    /// <param name="isExporBCV">Bat导出类型设置取得的值，如ExportJson中的值</param>
    /// <param name="isExporPBCV">Bat公共设置中的取值</param>
    /// <returns></returns>
    bool GetIsExport(bool isExporECV, bool isExporBCV, bool isExporPBCV);
    /// <summary>
    /// 导出路径，如：C:\Users\Administrator\Desktop
    /// </summary>
    string ExportPath { set; get; }
    string GetExportPath(string ExportPathECV, string ExportPathBCV, string ExportPathPBCV);
    /// <summary>
    /// 导出时是否按原目录结构保存
    /// </summary>
    bool IsExportKeepDirectoryStructure { set; get; }
    bool GetIsExportKeepDirectoryStructure(bool ExportKeepDirectoryStructureECV, bool ExportKeepDirectoryStructureBCV, bool ExportKeepDirectoryStructurePBCV);
    /// <summary>
    /// 导出名字，如：item
    /// </summary>
    string ExportName { set; get; }
    /// <summary>
    /// 获取导出时的表名
    /// </summary>
    /// <param name="ExportNameECV">Excel中config中设置的表名</param>
    /// <param name="ExcelName">Excel文件的名字</param>
    /// <param name="ExcelNameSplitString">Excel文件的名字中分割符，然后取第1部分为导出表名</param>
    /// <returns></returns>
    string GetExportName(string ExportNameECV, string ExcelName,string ExcelNameSplitString);
    /// <summary>
    /// 导出后缀，如：json;erl
    /// </summary>
    string ExportExtension { set; get; }
    /// <summary>
    /// 导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAdd { set; get; }
    string GetExportNameBeforeAdd(string ExportNameBeforeAddECV, string ExportNameBeforeAddBCV, string ExportNameBeforeAddPBCV);
    /// <summary>
    /// 导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMark { set; get; }
    string GetExportNameAfterLanguageMark(string ExportNameAfterLanguageMarkECV, string ExportNameAfterLanguageMarkBCV, string ExportNameAfterLanguageMarkPBCV);
    /// <summary>
    /// 导出文本
    /// </summary>
    string ExportContent { set; get; }
}





interface IBatExportMergeSetting
{
    /// <summary>
    /// 参数名，如：MergeTable
    /// </summary>
    string ExportMergeTableP { set; get; }
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
interface IBatExportAppLogSetting
{
    /// <summary>
    /// 参数名，如：AppLog
    /// </summary>
    string ExportAppLogP { set; get; }
}