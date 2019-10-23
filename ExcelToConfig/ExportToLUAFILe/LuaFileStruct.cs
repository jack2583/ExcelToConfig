using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct LuaFileStruct
{
    /// <summary>
    /// 存储本次要额外导出为LuaFile文件的Excel文件名
    /// </summary>
    public static List<string> ExportTableNames = new List<string>();


    /// <summary>
    /// 导出LuaFile文件的扩展名（不含点号），默认为lua
    /// </summary>
    public static string SaveExtension = "lua";
    /// <summary>
    ///导出LuaFile文件配置，bat脚本
    /// </summary>
    public const string Public_Config_Export = "ExportLuaFile";
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：是否导出LuaFile
    /// </summary>
    public const string Public_Config_IsExport = "IsExport";
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：是否需要导出erlang
    /// </summary>
    public static bool IsExport = false;
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：导出LuaFile的储存路径
    /// </summary>
    public const string Public_Config_ExportPath = "ExportPath";
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：保存LuaFile文件路径
    /// </summary>
    public static string SavePath = FileModule.CombinePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SaveLuaFile");
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：是否按原目录结构保存
    /// </summary>
    public const string Public_Config_IsExportKeepDirectoryStructure = "IsExportKeepDirectoryStructure";
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：是否将生成的文件按原Excel文件所在的目录结构进行存储（默认:是）
    /// </summary>
    public static bool IsExportKeepDirectoryStructure = true;
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：是否需要生成头部信息
    /// </summary>
    public const string Public_Config_IsNeedColumnInfo = "IsNeedColumnInfo";
    /// <summary>
    /// 导出LuaFile文件配置，bat脚本：用户输入的是否需要在生成lua文件的最上方用注释形式显示列信息（默认为不需要）
    /// </summary>
    public static bool IsNeedColumnInfo = false;

    /// <summary>
    /// 某个Excel表的配置：声明某张表格导出为erlang table时，是否将主键列的值作为table中的元素 
    /// </summary>
    public const string Excel_Config_AddKeyToLuaFileTable = "addKeyToLuaFileTable";
    /// <summary>
    /// 某个Excel表的配置：声明对某张表格不进行默认导出erlang的参数配置 
    /// </summary>
    public const string Excel_Config_NotExportLuaFileOriginalTable = "-NotExportLuaFileOriginalTable";
    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置特殊导出Erlang规则的配置参数名  
    /// </summary>
    public const string Excel_Config_TableSpecialExportLuaFile = "TableSpecialExportLuaFile";

    /// <summary>
    /// 某个Excel表的配置：声明对某张表格设置导出Erlang时是否忽略int等数字类型中的空值，如果忽略则不导出该字段。默认不忽略.false/true
    /// </summary>
    public const string Excel_Config_NotExportLuaFileNullNumber = "NotExportLuaFileNullNumber";

}
