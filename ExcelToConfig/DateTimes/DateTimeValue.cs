using System;

public class DateTimeValue
{
    /// <summary>
    /// Excel表中的声明字段类型为：Date类型格式，前缀，如：date(input=yyyy年MM月dd日 HH时mm分ss秒|toLua=yyyy/MM/dd HH:mm:ss)
    /// </summary>
    public const string DefineDateStartString = "date";// DEFINE_START_STRING

    /// <summary>
    /// Excel表中的声明字段类型为：Date类型格式中导入标识，如：date(input=yyyy年MM月dd日 HH时mm分ss秒|toLua=yyyy/MM/dd HH:mm:ss)
    /// </summary>
    public const string DateInputParamKey = "input";//INPUT_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:date型的输入格式 ,TABLE_INFO_EXTRA_PARAM_KEY_DATE_INPUT_FORMAT
    /// </summary>
    public const string DateInputFormat = "dateInputFormat";

    /// <summary>
    /// Excel表中的声明字段类型为：Time类型格式，前缀，如：time(input=HH时mm分ss秒|toLua=HH:mm:ss)
    /// </summary>
    public const string DefineTimeStartString = "time";// DEFINE_START_STRING

    /// <summary>
    /// Excel表中的声明字段类型为：Time类型格式中导入标识，如：time(input=HH时mm分ss秒|toLua=HH:mm:ss)
    /// </summary>
    public const string TimeInputParamKey = "input";// INPUT_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:time型的输入格式,TABLE_INFO_EXTRA_PARAM_KEY_TIME_INPUT_FORMAT
    /// </summary>
    public const string TimeInputFormat = "timeInputFormat";

    // 以下为TableInfo的ExtraParam所支持的key声明
    /// <summary>
    /// 未声明date型的输入格式时所采用的默认格式
    /// </summary>
    public const string APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT = "defaultDateInputFormat";

    /// <summary>
    /// 未声明date型的输入格式时所采用的默认格式
    /// </summary>
    public static string DefaultDateInputFormat = null;

    /// <summary>
    /// 未声明time型的输入格式时所采用的默认格式
    /// </summary>
    public const string APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT = "defaultTimeInputFormat";

    /// <summary>
    /// 未声明time型的输入格式时所采用的默认格式
    /// </summary>
    public static string DefaultTimeInputFormat = null;

    /// <summary>
    /// 以1970年1月1日作为计算距今秒数的参考时间，并且作为存储time型的DateTime变量的日期部分
    /// </summary>
    public static DateTime REFERENCE_DATE = new DateTime(1970, 1, 1);

    /// <summary>
    /// 此时间的DateTimeKind为Local，用于转为时间戳时用当前时区表示。北京时间的时间戳0表示1970-01-01 08:00:00
    /// </summary>
    public static DateTime REFERENCE_DATE_LOCAL = TimeZone.CurrentTimeZone.ToLocalTime(REFERENCE_DATE);

    /// <summary>
    /// 将MySQL中datetime、date型的默认格式作为本工具对date、time两种时间型进行检查并发现错误后的输出格式
    /// </summary>
    public const string APP_DEFAULT_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// 将MySQL中datetime、date型的默认格式作为本工具对date、time两种时间型进行检查并发现错误后的输出格式
    /// </summary>
    public const string APP_DEFAULT_TIME_FORMAT = "HH:mm:ss";
}