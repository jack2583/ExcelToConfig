using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class SaveJson
{
    public static bool SaveJsonFile(string excelName, string tableName,string content)
    {
        try
        {
            //string ExportJsonPath = null;
            //if (AppValues.ExportJsonPath == null)//如果不存在文件夹就创建
            //{
            //    ExportJsonPath = "ExportJson";
            //    if (!System.IO.Directory.Exists(ExportJsonPath))
            //    {
            //        System.IO.Directory.CreateDirectory(ExportJsonPath);

            //    }
            //}
            //else
            //{
            //    ExportJsonPath = AppValues.ExportJsonPath;
            //}
            string exportDirectoryPath = FileModule.GetExportDirectoryPath(excelName, JsonStruct.SavePath,JsonStruct.IsExportKeepDirectoryStructure);
            //如果文件夹不存在就创建
            if (Directory.Exists(exportDirectoryPath) == false)
                Directory.CreateDirectory(exportDirectoryPath);

            string fileName2 = string.Concat(JsonStruct.ExportNameBeforeAdd + tableName, ".", JsonStruct.SaveExtension);
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
