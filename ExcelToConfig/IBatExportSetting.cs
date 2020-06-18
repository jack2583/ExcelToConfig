using System;
using System.Collections.Generic;
using System.Text;

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
    bool ExportKeepDirectoryStructureBCV { set; get; }

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
    /// 参数名：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMarkBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMarkBCV { set; get; }
    /// <summary>
    /// 参数名：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberBCP { set; get; }
    /// <summary>
    /// 参数值：int long float为空时是否导出
    /// </summary>
    bool ExportNullNumberBCV { set; get; }

    /// <summary>
    /// 参数名：String为空时是否导出
    /// </summary>
    string ExportNullStringBCP { set; get; }
    /// <summary>
    /// 参数值：String为空时是否导出
    /// </summary>
    bool ExportNullStringBCV { set; get; }

    /// <summary>
    /// 参数名：Json为空时是否导出
    /// </summary>
    string ExportNullJsonBCP { set; get; }
    /// <summary>
    /// 参数值：Json为空时是否导出
    /// </summary>
    bool ExportNullJsonBCV { set; get; }

    /// <summary>
    /// 参数名：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayBCP { set; get; }
    /// <summary>
    /// 参数值：Araay为空(-1)时是否导出
    /// </summary>
    bool ExportNullAraayBCV { set; get; }
    /// <summary>
    /// 参数名：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictBCP { set; get; }
    /// <summary>
    /// 参数值：Dict为空(-1)时是否导出
    /// </summary>
    bool ExportNullDictBCV { set; get; }

    /// <summary>
    ///  参数名：导出时是否需要格式化
    /// </summary>
    string ExportFormatBCP { set; get; }
    /// <summary>
    ///  参数值：导出时是否需要格式化
    /// </summary>
    bool ExportFormatBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件中生成字段注释信息
    /// </summary>
    bool ExportFieldCommentBCV { set; get; }

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
