using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class SaveErlang
{

        /// <summary>
        /// 将某张Excel表格转换为lua table内容保存到文件
        /// </summary>
        public static bool SaveErlangFile(string excelName, string tableName, string content)
        {
            try
            {
            //if (ErlangStruct.IsBeforeNameAddtb == true)
            //{
            //    if (!fileName.StartsWith("tb_"))
            //        fileName = string.Concat("tb_", fileName);
            //}
            string exportDirectoryPath = FileModule.GetExportDirectoryPath(excelName, ErlangStruct.SavePath, ErlangStruct.IsExportKeepDirectoryStructure);
            //如果文件夹不存在就创建
            if (Directory.Exists(exportDirectoryPath) == false)
                Directory.CreateDirectory(exportDirectoryPath);

            string fileName2 = string.Concat(ErlangStruct.ExportNameBeforeAdd+tableName, ".", ErlangStruct.SaveExtension);
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
