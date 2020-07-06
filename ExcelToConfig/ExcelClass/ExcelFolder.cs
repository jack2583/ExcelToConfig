using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;

public class ExcelFolder
{
    //"ReadParam(ExcelPath=E:/亮剑H5/配置文档/Excel配置2|IsIncludeSubfolder=true|TheLanguage=_ft|IsNeedCheckParam=true|IsAllowedNullNumber=true)"
    public static string ReadParam = "PublicSetting";//"ReadParam";
    /// <summary>
    /// 指定Excel文件所在目录，默认为与本程序同级别的 Excel文件夹
    /// </summary>
    public static string ExcelPathParam = "ExcelPath";
    public static string ExcelPath = FileModule.CombinePath(AppValues.ProgramFolderPath, "Excel");

    /// <summary>
    /// 指定Excel文件目录是否包含子文件夹中（默认为不包含子文件夹）
    /// </summary>
    public static string IsIncludeSubfolderParam = "IsIncludeSubfolder";
    public static bool IsIncludeSubfolder = true;

    /// <summary>
    /// 指定某个语言，如繁体、英语
    /// 指定后则只读取以_ft _English结尾的Excel文件，不指定则默认只读简体
    /// </summary>
    public static string TheLanguageParam = "Language";
    public static string TheLanguage = "";

    /// <summary>
    /// 是否需要检查表格
    /// </summary>
    public static string IsNeedCheckParam = "IsNeedCheck";
    public static bool IsNeedCheck = true;

    /// <summary>
    /// 是否允许int、float型字段中存在空值
    /// </summary>
    public static string IsAllowedNullNumberParam = "IsAllowedNullNumber";
    public static bool IsAllowedNullNumber = true;

    /// <summary>
    /// 存储本次要导出的Excel文件名(key:表名，value文件所在路径
    /// </summary>
    public static Dictionary<string, string> ExportTables = new Dictionary<string, string>();


    /// <summary>
    /// 指定Excel文件目录下，所有文件（key：表名， value：文件所在路径）
    /// 如原Excel名为：daily_activity_ft-日常活动.xlsx，则表名为：daily_activity
    /// </summary>
    private static Dictionary<string, List<string>> AllExcelPaths = new Dictionary<string, List<string>>();
    /// <summary>
    /// 获取有效Excel文件,是否需要检查表格, 是否允许int、float型字段中存在空值
    /// </summary>
    public static void GetExportTables()
    {
        GetParamValue();
        CheckExcelPath();


        AllExcelPaths = RemoveTempFile(AllExcelPaths, ExcelTableSetting.ExcelTempFileFileNameStartString);
        FileModule.CheckSameName(AllExcelPaths, "xlsx");
        ExportTables=_getExportTables(AllExcelPaths, ExportPart, ExportExcept);
    }

    private static void GetParamValue()
    {
        if (AppValues.BatParamInfo.ContainsKey(ReadParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[ReadParam].ChildParam;
            ExcelPath = BatMethods.GetStringValue(ExcelPath, ChildParam, ExcelPathParam);
            IsIncludeSubfolder = BatMethods.GetBoolValue(IsIncludeSubfolder, ChildParam, IsIncludeSubfolderParam);
            TheLanguage = BatMethods.GetStringValue(TheLanguage, ChildParam, TheLanguageParam);
            IsNeedCheck = BatMethods.GetBoolValue(IsNeedCheck, ChildParam, IsNeedCheckParam);
            IsAllowedNullNumber = BatMethods.GetBoolValue(IsAllowedNullNumber, ChildParam, IsAllowedNullNumberParam);
        }

        if (AppLanguage.AppConfigIDArr == null && TheLanguage != "")
        {
            AppLog.LogErrorAndExit(string.Format("多语言配置错误:在Bat只设置了{0}({1}={2})，则必须在config.txt中设置{3}:{4}，格式如：MoreLanguage:_ft,_English", ReadParam, TheLanguageParam, TheLanguage,AppLanguage.AppConfigIDArrParam, TheLanguage));
        }
    }

    private static void CheckExcelPath()
    {
        
        //判断指定的Excel文件是否存在
        if (!Directory.Exists(ExcelPath))
            AppLog.LogErrorAndExit(string.Format("错误!!! 输入的Excel表格所在目录不存在，路径为:{0}", ExcelPath));

        ExcelPath = Path.GetFullPath(ExcelPath);
        AppLog.Log(string.Format("提示: 您选择的Excel所在路径：{0}", ExcelPath));


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
        AllExcelPaths = FileModule.GetFileInfo(ExcelPath, "xlsx", searchOption);

    }
    /// <summary>
    /// 去除以指定字符开头的临时文件
    /// </summary>
    /// <param name="allFilePaths">全部文件</param>
    /// <param name="isMoreLangue">是否为多语言</param>
    /// <param name="TempFileFileNameStartString">临时文件前缀</param>
    /// <returns></returns>
    private static Dictionary<string, List<string>> RemoveTempFile(Dictionary<string, List<string>> allFilePaths, string TempFileFileNameStartString = ExcelTableSetting.ExcelTempFileFileNameStartString)
    {
        Dictionary<string, List<string>> allFilePathsTemp = new Dictionary<string, List<string>>();
        if(AppLanguage.AppConfigIDArr == null)//只有基本语言
        {
            foreach (KeyValuePair<string, List<string>> kvp in allFilePaths)
            {
                if (!kvp.Key.StartsWith(TempFileFileNameStartString))
                {
                    allFilePathsTemp.Add(kvp.Key, kvp.Value);
                }
            }
        }
        else//多语言
        {
            Dictionary<string, List<string>> allFilePathsTemp2 = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, List<string>> kvp in allFilePaths)
            {
                //指定语言直接保存
                if (TheLanguage !="" && (ExcelMethods.GetTableName(kvp.Key)).EndsWith(TheLanguage))
                {
                    allFilePathsTemp.Add(ExcelMethods.GetTableName(kvp.Key), kvp.Value);
                    continue;
                }

                bool isNeedLanguage = true;
                foreach (string str in AppLanguage.AppConfigIDArr)
                {
                    if (ExcelMethods.GetTableName(kvp.Key).EndsWith(str))
                    {
                        isNeedLanguage = false;
                        break;
                    }
                }

                if (isNeedLanguage == true)
                    allFilePathsTemp2.Add(kvp.Key, kvp.Value);
                else
                    AppLog.LogWarning(string.Format("文件：{0}，已忽略，因为不是指定的语言", kvp.Key));
            }

            foreach (KeyValuePair<string, List<string>> kvp in allFilePathsTemp2)
            {
                if(!allFilePathsTemp.ContainsKey(ExcelMethods.GetTableName(kvp.Key)+ TheLanguage))
                    allFilePathsTemp.Add(ExcelMethods.GetTableName(kvp.Key), kvp.Value);
            }
        }
        return allFilePathsTemp;
    }
    /// <summary>
    /// 指定导出部分表（指定表名），优先判断指定导出，即如果指定部分导出，则指定部分排除无效
    /// </summary>
    public static string[] ExportPart = null;

    /// <summary>
    /// 指定排除部分表（指定表名），优先判断指定导出，即如果指定部分导出，则指定部分排除无效
    /// </summary>
    private static string[] ExportExcept = null;


    /// <summary>
    /// 获取指定文件夹内的Excel，保存到ExcelFolder.AllExcelPaths中
    /// </summary>
    /// <param name="pathString">指定文件路径</param>
    /// <param name="extension">文件扩展名</param>
    /// <param name="IncludeSubfolder">定Excel文件目录是否包含子文件夹中（默认为不包含子文件夹）</param>
    private static Dictionary<string, List<string>> getAllExcelPaths(string pathString, string extension, bool IncludeSubfolder)
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
    public static Dictionary<string, string> _getExportTables(Dictionary<string, List<string>> AllPaths, string[] Part, string[] Except)
    {

        Dictionary<string, string> temp = new Dictionary<string, string>();
        foreach (KeyValuePair<string, List<string>> kvp in AllExcelPaths)
        {
            if (!kvp.Key.StartsWith(ExcelTableSetting.ExcelTempFileFileNameStartString))
            {
                string k = kvp.Key;
                if (TheLanguage != "" && k.EndsWith(TheLanguage))
                {
                    k = k.Substring(0, k.Length - TheLanguage.Length);

                }
                temp.Add(k, kvp.Value[0]);
            }
        }
        return temp;
    }
}