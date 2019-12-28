using System;
using System.Collections.Generic;

public struct JsonStruct
{
    /// <summary>
    /// 存储本次要额外导出为json文件的Excel文件名
    /// </summary>
    public static List<string> ExportTableNames = new List<string>();

    /// <summary>
    /// 导出json文件的扩展名（不含点号），默认为json
    /// </summary>
    public static string SaveExtension = "json";

    /// <summary>
    /// 导出json文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Export = "ExportJson";

    /// <summary>
    /// 导出json文件配置，bat脚本：是否导出Json
    /// </summary>
    public const string Public_Config_IsExport = "IsExport";

    /// <summary>
    /// 导出json文件配置，bat脚本：是否需要导出json
    /// </summary>
    public static bool IsExport = false;

    /// <summary>
    /// 导出json文件配置，bat脚本：导出json的储存路径
    /// </summary>
    public const string Public_Config_ExportPath = "ExportPath";

    /// <summary>
    /// 导出json文件配置，bat脚本：保存JSON文件路径
    /// </summary>
    public static string SavePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SaveJSON");

    /// <summary>
    /// 导出json文件配置，bat脚本：是否按原目录结构保存
    /// </summary>
    public const string Public_Config_IsExportKeepDirectoryStructure = "IsExportKeepDirectoryStructure";

    /// <summary>
    /// 是否将生成的文件按原Excel文件所在的目录结构进行存储（默认:是）
    /// </summary>
    public static bool IsExportKeepDirectoryStructure = false;

    /// <summary>
    /// 导出json文件配置，bat脚本：导出的json文件是否生成为各行数据对应的json object包含在一个json array的形式，默认为否
    /// </summary>
    public const string Public_Config_IsArrayFormat = "IsArrayFormat";

    /// <summary>
    /// 导出的json文件是否生成为各行数据对应的json object包含在一个json array的形式，默认为否
    /// </summary>
    public static bool ExportJsonIsExportJsonArrayFormat = false;

    /// <summary>
    /// 导出json文件配置，bat脚本：导出的json文件，若生成包含在一个json object的形式，是否使每行字段信息对应的json object中包含主键列对应的键值对，默认为是
    /// </summary>
    public const string Public_Config_IsKeyColumnValue = "IsKeyColumnValue";

    /// <summary>
    /// 导出的json文件，若生成包含在一个json object的形式，是否使每行字段信息对应的json object中包含主键列对应的键值对，默认为是
    /// </summary>
    public static bool ExportJsonIsExportJsonMapIncludeKeyColumnValue = false;

    /// <summary>
    /// 导出json文件配置，bat脚本：导出的json文件中是否将json字符串整理为带缩进格式的形式，默认为是
    /// </summary>
    public const string Public_Config_IsFormat = "IsFormat";

    /// <summary>
    /// 导出的json文件中是否将json字符串整理为带缩进格式的形式，默认为否
    /// </summary>
    public static bool ExportJsonIsFormat = false;

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public const string Public_Config_ExportNameBeforeAdd = "ExportNameBeforeAdd";

    /// <summary>
    /// 是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public static string ExportNameBeforeAdd = "";

    /// <summary>
    ///  导出lua文件配置，bat脚本：声明对某张表格设置导出Lua时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Public_Config_NotExportJsonNull = "ExportJsonNullConfig";

    /// <summary>
    ///  导出lua文件配置，bat脚本：声明对某张表格设置导出json时空值显示为null还是[] 默认不忽略.false/true
    /// </summary>
    public const string Public_Config_JsonNullType = "JsonNullType";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置特殊导出Json规则的配置参数名
    /// </summary>
   // public const string Excel_Config_SpecialExportJson = "tableExportConfig";//"TableSpecialExportJson";
    /// <summary>
    /// 某个Excel表的配置：声明对某张表格不进行默认导出Json的参数配置
    /// </summary>
    public const string Excel_Config_NotExportJsonOriginalTable = "-notExportOriginalTable";//"NotExportJsonOriginalTable"

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Json的参数配置
    /// </summary>
    public const string Excel_Config_ExportJson = "ExportJson";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Json的参数配置
    /// </summary>
    public static bool IsExportJson = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Json的参数配置
    /// </summary>
    public const string Excel_Config_SpecialExportJson = "SpecialExportJson";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Lua的参数配置
    /// </summary>
    public static bool IsSpecialExportJson = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Json的，嵌套规则参数
    /// </summary>
    public static List<string> SpecialExportJsonParams;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Json时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Excel_Config_NotExportJsonNull = "ExportJsonNullConfig";//NotExportJsonNullNumber

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Json时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public static bool IsExportJsonNullConfig = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Json时为空值，显示为null 还是[]
    /// </summary>
    public static bool IsJsonNullType = true;

    /// <summary>
    /// 某个Excel表的配置：声明某张表格导出为lua table时，是否将主键列的值作为table中的元素
    /// </summary>
    public const string Excel_Config_AddKeyToJsonTable = "addKeyToLuaTable";//AddKeyToJsonTable
}