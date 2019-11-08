using System;
using System.Collections.Generic;

public class ExportToMySQLHelper
{
    public static void ExportToMySQL()
    {
        if (MySQLStruct.IsExport == false)
        {
            return;
        }
        string errorString = null;

        AppLog.Log("\n导出表格数据到MySQL数据库\n");

        errorString = null;
        List<string> errorInfo = new List<string>();
        foreach (KeyValuePair<string, TableInfo> kvp in AppValues.TableInfo)
        {
            string tableExportNameforMySQL = kvp.Key;
            TableInfo tableInfo = kvp.Value;
            foreach (FieldInfo fieldInfo in tableInfo.GetAllFieldInfo())
            {
                string databaseInfoString = null;
                if (string.IsNullOrEmpty(databaseInfoString))
                {
                    fieldInfo.DatabaseFieldName = null;
                    fieldInfo.DatabaseFieldType = null;
                }
                else
                {
                    int leftBracketIndex = databaseInfoString.IndexOf('(');
                    int rightBracketIndex = databaseInfoString.LastIndexOf(')');
                    if (leftBracketIndex == -1 || rightBracketIndex == -1 || leftBracketIndex > rightBracketIndex)
                    {
                        errorString = string.Format("表名为：{0}，的第 {1} 列（第 {2} 行声明字段名为 {3} 错误，\n导出到MySQL中表字段信息声明错误，必须在字段名后的括号中声明对应数据库中的数据类型）", tableInfo.ExcelName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), ExcelTableSetting.DataFieldExportDataBaseFieldInFoRowIndex, fieldInfo.DatabaseInfoString);
                        errorInfo.Add(errorString);
                    }

                    fieldInfo.DatabaseFieldName = databaseInfoString.Substring(0, leftBracketIndex);
                    fieldInfo.DatabaseFieldType = databaseInfoString.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
                }
            }
        }
        if (errorInfo.Count > 0)
        {
            AppLog.LogErrorAndExit(errorInfo.ToString());
        }

        TableExportToMySQLHelper.ConnectToDatabase(out errorString);
        if (!string.IsNullOrEmpty(errorString))
            AppLog.LogErrorAndExit(string.Format("无法连接至MySQL数据库：{0}\n导出至MySQL数据库被迫中止，请修正错误后重试\n", errorString));

        foreach (KeyValuePair<string, TableInfo> kvp in AppValues.TableInfo)
        {
            string tableExportNameforMySQL = kvp.Key;
            TableInfo tableInfo = kvp.Value;
            List<string> inputParams = new List<string>();
            if (TableAnalyzeHelper.GetOneConfigData(tableInfo, MySQLStruct.CONFIG_NAME_EXPORT_DATABASE_TABLE_NAME, ref inputParams))
            {
                tableExportNameforMySQL = inputParams[0];
            }

            if (TableAnalyzeHelper.GetOneConfigData(tableInfo, MySQLStruct.Excel_Config_ExportMySQL, ref MySQLStruct.IsExportMySQL))
            {
                if (MySQLStruct.IsExportMySQL == true)
                {
                    TableExportToMySQLHelper.ExportTableToDatabase(kvp.Value, tableExportNameforMySQL, out errorString);
                    if (!string.IsNullOrEmpty(errorString))
                        AppLog.LogErrorAndExit(string.Format("导出失败：{0}\n导出至MySQL数据库被迫中止，请修正错误后重试\n", errorString));
                }
            }
            else
            {
                //if (MySQLStruct.IsExport == true)
                //{
                TableExportToMySQLHelper.ExportTableToDatabase(kvp.Value, tableExportNameforMySQL, out errorString);
                if (!string.IsNullOrEmpty(errorString))
                    AppLog.LogErrorAndExit(string.Format("导出失败：{0}\n导出至MySQL数据库被迫中止，请修正错误后重试\n", errorString));
                //}
            }
        }
    }
}