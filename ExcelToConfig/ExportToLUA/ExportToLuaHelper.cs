using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportToLuaHelper
{
    public static void ExportToLua(TableInfo tableInfo)
    {
       string errorString = null;
        if (LuaStruct.ExportLuaIsFormat == false)
            LuaStruct.IndentationString = "";
        // 判断是否设置要是否要导出null字段
        TableAnalyzeHelper.GetOneConfigData(tableInfo, LuaStruct.Excel_Config_NotExportLuaNil,ref LuaStruct.IsExportLuaNilConfig);

        //if (tableInfo.TableConfig != null && tableInfo.TableConfig.ContainsKey(LuaStruct.Excel_Config_NotExportLuaNil))
        //{
        //    if (tableInfo.TableConfig[LuaStruct.Excel_Config_NotExportLuaNil].Count > 0)
        //    {
        //        if ("false".Equals(tableInfo.TableConfig[LuaStruct.Excel_Config_NotExportLuaNil][0], StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            LuaStruct.IsExportLuaNilConfig = false;
        //        }
        //    }
        //}


        // 以下为旧的代码。判断是否设置了特殊导出规则
        if (tableInfo.TableConfig != null && tableInfo.TableConfig.ContainsKey("tableExportConfig"))
        {
            List<string> inputParams = tableInfo.TableConfig[LuaStruct.Excel_Config_SpecialExportLua];
            if (inputParams.Contains(LuaStruct.Excel_Config_NotExportLuaOriginalTable))
            {
                LuaStruct.IsExportLua = false;
                if (inputParams.Count == 1)
                    AppLog.LogWarning(string.Format("警告：你设置了不对表格\"{0}\"按默认方式进行导出Lua，而又没有指定任何其他自定义导出规则，本工具对此表格不进行任何导出，请确认是否真要如此", tableInfo.TableName));
                else
                    AppLog.Log("你设置了不对此表进行默认规则导出Lua");
            }
            // 执行设置的特殊导出规则
            foreach (string param in inputParams)
            {
                if (!LuaStruct.Excel_Config_NotExportLuaOriginalTable.Equals(param, StringComparison.CurrentCultureIgnoreCase))
                {
                    AppLog.Log(string.Format("对此表格按\"{0}\"自定义规则进行导出Lua：", param));
                    TableExportToLuaHelper.SpecialExportTableToLua(tableInfo, param, out errorString);
                    if (errorString != null)
                        AppLog.LogErrorAndExit(string.Format("导出特殊lua失败：\n{0}\n", errorString));
                    else
                        AppLog.Log("导出特殊lua成功");
                }
            }
        }

        //以下为新的代码
        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, LuaStruct.Excel_Config_SpecialExportLua,ref LuaStruct.SpecialExportLuaParams))
        {
            // 特殊嵌套导出
            foreach (string param in LuaStruct.SpecialExportLuaParams)
            {
                AppLog.Log(string.Format("对此表格按\"{0}\"自定义规则进行导出Lua：", param));
                TableExportToLuaHelper.SpecialExportTableToLua(tableInfo, param, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(string.Format("导出特殊lua失败：\n{0}\n", errorString));
                else
                    AppLog.Log("导出特殊lua成功");
            }
        }

        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, LuaStruct.Excel_Config_ExportLua,ref LuaStruct.IsExportLua))
        {
            // 对表格按默认方式导出
            if (LuaStruct.IsExportLua == true)
            {
                TableExportToLuaHelper.ExportTableToLua(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出lua成功");
            }
        }
        else
        {
            // 对表格按默认方式导出
            if (LuaStruct.IsExport == true)
            {
                TableExportToLuaHelper.ExportTableToLua(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出lua成功");
            }
        }
        
    }
}
