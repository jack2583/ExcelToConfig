using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public  class CheckStruct
{
    // 声明整表检查的配置参数名
    public const string CONFIG_NAME_CHECK_TABLE = "tableCheckRule";
    /// <summary>
    /// 用户输入的是否需要检查表格（默认为检查）
    /// </summary>
    public static bool IsNeedCheck = true;

    /// <summary>
    /// 用户输入的是否允许int、float型字段中存在空值，默认不允许
    /// </summary>
    public static bool IsAllowedNullNumber = true;


}
