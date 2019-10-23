using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppLanguage
{

    /// <summary>
    /// App全局国际多语言配置，公共设置参数
    /// </summary>
    public const string Public_Config_MoreLanguage = "MoreLanguage";
    /// <summary>
    /// App全局国际多语言配置，公共设置参数
    /// </summary>
    public const string Public_Config_IsMoreLanguage = "IsMoreLanguage";
    /// <summary>
    /// App全局国际多语言配置，公共设置参数,默认非多语言
    /// </summary>
    public static bool IsMoreLanguage = false;
    /// <summary>
    /// App全局国际多语言配置，指定的多语言，如ft
    /// </summary>
    public const string Public_Config_NeedLanguage = "NeedLanguage";
    /// <summary>
    /// App全局导出通用配置,需要导出的语言版本，如ft，null则为默认语言
    /// </summary>
    public static string NeedLanguage = null;
    /// <summary>
    /// App全局国际多语言配置，公共设置参数
    /// </summary>
    public const string Public_Config_OtherLanguage = "OtherLanguage";
    /// <summary>
    /// App全局导出通用配置,除默认语言外的所有语言
    /// </summary>
    public static string[] OtherLanguage = null;
    /// <summary>
    /// App全局国际多语言配置，保存方试，true,新增文件方式即item_ft格式，false替换原简体方式
    /// </summary>
    public const string Public_Config_IsAddSaveType = "IsAddSaveType";
    /// <summary>
    /// App全局国际多语言配置，保存方式，true,新增文件方式即item_ft格式，false替换原简体方式
    /// </summary>
    public static bool IsAddSaveType = true;

    /// <summary>
    /// App全局国际多语言配置，是否提取原文，true,提取原文供翻译其他语言用，false正常导出
    /// </summary>
    public const string Public_Config_IsGetSourceTextFile = "IsGetSourceTextFile";
    /// <summary>
    /// App全局国际多语言配置，保存方试，true,提取原文供翻译其他语言用，false正常导出
    /// </summary>
    public static bool IsGetSourceTextFile = false;
    /// <summary>
    ///App全局国际多语言配置， 多语言字典
    /// </summary>
   // public const string Public_Config_LanguageDictPath = "LanguageDictPath";
    /// <summary>
    /// 多语言字典文件名
    /// </summary>
    public static string LanguageDictPath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "MoreLanguage");

    /// <summary>
    /// LanguageDict文件转为键值对形式（key：LanguageDict文件中的key名,即原名， value：对应的在指定语言下的翻译）
    /// </summary>
    public static Dictionary<string, string> LanguageDictData = new Dictionary<string, string>();
    /// <summary>
    /// SourceTextDict文件
    /// </summary>
    public static List<string> SourceTextData = new List<string>();

    /// <summary>
    /// 即提取原文集合
    /// </summary>
    public static void GetLanguageDictData(string text)
    {
        if (text == null)
            return;

        if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"[\u4e00-\u9fa5]"))
            return;

        if (IsMoreLanguage && NeedLanguage != null && (!SourceTextData.Contains(text)) && (!LanguageDictData.ContainsKey(text)))
        {
           
            SourceTextData.Add(text);
        }
    }
    /// <summary>
    /// 创建多语言集合，即提取原文待翻译
    /// </summary>
    public static void CreateLanguageDictFile()
    {
        if (IsGetSourceTextFile==true && IsMoreLanguage && NeedLanguage != null)
        {
            string NeedLanguageDicPath = FileModule.CombinePath(LanguageDictPath, "Language" + NeedLanguage);
            if (Directory.Exists(NeedLanguageDicPath) == false)
                Directory.CreateDirectory(NeedLanguageDicPath);

            string NeedLanguageDicFile = FileModule.CombinePath(NeedLanguageDicPath, NeedLanguage + "_dict.txt");
            string SourceText = FileModule.CombinePath(NeedLanguageDicPath, "SourceText.txt");

            StringBuilder content = new StringBuilder();

            content.AppendLine("#待翻译的文本：");
            content.AppendLine(string.Format("#翻译后，请将文本放入:\n#{0}\n#最末尾，并用Tab键分割。", NeedLanguageDicFile));
            foreach (string str in SourceTextData)
            {
                content.AppendLine(str);
            }
            // 保存为txt文件
            StreamWriter writer = new StreamWriter(SourceText, false, new UTF8Encoding(false));
            writer.Write(content.ToString());
            writer.Flush();
            writer.Close();
            if (!File.Exists(NeedLanguageDicFile))
            {
                writer = new StreamWriter(NeedLanguageDicFile, false, new UTF8Encoding(false));
                writer.Write("汉语\tChinese");
                writer.Flush();
                writer.Close();
            }
                AppLog.Log(string.Format("文本已提取成功，文件名为\n:\"{0}\"\n，请尽快拿去翻译", SourceText), ConsoleColor.Green);
                Console.WriteLine("按任意键退出");
                Console.ReadKey();
                Environment.Exit(0);
        }
           
    }
    /// <summary>
    /// 将翻译后的字典读取文件中
    /// </summary>
    public static void GetLanguageDictData()
    {
        string errorString = null;
        string NeedLanguageDicPath = FileModule.CombinePath(LanguageDictPath, "Language" + NeedLanguage);
        string NeedLanguageDicFile = FileModule.CombinePath(NeedLanguageDicPath, NeedLanguage + "_dict.txt");
        //if (IsGetSourceTextFile==true)
        //{
            if (!File.Exists(NeedLanguageDicFile))
            {
                return;
            }
        //}

        if (IsMoreLanguage && NeedLanguage!=null)
        {
            
            LanguageDictData = TxtConfigReader.ParseTxtConfigFile(NeedLanguageDicFile, "\t", out errorString);
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
        if (IsMoreLanguage && NeedLanguage != null)
        {
            if (LanguageDictData.ContainsKey(oldText))
                return LanguageDictData[oldText];
            else
                return oldText;
        }
        else
            return oldText;
    }
}
