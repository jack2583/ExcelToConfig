using System;
using System.Collections.Generic;
using System.Text;

public partial class TableExportToJsonHelper
{
    /// <summary>
    /// 按配置的特殊索引导出方式输出json文件（如果声明了在生成的json文件开头以注释形式展示列信息，将生成更直观的嵌套字段信息，而不同于普通导出规则的列信息展示）
    /// </summary>
    public static bool SpecialExportTableToJson(TableInfo tableInfo, string exportRule, out string errorString)
    {
        exportRule = exportRule.Trim();
        // 解析按这种方式导出后的json文件名
        int colonIndex = exportRule.IndexOf(':');

        // 解析table value中要输出的字段名
        List<FieldInfo> tableValueField = new List<FieldInfo>();
        // 解析完依次作为索引的字段以及table value中包含的字段后，按索引要求组成相应的嵌套数据结构
        Dictionary<object, object> data = new Dictionary<object, object>();
        //自定义导出规则检查
        TableCheckHelper.CheckSpecialExportRule(tableInfo, exportRule, out tableValueField, out data, out errorString);

        if (errorString != null)
        {
            errorString = string.Format("错误：对表格{0}按\"{1}\"规则进行特殊索引导出时发现以下错误，导出被迫停止，请修正错误后重试：\n{2}\n", tableInfo.TableName, exportRule, errorString);
            return false;
        }

        // 生成导出的文件内容
        StringBuilder content = new StringBuilder();

        // 生成数据内容开头
        content.Append("{");//content.AppendLine("{");

        // 当前缩进量
        int currentLevel = 1;

        // 逐层按嵌套结构输出数据
        _GetIndexFieldData(content, data, tableValueField, ref currentLevel, out errorString);
        if (errorString != null)
        {
            errorString = string.Format("错误：对表格{0}按\"{1}\"规则进行特殊索引导出时发现以下错误，导出被迫停止，请修正错误后重试：\n{2}\n", tableInfo.TableName, exportRule, errorString);
            return false;
        }

        // 去掉最后一个子元素后多余的英文逗号
        content.Remove(content.Length - 1, 1);
        // 生成数据内容结尾
        content.AppendLine("}");

        string exportString = content.ToString();

        // 如果声明了要整理为带缩进格式的形式
        if (JsonStruct.ExportJsonIsFormat == true)
        {
            exportString = _FormatJson2(exportString);
            // exportString = JsonConvert.SerializeObject(exportString);
        }

        // 保存为json文件
        string fileName = exportRule.Substring(0, colonIndex).Trim();
        if (SaveJson.SaveJsonFile(tableInfo.ExcelName, ExcelMethods.GetSaveTableName(fileName), exportString) == true)
        {
            errorString = null;
            try
            {
                LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(exportString);
            }
            catch (LitJson.JsonException exception)
            {
                errorString = "错误：导出json出现异常，请检查导出的json及Excel\n";
            }
            return true;
        }
        else
        {
            errorString = "保存为json文件失败\n";
            return false;
        }
    }

    /// <summary>
    /// 按指定索引方式导出数据时,通过此函数递归生成层次结构,当递归到最内层时输出指定table value中的数据
    /// </summary>
    private static void _GetIndexFieldData(StringBuilder content, Dictionary<object, object> parentDict, List<FieldInfo> tableValueField, ref int currentLevel, out string errorString)
    {
        string oneTableValueFieldData = null;

        foreach (var key in parentDict.Keys)
        {
            /*加上这2句大文件卡死*/
            //if (content.ToString().EndsWith("}"))
            //    content.Remove(content.Length - 1, 1).Append(",");

            // content.Append(_GetJsonIndentation(currentLevel));
            // 生成key
            //if(key==null | key.ToString()=="")
            //{
            //    errorString = "错误：嵌套导出的key不能为空！";
            //    AppLog.LogErrorAndExit(errorString);
            //    return;
            //}
            if (key.GetType() == typeof(int) || key.GetType() == typeof(long) || key.GetType() == typeof(float))
                content.Append("\"").Append(key).Append("\"");
            else if (key.GetType() == typeof(string))
            {
                //// 检查作为key值的变量名是否合法
                //TableCheckHelper.CheckFieldName(key.ToString(), out errorString);
                //if (errorString != null)
                //{
                //    errorString = string.Format("作为第{0}层索引的key值不是合法的变量名，你填写的为\"{1}\"", currentLevel - 1, key.ToString());
                //    return;
                //}
                //content.Append(key);

                content.Append("\"").Append(key).Append("\"");
            }
            else
            {
                errorString = string.Format("SpecialExportTableToJson中出现非法类型的索引列类型{0}", key.GetType());
                AppLog.LogErrorAndExit(errorString);
                return;
            }

            content.Append(":{");// content.AppendLine(":{");
            ++currentLevel;

            // 如果已是最内层，输出指定table value中的数据
            if (parentDict[key].GetType() == typeof(int))
            {
                foreach (FieldInfo fieldInfo in tableValueField)
                {
                    int rowIndex = (int)parentDict[key];
                    oneTableValueFieldData = _GetOneField(fieldInfo, rowIndex, out errorString);//
                    if (errorString != null)
                    {
                        errorString = string.Format("第{0}行的字段\"{1}\"（列号：{2}）导出数据错误：{3}", rowIndex + ExcelTableSetting.DataFieldDataStartRowIndex + 1, fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), errorString);
                        return;
                    }
                    else
                    {
                        if (oneTableValueFieldData == null)
                        {
                            errorString = string.Format("第{0}行的字段\"{1}\"（列号：{2}）导出数据错误：{3}", rowIndex + ExcelTableSetting.DataFieldDataStartRowIndex + 1, fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), "嵌套导出的值不能为空");
                            //AppLog.LogErrorAndExit(errorString);
                            //return;
                            content.Append("\"").Append(fieldInfo.FieldName).Append("\"");
                            content.Append(":");
                            switch (fieldInfo.DataType)
                            {
                                case DataType.Int:
                                case DataType.Long:
                                case DataType.Float:
                                    {
                                        content.Append("0").Append(",");
                                        break;
                                    }
                                case DataType.String:
                                    {
                                        content.Append("\"\"").Append(",");
                                        break;
                                    }
                                case DataType.Bool:
                                    {
                                        content.Append("false").Append(",");
                                        break;
                                    }
                                case DataType.Lang:
                                case DataType.Date:
                                case DataType.Time:
                                default:
                                    {
                                        content.Append("\"\"").Append(",");
                                        break;
                                    }
                            }
                        }
                        else
                            content.Append(oneTableValueFieldData);
                    }
                }
                // 去掉最后一个子元素后多余的英文逗号
                if(content.ToString().EndsWith(","))
                    content.Remove(content.Length - 1, 1);
            }
            // 否则继续递归生成索引key
            else
            {
                // if (content.ToString().EndsWith("}"))
                // content.Remove(content.Length - 1, 1).Append(",");

                _GetIndexFieldData(content, (Dictionary<object, object>)(parentDict[key]), tableValueField, ref currentLevel, out errorString);
                if (errorString != null)
                    return;

                if (content.ToString().EndsWith(","))
                    content.Remove(content.Length - 1, 1);
                //content.Append(",");
            }

            --currentLevel;
            // content.Append(_GetJsonIndentation(currentLevel));
            content.Append("},"); // content.AppendLine("},");
        }

        errorString = null;
    }

    //private static string _GetOneFieldColumnInfo(FieldInfo fieldInfo, int level)
    //{
    //    StringBuilder content = new StringBuilder();
    //    // content.AppendFormat("{0}{1, -" + _FIELD_NAME_MIN_LENGTH + "}{2}{3, -" + _FIELD_DATA_TYPE_MIN_LENGTH + "}{4}{5}\n", _COMMENT_OUT_STRING, _GetFieldNameIndentation(level) + fieldInfo.FieldName, _DEFINE_INDENTATION_STRING, fieldInfo.DataTypeString, _DEFINE_INDENTATION_STRING, fieldInfo.Desc);
    //    if (fieldInfo.DataType == DataType.Dict || fieldInfo.DataType == DataType.Array)
    //    {
    //        ++level;
    //        foreach (FieldInfo childFieldInfo in fieldInfo.ChildField)
    //            content.Append(_GetOneFieldColumnInfo(childFieldInfo, level));

    //        --level;
    //    }

    //    return content.ToString();
    //}
}