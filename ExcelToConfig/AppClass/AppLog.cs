using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 本工具控制台程序Log日志相关
/// </summary>
public  class AppLog
{

    #region Log日志文本类型：普通/警告/错误
    /// <summary>
    /// Log日志文本存储(普通日志）
    /// </summary>
    public static StringBuilder LogContent = new StringBuilder();
    /// <summary>
    ///  警告文本存储
    /// </summary>
    public static StringBuilder LogWarningContent = new StringBuilder();
    /// <summary>
    ///  错误文本存储
    /// </summary>
    public static StringBuilder LogErrorContent = new StringBuilder();
    #endregion
    /// <summary>
    ///AppLog文件配置，bat脚本
    /// </summary>
    public const string Public_Config_AppLog = "AppLog";
    #region 是否打印Log
    /// <summary>
    /// 是否打印Log日志（普通日志）
    /// </summary>
    public static bool IsPrintLog = true;
    public const string Public_Config_IsPrintLog = "IsPrintLog";
    /// <summary>
    /// 是否打印警告信息
    /// </summary>
    public static bool IsPrintLogWarning = false;
    public const string Public_Config_IsPrintLogWarning = "IsPrintLogWarning";
    /// <summary>
    /// 是否打印错误信息
    /// </summary>
    public static bool IsPrintLogError = true;
    public const string Public_Config_IsPrintLogError = "IsPrintLogError";
    #endregion
    #region 是否保存Log
    /// <summary>
    /// 是否保存Log日志（普通日志）
    /// </summary>
    public static bool IsSaveLog = false;
    public const string Public_Config_IsSaveLog = "IsSaveLog";
    /// <summary>
    /// 是否保存警告信息
    /// </summary>
    public static bool IsSaveLogWarning = false;
    public const string Public_Config_IsSaveLogWarning = "IsSaveLogWarning";
    /// <summary>
    /// 是否保存错误信息
    /// </summary>
    public static bool IsSaveLogError = false;
    public const string Public_Config_IsSaveLogError = "IsSaveLogError";
    #endregion
    /// <summary>
    /// Log打印保存(普通日志）
    /// </summary>
    /// <param name="logString">Log字符串信息</param>
    /// <param name="color">控制台文字颜色，默认White</param>
    public static void Log(string logString, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        if(IsPrintLog==true)
            Console.WriteLine(logString);
        //if(IsSaveLog==true)
        LogContent.AppendLine(logString);
    }
    /// <summary>
    /// 警告信息打印或保存
    /// </summary>
    /// <param name="warningString">警告字符串信息</param>
    /// <param name="color">控制台文字颜色，默认Yellow</param>
    public static void LogWarning(string warningString, ConsoleColor color = ConsoleColor.Yellow)
    {
        Console.ForegroundColor = color;
        if (IsPrintLogWarning == true)
            Console.WriteLine(warningString);
        //if (IsSaveLogWarning == true)
        LogWarningContent.AppendLine(warningString);
        
    }
    /// <summary>
    /// 错误信息打印或保存
    /// </summary>
    /// <param name="errorString">错误信息字符串</param>
    /// <param name="color">控制台文字颜色，默认Red</param>
    public static void LogError(string errorString, ConsoleColor color = ConsoleColor.Red)
    {
        Console.ForegroundColor = color;
        if (IsPrintLogError == true)
            Console.WriteLine(errorString);
        //if (IsSaveLogError == true)
        LogErrorContent.AppendLine(errorString);
    }

    /// <summary>
    /// 控制台打印错误信息并在用户按任意键后退出程序
    /// </summary>
    /// <param name="errorString">错误信息字符串</param>
    /// <param name="color">控制台文字颜色，默认Red</param>
    public static void LogErrorAndExit(string errorString, ConsoleColor color = ConsoleColor.Red)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(errorString);
        LogErrorContent.AppendLine(errorString);
        AppLog.SaveErrorInfoToFile("错误日志");
        Console.WriteLine("程序被迫退出，请修正错误后重试");
        Console.ReadKey();
        Environment.Exit(0);
    }

    /// <summary>
    /// 将程序运行中检查出的所有错误保存到文本文件中，存储目录为本工具所在目录
    /// </summary>
    /// <param name="SaveName">日志信息保存时的文件名，系统会在文件名后自动添加保存时的时间</param>
    /// <returns></returns>
    public static bool SaveErrorInfoToFile(string SaveName)
    {
        try
        {
            StringBuilder LogStr = new StringBuilder();
            if (IsSaveLog == true)
            {
                if(LogContent.Length>0)
                {
                    LogStr.AppendLine();
                    LogStr.AppendLine(LogContent.ToString().Replace("\n", System.Environment.NewLine));
                }
               
            }
            if(IsSaveLogWarning==true)
            {
                if(LogWarningContent.Length>0)
                {
                    LogStr.AppendLine();
                    LogStr.AppendLine(LogWarningContent.ToString().Replace("\n", System.Environment.NewLine));
                }
               
            }
            if(IsSaveLogError==true)
            {
                if(LogErrorContent.Length>0)
                {
                    LogStr.AppendLine();
                    LogStr.AppendLine(LogErrorContent.ToString().Replace("\n", System.Environment.NewLine));
                }
              
            }
            if(LogStr.Length>0)
            {
                string fileNameTime = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒}.txt", DateTime.Now);
                string fileName = string.Format("{0}{1}", SaveName, fileNameTime);
                string path1 = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string savePath = FileModule.CombinePath(path1, fileName);
                StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
                writer.WriteLine(LogStr.ToString().Replace("\n", System.Environment.NewLine));
                writer.Flush();
                writer.Close();

                Log(string.Format("错误信息已全部导出txt文件，文件名为\"{0}\"，存储在本工具所在目录下", fileName),ConsoleColor.Red);
            }
           
            return true;
        }
        catch
        {
            LogError("错误信息保存到txt文件时失败");
            return false;
        }
    }
}
