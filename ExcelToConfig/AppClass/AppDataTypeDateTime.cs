using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
