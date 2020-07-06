using System.Collections.Generic;

public class AppLang
{
    //Lang(IsLang= true | LangPath = lang.txt | IsLangNull = false)

    /// <summary>
    /// lang文件转为键值对形式（key：lang文件中的key名， value：对应的在指定语言下的翻译）
    /// </summary>
    public static Dictionary<string, string> LangData = new Dictionary<string, string>();

    public static string LangParam = "Lang";
    public static string IsLangParam = "IsLang";
    public static bool IsLang = false;
    public static string LangPathParam = "LangPath";
    public static string LangPath = "lang.txt";

    /// <summary>
    /// 声明当lang型数据key在lang文件中找不到对应值时，在lua文件输出字段值为空字符串的命令参数 
    /// </summary>
    public static string IsLangNullParam = "IsLangNull"; //"-printEmptyStringWhenLangNotMatching";

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：当lang型数据key在lang文件中找不到对应值时，是否在lua,json等文件输出字段值为空字符（h）
    /// </summary>
    public static bool IsLangNull = false;

    private static void GetParamValue()
    {
        if (AppValues.BatParamInfo.ContainsKey(LangParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[LangParam].ChildParam;
            IsLang = BatMethods.GetBoolValue(IsLang, ChildParam, IsLangParam);
            LangPath = BatMethods.GetStringValue(LangPath, ChildParam, LangPathParam);
            IsLangNull = BatMethods.GetBoolValue(IsLangNull, ChildParam, IsLangNullParam);
        }
    }
    public static void ReadLangData()
    {
        if (IsLang)
        {
            GetParamValue();
           string errorString = null;
            LangData = TxtConfigReader.ParseTxtConfigFile(LangPath, ":", out errorString);
            if (!string.IsNullOrEmpty(errorString))
                AppLog.LogErrorAndExit(errorString);
        }
    }
}