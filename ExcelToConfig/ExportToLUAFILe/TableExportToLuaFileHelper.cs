using System;
using System.Collections.Generic;
using System.Text;

public partial class TableExportToLuaFileHelper
{
    public static bool ExportTableToLuaFile(TableInfo tableInfo, out string errorString)
    {
        StringBuilder content = new StringBuilder();

        // 生成数据内容开头
        content.Append(tableInfo.TableName).AppendLine(" = {");

        // 当前缩进量
        int currentLevel = 1;

        // 判断是否设置要将主键列的值作为导出的table中的元素
        // bool isAddKeyToLuaTable = tableInfo.TableConfig != null && tableInfo.TableConfig.ContainsKey(ErlangStruct.Excel_Config_AddKeyToErlangTable) && tableInfo.TableConfig[ErlangStruct.Excel_Config_AddKeyToErlangTable].Count > 0 && "true".Equals(tableInfo.TableConfig[ErlangStruct.Excel_Config_AddKeyToErlangTable][0], StringComparison.CurrentCultureIgnoreCase);

        // 逐行读取表格内容生成lua table
        List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
        int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        for (int row = 0; row < dataCount; ++row)
        {
            // 将主键列作为key生成

            FieldInfo keyColumnField = allField[0];
            if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
                content.Append("    [").Append(keyColumnField.Data[row]).AppendLine("] = {");
            // 注意：像“1_2”这样的字符串作为table的key必须加[""]否则lua认为是语法错误
            else if (keyColumnField.DataType == DataType.String)
                content.Append("    [").Append(keyColumnField.Data[row]).AppendLine("] = {");
            else
            {
                errorString = "用ExportTableToLuaFile导出不支持的主键列数据类型";
                AppLog.LogErrorAndExit(errorString);
                return false;
            }

            //content.Append("#{");
            //  ++currentLevel;

            // 如果设置了要将主键列的值作为导出的table中的元素
            //if (isAddKeyToLuaTable == true)
            //{
            //    content.Append(_GetErlangIndentation(currentLevel));
            //    content.Append(keyColumnField.FieldName);
            //    content.Append(" = ");
            //    if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
            //        content.Append(keyColumnField.Data[row]);
            //    else if (keyColumnField.DataType == DataType.String)
            //        content.AppendFormat("\"{0}\"", keyColumnField.Data[row]);

            //    content.AppendLine(",");
            //}

            // 将其他列依次作为value生成
            for (int column = 1; column < allField.Count; ++column)
            {
                string oneFieldString = _GetOneField(allField[column], row, currentLevel, out errorString);
                if (errorString != null)
                {
                    errorString = string.Format("导出表格{0}失败，", tableInfo.TableName) + errorString;
                    return false;
                }
                else
                {
                    content.Append("        ");
                    content.Append(allField[column].FieldName).Append("=").Append(oneFieldString);
                    content.AppendLine(",");
                }
            }

            // 一行数据生成完毕后添加右括号结尾等
            // --currentLevel;
            // content.Append(_GetErlangIndentation(currentLevel));
            content.Remove(content.Length - 3, 1);
            content.AppendLine("    },");
        }

        content.Remove(content.Length - 3, 1);
        content.AppendLine("}");
        // 生成数据内容结尾
        content.Append("return ").Append(tableInfo.TableName);

        string exportString = content.ToString();
        //if (ErlangStruct.IsNeedColumnInfo == true)
        //    exportString = _GetColumnInfo(tableInfo) + exportString;

        // 保存为LuaFile文件
        if (SaveLuaFile.SaveLuaFileFile(tableInfo.TableName, tableInfo.TableName, exportString) == true)
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = "保存为LuaFile文件失败\n";
            return false;
        }
    }

    /// <summary>
    /// 生成要在LuaFile文件最上方以注释形式展示的列信息
    /// </summary>
    private static string _GetColumnInfo(TableInfo tableInfo)
    {
        // 变量名前的缩进量
        int level = 0;

        StringBuilder content = new StringBuilder();

        content.Append(System.Environment.NewLine);
        return content.ToString();
    }
}