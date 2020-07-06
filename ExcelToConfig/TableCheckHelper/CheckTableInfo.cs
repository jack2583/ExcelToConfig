using System;
using System.Collections.Generic;

public class CheckTableInfo
{
    // 声明整表检查的配置参数名
    public const string CONFIG_NAME_CHECK_TABLE = "tableCheckRule";



    public static void CheckTable()
    {
        if (ExcelFolder.IsNeedCheck == true)
        {
            string errorString;
            foreach (KeyValuePair<string, TableInfo> kvp in AppValues.TableInfo)
            {
                
                //合并过的表不再检查
                if(AppValues.MergeTableList!=null)
                {
                    if (AppValues.MergeTableList.ContainsKey(kvp.Key))
                        continue;
                }


                TableInfo tableInfo = kvp.Value;
                AppLog.Log(string.Format("\n检查表格\"{0}\"：", tableInfo.ExcelNameTips), ConsoleColor.Green);
                errorString = null;

                TableCheckHelper.CheckTable(tableInfo, out errorString);
                if (errorString != null)
                {
                    AppLog.LogError(string.Format("检查完成，存在以下错误：\n{0}", errorString));
                }
                else
                    AppLog.Log("检查完成，正确");

                if (AppLog.LogErrorContent.Length > 0)
                {
                    AppLog.SaveErrorInfoToFile("错误日志.txt");
                    AppLog.LogErrorAndExit("\n按任意键继续");
                }
            }
        }
    }
}