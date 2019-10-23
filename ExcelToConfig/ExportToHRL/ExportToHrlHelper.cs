using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportToHrlHelper
{
    public static void ExportToHrl(TableInfo tableInfo)
    {
       string errorString = null;

        // 对表格按默认方式导出
        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, HrlStruct.Excel_Config_ExportHrl, ref HrlStruct.IsExportHrl))
        {
            // 对表格按默认方式导出
            if (HrlStruct.IsExportHrl == true)
            {
                TableAnalyzeHelper.GetOneConfigData(tableInfo, HrlStruct.Excel_Config_HrlTopInfo, ref HrlStruct.HrlTopInfo);
                TableAnalyzeHelper.GetOneConfigData(tableInfo, HrlStruct.Excel_Config_HrlEndInfo, ref HrlStruct.HrlEndInfo);
                TableExportToHrlHelper.ExportTableToHrl(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出Hrl成功");
            }
        }
        else
        {
            // 对表格按默认方式导出
            if (HrlStruct.IsExport == true)
            {
                TableExportToHrlHelper.ExportTableToHrl(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出Hrl成功");
            }
        }
    }
}
