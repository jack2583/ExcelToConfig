using System;
using System.Collections.Generic;

public struct LuaStruct
{
    /// <summary>
    /// 存储本次要额外导出为lua文件的Excel文件名
    /// </summary>
    public static List<string> ExportTableNames = new List<string>();

    /// <summary>
    /// 导出lua文件的扩展名（不含点号），默认为lua
    /// </summary>
    public static string SaveExtension = "lua";

    /// <summary>
    ///导出lua文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Export = "ExportLua";

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否导出Lua
    /// </summary>
    public const string Public_Config_IsExport = "IsExport";

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否需要导出lua
    /// </summary>
    public static bool IsExport = false;

    /// <summary>
    /// 导出lua文件配置，bat脚本：导出lua的储存路径
    /// </summary>
    public const string Public_Config_ExportPath = "ExportPath";

    /// <summary>
    /// 导出lua文件配置，bat脚本：保存lua文件路径
    /// </summary>
    public static string SavePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SaveLua");

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否按原目录结构保存
    /// </summary>
    public const string Public_Config_IsExportKeepDirectoryStructure = "IsExportKeepDirectoryStructure";

    /// <summary>
    /// 是否将生成的文件按原Excel文件所在的目录结构进行存储（默认:是）
    /// </summary>
    public static bool IsExportKeepDirectoryStructure = true;

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否需要生成头部信息
    /// </summary>
    public const string Public_Config_IsNeedColumnInfo = "IsNeedColumnInfo";

    /// <summary>
    /// 用户输入的是否需要在生成lua文件的最上方用注释形式显示列信息（默认为不需要）
    /// </summary>
    public static bool IsNeedColumnInfo = true;

    /// <summary>
    /// 导出lua文件配置，bat脚本：导出的lua文件中是否将lua字符串整理为带缩进格式的形式，默认为是
    /// </summary>
    public const string Public_Config_IsFormat = "IsFormat";

    /// <summary>
    /// 导出的lua文件中是否将lua字符串整理为带缩进格式的形式，默认为否
    /// </summary>
    public static bool ExportLuaIsFormat = false;

    /// <summary>
    /// 导出lua文件配置，bat脚本：用于缩进lua table的字符串
    /// </summary>
    public const string Public_Config_IndentationString = "IndentationString";

    /// <summary>
    /// 导出lua文件配置，bat脚本：用于缩进lua table的字符串
    /// </summary>
    public static string IndentationString = "\t";

    /// <summary>
    /// 导出lua文件配置，bat脚本：是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public const string Public_Config_ExportNameBeforeAdd = "ExportNameBeforeAdd";

    /// <summary>
    /// 是否在生成的文件名前加上前缀，如：cfg_
    /// </summary>
    public static string ExportNameBeforeAdd = "";

    /// <summary>
    /// 导出lua文件配置，bat脚本：数组类型是否使用[1]=值  格式，默认是
    /// </summary>
    public const string Public_Config_IsArrayFieldName = "IsArrayFieldName";

    /// <summary>
    ///数组类型是否使用[1]=值  格式，默认是
    /// </summary>
    public static bool IsArrayFieldName = false;

    /// <summary>
    /// 导出lua文件配置，bat脚本：导出的lua文件内容是以表名开头还是以retun开启，默认 以retun开头
    /// </summary>
    public const string Public_Config_IsTableNameStart = "IsTableNameStart";

    /// <summary>
    ///导出的lua文件内容是以表名开头还是以retun开启，默认 以retun开头
    /// </summary>
    public static bool IsTableNameStart = false;

    /// <summary>
    ///  导出lua文件配置，bat脚本：声明对某张表格设置导出Lua时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Public_Config_NotExportLuaNil = "ExportLuaNilConfig";

    /// <summary>
    /// 某个Excel表的配置：声明某张表格导出为lua table时，是否将主键列的值作为table中的元素
    /// </summary>
    public const string Excel_Config_AddKeyToLuaTable = "addKeyToLuaTable";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格不进行默认导出Lua的参数配置
    /// </summary>
    public const string Excel_Config_NotExportLuaOriginalTable = "-notExportOriginalTable";//"NotExportLuaOriginalTable"

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Lua的参数配置
    /// </summary>
    public const string Excel_Config_ExportLua = "ExportLua";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Lua的参数配置
    /// </summary>
    public static bool IsExportLua = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Lua的参数配置
    /// </summary>
    public const string Excel_Config_SpecialExportLua = "SpecialExportLua";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Lua的参数配置
    /// </summary>
    public static bool IsSpecialExportLua = false;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要特殊嵌套导出Lua的，嵌套规则参数
    /// </summary>
    public static List<string> SpecialExportLuaParams;

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置特殊导出Lua规则的配置参数名
    /// </summary>
   // public const string Excel_Config_SpecialExportLua = "TableSpecialExportLua";//"TableSpecialExportLua" tableExportConfig

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Lua时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Excel_Config_NotExportLuaNil = "ExportLuaNilConfig";//NotExportLuaNullNumber

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出lua时是否忽略空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public static bool IsExportLuaNilConfig = false;

    /// <summary>
    /// Excel表中的声明字段类型为：Date类型格式中导出标识，如：date(input=yyyy年MM月dd日 HH时mm分ss秒|toLua=yyyy/MM/dd HH:mm:ss)
    /// </summary>
    public const string DateToExportParamKey = "toLua";//TO_DATABASE_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:date型导出至lua文件的格式,TABLE_INFO_EXTRA_PARAM_KEY_DATE_TO_LUA_FORMAT
    /// </summary>
    public const string DateToExportFormatKey = "dateToLuaFormat";

    /// <summary>
    ///  Excel表中的声明字段类型为：Time类型格式中导出标识，如：time(input=HH时mm分ss秒|toLua=HH:mm:ss)
    /// </summary>
    public const string TimeToExportParamKey = "toLua";// toDatabase TO_DATABASE_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:time型导出至lua文件的格式,TABLE_INFO_EXTRA_PARAM_KEY_TIME_TO_LUA_FORMAT
    /// </summary>
    public const string TimeToExportFormatKey = "timeToLuaFormat";

    /// <summary>
    /// 未声明date型导出至lua文件的格式时所采用的默认格式 DefaultDateToLuaFormat
    /// </summary>
    public static string DefaultDateToExportFormat = null;

    /// <summary>
    /// 未声明time型导出至lua文件的格式时所采用的默认格式
    /// </summary>
    public static string DefaultTimeToExportFormat = null;

    /// <summary>
    /// 未声明date型导出至lua文件的格式时所采用的默认格式  APP_CONFIG_KEY_DEFAULT_DATE_TO_LUA_FORMAT
    /// </summary>
    public const string DefaultDateToExportFormatKey = "defaultDateToLuaFormat";

    /// <summary>
    /// 未声明 time 型导出至 lua 文件的格式时所采用的默认格式  APP_CONFIG_KEY_DEFAULT_TIME_TO_LUA_FORMAT
    /// </summary>
    public const string DefaultTimeToExportFormatKey = "defaultTimeToLuaFormat";
}