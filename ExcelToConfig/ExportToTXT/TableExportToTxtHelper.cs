using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

public class TableExportToTxtHelper
{

    public static bool ExportTableToTxt(TableInfo tableInfo, out string errorString)
    {
       
       
        StringBuilder stringBuilder = new StringBuilder();

        // DataTable dt = AppValues.ExcelDataSet[tableInfo.ExcelFilePath].Tables[ExcelTableSetting.ExcelDataSheetName];
        foreach (DataTable dt in AppValues.ExcelDataSet[tableInfo.ExcelFilePath].Tables)
        {
            StringBuilder content = new StringBuilder();
            for (int row = 0; row < dt.Rows.Count; ++row)
            {
                List<string> strList = new List<string>();
                string str = null;
                for (int column = 0; column < dt.Columns.Count; ++column)
                {
                   
                    str = str + "{"+column+"}" + TxtStruct.ExportTxtSplitChar;
                    strList.Add(dt.Rows[row][column].ToString());
                }
               
               
                //for (int column = 0; column < dt.Columns.Count; ++column)
                //{
                    
                //    //str2 = str2 +" \""+ dt.Rows[row][column].ToString()+"\" ," ;
                //}
                string[] str2 = strList.ToArray();
               
                //str2 = str2.Remove(str2.Length - 1);
                //  str = str.TrimEnd(TxtStruct.ExportTxtSplitChar);
                // content.Append(str).Append(TxtStruct.ExportTxtLineChar);
                 content.Append(string.Format(str, str2)).Append(TxtStruct.ExportTxtLineChar);
                //content.AppendFormat(str, str2).Append(TxtStruct.ExportTxtLineChar);
            }
            string exportString = content.ToString();

            if (exportString.Length < 2)
                continue;

            if (TxtStruct.SavePath == null)
            {
                TxtStruct.SavePath = tableInfo.ExcelDirectoryName;
                TxtStruct.IsExportKeepDirectoryStructure = false;
            }
            // 保存为txt文件
            if (SaveTxt.SaveTxtFile(tableInfo.ExcelName, dt.TableName, exportString) == true)
            {
                errorString = null;
               // return true;
            }
            else
            {
                errorString =string.Format("{0}的数据表{1}保存为txt文件失败\n",tableInfo.ExcelName,tableInfo.TableConfig);
                stringBuilder.Append(errorString);
                //return false;
            }
        }
        if(stringBuilder==null)
        {
            errorString = null;
            return true;
        }
        else
        {

            errorString = stringBuilder.ToString();
            if(errorString.Length>0)
            {
                return false;
            }
            else
            {
                errorString = null;
                return true;
            }
                
        }
        


    }

    /// <summary>
    /// 按配置的特殊索引导出方式输出lua文件（如果声明了在生成的lua文件开头以注释形式展示列信息，将生成更直观的嵌套字段信息，而不同于普通导出规则的列信息展示）
    /// </summary>
    public static bool SpecialExportTableToLua(TableInfo tableInfo, string exportRule, out string errorString)
    {
        StringBuilder content = new StringBuilder();
        string exportString = content.ToString();

        exportRule = exportRule.Trim();
        // 解析按这种方式导出后的lua文件名
        int colonIndex = exportRule.IndexOf(':');
        if (colonIndex == -1)
        {
            errorString = string.Format("导出配置\"{0}\"定义错误，必须在开头声明导出txt文件名\n", exportRule);
            return false;
        }
        string fileName = exportRule.Substring(0, colonIndex).Trim();
        // 保存为txt文件
        if (SaveTxt.SaveTxtFile(tableInfo.TableName, fileName, exportString) == true)
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = "保存为txt文件失败\n";
            return false;
        }
    }


}
