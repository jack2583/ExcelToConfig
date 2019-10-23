using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class SaveTxt
{

        /// <summary>
        /// 将某张Excel表格转换为lua table内容保存到文件
        /// </summary>
        public static bool SaveTxtFile(string excelName, string sheetName, string content)
        {
            try
            {

                string exportDirectoryPath = FileModule.GetExportDirectoryPath(excelName, TxtStruct.SavePath, TxtStruct.IsExportKeepDirectoryStructure,false);
            //如果文件夹不存在就创建
            if (Directory.Exists(exportDirectoryPath) == false)
                Directory.CreateDirectory(exportDirectoryPath);

            
            if (sheetName.StartsWith("'"))
                sheetName=sheetName.Substring(1);
            if (sheetName.EndsWith("'"))
                sheetName = sheetName.Substring(0, sheetName.Length - 1);
            if(sheetName.EndsWith("$"))
                sheetName = sheetName.Substring(0, sheetName.Length - 1);

            string fileName2 = string.Concat(excelName+"-"+sheetName, ".", TxtStruct.SaveExtension);
            string savePath = FileModule.CombinePath(exportDirectoryPath, fileName2);
                StreamWriter writer = new StreamWriter(savePath, false, new UTF8Encoding(false));
                writer.Write(content);
                writer.Flush();
                writer.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
