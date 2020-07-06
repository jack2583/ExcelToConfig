using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 本工具控制台程序Log日志相关
/// </summary>
public class AppLog
{
    //AppLog(IsPrintLog=true|IsPrintLogWarning=true|IsPrintLogError=true|IsSaveLog=true|IsSaveLogWarning=true|IsSaveLogError=true)
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

    #endregion Log日志文本类型：普通/警告/错误

    /// <summary>
    ///AppLog文件配置，bat脚本
    /// </summary>
    public static string AppLogParam = "AppLog";

    public static bool IsPrintLog = true;
    public static string IsPrintLogParam = "IsPrintLog";
    public static bool IsPrintLogWarning = false;
    public static string IsPrintLogWarningParam = "IsPrintLogWarning";
    public static bool IsPrintLogError = true;
    public static string IsPrintLogErrorParam = "IsPrintLogError";
    public static bool IsSaveLog = false;
    public static string IsSaveLogParam = "IsSaveLog";
    public static bool IsSaveLogWarning = false;
    public static string IsSaveLogWarningParam = "IsSaveLogWarning";
    public static bool IsSaveLogError = false;
    public static string IsSaveLogErrorParam = "IsSaveLogError";

    public static void GetParamValue()
    {
        if (AppValues.BatParamInfo.ContainsKey(AppLogParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[AppLogParam].ChildParam;
            IsPrintLog = BatMethods.GetBoolValue(IsPrintLog, ChildParam, IsPrintLogParam);
            IsPrintLogWarning = BatMethods.GetBoolValue(IsPrintLogWarning, ChildParam, IsPrintLogWarningParam);
            IsPrintLogError = BatMethods.GetBoolValue(IsPrintLogError, ChildParam, IsPrintLogErrorParam);
            IsSaveLog = BatMethods.GetBoolValue(IsSaveLog, ChildParam, IsSaveLogParam);
            IsSaveLogWarning = BatMethods.GetBoolValue(IsSaveLogWarning, ChildParam, IsSaveLogWarningParam);
            IsSaveLogError = BatMethods.GetBoolValue(IsSaveLogError, ChildParam, IsSaveLogErrorParam);
        }
    }

    /// <summary>
    /// Log打印保存(普通日志）
    /// </summary>
    /// <param name="logString">Log字符串信息</param>
    /// <param name="color">控制台文字颜色，默认White</param>
    public static void Log(string logString, ConsoleColor color = ConsoleColor.White, bool Line = true)
    {
        if (Line == true)
        {
            Console.ForegroundColor = color;
            if (IsPrintLog == true)
                Console.WriteLine(logString);
            //if(IsSaveLog==true)
            LogContent.AppendLine(logString);
        }
        else
        {
            Console.ForegroundColor = color;
            if (IsPrintLog == true)
                Console.Write(logString);
            //if(IsSaveLog==true)
            LogContent.Append(logString);
        }

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
        AppLog.SaveErrorInfoToFile("错误日志.txt");
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
                if (LogContent.Length > 0)
                {
                    LogStr.Append(LogContent.ToString().Replace("\n", ""));
                }
            }
            if (IsSaveLogWarning == true)
            {
                if (LogWarningContent.Length > 0)
                {
                    LogStr.Append(LogWarningContent.ToString().Replace("\n", System.Environment.NewLine));
                }
            }
            if (IsSaveLogError == true)
            {
                if (LogErrorContent.Length > 0)
                {
                    LogStr.Append(LogErrorContent.ToString().Replace("\n", System.Environment.NewLine));
                }
            }
            if (LogStr.Length > 0)
            {
               // string fileNameTime = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒}.txt", DateTime.Now);
                string fileName = SaveName;// string.Format("{0}{1}", SaveName, fileNameTime);
                string path1 = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string savePath = FileModule.CombinePath(path1, fileName);
                StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
                writer.WriteLine(LogStr.ToString().Replace("\n", System.Environment.NewLine));
                writer.Flush();
                writer.Close();

               // Log(string.Format("错误信息已全部导出txt文件，文件名为\"{0}\"，存储在本工具所在目录下", fileName), ConsoleColor.Red);
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
interface IAppLogSetting
{
    /// <summary>
    /// 参数名，如：AppLog
    /// </summary>
    string AppLogParam { set; get; }
    /// <summary>
    /// 参数名：是否打印普通日志
    /// </summary>
    string IsPrintLogParam { set; get; }
    /// <summary>
    /// 参数值：是否打印普通日志
    /// </summary>
    bool IsPrintLog { set; get; }
    /// <summary>
    /// 参数名：是否打印警告日志
    /// </summary>
    string IsPrintLogWarningParam { set; get; }
    /// <summary>
    /// 参数值：是否打印警告日志
    /// </summary>
    bool IsPrintLogWarning { set; get; }
    /// <summary>
    /// 参数名：是否打印普通日志
    /// </summary>
    string IsPrintLogErrorParam { set; get; }
    /// <summary>
    /// 参数值：是否打印普通日志
    /// </summary>
    bool IsPrintLogError { set; get; }
    /// <summary>
    /// 参数名：是否将普通日志为txt输出
    /// </summary>
    string IsSaveLogParam { set; get; }
    /// <summary>
    /// 参数值：是否普通日志txt输出
    /// </summary>
    bool IsSaveLog { set; get; }
    /// <summary>
    /// 参数名：是否将警告日志为txt输出
    /// </summary>
    string IsSaveLogWarningParam { set; get; }
    /// <summary>
    /// 参数值：是否警告日志txt输出
    /// </summary>
    bool IsSaveLogWarning { set; get; }
    /// <summary>
    /// 参数名：是否将普通日志为txt输出
    /// </summary>
    string IsSaveLogErrorParam { set; get; }
    /// <summary>
    /// 参数值：是否普通日志txt输出
    /// </summary>
    bool IsSaveLogError { set; get; }
}