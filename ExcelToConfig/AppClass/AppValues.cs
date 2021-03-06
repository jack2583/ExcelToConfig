﻿using System.Collections.Generic;

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
    /// App全局导出lua、json等通用配置，bat脚本：ref检查时是否忽略找不到的表格
    /// </summary>
    public const string Public_Config_IsRefCheckNotTable = "IsRefCheckNotTable";
    /// <summary>
    /// App全局导出通用配置,ref检查时是否忽略找不到的表格
    /// </summary>
    public static bool IsRefCheckNotTable = false;

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：只导出部分表格式（优先判定）
    /// </summary>
    public const string Public_Config_OnlyExportPartExcel = "OnlyExportPartExcel";

    /// <summary>
    /// App全局导出lua、json等通用配置，bat脚本：指定不导出哪些表格式（后判定）
    /// </summary>
    public const string Public_Config_ExceptExportExcel = "ExceptExportExcel";

    /// <summary>
    ///App全局导出通用配置，bat脚本:ClientPath目录所在路径
    /// </summary>
    public const string Public_Config_ClientPath = "ClientPath";

    /// <summary>
    /// App全局导出通用配置,用户输入的Client目录所在路径
    /// </summary>
    public static string App_Config_ClientPath = null;

    /// <summary>
    ///App全局导出通用配置，bat脚本:读取excel的方式 ExcelDataReader OleDb
    /// </summary>
    public const string Public_Config_ReadExcelType = "ReadExcelType";

    /// <summary>
    /// App全局导出通用配置,读取excel的方式，OleDb 和 ExcelDataReader
    /// </summary>
    public static string App_Config_ReadExcelType = "ExcelDataReader";

    /// <summary>
    ///App全局导出通用配置，bat脚本:合并导出相关
    /// </summary>
    public const string Public_Config_MergeTable = "MergeTable";

    /// <summary>
    /// App全局导出通用配置，bat脚本:是否允许多个excel导出到同一个表中
    /// </summary>
    public const string Public_Config_IsExport = "IsMerge";

    /// <summary>
    /// App全局导出通用配置,是否允许多个excel导出到同一个表中
    /// </summary>
    public static bool IsMerge = false;

    /// <summary>
    /// App全局导出通用配置，bat脚本:合并后的表是否导出单个
    /// </summary>
    public const string Public_Config_ExportSingle = "IsMergeSingle";

    /// <summary>
    /// App全局导出通用配置,是否允许合并后的表是否导出单个
    /// </summary>
    public static bool IsExportSingle = false;

    /// <summary>
    ///  App全局导出通用配置,key=合并导出的tablename,list要合并的哪些表（不带中文）
    /// </summary>
    public static Dictionary<string, string[]> MergeList = new Dictionary<string, string[]>();

    public static Dictionary<string, List<TableInfo>> MergeTableList = new Dictionary<string, List<TableInfo>>();

    /// <summary>
    /// 合并过的表格不导出
    /// </summary>
    public static List<string> MergerTableName = new List<string>();

    /// <summary>
    ///App全局导出通用配置，bat脚本:是否打印详错误
    /// </summary>
    public const string Public_Config_Error = "IsError";

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
    /// 存储每个bat参数解析后的信息："MergeTable(IsMerge=true|item=item100,item101)"
    /// </summary>
    public static Dictionary<string, BatParamInfo> BatParamInfo = new Dictionary<string, BatParamInfo>();
    /// <summary>
    /// 未对某字段命名时，默认给予的字段名前缀  AUTO_FIELD_NAME_PREFIX
    /// </summary>
    public static string AutoFieldNamePrefix = "未命名字段";
}