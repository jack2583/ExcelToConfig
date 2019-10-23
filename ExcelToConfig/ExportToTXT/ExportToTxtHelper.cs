using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportToTxtHelper
{
    public static void ExportToTxt(TableInfo tableInfo)
    {
       string errorString = null;

        // 对表格按默认方式导出
        if (TableAnalyzeHelper.GetOneConfigData(tableInfo, TxtStruct.Excel_Config_ExportTxt, ref TxtStruct.IsExportTxt))
        {
            // 对表格按默认方式导出
            if (TxtStruct.IsExportTxt == true)
            {
                TableExportToTxtHelper.ExportTableToTxt(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出txt成功");
            }
        }
        else
        {
            // 对表格按默认方式导出
            if (TxtStruct.IsExport == true)
            {
                TableExportToTxtHelper.ExportTableToTxt(tableInfo, out errorString);
                if (errorString != null)
                    AppLog.LogErrorAndExit(errorString);
                else
                    AppLog.Log("按默认方式导出txt成功");
            }
        }
    }
}
