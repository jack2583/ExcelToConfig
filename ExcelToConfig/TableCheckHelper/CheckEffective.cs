﻿using System;
using System.Collections.Generic;
using System.Text;

public partial class TableCheckHelper
{
    /// <summary>
    /// 用于int、long、float、string、date或time型取值必须为指定有效取值中的一个的检查
    /// </summary>
    public static bool CheckEffective(FieldInfo fieldInfo, FieldCheckRule checkRule, out string errorString)
    {
        List<object> repeatedSetValue = null;
        List<int> errorDataIndex = null;
        _GetValueIsInSet(fieldInfo.Data, fieldInfo.DataType, checkRule.CheckRuleString, true, out repeatedSetValue, out errorDataIndex, out errorString);

        if (errorString == null)
        {
            if (repeatedSetValue.Count > 0)
            {
                foreach (object setValue in repeatedSetValue)
                {
                    if (fieldInfo.DataType == DataType.Int || fieldInfo.DataType == DataType.Long || fieldInfo.DataType == DataType.Float || fieldInfo.DataType == DataType.String)
                        AppLog.LogWarning(string.Format("警告：字段{0}（列号：{1}）的值有效性检查规则定义中，出现了相同的有效值\"{2}\"，本工具忽略此问题继续进行检查，需要你之后修正规则定义错误\n", fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), setValue));
                    else if (fieldInfo.DataType == DataType.Date)
                    {
                        DateTime dataTimeSetValue = (DateTime)setValue;
                        AppLog.LogWarning(string.Format("警告：字段{0}（列号：{1}）的值有效性检查规则定义中，出现了相同的有效值\"{2}\"，本工具忽略此问题继续进行检查，需要你之后修正规则定义错误\n", fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), dataTimeSetValue.ToString(DateTimeValue.APP_DEFAULT_DATE_FORMAT)));
                    }
                    else if (fieldInfo.DataType == DataType.Time)
                    {
                        DateTime dataTimeSetValue = (DateTime)setValue;
                        AppLog.LogWarning(string.Format("警告：字段{0}（列号：{1}）的值有效性检查规则定义中，出现了相同的有效值\"{2}\"，本工具忽略此问题继续进行检查，需要你之后修正规则定义错误\n", fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), dataTimeSetValue.ToString(DateTimeValue.APP_DEFAULT_TIME_FORMAT)));
                    }
                }
            }
            if (errorDataIndex.Count > 0)
            {
                StringBuilder illegalValueInfo = new StringBuilder();
                foreach (int dataIndex in errorDataIndex)
                {
                    if (fieldInfo.DataType == DataType.Int || fieldInfo.DataType == DataType.Long || fieldInfo.DataType == DataType.Float || fieldInfo.DataType == DataType.String)
                        illegalValueInfo.AppendFormat("第{0}行数据\"{1}\"不属于有效取值中的一个\n", dataIndex + ExcelTableSetting.DataFieldDataStartRowIndex + 1, fieldInfo.Data[dataIndex]);
                    else if (fieldInfo.DataType == DataType.Date)
                    {
                        DateTime dataTimeValue = (DateTime)fieldInfo.Data[dataIndex];
                        illegalValueInfo.AppendFormat("第{0}行数据\"{1}\"不属于有效取值中的一个\n", dataIndex + ExcelTableSetting.DataFieldDataStartRowIndex + 1, dataTimeValue.ToString(DateTimeValue.APP_DEFAULT_DATE_FORMAT));
                    }
                    else if (fieldInfo.DataType == DataType.Time)
                    {
                        DateTime dataTimeValue = (DateTime)fieldInfo.Data[dataIndex];
                        illegalValueInfo.AppendFormat("第{0}行数据\"{1}\"不属于有效取值中的一个\n", dataIndex + ExcelTableSetting.DataFieldDataStartRowIndex + 1, dataTimeValue.ToString(DateTimeValue.APP_DEFAULT_TIME_FORMAT));
                    }
                }

                errorString = illegalValueInfo.ToString();
                return false;
            }
            else
                return true;
        }
        else
        {
            errorString = errorString + "\n";
            return false;
        }
    }
}