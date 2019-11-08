using System;

public struct MySQLStruct
{
    /// <summary>
    /// 导出MySQL文件配置，bat脚本,声明将表格导出到MySQL数据库的命令参数
    /// </summary>
    public const string Public_Config_Export = "ExportMySQL";//"-exportMySQL";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：是否导出MySQL
    /// </summary>
    public const string Public_Config_IsExport = "IsExport";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：是否需要导出MySQL
    /// </summary>
    public static bool IsExport = false;

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库服务器
    /// </summary>
    public const string Public_Config_Server = "Server";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库服务器
    /// </summary>
    public static string Server = "127.0.0.1";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库端口
    /// </summary>
    public const string Public_Config_Port = "Port";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库端口
    /// </summary>
    public static string Port = "3306";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：
    /// </summary>
    public const string Public_Config_Uid = "Uid";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：
    /// </summary>
    public static string Uid = "root";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库密码
    /// </summary>
    public const string Public_Config_PassWord = "Password";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库密码
    /// </summary>
    public static string PassWord = "root";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库名
    /// </summary>
    public const string Public_Config_DataBase = "Database";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：数据库名
    /// </summary>
    public static string DataBase = "mydb";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：编码类型
    /// </summary>
    public const string Public_Config_Charset = "Charset";

    /// <summary>
    /// 导出MySQL文件配置，bat脚本：编码类型
    /// </summary>
    public static string Charset = "utf8";

    /// <summary>
    /// Excel表中的声明字段类型为：Date类型格式中导出标识，如：date(input=yyyy年MM月dd日 HH时mm分ss秒|toMySQL=yyyy/MM/dd HH:mm:ss)
    /// </summary>
    public const string DateToExportParamKey = "toMySQL";//TO_DATABASE_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:date型导出至MySQL数据库的格式,TABLE_INFO_EXTRA_PARAM_KEY_DATE_TO_DATABASE_FORMAT
    /// </summary>
    public const string DateToExportFormatKey = "dateToDatabaseFormat";

    /// <summary>
    /// 未声明date型导出至MySQL数据库的格式时所采用的默认格式,DefaultDateToDatabaseFormat
    /// </summary>
    public static string DefaultDateToExportFormat = null;

    /// <summary>
    ///  Excel表中的声明字段类型为：Time类型格式中导出标识，如：time(input=HH时mm分ss秒|toMySQL=HH:mm:ss)
    /// </summary>
    public const string TimeToExportParamKey = "toMySQL";// toDatabase TO_DATABASE_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:time型导出至MySQL数据库的格式,TimeToExportFormat TABLE_INFO_EXTRA_PARAM_KEY_TIME_TO_DATABASE_FORMAT
    /// </summary>
    public const string TimeToExportFormatKey = "timeToDatabaseFormat";

    /// <summary>
    /// 未声明time型导出至MySQL数据库的格式时所采用的默认格式,DefaultTimeToDatabaseFormat
    /// </summary>
    public static string DefaultTimeToExportFormat = null;

    /// <summary>
    /// 导出数据到MySQL中的date型字段的默认格式
    /// </summary>
    public const string APP_DEFAULT_ONLY_DATE_FORMAT = "yyyy-MM-dd";

    /// <summary>
    /// 创建MySQL数据库表格时额外指定的参数字符串
    /// 导出到 MySQL 数据库进行建表时额外添加的参数字符串，
    /// 比如可以通过设置使用的编码格式，避免插入的中文变成乱码。
    /// 例如：ENGINE=InnoDB DEFAULTCHARSET=utf8mb4 COLLATE = utf8mb4_bin
    /// </summary>
    public const string APP_CONFIG_KEY_CREATE_DATABASE_TABLE_EXTRA_PARAM = "createDatabaseTableExtraParam";

    // 声明某张表格导出到数据库中的表名
    public const string CONFIG_NAME_EXPORT_DATABASE_TABLE_NAME = "exportMySQLTableName";//"exportDatabaseTableName";

    // 声明某张表格导出到数据库中的说明信息
    public const string CONFIG_NAME_EXPORT_DATABASE_TABLE_COMMENT = "exportMySQLTableComment";//"exportDatabaseTableComment";

    // 声明某张表格导出到数据库中时string型字段中的空白单元格导出为数据库中的NULL
    public const string CONFIG_NAME_EXPORT_DATABASE_WRITE_NULL_FOR_EMPTY_STRING = "exportMySQLWriteNullForEmptyString";//"exportDatabaseWriteNullForEmptyString";

    /// <summary>
    /// 未声明time型导出至MySQL数据库的格式时所采用的默认格式,,APP_CONFIG_KEY_DEFAULT_TIME_TO_DATABASE_FORMAT
    /// </summary>
    public const string DefaultTimeToExportFormatKey = "defaultTimeToDatabaseFormat";

    /// <summary>
    /// 未声明date型导出至MySQL数据库的格式时所采用的默认格式  APP_CONFIG_KEY_DEFAULT_DATE_TO_DATABASE_FORMAT
    /// </summary>
    public const string DefaultDateToExportFormatKey = "defaultDateToDatabaseFormat";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出MySQL的参数配置
    /// </summary>
    public const string Excel_Config_ExportMySQL = "ExportMySQL";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格是否需要常规导出Lua的参数配置
    /// </summary>
    public static bool IsExportMySQL = false;

    ///// <summary>
    ///// 存储本次要额外导出为MySQL文件的Excel文件名
    ///// </summary>
    //public static List<string> ExportMySQLTableNames = new List<string>();
    ///// <summary>
    ///// 保存MySQL文件路径
    ///// </summary>
    //public static string SavePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SaveMySQL");
    ///// <summary>
    ///// 是否将生成的文件按原Excel文件所在的目录结构进行存储（默认:是）
    ///// </summary>
    //public static bool IsExportKeepDirectoryStructure = true;
    ///// <summary>
    ///// 导出MySQL文件的扩展名（不含点号），默认为MySQL
    ///// </summary>
    //public static string SaveExtension = "MySQL";

    ///// <summary>
    ///// 导出的MySQL文件是否生成为各行数据对应的MySQL object包含在一个MySQL array的形式，默认为否
    ///// </summary>
    //public static bool ExportMySQLIsExportMySQLArrayFormat=false;
    ///// <summary>
    ///// 导出的MySQL文件，若生成包含在一个MySQL object的形式，是否使每行字段信息对应的MySQL object中包含主键列对应的键值对，默认为是
    ///// </summary>
    //public static bool ExportMySQLIsExportMySQLMapIncludeKeyColumnValue=true;

    ///// <summary>
    ///// 导出的MySQL文件中是否将MySQL字符串整理为带缩进格式的形式，默认为是
    ///// </summary>
    //public static bool ExportMySQLIsFormat = true;

    ///// <summary>
    ///// 当lang型数据key在lang文件中找不到对应值时，是否在MySQL文件输出字段值为空字符（默认为输出nil）
    ///// </summary>
    //public static bool IsPrintEmptyStringWhenLangNotMatching = false;

    ///// <summary>
    ///// 声明对某张表格设置特殊导出MySQL规则的配置参数名
    ///// </summary>
    //public const string CONFIG_NAME_EXPORT = "tableExportMySQLConfig";

    ///// <summary>
    ///// 声明对某张表格不进行默认导出MySQL的参数配置
    ///// </summary>
    //public const string CONFIG_PARAM_NOT_EXPORT_ORIGINAL_TABLE = "-notExportMySQLOriginalTable";

    ///// <summary>
    ///// 声明导出MySQL时是否忽略int等数字类型中的空值，如果忽略则不导出该字段
    ///// </summary>
    //public const string CONFIG_NULL_EXPORT = "tableExportMySQLNullConfig";
}