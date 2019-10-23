using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

public partial class TableExportToErlangHelper
{
    /// <summary>
    /// 特殊导出erlang
    /// </summary>
    /// <param name="tableInfo"></param>
    /// <param name="exportRule"></param>
    /// <param name="errorString"></param>
    /// <returns></returns>
    public static bool SpecialExportTableToErlang(TableInfo tableInfo, string exportRule, out string errorString)
    {
        errorString = null;
        exportRule = exportRule.Trim();
        // 解析按这种方式导出后的erlang文件名
        int colonIndex = exportRule.IndexOf(':');
        if (colonIndex == -1)
        {
            errorString = string.Format("导出配置\"{0}\"定义错误，必须在开头声明导出lua文件名\n", exportRule);
            return false;
        }
        string fileName = exportRule.Substring(0, colonIndex).Trim();
        // 判断是否在最后的花括号内声明table value中包含的字段
        int leftBraceIndex = exportRule.LastIndexOf('{');
        int rightBraceIndex = exportRule.LastIndexOf('}');
        // 解析依次作为索引的字段名
        string indexFieldNameString = null;
        // 注意分析花括号时要考虑到未声明table value中的字段而在某索引字段完整性检查规则中用花括号声明了有效值的情况
        if (exportRule.EndsWith("}") && leftBraceIndex != -1)
            indexFieldNameString = exportRule.Substring(colonIndex + 1, leftBraceIndex - colonIndex - 1);
        else
            indexFieldNameString = exportRule.Substring(colonIndex + 1, exportRule.Length - colonIndex - 1);

        string[] indexFieldDefine = indexFieldNameString.Split(new char[] { '-' }, System.StringSplitOptions.RemoveEmptyEntries);
        // 用于索引的字段列表
        List<FieldInfo> indexField = new List<FieldInfo>();
        // 索引字段对应的完整性检查规则
        List<string> integrityCheckRules = new List<string>();
        if (indexFieldDefine.Length < 1)
        {
            errorString = string.Format("导出配置\"{0}\"定义错误，用于索引的字段不能为空，请按fileName:indexFieldName1-indexFieldName2{otherFieldName1,otherFieldName2}的格式配置\n", exportRule);
            return false;
        }
        // 检查字段是否存在且为int、float、string或lang型
        foreach (string fieldDefine in indexFieldDefine)
        {
            string fieldName = null;
            // 判断是否在字段名后用小括号声明了该字段的完整性检查规则
            int leftBracketIndex = fieldDefine.IndexOf('(');
            int rightBracketIndex = fieldDefine.IndexOf(')');
            if (leftBracketIndex > 0 && rightBracketIndex > leftBracketIndex)
            {
                fieldName = fieldDefine.Substring(0, leftBracketIndex);
                string integrityCheckRule = fieldDefine.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1).Trim();
                if (string.IsNullOrEmpty(integrityCheckRule))
                {
                    errorString = string.Format("导出配置\"{0}\"定义错误，用于索引的字段\"{1}\"若要声明完整性检查规则就必须在括号中填写否则不要加括号\n", exportRule, fieldName);
                    return false;
                }
                integrityCheckRules.Add(integrityCheckRule);
            }
            else
            {
                fieldName = fieldDefine.Trim();
                integrityCheckRules.Add(null);
            }

            FieldInfo fieldInfo = tableInfo.GetFieldInfoByFieldName(fieldName);
            if (fieldInfo == null)
            {
                errorString = string.Format("导出配置\"{0}\"定义错误，声明的索引字段\"{1}\"不存在\n", exportRule, fieldName);
                return false;
            }
            if (fieldInfo.DataType != DataType.Int && fieldInfo.DataType != DataType.Long && fieldInfo.DataType != DataType.Float && fieldInfo.DataType != DataType.String && fieldInfo.DataType != DataType.Lang)
            {
                errorString = string.Format("导出配置\"{0}\"定义错误，声明的索引字段\"{1}\"为{2}型，但只允许为int、long、float、string或lang型\n", exportRule, fieldName, fieldInfo.DataType);
                return false;
            }

            // 对索引字段进行非空检查
            if (fieldInfo.DataType == DataType.String)
            {
                FieldCheckRule stringNotEmptyCheckRule = new FieldCheckRule();
                stringNotEmptyCheckRule.CheckType = TableCheckType.NotEmpty;
                stringNotEmptyCheckRule.CheckRuleString = "notEmpty[trim]";
                TableCheckHelper.CheckNotEmpty(fieldInfo, stringNotEmptyCheckRule, out errorString);
                if (errorString != null)
                {
                    errorString = string.Format("按配置\"{0}\"进行自定义导出错误，string型索引字段\"{1}\"中存在以下空值，而作为索引的key不允许为空\n{2}\n", exportRule, fieldName, errorString);
                    return false;
                }
            }
            else if (fieldInfo.DataType == DataType.Lang)
            {
                FieldCheckRule langNotEmptyCheckRule = new FieldCheckRule();
                langNotEmptyCheckRule.CheckType = TableCheckType.NotEmpty;
                langNotEmptyCheckRule.CheckRuleString = "notEmpty[key|value]";
                TableCheckHelper.CheckNotEmpty(fieldInfo, langNotEmptyCheckRule, out errorString);
                if (errorString != null)
                {
                    errorString = string.Format("按配置\"{0}\"进行自定义导出错误，lang型索引字段\"{1}\"中存在以下空值，而作为索引的key不允许为空\n{2}\n", exportRule, fieldName, errorString);
                    return false;
                }
            }
            else if (CheckStruct.IsAllowedNullNumber == true)
            {
                FieldCheckRule numberNotEmptyCheckRule = new FieldCheckRule();
                numberNotEmptyCheckRule.CheckType = TableCheckType.NotEmpty;
                numberNotEmptyCheckRule.CheckRuleString = "notEmpty";
                TableCheckHelper.CheckNotEmpty(fieldInfo, numberNotEmptyCheckRule, out errorString);
                if (errorString != null)
                {
                    errorString = string.Format("按配置\"{0}\"进行自定义导出错误，{1}型索引字段\"{2}\"中存在以下空值，而作为索引的key不允许为空\n{3}\n", exportRule, fieldInfo.DataType.ToString(), fieldName, errorString);
                    return false;
                }
            }

            indexField.Add(fieldInfo);
        }

        // 解析table value中要输出的字段名
        List<FieldInfo> tableValueField = new List<FieldInfo>();
        // 如果在花括号内配置了table value中要输出的字段名
        if (exportRule.EndsWith("}") && leftBraceIndex != -1 && leftBraceIndex < rightBraceIndex)
        {
            string tableValueFieldName = exportRule.Substring(leftBraceIndex + 1, rightBraceIndex - leftBraceIndex - 1);
            string[] fieldNames = tableValueFieldName.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (fieldNames.Length < 1)
            {
                errorString = string.Format("导出配置\"{0}\"定义错误，花括号中声明的table value中的字段不能为空，请按fileName:indexFieldName1-indexFieldName2{otherFieldName1,otherFieldName2}的格式配置\n", exportRule);
                return false;
            }
            // 检查字段是否存在
            foreach (string fieldName in fieldNames)
            {
                FieldInfo fieldInfo = tableInfo.GetFieldInfoByFieldName(fieldName);
                if (fieldInfo == null)
                {
                    errorString = string.Format("导出配置\"{0}\"定义错误，声明的table value中的字段\"{1}\"不存在\n", exportRule, fieldName);
                    return false;
                }

                if (tableValueField.Contains(fieldInfo))
                    AppLog.LogWarning(string.Format("警告：导出配置\"{0}\"定义中，声明的table value中的字段存在重复，字段名为{1}（列号{2}），本工具只生成一次，请修正错误\n", exportRule, fieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1)));
                else
                    tableValueField.Add(fieldInfo);
            }
        }
        else if (exportRule.EndsWith("}") && leftBraceIndex == -1)
        {
            errorString = string.Format("导出配置\"{0}\"定义错误，声明的table value中花括号不匹配\n", exportRule);
            return false;
        }
        // 如果未在花括号内声明，则默认将索引字段之外的所有字段进行填充
        else
        {
            List<string> indexFieldNameList = new List<string>(indexFieldDefine);
            foreach (FieldInfo fieldInfo in tableInfo.GetAllClientFieldInfo())
            {
                if (!indexFieldNameList.Contains(fieldInfo.FieldName))
                    tableValueField.Add(fieldInfo);
            }
        }

        // 解析完依次作为索引的字段以及table value中包含的字段后，按索引要求组成相应的嵌套数据结构
        Dictionary<object, object> data = new Dictionary<object, object>();
        int rowCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        for (int i = 0; i < rowCount; ++i)
        {
            Dictionary<object, object> temp = data;
            // 生成除最内层的数据结构
            for (int j = 0; j < indexField.Count - 1; ++j)
            {
                FieldInfo oneIndexField = indexField[j];
                var tempData = oneIndexField.Data[i];
                if (!temp.ContainsKey(tempData))
                    temp.Add(tempData, new Dictionary<object, object>());

                temp = (Dictionary<object, object>)temp[tempData];
            }
            // 最内层的value存数据的int型行号（从0开始计）
            FieldInfo lastIndexField = indexField[indexField.Count - 1];
            var lastIndexFieldData = _GetOneField(lastIndexField, i, 1, out errorString);
            if (!temp.ContainsKey(lastIndexFieldData))
                temp.Add(lastIndexFieldData, i);
            else
            {
                errorString = string.Format("错误：对表格{0}按\"{1}\"规则进行特殊索引导出时发现第{2}行与第{3}行在各个索引字段的值完全相同，导出被迫停止，请修正错误后重试\n", tableInfo.TableName, exportRule, i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, temp[lastIndexFieldData]);
                AppLog.LogErrorAndExit(errorString);
                return false;
            }
        }
        // 进行数据完整性检查
        if (CheckStruct.IsNeedCheck == true)
        {
            TableCheckHelper.CheckTableIntegrity(indexField, data, integrityCheckRules, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("错误：对表格{0}按\"{1}\"规则进行特殊索引导时未通过数据完整性检查，导出被迫停止，请修正错误后重试：\n{2}\n", tableInfo.TableName, exportRule, errorString);
                return false;
            }
        }

        // 生成导出的文件内容
        StringBuilder content = new StringBuilder();
        // 生成数据内容开头
        content.AppendLine("%%--- coding:utf-8 ---");
        content.Append("-module(").Append(ErlangStruct.ExportNameBeforeAdd + tableInfo.TableName).AppendLine(").");

        string oneFieldString = null;
        // 当前缩进量
        int currentLevel = 1;
        foreach (var key in data.Keys)
        {
            // 生成key
            if (key.GetType() == typeof(int) || key.GetType() == typeof(float))
                content.Append("get(").Append(key).AppendLine(")->");
            else if (key.GetType() == typeof(string))
            {
                content.Append("get(").Append(key).AppendLine(")->");
            }
            else
            {
                errorString = string.Format("SpecialExportTableToErlang中出现非法类型的索引列类型{0}", key.GetType());
                AppLog.LogErrorAndExit(errorString);
                return false;
            }

           // content.Append(_GetErlangIndentation(currentLevel));
            content.Append("  #{");

            // 如果已是最内层，输出指定table value中的数据
            if (data[key].GetType() == typeof(int))
            {
                int column = 0;
                foreach (FieldInfo fieldInfo in tableValueField)
                {
                    column++;
                    if (column > 1)
                    {
                        content.Append(_GetErlangIndentation(currentLevel));
                    }
                    int rowIndex = (int)data[key];
                    oneFieldString = _GetOneField(fieldInfo, rowIndex, currentLevel, out errorString);
                    if (errorString != null)
                    {
                        errorString = string.Format("导出表格{0}失败，", tableInfo.TableName) + errorString;
                        return false;
                    }
                    else
                    {
                        if (column > 1)
                            content.Append(",");

                        content.Append("'").Append(fieldInfo.FieldName).Append("' => ").AppendLine(oneFieldString);
                    }
                }
            }
            content.AppendLine("  },");
        }



        // 生成数据内容结尾
        content.AppendLine("get(_N) -> false.");
        content.AppendLine("get_list() ->");
        content.Append("[");
        foreach (var key in data.Keys)
        {
            content.Append(key).Append(",");
        }

        content.Remove(content.Length - 1, 1);
        content.Append("]");

        string exportString = content.ToString();
        //if (ErlangStruct.IsNeedColumnInfo == true)
        //    exportString = _GetColumnInfo(tableInfo) + exportString;


        // 保存为erlang文件
        if (SaveErlang.SaveErlangFile(tableInfo.ExcelName, ExcelMethods.GetSaveTableName(fileName), exportString) == true)
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = "保存为erlang文件失败\n";
            return false;
        }
    }
    public static bool ExportTableToErlang(TableInfo tableInfo, out string errorString)
    {
        StringBuilder content = new StringBuilder();


        // 当前缩进量
        int currentLevel = 1;

        // 判断是否设置要将主键列的值作为导出的table中的元素
        bool isAddKeyToLuaTable = tableInfo.TableConfig != null && tableInfo.TableConfig.ContainsKey(ErlangStruct.Excel_Config_AddKeyToErlangTable) && tableInfo.TableConfig[ErlangStruct.Excel_Config_AddKeyToErlangTable].Count > 0 && "true".Equals(tableInfo.TableConfig[ErlangStruct.Excel_Config_AddKeyToErlangTable][0], StringComparison.CurrentCultureIgnoreCase);

        // 逐行读取表格内容生成lua table
        List<FieldInfo> allField = tableInfo.GetAllClientFieldInfo();
        int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        for (int row = 0; row < dataCount; ++row)
        {
            
            // 将主键列作为key生成

            FieldInfo keyColumnField = allField[0];
            
            if (keyColumnField.DataType == DataType.Int || keyColumnField.DataType == DataType.Long)
                content.Append("get(").Append(keyColumnField.Data[row]).AppendLine(")->");
            // 注意：像“1_2”这样的字符串作为table的key必须加[""]否则lua认为是语法错误
            else if (keyColumnField.DataType == DataType.String)
            {
                string FieldString = _GetOneField(keyColumnField, row, currentLevel, out errorString);
                content.Append("get(").Append(FieldString).AppendLine(")->");
            }
               
            else
            {
                errorString = "用ExportTableToLua导出不支持的主键列数据类型";
                AppLog.LogErrorAndExit(errorString);
                return false;
            }
           // content.Append(_GetErlangIndentation(currentLevel));
            content.Append("  #{");
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
            int i = 0;
            for (int column = 1; column < allField.Count; ++column)
            {
                string oneFieldString = _GetOneField(allField[column], row, currentLevel, out errorString);
                if (oneFieldString == null)
                    continue;
                else
                    i++;
                if (i > 1)
                {
                    if (ErlangStruct.ExportErlangIsFormat == true)
                        content.Append(_GetErlangIndentation(currentLevel));
                }
                
                
                if (errorString != null)
                {
                    errorString = string.Format("导出表格{0}失败，", tableInfo.TableName) + errorString;
                    return false;
                }
                else
                {
                    if(i>1)
                        content.Append(",");

                    if (ErlangStruct.ExportErlangIsFormat == true)
                        content.Append("'").Append(allField[column].FieldName).Append("' => ").AppendLine(oneFieldString);
                    else
                        content.Append("'").Append(allField[column].FieldName).Append("' => ").Append(oneFieldString);
                    //content.AppendLine(",");


                }
                   
            }

            // 一行数据生成完毕后添加右括号结尾等
            // --currentLevel;
            // content.Append(_GetErlangIndentation(currentLevel));
            //content.Remove(content.Length - 3, 1);
            content.AppendLine("};");
        }

        string exportString2 = content.ToString();
        //if (ErlangStruct.ExportErlangIsFormat == false)
        //{
        //    StringBuilder stringBuilder2 = new StringBuilder();
        //    for (int i = 0; i < exportString2.Length; ++i)
        //    {
        //        char c = exportString2[i];

        //        if (c == '\n' || c == '\r')
        //        {

        //        }
        //        else
        //            stringBuilder2.Append(c);
        //    }
        //    exportString2 = stringBuilder2.ToString();
        //}
        StringBuilder stringBuilder = new StringBuilder();
        // 生成数据内容开头
        stringBuilder.AppendLine("%%--- coding:utf-8 ---");
        stringBuilder.Append("-module(").Append(ErlangStruct.ExportNameBeforeAdd + tableInfo.TableName).AppendLine(").");
        stringBuilder.AppendLine(@"-export([get/1,get_list/0]).");
        stringBuilder.Append(exportString2);


        // 生成数据内容结尾
        stringBuilder.AppendLine("get(_N) -> false.");
        stringBuilder.AppendLine("get_list() ->");
        stringBuilder.Append("[");
        for (int i=0;i< dataCount;i++)
        {
            string FieldString = _GetOneField(allField[0], i, currentLevel, out errorString);
            stringBuilder.Append(FieldString).Append(",");
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        stringBuilder.Append("].");

        string exportString = stringBuilder.ToString();
        //if (ErlangStruct.IsNeedColumnInfo == true)
        //    exportString = _GetColumnInfo(tableInfo) + exportString;


        // 保存为erlang文件
        if (SaveErlang.SaveErlangFile(tableInfo.ExcelName, ExcelMethods.GetSaveTableName(tableInfo.TableName), exportString) == true)
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = "保存为erlang文件失败\n";
            return false;
        }
    }
    /// <summary>
    /// 生成要在Erlang文件最上方以注释形式展示的列信息
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
