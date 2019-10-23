using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class ReadExcelHelper
{
    public static void GetFileState(Dictionary<string, string> ExportTables)
    {
        
        StringBuilder content = new StringBuilder();
        string filePath = null;
        foreach (KeyValuePair<string, string> kvp in ExportTables)
        {
            filePath = kvp.Value;
            // 检查文件是否存在且没被打开
            FileState fileState = GetFileState(filePath);
            if (fileState == FileState.Inexist)
            {
                content.Append(string.Format("{0}文件不存在\n", filePath));
               
            }
            else if (fileState == FileState.IsOpen)
            {
                content.Append(string.Format("{0}文件正在被其他软件打开\n\n", filePath));
               
            }

        }
        string errorString = content.ToString();
        if(errorString.Length>0)
        {
            AppLog.LogErrorAndExit(string.Format("错误：存在以下错误，请处理后再执行：\n{0}", errorString));
        }
           
    }
    /// <summary>
    /// 将指定Excel文件的内容读取到DataSet中
    /// </summary>
    public static DataSet ReadXlsxFileForOleDb(string filePath,string excelName,ref string tableName, out string errorString)
    {

        OleDbConnection conn = null;
        OleDbDataAdapter da = null;
        DataSet ds = null;

        try
        {
            // 初始化连接并打开
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1\"";

            conn = new OleDbConnection(connectionString);
            conn.Open();

            // 获取数据源的表定义元数据                       
            DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            tableName = ExcelMethods.GetTableName(excelName);
            List<string> excelSheetNames = new List<string>();
            excelSheetNames = ExcelMethods.GetExcelSheetName(tableName, dtSheet);

            if (excelSheetNames.Count==1)
            {
                if(excelSheetNames.Contains(ExcelTableSetting.ExcelConfigSheetName))
                {
                    errorString = string.Format("错误：{0}中不含有Sheet名为{1}或以{2}开头的数据表", filePath, ExcelTableSetting.ExcelDataSheetName.Replace("$", ""), tableName);
                    return null;
                }
            }
            else if(excelSheetNames.Count == 0)
            {
                errorString = string.Format("错误：{0}中不含有Sheet名为{1}或以{2}开头的数据表", filePath, ExcelTableSetting.ExcelDataSheetName.Replace("$", ""), tableName);
                return null;
            }

            ds = new DataSet();
            foreach (string sheetName in excelSheetNames)
            {
                if (sheetName!=ExcelTableSetting.ExcelConfigSheetName)
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(String.Format("Select * FROM [{0}]", sheetName), conn);
                    da.Fill(ds, sheetName);

                    // 删除表格末尾的空行
                    DataRowCollection rows = ds.Tables[sheetName].Rows;
                    int rowCount = rows.Count;
                    for (int i = rowCount - 1; i >= ExcelTableSetting.DataFieldDataStartRowIndex; --i)
                    {
                        if (string.IsNullOrEmpty(rows[i][0].ToString()))
                            rows.RemoveAt(i);
                        else
                            break;
                    }
                }
                else
                {
                    //da.Dispose();
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(String.Format("Select * FROM [{0}]", ExcelTableSetting.ExcelConfigSheetName), conn);
                    da.Fill(ds, ExcelTableSetting.ExcelConfigSheetName);
                }
            }
        }
        catch
        {
            errorString = "错误：连接Excel失败，你可能尚未安装Office数据连接组件: http://www.microsoft.com/en-US/download/details.aspx?id=23734 \n";
            tableName = null;
            return null;
        }
        finally
        {
            // 关闭连接
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                // 由于C#运行机制，即便因为表格中没有Sheet名为data的工作簿而return null，也会继续执行finally，而此时da为空，故需要进行判断处理
                if (da != null)
                    da.Dispose();
                conn.Dispose();
            }
        }

        errorString = null;
        return ds;
    }

    /// <summary>
    /// 将指定Excel文件的内容读取到DataSet中
    /// </summary>
    public static DataSet ReadXlsxFileForExcelDataReader(string filePath, string excelName, ref string tableName, out string errorString)
    {
        DataSet ds = null;
        errorString = null;
        var file = new FileInfo(filePath);
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            IExcelDataReader reader = null;
            if (file.Extension == ".xls")
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.Extension == ".xlsx" || file.Extension == ".xlsm")
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            if (reader == null)
            {
                errorString = "只支持的.xls，.xlsx，.xlsm三种excel扩展类型，此文件的类型是" + file.Extension + "\n";
                return null;
            }



            ds = reader.AsDataSet();

            tableName = ExcelMethods.GetTableName(excelName);
            //List<string> excelSheetNames = new List<string>();
            //excelSheetNames = ExcelMethods.GetExcelSheetName(tableName, dtSheet);

            //if (excelSheetNames.Count == 1)
            //{
            //    if (!excelSheetNames.Contains(ExcelTableSetting.ExcelDataSheetName))
            //    {
            //        errorString = string.Format("错误：{0}中不含有Sheet名为{1}或以{2}开头的数据表", filePath, ExcelTableSetting.ExcelDataSheetName.Replace("$", ""), tableName);
            //        return null;
            //    }
            //}
            //else if (excelSheetNames.Count == 0)
            //{
            //    errorString = string.Format("错误：{0}中不含有Sheet名为{1}或以{2}开头的数据表", filePath, ExcelTableSetting.ExcelDataSheetName.Replace("$", ""), tableName);
            //    return null;
            //}

            var removeDataTableList = new List<DataTable>();
            tableName = ExcelMethods.GetTableName(excelName);
            foreach (DataTable da in ds.Tables)
            {
                //Utils.Log(string.Format("Table: {0}", da.TableName), ConsoleColor.Cyan);
                if (da.TableName.Equals(ExcelTableSetting.ExcelDataSheetName.Replace("$", "")))
                {
                    da.TableName = ExcelTableSetting.ExcelDataSheetName;
                }
                else if (da.TableName.Equals(ExcelTableSetting.ExcelConfigSheetName.Replace("$", "")))
                {
                    da.TableName = ExcelTableSetting.ExcelConfigSheetName;
                }
                else if (da.TableName.StartsWith(tableName))
                {
                   // da.TableName = da.TableName;
                }
                else
                {
                    removeDataTableList.Add(da);
                }
            }
            foreach (DataTable da in removeDataTableList)
            {
                ds.Tables.Remove(da);
            }
            //foreach (DataTable da in ds.Tables)
            //{
            //	Utils.Log(string.Format("Table: {0}", da.TableName), ConsoleColor.Cyan);
            //}

        }

        bool removeConfig = false;
        foreach(DataTable da in ds.Tables)
        {
            DataRowCollection rows = da.Rows;
            int rowCount = rows.Count;

            if (da.TableName != ExcelTableSetting.ExcelConfigSheetName)
            {
                // 删除表格末尾的空行
                for (int i = rowCount - 1; i >= ExcelTableSetting.DataFieldDataStartRowIndex; --i)
                {
                    if (string.IsNullOrEmpty(rows[i][0].ToString()))
                        rows.RemoveAt(i);
                    else
                        break;
                }
            }
            else
            {
                if (rowCount == 0)
                    removeConfig = true;
            }
        }

        if(removeConfig==true)
            ds.Tables.Remove(ds.Tables[ExcelTableSetting.ExcelConfigSheetName]);

        //// 删除表格末尾的空行
        //DataRowCollection rows = ds.Tables[ExcelTableSetting.ExcelDataSheetName].Rows;
        //int rowCount = rows.Count;
        //for (int i = rowCount - 1; i >= ExcelTableSetting.DataFieldDataStartRowIndex; --i)
        //{
        //    if (string.IsNullOrEmpty(rows[i][0].ToString()))
        //        rows.RemoveAt(i);
        //    else
        //        break;
        //}
        return ds;
    }
    /// <summary>
    /// 使用NPOI将指定Excel文件的内容读取到DataSet中
    /// </summary>
    public static DataSet ReadXlsxFileForNPOI(string filePath, string excelName, ref string tableName, out string errorString)
    {
        errorString = null;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string fileType = Path.GetExtension(filePath).ToLower();

        tableName = ExcelMethods.GetTableName(excelName);
        List<string> excelSheetNames = new List<string>();
        
        try
        {
            ISheet sheet = null;
            int sheetNumber = 0;
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            if (fileType == ".xlsx")
            {
                // 2007版本
                XSSFWorkbook workbook = new XSSFWorkbook(fs);
                sheetNumber = workbook.NumberOfSheets;
                for (int i = 0; i < sheetNumber; i++)
                {
                    string sheetName = workbook.GetSheetName(i);
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet != null)
                    {
                        dt = GetSheetDataTable(sheet, out errorString);
                        if (dt != null)
                        {
                            if (sheetName == ExcelTableSetting.ExcelDataSheetName || sheetName == ExcelTableSetting.ExcelConfigSheetName || sheetName.StartsWith(tableName))
                            {
                                dt.TableName = sheetName.Trim();
                                ds.Tables.Add(dt);
                            }

                        }
                    }
                }
            }
            else if (fileType == ".xls")
            {
                // 2003版本
                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                sheetNumber = workbook.NumberOfSheets;
                for (int i = 0; i < sheetNumber; i++)
                {
                    string sheetName = workbook.GetSheetName(i);
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet != null)
                    {
                        dt = GetSheetDataTable(sheet, out errorString);
                        if (dt != null)
                        {
                            if (sheetName == ExcelTableSetting.ExcelDataSheetName || sheetName == ExcelTableSetting.ExcelConfigSheetName || sheetName.StartsWith(tableName))
                            {
                                dt.TableName = sheetName.Trim();
                                ds.Tables.Add(dt);
                            }
                        }
                    }
                }
            }
            return ds;
        }
        catch (Exception ex)
        {
            errorString = ex.Message;
            return null;
        }
    }
    /// <summary>
    /// 使用NPIO获取sheet表对应的DataTable
    /// </summary>
    /// <param name="sheet">Excel工作表</param>
    /// <param name="errorString"></param>
    /// <returns></returns>
    private static DataTable GetSheetDataTable(ISheet sheet, out string errorString)
    {
        errorString = "";
        DataTable dt = new DataTable();
        string sheetName = sheet.SheetName;
        int startIndex = 0;// sheet.FirstRowNum;
        int lastIndex = sheet.LastRowNum;
        //最大列数
        int cellCount = 0;
        IRow maxRow = sheet.GetRow(0);
        for (int i = startIndex; i <= lastIndex; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row != null && cellCount < row.LastCellNum)
            {
                cellCount = row.LastCellNum;
                maxRow = row;
            }
        }
        //列名设置
        try
        {
            for (int i = 0; i < maxRow.LastCellNum; i++)//maxRow.FirstCellNum
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());
                //DataColumn column = new DataColumn("Column" + (i + 1).ToString());
                //dt.Columns.Add(column);
            }
        }
        catch
        {
            errorString = "工作表" + sheetName + "中无数据";
            return null;
        }
        //数据填充
        for (int i = startIndex; i <= lastIndex; i++)
        {
            IRow row = sheet.GetRow(i);
            DataRow drNew = dt.NewRow();
            if (row != null)
            {
                for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                {
                    if (row.GetCell(j) != null)
                    {
                        ICell cell = row.GetCell(j);
                        switch (cell.CellType)
                        {
                            case CellType.Blank:
                                drNew[j] = "";
                                break;
                            case CellType.Numeric:
                                short format = cell.CellStyle.DataFormat;
                                //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                if (format == 14 || format == 31 || format == 57 || format == 58)
                                    drNew[j] = cell.DateCellValue;
                                else
                                    drNew[j] = cell.NumericCellValue;
                                if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                    drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                break;
                            case CellType.String:
                                drNew[j] = cell.StringCellValue;
                                break;
                            case CellType.Formula:
                                try
                                {
                                    drNew[j] = cell.NumericCellValue;
                                    if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                        drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                }
                                catch
                                {
                                    try
                                    {
                                        drNew[j] = cell.StringCellValue;
                                    }
                                    catch { }
                                }
                                break;
                            default:
                                drNew[j] = cell.StringCellValue;
                                break;
                        }
                    }
                }
            }
            dt.Rows.Add(drNew);
        }
        return dt;
    }







    [DllImport("kernel32.dll")]
    private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);
    private const int OF_READWRITE = 2;
    private const int OF_SHARE_DENY_NONE = 0x40;
    private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
    /// <summary>
    /// 获取某个文件的状态
    /// </summary>
    public static FileState GetFileState(string filePath)
    {
        if (File.Exists(filePath))
        {
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
                return FileState.IsOpen;

            CloseHandle(vHandle);
            return FileState.Available;
        }
        else
            return FileState.Inexist;
    }
    public enum FileState
    {
        Inexist,     // 不存在
        IsOpen,      // 已被打开
        Available,   // 当前可用
    }
}
