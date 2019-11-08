using LitJson;
using System.Collections.Generic;
using System.Data;
using System.Text;

public partial class TableAnalyzeHelper
{
    /// <summary>
    /// 解析json型数据的定义，将json通过LitJson库解析出来
    /// </summary>
    private static bool _AnalyzeJsonType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        StringBuilder errorStringBuilder = new StringBuilder();
        fieldInfo.Data = new List<object>();
        fieldInfo.JsonString = new List<string>();
        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            // 如果本行该字段的父元素标记为无效则该数据也标为无效
            if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
            {
                fieldInfo.Data.Add(null);
                fieldInfo.JsonString.Add(null);
            }
            else
            {
                string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                if (string.IsNullOrEmpty(inputData))
                {
                    fieldInfo.Data.Add(null);
                    fieldInfo.JsonString.Add(null);
                }
                else
                {
                    fieldInfo.JsonString.Add(inputData);
                    try
                    {
                        JsonData jsonData = JsonMapper.ToObject(inputData);
                        fieldInfo.Data.Add(jsonData);
                    }
                    catch (JsonException exception)
                    {
                        errorStringBuilder.AppendFormat("第{0}行所填json字符串（{1}）非法，原因为：{2}\n", row + ExcelTableSetting.DataFieldDataStartRowIndex + 1, inputData, exception.Message);
                    }
                }
            }
        }

        nextFieldColumnIndex = columnIndex + 1;
        errorString = errorStringBuilder.ToString();
        if (string.IsNullOrEmpty(errorString))
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = string.Concat("以下行中所填json字符串非法：\n", errorString);
            return false;
        }
    }
}