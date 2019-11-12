using System.Collections.Generic;

public class AppValues
{
    /// <summary>
    /// 本工具所在目录ProgramFolderPath，不能用System.Environment.CurrentDirectory因为当本工具被其他程序调用时取得的CurrentDirectory将是调用者的路径
    /// </summary>
    public static string ProgramFolderPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：公共设置参数
    /// </summary>
    public const string Public_Config_PublicSetting = "PublicSetting";

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：是否包含子目录
    /// </summary>
    public const string Public_Config_IsIncludeSubfolder = "IsIncludeSubfolder";

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：是否充许数字型为空
    /// </summary>
    public const string Public_Config_IsAllowedNullNumber = "IsAllowedNullNumber";

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：是否需要检查
    /// </summary>
    public const string Public_Config_IsNeedCheck = "IsNeedCheck";

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：只导出部分表格式（优先判定）
    /// </summary>
    public const string Public_Config_OnlyExportPartExcel = "OnlyExportPartExcel";

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：指定不导出哪些表格式（后判定）
    /// </summary>
    public const string Public_Config_ExceptExportExcel = "ExceptExportExcel";

    /// <summary>
    /// App全局导出通用配置,是否调用bat脚本复制文件参数
    /// </summary>
    public const string Public_Config_IsCopy = "IsCopy";

    /// <summary>
    /// App全局导出通用配置,是否调用bat脚本复制文件
    /// </summary>
    public static bool App_Config_IsCopy = false;

    /// <summary>
    /// App全局导出通用配置,bat脚本复制文件的名字参数
    /// </summary>
    public const string Public_Config_CopyBatName = "CopyBatName";

    /// <summary>
    /// App全局导出通用配置,bat脚本复制文件的名字
    /// </summary>
    public static string App_Config_CopyBatName = "copy.bat";

    /// <summary>
    ///App全局导出通用配置，bat脚本:ClientPath目录所在路径
    /// </summary>
    public const string Public_Config_ClientPath = "ClientPath";

    /// <summary>
    /// App全局导出通用配置,用户输入的Client目录所在路径
    /// </summary>
    public static string App_Config_ClientPath = null;

    /// <summary>
    ///App全局导出通用配置，bat脚本:读取excel的方式
    /// </summary>
    public const string Public_Config_ReadExcelType = "ReadExcelType";
    /// <summary>
    /// App全局导出通用配置,读取excel的方式，OleDb 和 ExcelDataReader
    /// </summary>
    public static string App_Config_ReadExcelType = "OleDb";

    /// <summary>
    ///App全局导出通用配置，bat脚本:是否允许多个excel导出到同一个表中
    /// </summary>
    public const string Public_Config_MergeTable = "MergeTable";
    /// <summary>
    /// App全局导出通用配置,是否允许多个excel导出到同一个表中
    /// </summary>
    public static bool App_Config_MergeTable = false;

    /// <summary>
    ///App全局导出通用配置，bat脚本:是否允许多个excel导出到同一个表中
    /// </summary>
    public const string Public_Config_Error = "Error";
    /// <summary>
    /// App全局导出通用配置,是否允许多个excel导出到同一个表中
    /// </summary>
    public static bool App_Config_Error = false;

    /// <summary>
    /// 存储每张Excel表格的原始数据（key：文件完整路径）
    /// </summary>
    public static Dictionary<string, System.Data.DataSet> ExcelDataSet = new Dictionary<string, System.Data.DataSet>();

    /// <summary>
    /// 存储每张Excel表格解析成的本工具所需的数据结构（key：表名）
    /// 用于处理一个excel有多个sheet数据表情况
    /// </summary>
    public static Dictionary<string, List<TableInfo>> TableInfoList = new Dictionary<string, List<TableInfo>>();

    /// <summary>
    /// 存储每张Excel表格解析成的本工具所需的数据结构（key：表名）
    /// </summary>
    public static Dictionary<string, TableInfo> TableInfo = new Dictionary<string, TableInfo>();

    /// <summary>
    /// config文件转为键值对形式（key：配置文件中的key名， value：对应的配置规则字符串）
    /// </summary>
    public static Dictionary<string, string> ConfigData = new Dictionary<string, string>();

    /// <summary>
    ///Lang文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Config = "Config";

    /// <summary>
    ///  Lang文件配置，bat脚本：Lang文件路径
    /// </summary>
    public const string Public_Config_ConfigPath = "ConfigPath";

    /// <summary>
    /// 配置文件（配置自定义的检查规则）的文件名
    /// </summary>
    public static string ConfigPath = FileModule.CombinePath(AppValues.ProgramFolderPath, "config.txt");
}