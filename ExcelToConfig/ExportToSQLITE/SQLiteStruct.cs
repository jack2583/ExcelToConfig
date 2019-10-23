using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct SQLiteStruct
{
    /// <summary>
    /// 声明将表格导出到SQLite数据库的命令参数
    /// </summary>
    public const string EXPORT_PARAM_STRING = "-exportSQLite";

    /// <summary>
    /// SQLite连接字符串
    /// connectMySQLString:server=192.168.18.48;port=32001;uid=root;password=2J89d7*(JDdih&_78$56;database=xz_game;Charset=utf8;
    /// </summary>
    public const string APP_CONFIG_KEY_SQLITE_CONNECT_STRING = "connectSQLiteString";

    /// <summary>
    /// 创建SQLite数据库表格时额外指定的参数字符串
    /// </summary>
    public const string APP_CONFIG_KEY_CREATE_DATABASE_TABLE_EXTRA_PARAM = "createDatabaseTableExtraParamSQLite";


    /// <summary>
    /// 导出SQLite连接字符串
    /// </summary>
    public static string ExportConnectString = null;



    // 声明某张表格导出到数据库中的表名
    public const string CONFIG_NAME_EXPORT_DATABASE_TABLE_NAME = "exportSQLiteTableName";
    // 声明某张表格导出到数据库中的说明信息
    public const string CONFIG_NAME_EXPORT_DATABASE_TABLE_COMMENT = "exportSQLiteTableComment";
    // 声明某张表格导出到数据库中时string型字段中的空白单元格导出为数据库中的NULL
    public const string CONFIG_NAME_EXPORT_DATABASE_WRITE_NULL_FOR_EMPTY_STRING = "exportSQLiteWriteNullForEmptyString";


    // date型导出至SQLite数据库的格式
    public const string TABLE_INFO_EXTRA_PARAM_KEY_DATE_TO_DATABASE_FORMAT = "dateToSQLiteFormat";
    // time型导出至SQLite数据库的格式
    public const string TABLE_INFO_EXTRA_PARAM_KEY_TIME_TO_DATABASE_FORMAT = "timeToSQLiteFormat";

    // 将SQLite中datetime、date型的默认格式作为本工具对date、time两种时间型进行检查并发现错误后的输出格式
    public const string APP_DEFAULT_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
    public const string APP_DEFAULT_TIME_FORMAT = "HH:mm:ss";
    // 导出数据到SQLite中的date型字段的默认格式
    public const string APP_DEFAULT_ONLY_DATE_FORMAT = "yyyy-MM-dd";

}
