//using LitJson;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;

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
                string inputData =dt.Rows[row][columnIndex].ToString().Trim();
                AppLanguage.GetLanguageDictData(inputData);
                inputData = AppLanguage.GetNewLanguageText(inputData);
                if (string.IsNullOrEmpty(inputData))
                {
                    fieldInfo.Data.Add(null);
                    fieldInfo.JsonString.Add(null);
                }
                else if (inputData == "[]")
                {
                    fieldInfo.Data.Add(null);
                    fieldInfo.JsonString.Add("[]");
                }
                else
                {
                    fieldInfo.JsonString.Add(inputData);
                    try
                    {
                        try
                        {
                            LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(inputData);

                            object jsonData2 = JsonConvert.DeserializeObject(inputData);
                            // TestModel testModel = JsonConvert.DeserializeObject<TestModel>(inputData);
                            //fieldInfo.Data.Add(jsonData);
                            //Count = “jsonData.Count”引发了类型“System.InvalidOperationException”的异常
                            if (jsonData.Count > 0)
                                fieldInfo.Data.Add(jsonData);
                        }
                        catch
                        {
                            errorStringBuilder.AppendFormat("第{0}行所填json字符串（{1}）非法，原因为：不是合的json字符\n", row + ExcelTableSetting.DataFieldDataStartRowIndex + 1, inputData);
                        }
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