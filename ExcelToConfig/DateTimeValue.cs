using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DateTimeValue
{
    // 以下为TableInfo的ExtraParam所支持的key声明

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:date型的输入格式
    /// </summary>
    public const string TABLE_INFO_EXTRA_PARAM_KEY_DATE_INPUT_FORMAT = "dateInputFormat";

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:date型导出至lua文件的格式
    /// </summary>
    public const string TABLE_INFO_EXTRA_PARAM_KEY_DATE_TO_LUA_FORMAT = "dateToLuaFormat";

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:date型导出至MySQL数据库的格式
    /// </summary>
    public const string TABLE_INFO_EXTRA_PARAM_KEY_DATE_TO_DATABASE_FORMAT = "dateToDatabaseFormat";
    // 
    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:time型的输入格式
    /// </summary>
    public const string TABLE_INFO_EXTRA_PARAM_KEY_TIME_INPUT_FORMAT = "timeInputFormat";
    // 
    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:time型导出至lua文件的格式
    /// </summary>
    public const string TABLE_INFO_EXTRA_PARAM_KEY_TIME_TO_LUA_FORMAT = "timeToLuaFormat";

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:time型导出至MySQL数据库的格式
    /// </summary>
    public const string TABLE_INFO_EXTRA_PARAM_KEY_TIME_TO_DATABASE_FORMAT = "timeToDatabaseFormat";


    /// <summary>
    /// 将MySQL中datetime、date型的默认格式作为本工具对date、time两种时间型进行检查并发现错误后的输出格式
    /// </summary>
    public const string APP_DEFAULT_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
    /// <summary>
    /// 将MySQL中datetime、date型的默认格式作为本工具对date、time两种时间型进行检查并发现错误后的输出格式
    /// </summary>
    public const string APP_DEFAULT_TIME_FORMAT = "HH:mm:ss";

    /// <summary>
    /// 导出数据到MySQL中的date型字段的默认格式
    /// </summary>
    public const string APP_DEFAULT_ONLY_DATE_FORMAT = "yyyy-MM-dd";

    /// <summary>
    /// 以1970年1月1日作为计算距今秒数的参考时间，并且作为存储time型的DateTime变量的日期部分
    /// </summary>
    public static DateTime REFERENCE_DATE = new DateTime(1970, 1, 1);

    /// <summary>
    /// 此时间的DateTimeKind为Local，用于转为时间戳时用当前时区表示。北京时间的时间戳0表示1970-01-01 08:00:00
    /// </summary>
    public static DateTime REFERENCE_DATE_LOCAL = TimeZone.CurrentTimeZone.ToLocalTime(REFERENCE_DATE);
    /// <summary>
    /// 未声明date型的输入格式时所采用的默认格式
    /// </summary>
    public static string DefaultDateInputFormat = null;

    /// <summary>
    /// 未声明date型导出至lua文件的格式时所采用的默认格式
    /// </summary>
    public static string DefaultDateToLuaFormat = null;

    /// <summary>
    /// 未声明date型导出至MySQL数据库的格式时所采用的默认格式
    /// </summary>
    public static string DefaultDateToDatabaseFormat = null;

    /// <summary>
    /// 未声明time型的输入格式时所采用的默认格式
    /// </summary>
    public static string DefaultTimeInputFormat = null;

    /// <summary>
    /// 未声明time型导出至lua文件的格式时所采用的默认格式
    /// </summary>
    public static string DefaultTimeToLuaFormat = null;

    /// <summary>
    /// 未声明time型导出至MySQL数据库的格式时所采用的默认格式
    /// </summary>
    public static string DefaultTimeToDatabaseFormat = null;
}
