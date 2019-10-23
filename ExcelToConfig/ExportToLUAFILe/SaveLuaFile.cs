using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class SaveLuaFile
{

    /// <summary>
    /// 将某张Excel表格转换为LuaFile table内容保存到文件
    /// </summary>
    public static bool SaveLuaFileFile(string tableName, string fileName, string content)
        {
            try
            {
                string exportDirectoryPath = FileModule.GetExportDirectoryPath(tableName, LuaFileStruct.SavePath, LuaFileStruct.IsExportKeepDirectoryStructure);
            //如果文件夹不存在就创建
            if (Directory.Exists(exportDirectoryPath) == false)
                Directory.CreateDirectory(exportDirectoryPath);

            string fileName2 = string.Concat(fileName, ".", LuaFileStruct.SaveExtension);
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
