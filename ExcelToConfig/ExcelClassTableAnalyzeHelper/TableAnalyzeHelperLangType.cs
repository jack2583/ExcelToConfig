using System;
using System.Collections.Generic;
using System.Data;

public partial class TableAnalyzeHelper
{
    private static bool _AnalyzeLangType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        fieldInfo.LangKeys = new List<object>();
        fieldInfo.Data = new List<object>();

        // 如果是统一指定key名规则，需要解析替换规则
        Dictionary<string, List<object>> replaceInfo = null;
        string configString = null;
        if (!fieldInfo.DataTypeString.Equals("lang", StringComparison.CurrentCultureIgnoreCase))
        {
            replaceInfo = _GetLangKeyReplaceInfo(fieldInfo.DataTypeString, tableInfo, out configString, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("lang型格式定义错误，{0}", errorString);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }

            if (replaceInfo.Count < 1)
                AppLog.LogWarning(string.Format("警告：表格{0}中的Lang型字段{1}（列号：{2}）进行统一配置但使用的是完全相同的key名（{3}），请确定是否要这样设置", tableInfo.TableName, fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), configString));
        }

        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            // 如果本行该字段的父元素标记为无效则该数据也标为无效
            if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
            {
                fieldInfo.LangKeys.Add(null);
                fieldInfo.Data.Add(null);
                continue;
            }
            string inputData = null;
            // 未统一指定key名规则的Lang型数据，需在单元格中填写key
            if (replaceInfo == null)
                inputData = dt.Rows[row][columnIndex].ToString().Trim();
            // 统一指定key名规则的Lang型数据，根据规则生成在Lang文件中的具体key名
            else
            {
                inputData = configString;
                foreach (var item in replaceInfo)
                {
                    if (item.Value[row - ExcelTableSetting.DataFieldDataStartRowIndex] != null)
                        inputData = configString.Replace(item.Key, item.Value[row - ExcelTableSetting.DataFieldDataStartRowIndex].ToString());
                }
            }

            if (string.IsNullOrEmpty(inputData))
            {
                fieldInfo.LangKeys.Add(string.Empty);
                fieldInfo.Data.Add(null);
            }
            else
            {
                fieldInfo.LangKeys.Add(inputData);
                if (AppLang.LangData.ContainsKey(inputData))
                    fieldInfo.Data.Add(AppLang.LangData[inputData]);
                else
                    fieldInfo.Data.Add(null);
            }
        }

        errorString = null;
        nextFieldColumnIndex = columnIndex + 1;
        return true;
    }

    /// <summary>
    /// 获取统一配置的Lang型字段定义在合成key时需进行替换的信息（key：要替换的字符串形如{fieldName}， value：对应的字段数据列表）
    /// </summary>
    private static Dictionary<string, List<object>> _GetLangKeyReplaceInfo(string defineString, TableInfo tableInfo, out string configString, out string errorString)
    {
        Dictionary<string, List<object>> replaceInfo = new Dictionary<string, List<object>>();

        // 取出括号中key的配置
        int leftBracketIndex = defineString.IndexOf('(');
        int rightBracketIndex = defineString.LastIndexOf(')');
        if (leftBracketIndex == -1 || rightBracketIndex == -1 || leftBracketIndex > rightBracketIndex)
        {
            errorString = "lang类型统一key格式声明错误，必须形如lang(xxx{fieldName}xxx)，其中xxx可为任意内容，花括号中为拼成key的该行数据取哪个字段名下的值";
            configString = null;
            return null;
        }
        configString = defineString.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1).Trim();
        if (string.IsNullOrEmpty(configString))
        {
            errorString = "lang类型统一key格式声明错误，括号内声明的key名规则不能为空";
            return null;
        }

        int leftBraceIndex = -1;
        for (int i = 0; i < defineString.Length; ++i)
        {
            if (defineString[i] == '{' && leftBraceIndex == -1)
                leftBraceIndex = i;
            else if (defineString[i] == '}' && leftBraceIndex != -1)
            {
                // 取出花括号中包含的字段名检查是否存在
                string refFieldName = defineString.Substring(leftBraceIndex + 1, i - leftBraceIndex - 1).Trim();
                FieldInfo refFielfInfo = tableInfo.GetFieldInfoByFieldName(refFieldName);
                if (refFielfInfo != null)
                    replaceInfo.Add(defineString.Substring(leftBraceIndex, i - leftBraceIndex + 1), refFielfInfo.Data);
                else
                {
                    errorString = string.Format("lang类型统一key格式声明错误，找不到名为{0}的字段，注意所引用的字段必须在这个lang字段之前声明且不为集合类型子元素，否则无法找到", refFieldName);
                    return null;
                }

                leftBraceIndex = -1;
            }
        }

        errorString = null;
        return replaceInfo;
    }
}