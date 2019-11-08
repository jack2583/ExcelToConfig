using System;
using System.Collections.Generic;

public struct HrlStruct
{
    /// <summary>
    /// 存储本次要额外导出为Hrl文件的Excel文件名
    /// </summary>
    public static List<string> ExportTableNames = new List<string>();

    /// <summary>
    /// 导出Hrl文件的扩展名（不含点号），默认为Hrl
    /// </summary>
    public static string SaveExtension = "hrl";

    /// <summary>
    ///导出erl文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Export = "ExportHrl";

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：是否导出Hrl
    /// </summary>
    public const string Public_Config_IsExport = "IsExport";

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：是否需要导出Hrl
    /// </summary>
    public static bool IsExport = false;

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：导出lua的储存路径
    /// </summary>
    public const string Public_Config_ExportPath = "ExportPath";

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：保存lua文件路径
    /// </summary>
    public static string SavePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SaveHrl");

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：是否按原目录结构保存
    /// </summary>
    public const string Public_Config_IsExportKeepDirectoryStructure = "IsExportKeepDirectoryStructure";

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：是否将生成的文件按原Excel文件所在的目录结构进行存储（默认:是）
    /// </summary>
    public static bool IsExportKeepDirectoryStructure = true;

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：是否需要生成头部信息
    /// </summary>
    public const string Public_Config_IsNeedColumnInfo = "IsNeedColumnInfo";

    /// <summary>
    /// 导出Hrl文件配置，bat脚本：用户输入的是否需要在生成lua文件的最上方用注释形式显示列信息（默认为不需要）
    /// </summary>
    public static bool IsNeedColumnInfo = false;

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public const string Public_Config_ExportNameBeforeAdd = "ExportNameBeforeAdd";

    /// <summary>
    /// 是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public static string ExportNameBeforeAdd = "";

    /// <summary>
    /// 某个Excel表的配置：声明某张表格导出为Hrl table时，是否将主键列的值作为table中的元素
    /// </summary>
    public const string Excel_Config_AddKeyToHrlTable = "addKeyToHrlTable";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格不进行默认导出Hrl的参数配置
    /// </summary>
    public const string Excel_Config_NotExportHrlOriginalTable = "-NotExportHrlOriginalTable";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Hrl的参数配置
    /// </summary>
    public const string Excel_Config_ExportHrl = "ExportHrl";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Hrl的参数配置
    /// </summary>
    public static bool IsExportHrl = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Hrl的参数配置
    /// </summary>
    public const string Excel_Config_SpecialExportHrl = "SpecialExportHrl";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Hrl的参数配置
    /// </summary>
    public static bool IsSpecialExportHrl = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Hrl的，嵌套规则参数
    /// </summary>
    public static List<string> SpecialExportHrlParams;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Hrl时是否忽略int等数字类型中的空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Excel_Config_NotExportHrlNullNumber = "NotExportHrlNullNumber";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格导出Hrl的头部信息参数配置
    /// </summary>
    public const string Excel_Config_HrlTopInfo = "HrlTopInfo";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格导出Hrl的头部信息参数配置
    /// </summary>
    public static string HrlTopInfo = "%%--- coding:utf-8 ---\n-ifndef('ERRCODE_HRL').\n-define('ERRCODE_HRL', true).";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格导出Hrl的头部信息参数配置
    /// </summary>
    public const string Excel_Config_HrlEndInfo = "HrlEndInfo";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格导出Hrl的头部信息参数配置
    /// </summary>
    public static string HrlEndInfo = "-endif.";
}