using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
                    TableExportToMySQLHelper.ExportTableToDatabase(kvp.Value, tableExportNameforMySQL,out errorString);
                    if (!string.IsNullOrEmpty(errorString))
                        AppLog.LogErrorAndExit(string.Format("导出失败：{0}\n导出至MySQL数据库被迫中止，请修正错误后重试\n", errorString));
                //}
            }
        }
        
    }
}

