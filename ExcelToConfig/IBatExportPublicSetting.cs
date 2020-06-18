using System;
using System.Collections.Generic;
using System.Text;


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
    bool ExportKeepDirectoryStructurePBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddPBCV { set; get; }
    /// <summary>
    /// 参数名：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMarkPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMarkPBCV { set; get; }
    /// <summary>
    /// 参数名：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberPBCP { set; get; }
    /// <summary>
    /// 参数值：int long float为空时是否导出
    /// </summary>
    bool ExportNullNumberPBCV { set; get; }

    /// <summary>
    /// 参数名：String为空时是否导出
    /// </summary>
    string ExportNullStringPBCP { set; get; }
    /// <summary>
    /// 参数值：String为空时是否导出
    /// </summary>
    bool ExportNullStringPBCV { set; get; }

    /// <summary>
    /// 参数名：Json为空时是否导出
    /// </summary>
    string ExportNullJsonPBCP { set; get; }
    /// <summary>
    /// 参数值：Json为空时是否导出
    /// </summary>
    bool ExportNullJsonPBCV { set; get; }

    /// <summary>
    /// 参数名：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullAraayPBCP { set; get; }
    /// <summary>
    /// 参数值：Araay为空(-1)时是否导出
    /// </summary>
    bool ExportNullAraayPBCV { set; get; }
    /// <summary>
    /// 参数名：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictPBCP { set; get; }
    /// <summary>
    /// 参数值：Dict为空(-1)时是否导出
    /// </summary>
    bool ExportNullDictPBCV { set; get; }

    /// <summary>
    ///  参数名：导出时是否需要格式化
    /// </summary>
    string ExportFormatPBCP { set; get; }
    /// <summary>
    ///  参数值：导出时是否需要格式化
    /// </summary>
    bool ExportFormatPBCV { set; get; }

    /// <summary>
    /// 参数名：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentPBCP { set; get; }
    /// <summary>
    /// 参数值：导出时在文件中生成字段注释信息
    /// </summary>
    bool ExportFieldCommentPBCV { set; get; }

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
