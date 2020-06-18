using System;
using System.Collections.Generic;
using System.Text;

public partial class TableExportToLuaHelper
{
    // 生成lua文件上方字段描述的配置
    // 每行开头的lua注释声明
    private static string _COMMENT_OUT_STRING = "-- ";

    // 变量名、数据类型、描述声明之间的间隔字符串
    private static string _DEFINE_INDENTATION_STRING = "   ";

    // dict子元素相对于父dict变量名声明的缩进字符串
    private static string _DICT_CHILD_INDENTATION_STRING = "   ";

    // 变量名声明所占的最少字符数
    private static int _FIELD_NAME_MIN_LENGTH = 30;

    // 数据类型声明所占的最少字符数
    private static int _FIELD_DATA_TYPE_MIN_LENGTH = 30;

    public static bool ExportTableToLua(TableInfo tableInfo, out string errorString)
    {
        StringBuilder content = new StringBuilder();

        // 生成数据内容开头
        if (LuaStruct.IsTableNameStart)
        {
            content.Append(tableInfo.TableName).AppendLine(" = {");
        }
        else
        {
            content.AppendLine("return {");
        }

        // 当前缩进量
        int currentLevel = 1;

        // 判断是否设置要将主键列的值作为导出的table中的元素
        bool isAddKeyToLuaTable = tableInfo.TableConfigData2 != null && tableInfo.TableConfigData2.ContainsKey(LuaStruct.Excel_Config_AddKeyToLuaTable) && tableInfo.TableConfigData2[LuaStruct.Excel_Config_AddKeyToLuaTable].Count > 0 && "true".Equals(tableInfo.TableConfigData2[LuaStruct.Excel_Config_AddKeyToLuaTable][0], StringComparison.CurrentCultureIgnoreCase);

        // 逐行读取表格内容生成lua table
        List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
        int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        for (int row = 0; row < dataCount; ++row)
        {
            // 将主键列作为key生成
            content.Append(_GetLuaIndentation(currentLevel));
            FieldInfo keyColumnField = allField[0];
            if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
                content.AppendFormat("[{0}]", keyColumnField.Data[row]);
            // 注意：像“1_2”这样的字符串作为table的key必须加[""]否则lua认为是语法错误
            else if (keyColumnField.DataType == DataType.String)
                content.AppendFormat("[\"{0}\"]", keyColumnField.Data[row]);
            else
            {
                errorString = "用ExportTableToLua导出不支持的主键列数据类型";
                AppLog.LogErrorAndExit(errorString);
                return false;
            }

            content.AppendLine(" = {");
            ++currentLevel;

            // 如果设置了要将主键列的值作为导出的table中的元素
            if (isAddKeyToLuaTable == true)
            {
                content.Append(_GetLuaIndentation(currentLevel));
                content.Append(keyColumnField.FieldName);
                content.Append(" = ");
                if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
                    content.Append(keyColumnField.Data[row]);
                else if (keyColumnField.DataType == DataType.String)
                    content.AppendFormat("\"{0}\"", keyColumnField.Data[row]);

                content.AppendLine(",");
            }

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
                    content.Append(oneFieldString);
            }

            // 一行数据生成完毕后添加右括号结尾等
            --currentLevel;
            content.Append(_GetLuaIndentation(currentLevel));
            content.AppendLine("},");
        }

        // 生成数据内容结尾
        content.AppendLine("}");
        if (LuaStruct.IsTableNameStart)
        {
            content.Append("return ").Append(tableInfo.TableName);
        }
        string exportString = content.ToString();

        if (LuaStruct.ExportLuaIsFormat == false)
        {
            StringBuilder stringBuilder2 = new StringBuilder();
            for (int i = 0; i < exportString.Length; ++i)
            {
                char c = exportString[i];

                if (c == '\n' || c == '\r' || c.ToString() == " ")
                {
                }
                else
                    stringBuilder2.Append(c);
            }
            exportString = stringBuilder2.ToString();
        }

        if (LuaStruct.IsNeedColumnInfo == true)
            exportString = _GetColumnInfo(tableInfo) + exportString;

        // 保存为lua文件
        if (SaveLua.SaveLuaFile(tableInfo.ExcelName, ExcelMethods.GetSaveTableName(tableInfo.TableName), exportString) == true)
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = "保存为lua文件失败\n";
            return false;
        }
    }

    /// <summary>
    /// 生成要在lua文件最上方以注释形式展示的列信息
    /// </summary>
    private static string _GetColumnInfo(TableInfo tableInfo)
    {
        // 变量名前的缩进量
        int level = 0;

        StringBuilder content = new StringBuilder();
        foreach (FieldInfo fieldInfo in tableInfo.GetAllClientFieldInfo())
            content.Append(_GetOneFieldColumnInfo(fieldInfo, level));

        content.Append(System.Environment.NewLine);
        return content.ToString();
    }

    private static string _GetOneFieldColumnInfo(FieldInfo fieldInfo, int level)
    {
        StringBuilder content = new StringBuilder();
        content.AppendFormat("{0}{1, -" + _FIELD_NAME_MIN_LENGTH + "}{2}{3, -" + _FIELD_DATA_TYPE_MIN_LENGTH + "}{4}{5}\n", _COMMENT_OUT_STRING, _GetFieldNameIndentation(level) + fieldInfo.FieldName, _DEFINE_INDENTATION_STRING, fieldInfo.DataTypeString, _DEFINE_INDENTATION_STRING, fieldInfo.Desc);
        if (fieldInfo.DataType == DataType.Dict || fieldInfo.DataType == DataType.Array)
        {
            ++level;
            foreach (FieldInfo childFieldInfo in fieldInfo.ChildField)
                content.Append(_GetOneFieldColumnInfo(childFieldInfo, level));

            --level;
        }

        return content.ToString();
    }

    /// <summary>
    /// 生成columnInfo变量名前的缩进字符串
    /// </summary>
    private static string _GetFieldNameIndentation(int level)
    {
        StringBuilder indentationStringBuilder = new StringBuilder();
        for (int i = 0; i < level; ++i)
            indentationStringBuilder.Append(_DICT_CHILD_INDENTATION_STRING);

        return indentationStringBuilder.ToString();
    }
}