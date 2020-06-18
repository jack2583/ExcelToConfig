using System;
using System.Collections.Generic;
using System.Text;

interface ICheckSetting
{
    /// <summary>
    /// 某个表读取时值为空时，是否给预检查通过（如果Numbler String Json设置了具体的，以Numbler String Json为准）
    /// </summary>
    bool IsCheckNullAny { set; get; }
    /// <summary>
    /// ExcelConfig参数名：读取时值为空时，是否给预检查通过（如果Numbler String Json设置了具体的，以Numbler String Json为准）
    /// </summary>
    string IsCheckNullAnyECP { set; get; }
    /// <summary>
    /// ExcelConfig参数值：读取时值为空时，是否给预检查通过（如果Numbler String Json设置了具体的，以Numbler String Json为准）
    /// </summary>
    bool IsCheckNullAnyECV { set; get; }
    /// <summary>
    /// Bat参数名：读取时值为空时，是否给预检查通过（如果Numbler String Json设置了具体的，以Numbler String Json为准）
    /// </summary>
    string IsCheckNullAnyBCP { set; get; }
    /// <summary>
    /// Bat参数值：读取时值为空时，是否给预检查通过（如果Numbler String Json设置了具体的，以Numbler String Json为准）
    /// </summary>
    bool IsCheckNullAnyBCV { set; get; }
    /// <summary>
    /// 获取最终的
    /// 某个表读取时值为空时，是否给预检查通过（如果Numbler String Json设置了具体的，以Numbler String Json为准）
    /// </summary>
    /// <param name="IsCheckNullAnyECV"> ExcelConfig参数值（最高级）</param>
    /// <param name="IsCheckNullAnyBCV">Bat参数值</param>
    /// <returns></returns>
    bool GetIsCheckNullAny(bool IsCheckNullAnyECV, bool IsCheckNullAnyBCV);



    /// <summary>
    /// 某个表读取时int long float类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullNumber { set; get; }
    /// <summary>
    /// ExcelConfig参数名：读取时int long float类型中值为空时，是否给预检查通过
    /// </summary>
    string IsCheckNullNumberECP { set; get; }
    /// <summary>
    /// ExcelConfig参数值：读取时int long float类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullNumberECV { set; get; }
    /// <summary>
    /// Bat参数名：读取时int long float类型中值为空时，是否给预检查通过
    /// </summary>
    string IsCheckNullNumberBCP { set; get; }
    /// <summary>
    /// Bat参数值：读取时int long float类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullNumberBCV { set; get; }
    /// <summary>
    /// 获取最终的
    /// 某个表读取时int long float类型中值为空时，是否给预检查通过
    /// </summary>
    /// <param name="IsCheckNullNumberECV"> ExcelConfig参数值（最高级）</param>
    /// <param name="IsCheckNullNumberBCV">Bat参数值</param>
    /// <returns></returns>
    bool GetIsCheckNullNumber(bool IsCheckNullNumberECV, bool IsCheckNullNumberBCV);


    /// <summary>
    /// 某个表读取时string类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullString { set; get; }
    /// <summary>
    /// ExcelConfig参数名：读取时string类型中值为空时，是否给预检查通过
    /// </summary>
    string IsCheckNullStringECP { set; get; }
    /// <summary>
    /// ExcelConfig参数值：读取时string类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullStringECV { set; get; }
    /// <summary>
    /// Bat参数名：读取时string类型中值为空时，是否给预检查通过
    /// </summary>
    string IsCheckNullStringBCP { set; get; }
    /// <summary>
    /// Bat参数值：读取时string类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullStringBCV { set; get; }
    /// <summary>
    /// 获取最终的
    /// 某个表读取时string类型中值为空时，是否给预检查通过
    /// </summary>
    /// <param name="IsCheckNullStringECV"> ExcelConfig参数值（最高级）</param>
    /// <param name="IsCheckNullStringBCV">Bat参数值</param>
    /// <returns></returns>
    bool GetIsCheckNullString(bool IsCheckNullStringECV, bool IsCheckNullStringBCV);


    /// <summary>
    /// 某个表读取时Json类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullJson { set; get; }
    /// <summary>
    /// ExcelConfig参数名：读取时Json类型中值为空时，是否给预检查通过
    /// </summary>
    string IsCheckNullJsonECP { set; get; }
    /// <summary>
    /// ExcelConfig参数值：读取时string类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullJsonECV { set; get; }
    /// <summary>
    /// Bat参数名：读取时Json类型中值为空时，是否给预检查通过
    /// </summary>
    string IsCheckNullJsonBCP { set; get; }
    /// <summary>
    /// Bat参数值：读取时Json类型中值为空时，是否给预检查通过
    /// </summary>
    bool IsCheckNullJsonBCV { set; get; }
    /// <summary>
    /// 获取最终的
    /// 某个表读取时Json类型中值为空时，是否给预检查通过
    /// </summary>
    /// <param name="IsCheckNullJsonECV"> ExcelConfig参数值（最高级）</param>
    /// <param name="IsCheckNullJsonBCV">Bat参数值</param>
    /// <returns></returns>
    bool GetIsCheckNullJson(bool IsCheckNullJsonECV, bool IsCheckNullJsonBCV);
}
