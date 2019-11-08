using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ExcelFolder
{
    /// <summary>
    /// 指定Excel文件所在目录，默认为与本程序同级别的 Excel文件夹
    /// </summary>
    public static string ExcelFolderPath = FileModule.CombinePath(AppValues.ProgramFolderPath, "Excel");

    /// <summary>
    /// 指定Excel文件目录是否包含子文件夹中（默认为不包含子文件夹）
    /// </summary>
    public static bool IsIncludeSubfolder = true;

    /// <summary>
    /// 指定Excel文件目录下，所有文件（key：表名， value：文件所在路径）
    /// </summary>
    public static Dictionary<string, List<string>> AllExcelPaths = new Dictionary<string, List<string>>();

    /// <summary>
    /// 指定导出部分表（指定表名），优先判断指定导出，即如果指定部分导出，则指定部分排除无效
    /// </summary>
    public static string[] ExportPart = null;

    /// <summary>
    /// 指定排除部分表（指定表名），优先判断指定导出，即如果指定部分导出，则指定部分排除无效
    /// </summary>
    public static string[] ExportExcept = null;

    /// <summary>
    /// 存储本次要导出的Excel文件名(key:表名，value文件所在路径
    /// </summary>
    public static Dictionary<string, string> ExportTables = new Dictionary<string, string>();

    /// <summary>
    /// 获取指定文件夹内的Excel，保存到ExcelFolder.AllExcelPaths中
    /// </summary>
    /// <param name="pathString">指定文件路径</param>
    /// <param name="extension">文件扩展名</param>
    /// <param name="IncludeSubfolder">定Excel文件目录是否包含子文件夹中（默认为不包含子文件夹）</param>
    public static Dictionary<string, List<string>> getAllExcelPaths(string pathString, string extension, bool IncludeSubfolder)
    {
        Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>();
        SearchOption searchOption;
        if (IsIncludeSubfolder == true)
        {
            searchOption = SearchOption.AllDirectories;
        }
        else
        {
            searchOption = SearchOption.TopDirectoryOnly;
        }
        //获取指定文件夹夹所有Excel文件
        temp = FileModule.GetFileInfo(pathString, extension, searchOption);
        return temp;
    }

    /// <summary>
    /// 获取实际要导出的Excel，保存到ExcelFolder.ExportTables中
    /// </summary>
    /// <param name="AllPaths">指定Excel文件目录下，所有文件（key：表名， value：文件所在路径）</param>
    /// <param name="Part">指定导出部分表（指定表名），优先判断指定导出，即如果指定部分导出，则指定部分排除无效</param>
    /// <param name="Except">指定排除部分表（指定表名），优先判断指定导出，即如果指定部分导出，则指定部分排除无效</param>
    /// <returns></returns>
    public static Dictionary<string, string> getExportTables(Dictionary<string, List<string>> AllPaths, string[] Part, string[] Except)
    {
        Dictionary<string, string> temp = new Dictionary<string, string>();

        if (Part != null && Part.Count() > 0)
        {
            foreach (KeyValuePair<string, List<string>> kvp in AllExcelPaths)
            {
                if (Part.Contains(kvp.Key) && !kvp.Key.StartsWith(ExcelTableSetting.ExcelTempFileFileNameStartString))
                {
                    temp.Add(kvp.Key, kvp.Value[0]);
                }
            }
        }
        else if (Except != null && Except.Count() > 0)
        {
            foreach (KeyValuePair<string, List<string>> kvp in AllExcelPaths)
            {
                if (!Except.Contains(kvp.Key) && !kvp.Key.StartsWith(ExcelTableSetting.ExcelTempFileFileNameStartString))
                {
                    temp.Add(kvp.Key, kvp.Value[0]);
                }
            }
        }
        else
        {
            foreach (KeyValuePair<string, List<string>> kvp in AllExcelPaths)
            {
                if (!kvp.Key.StartsWith(ExcelTableSetting.ExcelTempFileFileNameStartString))
                {
                    temp.Add(kvp.Key, kvp.Value[0]);
                }
            }
        }
        return temp;
    }
}