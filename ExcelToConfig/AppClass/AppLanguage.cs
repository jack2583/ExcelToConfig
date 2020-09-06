using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class AppLanguage
{
    /// <summary>
    /// Config.txt中的多语言配置，即excel现有多少种类型
    /// Language:_ft,_yn,_English
    /// daily_activity_ft-日常活动.xlsx，daily_activity_yn-日常活动.xlsx，daily_activity_English-日常活动.xlsx
    /// </summary>
    public static string AppConfigIDArrParam =  "Language";
    public static string[] AppConfigIDArr;
    public static char SplitChar = ',';

    public static string MoreLanguageParam = "PublicSetting";// "MoreLanguage";
    public static string IsExtractChineseParam = "IsExtractChinese";
    public static bool IsExtractChinese = false;
    /// <summary>
    /// SourceTextDict文件
    /// </summary>
    public static List<string> SourceTextData = new List<string>();
    /// <summary>
    /// LanguageDict文件转为键值对形式（key：LanguageDict文件中的key名,即原名， value：对应的在指定语言下的翻译）
    /// </summary>
    public static Dictionary<string, string> LanguageDictData = new Dictionary<string, string>();

    public static void GetParamValue()
    {
        if (AppValues.ConfigData != null && AppValues.ConfigData.ContainsKey(AppConfigIDArrParam))
        {
            string AppConfigID = AppValues.ConfigData[AppConfigIDArrParam];
            if (AppConfigID != null && AppConfigID != "")
            {
                if (AppConfigID.Contains(new string(new char[] { SplitChar })))
                {
                    AppConfigIDArr = AppConfigID.Split(new char[] { SplitChar }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                    AppConfigIDArr = new string[] { AppConfigID };
            }
        }

        if (AppValues.BatParamInfo.ContainsKey(MoreLanguageParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[MoreLanguageParam].ChildParam;
            IsExtractChinese = BatMethods.GetBoolValue(IsExtractChinese, ChildParam, IsExtractChineseParam);

            //    IsMoreLanguage = BatMethods.GetBoolValue(IsMoreLanguage, ChildParam, IsMoreLanguageParam);
            //    NeedLanguage = BatMethods.GetStringValue(NeedLanguage, ChildParam, NeedLanguageParam);
            //    OtherLanguage = BatMethods.GetStringValue(OtherLanguage, ChildParam, OtherLanguageParam);
            //    IsAddSaveType = BatMethods.GetBoolValue(IsAddSaveType, ChildParam, IsAddSaveTypeParam);
            //    IsGetSourceTextFile = BatMethods.GetBoolValue(IsGetSourceTextFile, ChildParam, IsGetSourceTextFileParam);
        }
    //if (OtherLanguage != null)
    //    OtherLanguageArr = OtherLanguage.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
}
    /// <summary>
    /// 即提取原文集合
    /// </summary>
    public static void GetLanguageDictData(string text)
    {
        if (ExcelFolder.TheLanguage != "")
        {
            if (text == null)
                return;

            if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"[\u4e00-\u9fa5]"))
                return;
            text = text.Replace("\n", "\\n");
            if (!SourceTextData.Contains(text))
            {
                if(!LanguageDictData.ContainsKey(text))
                    SourceTextData.Add(text);
            }
        }
    }
    /// <summary>
    /// 创建多语言集合，即将简体文本提出取来待翻译
    /// </summary>
    public static void CreateLanguageDictFile()
    {
        if (ExcelFolder.TheLanguage != "" && IsExtractChinese == true)
        {
            string MoreLanguagePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Language");
            string TheLanguageDicPath = FileModule.CombinePath(MoreLanguagePath, "Language" + ExcelFolder.TheLanguage);
            if (Directory.Exists(TheLanguageDicPath) == false)
                Directory.CreateDirectory(TheLanguageDicPath);

            string TheLanguageDicFile = FileModule.CombinePath(TheLanguageDicPath, ExcelFolder.TheLanguage + "_dict.txt");
            string SourceText = FileModule.CombinePath(TheLanguageDicPath, "SourceText.txt");

            StringBuilder content = new StringBuilder();

            content.AppendLine("#待翻译的文本：");
            content.AppendLine(string.Format("#翻译后，请将文本放入:\n#{0}\n#最末尾，并用Tab键分割。", TheLanguageDicFile));
            foreach (string str in SourceTextData)
            {
                content.AppendLine(str);
            }
            // 保存为txt文件
            StreamWriter writer = new StreamWriter(SourceText, false, new UTF8Encoding(false));
            writer.Write(content.ToString());
            writer.Flush();
            writer.Close();
            if (!File.Exists(TheLanguageDicFile))
            {
                writer = new StreamWriter(TheLanguageDicFile, false, new UTF8Encoding(false));
                writer.Write("原文\t译文\n简体\tEnglish");
                writer.Flush();
                writer.Close();
            }
            AppLog.Log("文本已提取成功,请尽快拿去翻译,提取出的文件为:", ConsoleColor.Green);
            AppLog.Log(string.Format("{0}", SourceText), ConsoleColor.Yellow);
            // AppLog.Log("\n按任意键退出");

#if DEBUG
                Console.ReadKey();
#endif
            Environment.Exit(0);
        }
    }

    /// <summary>
    /// 将翻译后的字典读取文件中
    /// </summary>
    public static void GetLanguageDictData()
    {
        if (ExcelFolder.TheLanguage != "")
        {
            string errorString = null;
            string MoreLanguagePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Language"); ;
            string TheLanguageDicPath = FileModule.CombinePath(MoreLanguagePath, "Language" + ExcelFolder.TheLanguage);
            string TheLanguageDicFile = FileModule.CombinePath(TheLanguageDicPath, ExcelFolder.TheLanguage + "_dict.txt");
            if (!File.Exists(TheLanguageDicFile))
            {
                return;
            }
            LanguageDictData = TxtConfigReader.ParseTxtConfigFile(TheLanguageDicFile, "\t", out errorString);
            if (!string.IsNullOrEmpty(errorString))
                AppLog.LogErrorAndExit(errorString);

        }
    }

    /// <summary>
    /// 根据字典获取翻译后，如果在字典找不到则返回原文
    /// </summary>
    /// <param name="">原文本</param>
    /// <returns></returns>
    public static string GetNewLanguageText(string oldText)
    {
        if (ExcelFolder.TheLanguage != "")
        {
            if (LanguageDictData.ContainsKey(oldText))
                return LanguageDictData[oldText];
            else
                return oldText;
        }
        else
            return oldText;
    }







    ////MoreLanguage(IsMoreLanguage=false|NeedLanguage=_ft|OtherLanguage=_yn,_English|IsAddSaveType=false|IsGetSourceTextFile=false)
    //public static string MoreLanguageParam = "MoreLanguage";
    //public static string IsMoreLanguageParam = "IsMoreLanguage";
    //public static bool IsMoreLanguage = false;
    ///// <summary>
    ///// App全局导出通用配置,需要导出的语言版本，如ft，null则为默认语言
    ///// </summary>
    //public static string NeedLanguageParam = "NeedLanguage";
    //public static string NeedLanguage = null;
    //public static string OtherLanguageParam = "OtherLanguage";
    //private static string OtherLanguage = null;
    //public static string[] OtherLanguageArr = null;

    ///// <summary>
    ///// App全局国际多语言配置，保存方试，true,新增文件方式即item_ft格式，false替换原简体方式
    ///// </summary>
    //public static string IsAddSaveTypeParam = "IsAddSaveType";
    //public static bool IsAddSaveType = true;

    ///// <summary>
    ///// App全局国际多语言配置，是否提取原文，true,提取原文供翻译其他语言用，false正常导出 
    ///// </summary>
    //public static string IsGetSourceTextFileParam = "IsGetSourceTextFile";
    //public static bool IsGetSourceTextFile = false;



    /// <summary>
    ///App全局国际多语言配置， 多语言字典
    /// </summary>
    // public const string Public_Config_LanguageDictPath = "LanguageDictPath";
    /// <summary>
    /// 多语言字典文件名
    /// </summary>
    //public static string LanguageDictPath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Language");








}