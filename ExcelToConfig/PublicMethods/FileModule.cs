using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 文件相关方法
/// </summary>
public class FileModule
{
    /// <summary>
    /// 在指定文件夹下，某个文件名对应的所有文件路径（key：文件名， value：文件所在路径）
    /// </summary>
    public static Dictionary<string, List<string>> AllPathsOfName = new Dictionary<string, List<string>>();

    /// <summary>
    /// Dictionary<路径+(.扩展名)TopDirectoryOnly/AllDirectories, Dictionary<文件名, List<该文件对应所有完整文件路径>>>
    /// </summary>
    public static Dictionary<string, Dictionary<string, List<string>>> AllFileInfos = new Dictionary<string, Dictionary<string, List<string>>>();

    /// <summary>
    /// 读取文件到字典
    /// </summary>
    /// <param name="pathString">指定路径</param>
    /// <param name="extension">扩展名</param>
    /// <param name="searchOption">搜索子目录还是不搜索</param>
    /// <returns></returns>
    public static Dictionary<string, List<string>> GetFileInfo(string pathString, string extension, SearchOption searchOption)
    {
        string fileInFokey = pathString + @"(." + extension + @")" + searchOption.ToString();
        if (AllFileInfos.ContainsKey(fileInFokey))//如果存在就直接返回
        {
            return AllFileInfos[fileInFokey];
        }

        //如果不存在就进行添加
        AllFileInfos.Add(fileInFokey, new Dictionary<string, List<string>>());
        // string[] filePaths = Directory.GetFiles(pathString, "*." + extension, searchOption);
        var filePaths = Directory.EnumerateFiles(pathString, "*." + extension, searchOption);
        foreach (string filepath in filePaths)
        {
            string filename = Path.GetFileNameWithoutExtension(filepath);
            if (AllFileInfos[fileInFokey].ContainsKey(filename))
            {
                AllFileInfos[fileInFokey][filename].Add(filepath);
            }
            else
            {
                AllFileInfos[fileInFokey].Add(filename, new List<string> { filepath });
            }
        }
        return AllFileInfos[fileInFokey];
    }

    /// <summary>
    /// 去除以指定字符开头的临时文件
    /// </summary>
    /// <param name="allFilePaths">全部文件</param>
    /// <param name="isMoreLangue">是否为多语言</param>
    /// <param name="TempFileFileNameStartString">临时文件前缀</param>
    /// <returns></returns>
    public static Dictionary<string, List<string>> RemoveTempFile(Dictionary<string, List<string>> allFilePaths, bool isMoreLangue, string TempFileFileNameStartString = ExcelTableSetting.ExcelTempFileFileNameStartString)
    {
        Dictionary<string, List<string>> allFilePathsTemp = new Dictionary<string, List<string>>();
        if (isMoreLangue == false)
        {
            foreach (KeyValuePair<string, List<string>> kvp in allFilePaths)
            {
                if (!kvp.Key.StartsWith(TempFileFileNameStartString))
                {
                    allFilePathsTemp.Add(kvp.Key, kvp.Value);
                }
            }
        }
        else//多语言下处理
        {
            if (AppLanguage.OtherLanguage == null)
            {
                foreach (KeyValuePair<string, List<string>> kvp in allFilePaths)
                {
                    if (!kvp.Key.StartsWith(TempFileFileNameStartString))
                    {
                        allFilePathsTemp.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, List<string>> kvp in allFilePaths)
                {
                    if (!kvp.Key.StartsWith(TempFileFileNameStartString))
                    {
                        bool isNeedLanguage = true;
                        foreach (string str in AppLanguage.OtherLanguage)
                        {
                            if (ExcelMethods.GetTableName(kvp.Key).EndsWith(str))
                            {
                                isNeedLanguage = false;
                                break;
                            }
                        }
                        if (isNeedLanguage == true)
                            allFilePathsTemp.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }
        return allFilePathsTemp;
    }

    /// <summary>
    /// 检查是否存在同名文件，若存在则中止程序
    /// </summary>
    /// <param name="allFilePaths">指定的全部文件</param>
    /// <param name="extension">扩展名</param>
    public static void CheckSameName(Dictionary<string, List<string>> allFilePaths, string extension)
    {
        string SameNameFileTemp = null;
        foreach (KeyValuePair<string, List<string>> kvp in allFilePaths)
        {
            if (kvp.Value.Count > 1)
            {
                SameNameFileTemp = SameNameFileTemp + "\n存在同名" + extension + "文件：" + kvp.Key + "，位置如下：";
                foreach (string st in kvp.Value)
                {
                    SameNameFileTemp = SameNameFileTemp + "\n" + st;
                }
            }
        }
        //判断是否存在同名文件，若存在则打印出来，退出程序
        if (SameNameFileTemp == null)
        {
            // AppLog.Log(string.Format("Excel同名检查完毕，没有发现同名文件\n"), ConsoleColor.Green);
        }
        else
        {
            AppLog.LogError(SameNameFileTemp);
            AppLog.LogError("\n存在上面所列同名文件，包含子文件夹都不充许存在同名文件\n");
            AppLog.SaveErrorInfoToFile("错误日志");
            AppLog.LogErrorAndExit("\n按任意键继续");
        }
    }

    /// <summary>
    /// 合并两个路径字符串，与.Net类库中的Path.Combine不同，本函数不会因为path2以目录分隔符开头就认为是绝对路径，然后直接返回path2
    /// </summary>
    /// <param name="path1">第1个路径</param>
    /// <param name="path2">第2个路径</param>
    /// <returns></returns>
    public static string CombinePath(string path1, string path2)
    {
        path1 = path1.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        path2 = path2.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        if (path2.StartsWith(Path.DirectorySeparatorChar.ToString()))
            path2 = path2.Substring(1, path2.Length - 1);

        return Path.Combine(path1, path2);
    }

    /// <summary>
    /// 获取导出某种生成文件的存储目录
    /// </summary>
    public static string GetExportDirectoryPath(string tableName, string exportRootPath, bool IsExportKeepDirectoryStructure, bool IsRemoveChinese = true)
    {
        if (IsExportKeepDirectoryStructure == true)
        {
            // 获取表格相对于Excel文件夹的相对路径
            string excelFolderPath = ExcelFolder.ExcelFolderPath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
            if (!excelFolderPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                excelFolderPath = excelFolderPath + Path.DirectorySeparatorChar;

            Uri excelFolderUri = new Uri(excelFolderPath);
            Uri fileUri = new Uri(ExcelFolder.ExportTables[tableName]);
            Uri relativeUri = excelFolderUri.MakeRelativeUri(fileUri);
            // 注意：Uri转为的字符串中会将中文转义为%xx，需要恢复为非转义形式
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (IsRemoveChinese == true)
            {
                string[] SplitRelativePath = relativePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string[] tempSplitRelativePath;
                relativePath = "";
                for (int i = 0; i < SplitRelativePath.Length - 1; i++)
                {
                    tempSplitRelativePath = SplitRelativePath[i].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    relativePath = relativePath + tempSplitRelativePath[0] + "/";
                }
                relativePath = relativePath + SplitRelativePath[SplitRelativePath.Length - 1];
            }

            return Path.GetDirectoryName(CombinePath(exportRootPath, relativePath));
        }
        else
            return exportRootPath;
    }
}