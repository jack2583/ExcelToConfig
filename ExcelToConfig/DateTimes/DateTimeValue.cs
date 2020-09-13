using System;

public class DateTimeValue
{
    /// <summary>
    /// Excel表中的声明字段类型为：Date类型格式，前缀，如：date(input=yyyy年MM月dd日 HH时mm分ss秒|toLua=yyyy/MM/dd HH:mm:ss)
    /// </summary>
    public const string DefineDateStartString = "date";// DEFINE_START_STRING
    /// <summary>
    /// Excel表中的声明字段类型为：Time类型格式，前缀，如：time(input=HH时mm分ss秒|toLua=HH:mm:ss)
    /// </summary>
    public const string DefineTimeStartString = "time";// DEFINE_START_STRING


    /// <summary>
    /// Excel表中的声明字段类型为：Date类型格式中导入标识，如：date(input=yyyy年MM月dd日 HH时mm分ss秒|toLua=yyyy/MM/dd HH:mm:ss)
    /// </summary>
   // public const string DateInputParamKey = "input";//INPUT_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:date型的输入格式 ,TABLE_INFO_EXTRA_PARAM_KEY_DATE_INPUT_FORMAT
    /// </summary>
   // public const string DateInputFormat = "dateInputFormat";



    /// <summary>
    /// Excel表中的声明字段类型为：Time类型格式中导入标识，如：time(input=HH时mm分ss秒|toLua=HH:mm:ss)
    /// </summary>
  //  public const string TimeInputParamKey = "input";// INPUT_PARAM_KEY

    /// <summary>
    /// TableInfo的ExtraParam所支持的key声明:time型的输入格式,TABLE_INFO_EXTRA_PARAM_KEY_TIME_INPUT_FORMAT
    /// </summary>
    //public const string TimeInputFormat = "timeInputFormat";

    // 以下为TableInfo的ExtraParam所支持的key声明
    /// <summary>
    /// config.txt参数名，未声明date型的输入格式时所采用的默认格式
    /// </summary>
    public const string APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT = "defaultDateInputFormat";

    /// <summary>
    /// 未声明date型的输入格式时所采用的默认格式
    /// </summary>
    public static string DefaultDateInputFormat = "yyyy/MM/dd HH:mm:ss";// null;

    /// <summary>
    ///  config.txt参数名，未声明time型的输入格式时所采用的默认格式
    /// </summary>
    public const string APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT = "defaultTimeInputFormat";

    /// <summary>
    /// 未声明time型的输入格式时所采用的默认格式
    /// </summary>
    public static string DefaultTimeInputFormat = "HH:mm:ss";//null;

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

    /// <summary>
    /// 解析date型的格式类型
    /// </summary>
    public static DateFormatType GetDateFormatType(string formatString)
    {
        formatString = formatString.Trim();
        if ("#1970sec".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return DateFormatType.ReferenceDateSec;
        else if ("#1970msec".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return DateFormatType.ReferenceDateMsec;
        else if ("#dateTable".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return DateFormatType.DataTable;
        else
            return DateFormatType.FormatString;
    }

    /// <summary>
    /// 解析time型的格式类型
    /// </summary>
    public static TimeFormatType GetTimeFormatType(string formatString)
    {
        formatString = formatString.Trim();
        if ("#sec".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return TimeFormatType.ReferenceTimeSec;
        else
            return TimeFormatType.FormatString;
    }

}
/// <summary>
/// date型数据的输入导出格式
/// </summary>
public enum DateFormatType
{
    /// <summary>
    /// 符合C#类库要求的标准时间格式
    /// </summary>
    FormatString,          // 符合C#类库要求的标准时间格式

    /// <summary>
    /// 用距离1970年1月1日的秒数表示
    /// </summary>
    ReferenceDateSec,      // 用距离1970年1月1日的秒数表示

    /// <summary>
    /// 用距离1970年1月1日的毫秒数表示
    /// </summary>
    ReferenceDateMsec,     // 用距离1970年1月1日的毫秒数表示

    /// <summary>
    /// 生成调用lua库函数os.date的代码形式
    /// </summary>
    DataTable,             // 生成调用lua库函数os.date的代码形式
}

/// <summary>
/// time型数据的输入导出格式
/// </summary>
public enum TimeFormatType
{
    FormatString,          // 符合C#类库要求的标准时间格式
    ReferenceTimeSec,      // 用距离0点的秒数表示
}

public enum DateTimeTypeKey
{
    input,
    toLua,
    toJson,
    toServerJson,
    toMySQL,
    toSQLITE,
    toTxt,
}
//示例：判断 input是否存在于DateTimeTypeKey枚举值中
//DateTimeTypeKey flag;
//if (Enum.TryParse<DateTimeTypeKey>("input", true, out flag))
//{
//    Console.Write("ok");
//}