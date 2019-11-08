using System;
using System.Collections.Generic;

public struct ErlangStruct
{
    /// <summary>
    /// 存储本次要额外导出为erlang文件的Excel文件名
    /// </summary>
    public static List<string> ExportTableNames = new List<string>();

    /// <summary>
    /// 导出erlang文件的扩展名（不含点号），默认为erlang
    /// </summary>
    public static string SaveExtension = "erl";

    /// <summary>
    ///导出erl文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Export = "ExportErlang";

    /// <summary>
    /// 导出erlang文件配置，bat脚本：是否导出erlang
    /// </summary>
    public const string Public_Config_IsExport = "IsExport";

    /// <summary>
    /// 导出erlang文件配置，bat脚本：是否需要导出erlang
    /// </summary>
    public static bool IsExport = false;

    /// <summary>
    /// 导出erlang文件配置，bat脚本：导出lua的储存路径
    /// </summary>
    public const string Public_Config_ExportPath = "ExportPath";

    /// <summary>
    /// 导出erlang文件配置，bat脚本：保存lua文件路径
    /// </summary>
    public static string SavePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SaveErlang");

    /// <summary>
    /// 导出erlang文件配置，bat脚本：是否按原目录结构保存
    /// </summary>
    public const string Public_Config_IsExportKeepDirectoryStructure = "IsExportKeepDirectoryStructure";

    /// <summary>
    /// 导出erlang文件配置，bat脚本：是否将生成的文件按原Excel文件所在的目录结构进行存储（默认:是）
    /// </summary>
    public static bool IsExportKeepDirectoryStructure = false;

    /// <summary>
    /// 导出Erlang文件配置，bat脚本：数组类型是否使用[1]=值  格式，默认是
    /// </summary>
    public const string Public_Config_IsArrayFieldName = "IsArrayFieldName";

    /// <summary>
    ///数组类型是否使用[1]=值  格式，默认是
    /// </summary>
    public static bool IsArrayFieldName = false;

    /// <summary>
    /// 导出erlang文件配置，bat脚本：是否需要生成头部信息
    /// </summary>
    public const string Public_Config_IsNeedColumnInfo = "IsNeedColumnInfo";

    /// <summary>
    /// 导出erlang文件配置，bat脚本：用户输入的是否需要在生成lua文件的最上方用注释形式显示列信息（默认为不需要）
    /// </summary>
    public static bool IsNeedColumnInfo = false;

    /// <summary>
    /// 导出erlang文件配置，bat脚本：导出的lua文件中是否将erlang字符串整理为带缩进格式的形式，默认为是
    /// </summary>
    public const string Public_Config_IsFormat = "IsFormat";

    /// <summary>
    /// 导出的erlang文件中是否将erlang字符串整理为带缩进格式的形式，默认为否
    /// </summary>
    public static bool ExportErlangIsFormat = true;

    /// <summary>
    /// 导出erlang文件配置，bat脚本：用于缩进erlang table的字符串
    /// </summary>
    public const string Public_Config_IndentationString = "IndentationString";

    /// <summary>
    /// 导出erlang文件配置，bat脚本：用于缩进erlang table的字符串
    /// </summary>
    public static string IndentationString = "    ";

    /// <summary>
    /// 导出erlang文件配置，bat脚本：是否在生成的文件名前加上前缀，如：tb_
    /// </summary>
    public const string Public_Config_ExportNameBeforeAdd = "ExportNameBeforeAdd";

    /// <summary>
    /// 是否在生成的文件名前加上前缀，如：tb_
    /// </summary>
    public static string ExportNameBeforeAdd = "";

    /// <summary>
    ///  导出lua文件配置，bat脚本：声明对某张表格设置导出Lua时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Public_Config_NotExportErlangNull = "ExportErlangNullConfig";

    /// <summary>
    /// 某个Excel表的配置：声明某张表格导出为erlang table时，是否将主键列的值作为table中的元素
    /// </summary>
    public const string Excel_Config_AddKeyToErlangTable = "addKeyToErlangTable";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格不进行默认导出erlang的参数配置
    /// </summary>
    public const string Excel_Config_NotExportErlangOriginalTable = "-NotExportErlangOriginalTable";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Erlang的参数配置
    /// </summary>
    public const string Excel_Config_ExportErlang = "ExportErlang";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Erlang的参数配置
    /// </summary>
    public static bool IsExportErlang = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Erlang的参数配置
    /// </summary>
    public const string Excel_Config_SpecialExportErlang = "SpecialExportErlang";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Erlang的参数配置
    /// </summary>
    public static bool IsSpecialExportErlang = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Erlang的，嵌套规则参数
    /// </summary>
    public static List<string> SpecialExportErlangParams;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Erlang时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Excel_Config_NotExportErlangNull = "ExportErlangNullConfig";//NotExportJsonNullNumber

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Erlang时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public static bool IsExportErlangNullConfig = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Erlang时是否忽略int等数字类型中的空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Excel_Config_NotExportErlangNullNumber = "NotExportErlangNullNumber";
}