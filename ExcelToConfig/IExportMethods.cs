using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 导出需实现的方法
/// </summary>
interface IExportMethods
{
    /// <summary>
    /// 获取最终导出的保存路径+文件名+后缀
    /// ExportPath+IsExportKeepDirectoryStructure+ExportNameBeforeAdd+ExportName+ExportNameAfterLanguageMark+ExportExtension
    /// </summary>
    /// <param name="ExportPath">导出路径，如：C:\Users\Administrator\Desktop</param>
    /// <param name="IsExportKeepDirectoryStructure">导出时是否按原目录结构保存</param>
    /// <param name="ExportName">导出名字，如：item</param>
    /// <param name="ExportExtension">导出后缀，如：json;erl</param>
    /// <param name="ExportNameBeforeAdd">导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_</param>
    /// <param name="ExportNameAfterLanguageMark">导出时在文件名后添加语言标识，母语通常默认为空,如_ft</param>
    /// <returns>返回最终的路径</returns>
    string GetSavePath(string ExportPath, bool IsExportKeepDirectoryStructure, string ExportName, string ExportExtension, string ExportNameBeforeAdd, string ExportNameAfterLanguageMark);


}
