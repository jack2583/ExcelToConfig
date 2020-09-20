using LitJson;
using System.Collections.Generic;
using System.Data;
using System.Text;

public partial class TableAnalyzeHelper
{
    private static bool _AnalyzeMapStringType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        StringBuilder errorStringBuilder = new StringBuilder();

        fieldInfo.MapStringFormatDefine =GetMapStringFormatDefine(fieldInfo.DataTypeString, out errorString);
        if (errorString != null)
        {
            errorString = string.Format("{0}，你输入的类型定义字符串为{1}", errorString, fieldInfo.DataTypeString);
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }
        // 将填写的数据存在FieldInfo的JsonString变量中，然后将数据转为JsonData存在Data变量中
        fieldInfo.JsonString = new List<string>();
        fieldInfo.Data = new List<object>();
        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            // 如果本行该字段的父元素标记为无效则该数据也标为无效
            if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
            {
                fieldInfo.JsonString.Add(null);
                fieldInfo.Data.Add(null);
            }
            else
            {
                string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                if (string.IsNullOrEmpty(inputData))
                {
                    fieldInfo.JsonString.Add(null);
                    fieldInfo.Data.Add(null);
                }
                else
                {
                    fieldInfo.JsonString.Add(inputData);

                    // 将输入的数据转为JsonData
                    JsonData jsonData = GetMapStringData(inputData, fieldInfo.MapStringFormatDefine, out errorString);
                    if (errorString == null)
                        fieldInfo.Data.Add(jsonData);
                    else
                    {
                        errorStringBuilder.AppendFormat("第{0}行填写的的数据（{1}）非法，{2}\n", row - ExcelTableSetting.DataFieldDataStartRowIndex + 1, inputData, errorString);
                        fieldInfo.Data.Add(null);
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
            errorString = string.Format("以下行中的数据不符合该字段mapString类型（{0}）的格式要求：\n{1}", fieldInfo.DataTypeString, errorString);
            return false;
        }
    }
}