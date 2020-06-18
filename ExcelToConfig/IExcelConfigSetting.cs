using System;
using System.Collections.Generic;
using System.Text;

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
    bool ExportKeepDirectoryStructureECV { set; get; }

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
    /// 参数名：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMarkECP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMarkECV { set; get; }
    /// <summary>
    /// 参数名：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberECP { set; get; }
    /// <summary>
    /// 参数值：int long float为空时是否导出
    /// </summary>
    bool ExportNullNumberECV { set; get; }

    /// <summary>
    /// 参数名：String为空时是否导出
    /// </summary>
    string ExportNullStringECP { set; get; }
    /// <summary>
    /// 参数值：String为空时是否导出
    /// </summary>
    bool ExportNullStringECV { set; get; }

    /// <summary>
    /// 参数名：Json为空时是否导出
    /// </summary>
    string ExportNullJsonECP { set; get; }
    /// <summary>
    /// 参数值：Json为空时是否导出
    /// </summary>
    bool ExportNullJsonECV { set; get; }

    /// <summary>
    /// 参数名：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayECP { set; get; }
    /// <summary>
    /// 参数值：Araay为空(-1)时是否导出
    /// </summary>
    bool ExportNullAraayECV { set; get; }
    /// <summary>
    /// 参数名：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictECP { set; get; }
    /// <summary>
    /// 参数值：Dict为空(-1)时是否导出
    /// </summary>
    bool ExportNullDictECV { set; get; }

    /// <summary>
    ///  参数名：导出时是否需要格式化
    /// </summary>
    string ExportFormatECP { set; get; }
    /// <summary>
    ///  参数值：导出时是否需要格式化
    /// </summary>
    bool ExportFormatECV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentECP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件中生成字段注释信息
    /// </summary>
    bool ExportFieldCommentECV { set; get; }

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

    /*---------------------------------*/
    /// <summary>
    /// 参数名：导出时合并到指定表。同一个表只能合并一个，不能根据json erlang等分别合并
    /// </summary>
    string ExportMergeToTableECP { set; get; }
    /// <summary>
    /// 参数值：导出时合并到指定表
    /// </summary>
    string ExportMergeToTableECV { set; get; }


    void GetExcelConfigValue(TableInfo tableInfo);
}
