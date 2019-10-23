using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public struct SvnStruct
    {

    /// <summary>
    ///svn文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Svn = "Svn";

    /// <summary>
    /// 更新svn文件配置，bat脚本：svn路径
    /// </summary>
    public const string Public_Config_SvnPath = "SvnPath";
    public static string SvnPath = new DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase).Parent.Parent.FullName.ToString();

    /// <summary>
    /// 更新svn文件配置，bat脚本：是否导出svn
    /// </summary>
    public const string Public_Config_IsUpdateSvn = "IsUpdateSvn";
    /// <summary>
    /// 更新svn文件配置，bat脚本：是否更新svn
    /// </summary>
    public static bool IsUpdateSvn = false;
    /// <summary>
    /// 更新svn文件配置，bat脚本：更新svn参数
    /// </summary>
    public const string Public_Config_UpdateSvnCloseonend = "UpdateSvnCloseonend";
    /// <summary>
    /// 更新svn文件配置，bat脚本：更新svn参数
    /// </summary>
    public static int UpdateSvnCloseonend=2;


    /// <summary>
    /// 更新svn文件配置，bat脚本：是否导出svn
    /// </summary>
    public const string Public_Config_IsCommitSvn = "IsCommitSvn";
    /// <summary>
    /// 更新svn文件配置，bat脚本：是否提交svn
    /// </summary>
    public static bool IsCommitSvn = false;
    /// <summary>
    /// 更新svn文件配置，bat脚本：更新svn参数
    /// </summary>
    public const string Public_Config_CommitSvnCloseonend = "CommitSvnCloseonend";
    /// <summary>
    /// 更新svn文件配置，bat脚本：更新svn参数
    /// </summary>
    public static int CommitSvnCloseonend = 0;
}

