using System;
using System.Collections.Generic;

public struct TxtStruct
{
    /// <summary>
    /// 存储本次要额外导出为txt文件的Excel文件名
    /// </summary>
    public static List<string> ExportTxtTableNames = new List<string>();

    /// <summary>
    /// 导出txt文件的扩展名（不含点号），默认为txt
    /// </summary>
    public static string SaveExtension = "txt";

    /// <summary>
    ///导出txt文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Export = "ExportTxt";

    /// <summary>
    /// 导出txt文件配置，bat脚本：是否导出erlang
    /// </summary>
    public const string Public_Config_IsExport = "IsExport";

    /// <summary>
    /// 导出txt文件配置，bat脚本：是否需要导出erlang
    /// </summary>
    public static bool IsExport = false;

    /// <summary>
    /// 导出txt文件配置，bat脚本：导出lua的储存路径
    /// </summary>
    public const string Public_Config_ExportPath = "ExportPath";

    /// <summary>
    /// 保存txt文件路径
    /// </summary>
    public static string SavePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SaveTxt");

    /// <summary>
    /// 导出txt文件配置，bat脚本：是否按原目录结构保存
    /// </summary>
    public const string Public_Config_IsExportKeepDirectoryStructure = "IsExportKeepDirectoryStructure";

    /// <summary>
    /// 导出txt文件配置，bat脚本：是否将生成的文件按原Excel文件所在的目录结构进行存储（默认:是）
    /// </summary>
    public static bool IsExportKeepDirectoryStructure = true;

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public const string Public_Config_ExportNameBeforeAdd = "ExportNameBeforeAdd";

    /// <summary>
    /// 是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public static string ExportNameBeforeAdd = "";
    /// <summary>
    /// App全局导出通用配置，bat脚本:是否config配置
    /// </summary>
    public const string Public_Config_IsExportConfig = "IsExportConfig";
    /// <summary>
    /// App全局导出通用配置,是否允是否config配置
    /// </summary>
    public static bool IsExportConfig = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Txt的参数配置
    /// </summary>
    public const string Excel_Config_ExportTxt = "ExportTxt";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Txt的参数配置
    /// </summary>
    public static bool IsExportTxt = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Txt的参数配置
    /// </summary>
    public const string Excel_Config_SpecialExportTxt = "SpecialExportTxt";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Txt的参数配置
    /// </summary>
    public static bool IsSpecialExportTxt = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Txt的，嵌套规则参数
    /// </summary>
    public static List<string> SpecialExportTxtParams;

    /// <summary>
    /// 当lang型数据key在lang文件中找不到对应值时，是否在txt文件输出字段值为空字符（默认为输出空）
    /// </summary>
    public static bool IsPrintEmptyStringWhenLangNotMatching = false;

    /// <summary>
    /// 导出txt文件中的字段分隔符，默认为Tab键
    /// </summary>
    public static char ExportSpaceString = '\t';

    /// <summary>
    ///  导出txt文件中的换行符，默认为\n键
    /// </summary>
    public static char ExportLineString = '\n';
}