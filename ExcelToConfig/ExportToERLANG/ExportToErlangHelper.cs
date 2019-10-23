using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportToErlangHelper
{
    public static void ExportToErlang(TableInfo tableInfo)
    {
       string errorString = null;
        if (ErlangStruct.ExportErlangIsFormat == false)
            ErlangStruct.IndentationString = "";
        // 判断是否设置要是否要导出null字段
        TableAnalyzeHelper.GetOneConfigData(tableInfo, ErlangStruct.Excel_Config_NotExportErlangNull, ref ErlangStruct.IsExportErlangNullConfig);
        
        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, ErlangStruct.Excel_Config_SpecialExportErlang, ref ErlangStruct.SpecialExportErlangParams))
        {
            // 特殊嵌套导出
            foreach (string param in ErlangStruct.SpecialExportErlangParams)
            {
                AppLog.Log(string.Format("对此表格按\"{0}\"自定义规则进行导出erlang：", param));
                TableExportToErlangHelper.SpecialExportTableToErlang(tableInfo, param, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(string.Format("导出特殊erlang失败：\n{0}\n", errorString));
                else
                    AppLog.Log("导出特殊erlang成功");
            }
        }

        // 对表格按默认方式导出
        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, ErlangStruct.Excel_Config_ExportErlang, ref ErlangStruct.IsExportErlang))
        {
            // 对表格按默认方式导出
            if (ErlangStruct.IsExportErlang == true)
            {
                TableExportToErlangHelper.ExportTableToErlang(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出erlang成功");
            }
        }
        else
        {
            // 对表格按默认方式导出
            if (ErlangStruct.IsExport == true)
            {
                TableExportToErlangHelper.ExportTableToErlang(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出erlang成功");
            }
        }
    }
}
