using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

  public  class AppLang
    {
    /// <summary>
    /// lang文件转为键值对形式（key：lang文件中的key名， value：对应的在指定语言下的翻译）
    /// </summary>
    public static Dictionary<string, string> LangData = new Dictionary<string, string>();
    /// <summary>
    ///Lang文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Lang = "Lang";
    /// <summary>
    /// Lang文件配置，bat脚本：是否设置Lang
    /// </summary>
    public const string Public_Config_IsLang = "IsLang";
    /// <summary>
    /// Lang文件配置，bat脚本：是否设置Lang
    /// </summary>
    public static bool IsLang = false;
    /// <summary>
    ///  Lang文件配置，bat脚本：Lang文件路径
    /// </summary>
    public const string Public_Config_LangPath = "LangPath";
    /// <summary>
    /// 用户输入的国际化文件所在路径
    /// </summary>
    public static string LangPath = "lang.txt";
    /// <summary>
    /// 声明当lang型数据key在lang文件中找不到对应值时，在lua文件输出字段值为空字符串的命令参数
    /// </summary>
    public const string Public_Config_IsLangNull = "IsLangNull"; //"-printEmptyStringWhenLangNotMatching";
    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：当lang型数据key在lang文件中找不到对应值时，是否在lua,json等文件输出字段值为空字符（h）
    /// </summary>
    public static bool IsLangNull = false;


}

