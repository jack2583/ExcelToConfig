using System;
using System.Collections.Generic;
using System.Data;


public class ExcelMethods
{
    /// <summary>
    /// 将Excel中的列编号转为列名称（第1列为A，第28列为AB）
    /// </summary>
    public static string GetExcelColumnName(int columnNumber)
    {
        string columnName = string.Empty;

        if (columnNumber <= 26)
            columnName = ((char)('A' + columnNumber - 1)).ToString();
        else
        {
            int quotient = columnNumber / 26;
            int remainder = columnNumber % 26;
            char first;
            char second;
            if (remainder == 0)
            {
                first = (char)('A' + quotient - 2);
                second = 'Z';
            }
            else
            {
                first = (char)('A' + quotient - 1);
                second = (char)('A' + remainder - 1);
            }

            columnName = string.Concat(first, second);
        }

        return columnName;
    }

    /// <summary>
    /// 获取Excel对应的表名，可将形如 item-物品 的excel获取出表名为：item
    /// </summary>
    /// <param name="excelName">Excel名字，如item-物品 或item</param>
    /// <returns></returns>
    public static string GetTableName(string excelName, string splitStr = "-")
    {
        if (excelName.Contains(splitStr))
        {
            string[] fileNames = null;
            fileNames = excelName.Split(new string[] { splitStr }, StringSplitOptions.RemoveEmptyEntries);
            excelName = fileNames[0];
        }

        return excelName;
    }

    /// <summary>
    /// 获取多语言下导出文件的真实名字
    /// </summary>
    /// <param name="tableName">TableInfo.tableName</param>
    /// <returns></returns>
    public static string GetSaveTableName(string tableName)
    {
        if (AppLanguage.IsAddSaveType == true || AppLanguage.IsMoreLanguage == false || AppLanguage.NeedLanguage == null || tableName.EndsWith(AppLanguage.NeedLanguage))
        {
            return tableName;
        }
        else
            return tableName.Substring(0, tableName.Length - AppLanguage.NeedLanguage.Length);
    }

    public static List<string> GetExcelSheetName(string excelName, DataTable dtSheet)
    {
        List<string> excelSheetNames = new List<string>();

        string sheetName = null;
        for (int i = 0; i < dtSheet.Rows.Count; ++i)
        {
            sheetName = dtSheet.Rows[i]["TABLE_NAME"].ToString();

            if (sheetName == ExcelTableSetting.ExcelDataSheetName || sheetName == ExcelTableSetting.ExcelConfigSheetName)
                excelSheetNames.Add(sheetName);
            else if (sheetName.StartsWith("'" + excelName))
                excelSheetNames.Add(sheetName);
        }

        return excelSheetNames;
    }
}