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
    /// 导出路径，如：C:\Users\Administrator\Desktop
    /// </summary>
    string ExportPath { set; get; }
    /// <summary>
    /// 导出时是否按原目录结构保存
    /// </summary>
    bool IsExportKeepDirectoryStructure { set; get; }
    /// <summary>
    /// 导出名字，如：item
    /// </summary>
    string ExportName { set; get; }
    /// <summary>
    /// 导出后缀，如：json;erl
    /// </summary>
    string ExportExtension { set; get; }
    /// <summary>
    /// 导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAdd { set; get; }
    /// <summary>
    /// 导出文本
    /// </summary>
    string ExportContent { set; get; }

    /// <summary>
    /// 获取最终导出的保存路径+文件名+后缀
    /// </summary>
    /// <returns></returns>
    string GetSavePath();

}
/// <summary>
/// 导出设置
/// </summary>
interface IExportSetting
{
    /// <summary>
    /// int long float为空时是否导出
    /// </summary>
    bool ExportNullNumber { set; get; }
    /// <summary>
    /// string为空时是否导出
    /// </summary>
    bool ExportNullString { set; get; }
    /// <summary>
    /// json为空时是否导出
    /// </summary>
    bool ExportNullJson { set; get; }
    /// <summary>
    /// array类型为空（-1）时是否导出
    /// </summary>
    bool ExportNullArray { set; get; }
    /// <summary>
    /// dict为空（-1）时是否导出
    /// </summary>
    bool ExportNullDict { set; get; }
    /// <summary>
    /// 导出时是否需要格式化
    /// </summary>
    bool ExportFormat { set; get; }
    /// <summary>
    /// 导出时在文件中是否需要生成字段注释信息
    /// </summary>
    bool ExportFieldComment { set; get; }
    /// <summary>
    /// 导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWords { set; get; }

    /// <summary>
    /// 导出时的缩进符
    /// </summary>
    string ExportIndentationString { set; get; }
    /// <summary>
    /// 导出时的间隔符
    /// </summary>
    string ExportSpaceString { set; get; }

}
/// <summary>
/// Excel中config参数接口
/// </summary>
interface IExcelConfigSetting
{
    /// <summary>
    /// 参数名：是否需要导出，如果为空则向上找对应参数，顺序为：IExcelConfigSetting设置>IBatExportSetting等设置>IBatExportPublicSetting公共设置
    /// </summary>
    string IsExportECP { set; get; }
    /// <summary>
    /// 参数值：是否需要导出
    /// </summary>
    bool isExporECV { set; get; }

    /// <summary>
    /// 参数名：导出路径
    /// </summary>
    string ExportPathECP { set; get; }
    /// <summary>
    /// 参数值：导出路径
    /// </summary>
    string ExportPathECV { set; get; }

    /// <summary>
    /// 参数名：导出时是否按原目录结构保存
    /// </summary>
    string ExportKeepDirectoryStructureECP { set; get; }
    /// <summary>
    /// 参数值：导出时是否按原目录结构保存
    /// </summary>
    string ExportKeepDirectoryStructureECV { set; get; }

    /// <summary>
    /// 参数名：导出名字,如 item
    /// </summary>
    string ExportNameECP { set; get; }
    /// <summary>
    /// 参数值：导出名字
    /// </summary>
    string ExportNameECV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddECP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddECV { set; get; }

    /// <summary>
    /// 参数名：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberECP { set; get; }
    /// <summary>
    /// 参数值：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberECV { set; get; }

    /// <summary>
    /// 参数名：String为空时是否导出
    /// </summary>
    string ExportNullStringECP { set; get; }
    /// <summary>
    /// 参数值：String为空时是否导出
    /// </summary>
    string ExportNullStringECV { set; get; }

    /// <summary>
    /// 参数名：Json为空时是否导出
    /// </summary>
    string ExportNullJsonECP { set; get; }
    /// <summary>
    /// 参数值：Json为空时是否导出
    /// </summary>
    string ExportNullJsonECV { set; get; }

    /// <summary>
    /// 参数名：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayECP { set; get; }
    /// <summary>
    /// 参数值：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayECV { set; get; }
    /// <summary>
    /// 参数名：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictECP { set; get; }
    /// <summary>
    /// 参数值：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictECV { set; get; }

    /// <summary>
    ///  参数名：导出时是否需要格式化
    /// </summary>
    string ExportFormatECP { set; get; }
    /// <summary>
    ///  参数值：导出时是否需要格式化
    /// </summary>
    string ExportFormatECV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentECP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentECV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWordsECP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWordsECV { set; get; }

    /// <summary>
    /// 参数名：导出时的缩进符
    /// </summary>
    string ExportIndentationStringECP { set; get; }
    /// <summary>
    /// 参数值：导出时的缩进符
    /// </summary>
    string ExportIndentationStringECV { set; get; }

    /// <summary>
    /// 参数名：导出时的间隔符
    /// </summary>
    string ExportSpaceStringECP { set; get; }
    /// <summary>
    /// 参数值：导出时的间隔符
    /// </summary>
    string ExportSpaceStringECV { set; get; }
}
/// <summary>
/// 某个导出类型（json，elang等）BAT导出设置
/// </summary>
interface IBatExportSetting
{
    /// <summary>
    /// 参数名，如：ExportJson
    /// </summary>
    string ExportTypeB { set; get; }
    /// <summary>
    /// 参数名：是否需要导出，如果为空则向上找对应参数，顺序为：IExcelConfigSetting设置>IBatExportSetting等设置>IBatExportPublicSetting公共设置
    /// </summary>
    string IsExportBCP { set; get; }
    /// <summary>
    /// 参数值：是否需要导出
    /// </summary>
    bool isExporBCV { set; get; }

    /// <summary>
    /// 参数名：导出路径
    /// </summary>
    string ExportPathBCP { set; get; }
    /// <summary>
    /// 参数值：导出路径
    /// </summary>
    string ExportPathBCV { set; get; }

    /// <summary>
    /// 参数名：导出时是否按原目录结构保存
    /// </summary>
    string ExportKeepDirectoryStructureBCP { set; get; }
    /// <summary>
    /// 参数值：导出时是否按原目录结构保存
    /// </summary>
    string ExportKeepDirectoryStructureBCV { set; get; }

    /// <summary>
    /// 参数名：导出名字,如 item
    /// </summary>
    string ExportNameBCP { set; get; }
    /// <summary>
    /// 参数值：导出名字
    /// </summary>
    string ExportNameBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddBCV { set; get; }

    /// <summary>
    /// 参数名：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberBCP { set; get; }
    /// <summary>
    /// 参数值：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberBCV { set; get; }

    /// <summary>
    /// 参数名：String为空时是否导出
    /// </summary>
    string ExportNullStringBCP { set; get; }
    /// <summary>
    /// 参数值：String为空时是否导出
    /// </summary>
    string ExportNullStringBCV { set; get; }

    /// <summary>
    /// 参数名：Json为空时是否导出
    /// </summary>
    string ExportNullJsonBCP { set; get; }
    /// <summary>
    /// 参数值：Json为空时是否导出
    /// </summary>
    string ExportNullJsonBCV { set; get; }

    /// <summary>
    /// 参数名：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayBCP { set; get; }
    /// <summary>
    /// 参数值：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayBCV { set; get; }
    /// <summary>
    /// 参数名：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictBCP { set; get; }
    /// <summary>
    /// 参数值：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictBCV { set; get; }

    /// <summary>
    ///  参数名：导出时是否需要格式化
    /// </summary>
    string ExportFormatBCP { set; get; }
    /// <summary>
    ///  参数值：导出时是否需要格式化
    /// </summary>
    string ExportFormatBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWordsBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWordsBCV { set; get; }

    /// <summary>
    /// 参数名：导出时的缩进符
    /// </summary>
    string ExportIndentationStringBCP { set; get; }
    /// <summary>
    /// 参数值：导出时的缩进符
    /// </summary>
    string ExportIndentationStringBCV { set; get; }

    /// <summary>
    /// 参数名：导出时的间隔符
    /// </summary>
    string ExportSpaceStringBCP { set; get; }
    /// <summary>
    /// 参数值：导出时的间隔符
    /// </summary>
    string ExportSpaceStringBCV { set; get; }
}
interface IBatExportPublicSetting
{
    /// <summary>
    /// 参数名，如：PublicSetting
    /// </summary>
    string ExportTypeP { set; get; }
    /// <summary>
    /// 参数名：是否需要导出，如果为空则向上找对应参数，顺序为：IExcelConfigSetting设置>IBatExportSetting等设置>IBatExportPublicSetting公共设置
    /// </summary>
    string IsExportPBCP { set; get; }
    /// <summary>
    /// 参数值：是否需要导出
    /// </summary>
    bool isExporPBCV { set; get; }

    /// <summary>
    /// 参数名：导出路径
    /// </summary>
    string ExportPathPBCP { set; get; }
    /// <summary>
    /// 参数值：导出路径
    /// </summary>
    string ExportPathPBCV { set; get; }

    /// <summary>
    /// 参数名：导出时是否按原目录结构保存
    /// </summary>
    string ExportKeepDirectoryStructurePBCP { set; get; }
    /// <summary>
    /// 参数值：导出时是否按原目录结构保存
    /// </summary>
    string ExportKeepDirectoryStructurePBCV { set; get; }

    /// <summary>
    /// 参数名：导出名字,如 item
    /// </summary>
    string ExportNamePBCP { set; get; }
    /// <summary>
    /// 参数值：导出名字
    /// </summary>
    string ExportNamePBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddPBCV { set; get; }

    /// <summary>
    /// 参数名：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberPBCP { set; get; }
    /// <summary>
    /// 参数值：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberPBCV { set; get; }

    /// <summary>
    /// 参数名：String为空时是否导出
    /// </summary>
    string ExportNullStringPBCP { set; get; }
    /// <summary>
    /// 参数值：String为空时是否导出
    /// </summary>
    string ExportNullStringPBCV { set; get; }

    /// <summary>
    /// 参数名：Json为空时是否导出
    /// </summary>
    string ExportNullJsonPBCP { set; get; }
    /// <summary>
    /// 参数值：Json为空时是否导出
    /// </summary>
    string ExportNullJsonPBCV { set; get; }

    /// <summary>
    /// 参数名：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayPBCP { set; get; }
    /// <summary>
    /// 参数值：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayPBCV { set; get; }
    /// <summary>
    /// 参数名：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictPBCP { set; get; }
    /// <summary>
    /// 参数值：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictPBCV { set; get; }

    /// <summary>
    ///  参数名：导出时是否需要格式化
    /// </summary>
    string ExportFormatPBCP { set; get; }
    /// <summary>
    ///  参数值：导出时是否需要格式化
    /// </summary>
    string ExportFormatPBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentPBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWordsPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWordsPBCV { set; get; }

    /// <summary>
    /// 参数名：导出时的缩进符
    /// </summary>
    string ExportIndentationStringPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时的缩进符
    /// </summary>
    string ExportIndentationStringPBCV { set; get; }

    /// <summary>
    /// 参数名：导出时的间隔符
    /// </summary>
    string ExportSpaceStringPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时的间隔符
    /// </summary>
    string ExportSpaceStringPBCV { set; get; }
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