using System;

public class ExportToJsonHelper
{
    public static void ExportToJson(TableInfo tableInfo)
    {
        string errorString = null;
        // 判断是否设置要是否要导出null字段
        TableAnalyzeHelper.GetOneConfigData(tableInfo, JsonStruct.Excel_Config_NotExportJsonNull, ref JsonStruct.IsExportJsonNullConfig);
        //if (tableInfo.TableConfig != null && tableInfo.TableConfig.ContainsKey(JsonStruct.Excel_Config_NotExportJsonNull))
        //{
        //    if (tableInfo.TableConfig[JsonStruct.Excel_Config_NotExportJsonNull].Count > 0)
        //    {
        //        if ("false".Equals(tableInfo.TableConfig[JsonStruct.Excel_Config_NotExportJsonNull][0], StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            JsonStruct.IsExportJsonNullConfig = false;
        //        }
        //    }
        //}

        // 以下为旧代码：判断是否设置了特殊导出规则
        /*
        if (tableInfo.TableConfig != null && tableInfo.TableConfig.ContainsKey(JsonStruct.Excel_Config_SpecialExportJson))
        {
            List<string> inputParams = tableInfo.TableConfig[JsonStruct.Excel_Config_SpecialExportJson];
            if (inputParams.Contains(JsonStruct.Excel_Config_NotExportJsonOriginalTable))
            {
                JsonStruct.IsExportJson = false;
                if (inputParams.Count == 1)
                    AppLog.LogWarning(string.Format("警告：你设置了不对表格\"{0}\"按默认方式进行导出json，而又没有指定任何其他自定义导出规则，本工具对此表格不进行任何导出，请确认是否真要如此", tableInfo.TableName));
                else
                    AppLog.Log("你设置了不对此表进行默认规则导出json");
            }
            // 执行设置的特殊导出规则
            foreach (string param in inputParams)
            {
                if (!JsonStruct.Excel_Config_NotExportJsonOriginalTable.Equals(param, StringComparison.CurrentCultureIgnoreCase))
                {
                    AppLog.Log(string.Format("对此表格按\"{0}\"自定义规则进行导出：", param));
                   TableExportToJsonHelper.SpecialExportTableToJson(tableInfo, param, out errorString);
                    if (errorString != null)
                        AppLog.LogErrorAndExit(string.Format("导出特殊json失败：\n{0}\n", errorString));
                    else
                        AppLog.Log("导出特殊json成功");
                }
            }
        }
        */
        //以下为新的代码
        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, JsonStruct.Excel_Config_SpecialExportJson, ref JsonStruct.SpecialExportJsonParams))
        {
            // 特殊嵌套导出
            foreach (string param in JsonStruct.SpecialExportJsonParams)
            {
                AppLog.Log(string.Format("对此表格按\"{0}\"自定义规则进行导出json：", param));
                TableExportToJsonHelper.SpecialExportTableToJson(tableInfo, param, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(string.Format("导出特殊json失败：\n{0}\n", errorString));
                else
                    AppLog.Log("导出特殊json成功");
            }
        }

        // 对表格按默认方式导出
        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, JsonStruct.Excel_Config_ExportJson, ref JsonStruct.IsExportJson))
        {
            // 对表格按默认方式导出
            if (JsonStruct.IsExportJson == true)
            {
                TableExportToJsonHelper.ExportTableToJson(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出json成功");
            }
        }
        else
        {
            // 对表格按默认方式导出
            if (JsonStruct.IsExport == true)
            {
                TableExportToJsonHelper.ExportTableToJson(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出json成功");
            }
        }
    }
}