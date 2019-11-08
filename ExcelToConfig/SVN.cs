using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public static class SVN
{
    private static string _svnPath = "";

    public static void DelAndUpdateSvnFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        ExecuteProcess(GetSvnProcPath(), string.Format("/command:update /path:{0} /closeonend:2", path));
    }

    public static void RevertAndUpdateSvnDirectory(string path)
    {
        ExecuteProcess(GetSvnProcPath(), string.Format("/command:revert -r /path:{0} /closeonend:2", path));
        ExecuteProcess(GetSvnProcPath(), string.Format("/command:update /path:{0} /closeonend:2", path));
    }

    /// <summary>
    /// 更新SVN，默认如果没发生错误和冲突则自动关闭对话框
    /// </summary>
    /// <param name="path"></param>
    /// <param name="closeonend"></param>
    public static void UpdateSvnDirectory(string path, int closeonend = 2)
    {
        ExecuteProcess(GetSvnProcPath(), string.Format("/command:update /path:{0} /closeonend:{1}", path, closeonend));
    }

    /// <summary>
    /// 提交SVN，默认不自动关闭对话框
    /// </summary>
    /// <param name="path"></param>
    /// <param name="closeonend"></param>
    public static void CommitSvnDirectory(string path, int closeonend = 0)
    {
        ExecuteProcess(GetSvnProcPath(), string.Format("/command:commit /path:{0} /closeonend:{1}", path, closeonend));
    }

    public static void ProcessSvnCommand(string command)
    {
        ExecuteProcess(GetSvnProcPath(), command);
    }

    private static List<string> drives = new List<string>() { "c:", "d:", "e:", "f:" };

    private static string svnPath = @"\Program Files\TortoiseSVN\bin\";
    private static string svnProc = @"TortoiseProc.exe";
    private static string svnProcPath = "";

    private static string GetSvnProcPath()
    {
        if (_svnPath != string.Empty)
        {
            return _svnPath;
        }
        foreach (string item in drives)
        {
            string path = string.Concat(item, svnPath, svnProc);
            if (File.Exists(path))
            {
                _svnPath = path;
                break;
            }
        }
        if (_svnPath == string.Empty)
        {
            _svnPath = "";// OpenFilePanel("Select TortoiseProc.exe", "c:\\", "exe");
        }
        //可将路径存到本地注册表
        return _svnPath;
    }

    /// <summary>
    /// 对本地或远程进程进行访问，以及启动或停止本地进程。
    /// </summary>
    /// <param name="filePath">exe路径</param>
    /// <param name="command">exe命令参数</param>
    /// <param name="workPath">工作路径</param>
    /// <param name="seconds">等待进程等待时间，0无限等待，其他毫秒</param>
    public static void ExecuteProcess(string filePath, string command, string workPath = "", int seconds = 0)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        Process process = new Process();//创建进程对象
        process.StartInfo.WorkingDirectory = workPath;
        process.StartInfo.FileName = filePath;
        process.StartInfo.Arguments = command;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = false;//不重定向输出
        try
        {
            if (process.Start())
            {
                if (seconds == 0)
                {
                    process.WaitForExit(); //无限等待进程结束
                }
                else
                {
                    process.WaitForExit(seconds); //等待毫秒
                }
            }
        }
        catch (Exception e)
        {
            AppLog.LogErrorAndExit(e.Message);
        }
        finally
        {
            process.Close();
        }
    }
}