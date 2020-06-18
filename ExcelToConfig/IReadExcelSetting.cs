using System;
using System.Collections.Generic;
using System.Text;

interface IReadExcelSetting
{
    /// <summary>
    /// 某个表读取时int long float类型中值为空时，是否以默认值0读取
    /// </summary>
    bool IsReadNullNumber { set; get; }
    /// <summary>
    /// ExcelConfig参数名：读取时int long float类型中值为空时，是否以默认值0读取
    /// </summary>
    string IsReadNullNumberECP { set; get; }
    /// <summary>
    /// ExcelConfig参数值：读取时int long float类型中值为空时，是否以默认值0读取
    /// </summary>
    bool IsReadNullNumberECV { set; get; }
    /// <summary>
    /// Bat参数名：读取时int long float类型中值为空时，是否以默认值0读取
    /// </summary>
    string IsReadNullNumberBCP { set; get; }
    /// <summary>
    /// Bat参数值：读取时int long float类型中值为空时，是否以默认值0读取
    /// </summary>
    bool IsReadNullNumberBCV { set; get; }
    /// <summary>
    /// 获取最终的
    /// 某个表读取时int long float类型中值为空时，是否以默认值0读取
    /// </summary>
    /// <param name="IsReadNullNumberECV"> ExcelConfig参数值（最高级）</param>
    /// <param name="IsReadNullNumberBCV">Bat参数值</param>
    /// <returns></returns>
    bool GetIsReadNullNumber(bool IsReadNullNumberECV, bool IsReadNullNumberBCV);


    /// <summary>
    /// 某个表读取时string类型中值为空时，是否以默认值""读取
    /// </summary>
    bool IsReadNullString { set; get; }
    /// <summary>
    /// ExcelConfig参数名：读取时string类型中值为空时，是否以默认值""读取
    /// </summary>
    string IsReadNullStringECP { set; get; }
    /// <summary>
    /// ExcelConfig参数值：读取时string类型中值为空时，是否以默认值""读取
    /// </summary>
    bool IsReadNullStringECV { set; get; }
    /// <summary>
    /// Bat参数名：读取时string类型中值为空时，是否以默认值""读取
    /// </summary>
    string IsReadNullStringBCP { set; get; }
    /// <summary>
    /// Bat参数值：读取时string类型中值为空时，是否以默认值""读取
    /// </summary>
    bool IsReadNullStringBCV { set; get; }
    /// <summary>
    /// 获取最终的
    /// 某个表读取时string类型中值为空时，是否以默认值""读取
    /// </summary>
    /// <param name="IsReadNullStringECV"> ExcelConfig参数值（最高级）</param>
    /// <param name="IsReadNullStringBCV">Bat参数值</param>
    /// <returns></returns>
    bool GetIsReadNullString(bool IsReadNullStringECV, bool IsReadNullStringBCV);


    /// <summary>
    /// 某个表读取时Json类型中值为空时，是否以默认值[]读取
    /// </summary>
    bool IsReadNullJson { set; get; }
    /// <summary>
    /// ExcelConfig参数名：读取时Json类型中值为空时，是否以默认值[]读取
    /// </summary>
    string IsReadNullJsonECP { set; get; }
    /// <summary>
    /// ExcelConfig参数值：读取时string类型中值为空时，是否以默认值[]读取
    /// </summary>
    bool IsReadNullJsonECV { set; get; }
    /// <summary>
    /// Bat参数名：读取时Json类型中值为空时，是否以默认值[]读取
    /// </summary>
    string IsReadNullJsonBCP { set; get; }
    /// <summary>
    /// Bat参数值：读取时Json类型中值为空时，是否以默认值[]读取
    /// </summary>
    bool IsReadNullJsonBCV { set; get; }
    /// <summary>
    /// 获取最终的
    /// 某个表读取时Json类型中值为空时，是否以默认值[]读取
    /// </summary>
    /// <param name="IsReadNullJsonECV"> ExcelConfig参数值（最高级）</param>
    /// <param name="IsReadNullJsonBCV">Bat参数值</param>
    /// <returns></returns>
    bool GetIsReadNullJson(bool IsReadNullJsonECV, bool IsReadNullJsonBCV);
}
