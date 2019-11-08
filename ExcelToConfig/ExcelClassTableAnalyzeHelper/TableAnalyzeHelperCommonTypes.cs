using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public partial class TableAnalyzeHelper
{
    /// <summary>
    /// 将填写的数据类型字符串解析为DataType的枚举
    /// </summary>
    private static DataType _AnalyzeDataType(string inputTypeString)
    {
        if (string.IsNullOrEmpty(inputTypeString))
            return DataType.Invalid;

        string typeString = inputTypeString.Trim();

        if (typeString.StartsWith("int", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Int;
        if (typeString.StartsWith("long", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Long;
        else if (typeString.StartsWith("float", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Float;
        else if (typeString.StartsWith("string", StringComparison.CurrentCultureIgnoreCase))
            return DataType.String;
        else if (typeString.StartsWith("lang", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Lang;
        else if (typeString.StartsWith("bool", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Bool;
        else if (typeString.StartsWith("date", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Date;
        else if (typeString.StartsWith("time", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Time;
        else if (typeString.StartsWith("json", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Json;
        else if (typeString.StartsWith("tableString", StringComparison.CurrentCultureIgnoreCase))
            return DataType.TableString;
        else if (typeString.StartsWith("mapString", StringComparison.CurrentCultureIgnoreCase))
            return DataType.MapString;
        else if (typeString.StartsWith("array", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Array;
        else if (typeString.StartsWith("dict", StringComparison.CurrentCultureIgnoreCase))
            return DataType.Dict;
        else
            return DataType.Invalid;
    }

    private static bool _AnalyzeIntType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        fieldInfo.Data = new List<object>();
        // 记录非法数据的行号以及数据值（key：行号， value：数据值）
        Dictionary<int, string> invalidInfo = new Dictionary<int, string>();

        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            // 如果本行该字段的父元素标记为无效则该数据也标为无效
            if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                fieldInfo.Data.Add(null);
            else
            {
                string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                if (string.IsNullOrEmpty(inputData))
                {
                    if (CheckStruct.IsAllowedNullNumber == true)
                        fieldInfo.Data.Add(null);
                    else
                        invalidInfo.Add(row, inputData);
                }
                else
                {
                    int intValue;
                    bool isValid = int.TryParse(inputData, out intValue);
                    if (isValid)
                        fieldInfo.Data.Add(intValue);
                    else
                        invalidInfo.Add(row, inputData);
                }
            }
        }

        if (invalidInfo.Count > 0)
        {
            StringBuilder invalidDataInfo = new StringBuilder();
            invalidDataInfo.Append("以下行中数据不是合法的int类型的值：\n");

            foreach (var item in invalidInfo)
                invalidDataInfo.AppendFormat("第{0}行，错误地填写数据为\"{1}\"\n", item.Key + 1, item.Value);

            errorString = invalidDataInfo.ToString();
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }
        else
        {
            errorString = null;
            nextFieldColumnIndex = columnIndex + 1;
            return true;
        }
    }

    private static bool _AnalyzeLongType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        fieldInfo.Data = new List<object>();
        // 记录非法数据的行号以及数据值（key：行号， value：数据值）
        Dictionary<int, string> invalidInfo = new Dictionary<int, string>();

        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            // 如果本行该字段的父元素标记为无效则该数据也标为无效
            if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                fieldInfo.Data.Add(null);
            else
            {
                string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                if (string.IsNullOrEmpty(inputData))
                {
                    if (CheckStruct.IsAllowedNullNumber == true)
                        fieldInfo.Data.Add(null);
                    else
                        invalidInfo.Add(row, inputData);
                }
                else
                {
                    long longValue;
                    bool isValid = long.TryParse(inputData, out longValue);
                    if (isValid)
                        fieldInfo.Data.Add(longValue);
                    else
                        invalidInfo.Add(row, inputData);
                }
            }
        }

        if (invalidInfo.Count > 0)
        {
            StringBuilder invalidDataInfo = new StringBuilder();
            invalidDataInfo.Append("以下行中数据不是合法的long类型的值：\n");

            foreach (var item in invalidInfo)
                invalidDataInfo.AppendFormat("第{0}行，错误地填写数据为\"{1}\"\n", item.Key + 1, item.Value);

            errorString = invalidDataInfo.ToString();
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }
        else
        {
            errorString = null;
            nextFieldColumnIndex = columnIndex + 1;
            return true;
        }
    }

    private static bool _AnalyzeFloatType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        fieldInfo.Data = new List<object>();
        // 记录非法数据的行号以及数据值（key：行号， value：数据值）
        Dictionary<int, string> invalidInfo = new Dictionary<int, string>();

        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            // 如果本行该字段的父元素标记为无效则该数据也标为无效
            if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                fieldInfo.Data.Add(null);
            else
            {
                string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                if (string.IsNullOrEmpty(inputData))
                {
                    if (CheckStruct.IsAllowedNullNumber == true)
                        fieldInfo.Data.Add(null);
                    else
                        invalidInfo.Add(row, inputData);
                }
                else
                {
                    double floatValue;
                    bool isValid = double.TryParse(inputData, out floatValue);
                    if (isValid)
                        fieldInfo.Data.Add(floatValue);
                    else
                        invalidInfo.Add(row, inputData);
                }
            }
        }

        if (invalidInfo.Count > 0)
        {
            StringBuilder invalidDataInfo = new StringBuilder();
            invalidDataInfo.Append("以下行中数据不是合法的float类型的值：\n");

            foreach (var item in invalidInfo)
                invalidDataInfo.AppendFormat("第{0}行，错误地填写数据为\"{1}\"\n", item.Key + 1, item.Value);

            errorString = invalidDataInfo.ToString();
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }
        else
        {
            errorString = null;
            nextFieldColumnIndex = columnIndex + 1;
            return true;
        }
    }

    private static bool _AnalyzeBoolType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        fieldInfo.Data = new List<object>();
        // 记录非法数据的行号以及数据值（key：行号， value：数据值）
        Dictionary<int, string> invalidInfo = new Dictionary<int, string>();

        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            // 如果本行该字段的父元素标记为无效则该数据也标为无效
            if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                fieldInfo.Data.Add(null);
            else
            {
                string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                if ("1".Equals(inputData) || "true".Equals(inputData, StringComparison.CurrentCultureIgnoreCase))
                    fieldInfo.Data.Add(true);
                else if ("0".Equals(inputData) || "false".Equals(inputData, StringComparison.CurrentCultureIgnoreCase))
                    fieldInfo.Data.Add(false);
                else
                    invalidInfo.Add(row, inputData);
            }
        }

        if (invalidInfo.Count > 0)
        {
            StringBuilder invalidDataInfo = new StringBuilder();
            invalidDataInfo.Append("以下行中数据不是合法的bool值，正确填写bool值方式为填1或true代表真，0或false代表假：\n");

            foreach (var item in invalidInfo)
                invalidDataInfo.AppendFormat("第{0}行，错误地填写数据为\"{1}\"\n", item.Key + 1, item.Value);

            errorString = invalidDataInfo.ToString();
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }
        else
        {
            errorString = null;
            nextFieldColumnIndex = columnIndex + 1;
            return true;
        }
    }

    private static bool _AnalyzeStringType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        // 检查string型字段数据格式声明是否正确
        if (!"string(toErlang)".Equals(fieldInfo.DataTypeString) && !"string(trim)".Equals(fieldInfo.DataTypeString) && !"string".Equals(fieldInfo.DataTypeString))
        {
            errorString = string.Format("错误：string型字段定义非法，若要导出erlang特殊字符，声明为：string(toErlang)。若要自动去除输入字符串的首尾空白字符请将数据类型声明为\"string(trim)\"，否则声明为\"string\"，而你输入的为\"{0}\"", fieldInfo.DataTypeString);
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }

        fieldInfo.Data = new List<object>();

        if ("string(toErlang)".Equals(fieldInfo.DataTypeString, StringComparison.CurrentCultureIgnoreCase))
        {
            fieldInfo.ExportTable = FieldInfo.ExportTableType.ToErlang;
            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string str = dt.Rows[row][columnIndex].ToString();
                    AppLanguage.GetLanguageDictData(str);
                    fieldInfo.Data.Add(AppLanguage.GetNewLanguageText(str));
                }
            }
        }
        else if ("string(trim)".Equals(fieldInfo.DataTypeString, StringComparison.CurrentCultureIgnoreCase))
        {
            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                // 如果本行该字段的父元素标记为无效则该数据也标为无效
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string str = dt.Rows[row][columnIndex].ToString().Trim();
                    AppLanguage.GetLanguageDictData(str);
                    fieldInfo.Data.Add(AppLanguage.GetNewLanguageText(str));
                }
            }
        }
        else
        {
            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string str = dt.Rows[row][columnIndex].ToString();
                    AppLanguage.GetLanguageDictData(str);
                    fieldInfo.Data.Add(AppLanguage.GetNewLanguageText(str));
                }
            }
        }

        errorString = null;
        nextFieldColumnIndex = columnIndex + 1;
        return true;
    }
}