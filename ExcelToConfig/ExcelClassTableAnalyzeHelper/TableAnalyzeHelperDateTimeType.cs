using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

public partial class TableAnalyzeHelper
{
    /// <summary>
    /// 解析date型的格式类型
    /// </summary>
    public static DateFormatType GetDateFormatType(string formatString)
    {
        formatString = formatString.Trim();
        if ("#1970sec".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return DateFormatType.ReferenceDateSec;
        else if ("#1970msec".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return DateFormatType.ReferenceDateMsec;
        else if ("#dateTable".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return DateFormatType.DataTable;
        else
            return DateFormatType.FormatString;
    }

    /// <summary>
    /// 解析time型的格式类型
    /// </summary>
    public static TimeFormatType GetTimeFormatType(string formatString)
    {
        formatString = formatString.Trim();
        if ("#sec".Equals(formatString, StringComparison.CurrentCultureIgnoreCase))
            return TimeFormatType.ReferenceTimeSec;
        else
            return TimeFormatType.FormatString;
    }

    /// <summary>
    /// 解析date型数据的定义
    /// </summary>
    private static bool _AnalyzeDateType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        // 解析date型输入导出格式的声明
        if (!DateTimeValue.DefineDateStartString.Equals(fieldInfo.DataTypeString, StringComparison.CurrentCultureIgnoreCase))
        {
            int leftBracketIndex = fieldInfo.DataTypeString.IndexOf('(');
            int rightBracketIndex = fieldInfo.DataTypeString.LastIndexOf(')');
            if (leftBracketIndex == -1 && rightBracketIndex == -1)
            {
                errorString = "date型格式定义错误";
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            if (!(leftBracketIndex != -1 || rightBracketIndex > leftBracketIndex))
            {
                errorString = "date型格式定义错误，括号不匹配";
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }

            // 解析声明的时间格式
            string defineString = fieldInfo.DataTypeString.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1).Trim();
            if (string.IsNullOrEmpty(defineString))
            {
                errorString = "date型格式定义错误，若要声明时间格式就必须在括号中填写，否则不要加括号，本工具会采用config配置文件中设置的默认时间格式";
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            // 通过|分隔各个时间参数
            string[] defineParams = defineString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder paramDefineErrorStringBuilder = new StringBuilder();
            const string ERROR_STRING_FORMAT = "配置项\"{0}\"设置的格式\"{1}\"错误：{2}\n";
            foreach (string defineParam in defineParams)
            {
                // 通过=分隔参数项的key和value
                string[] paramKeyAndValue = defineParam.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (paramKeyAndValue.Length != 2)
                {
                    paramDefineErrorStringBuilder.AppendFormat("配置参数\"{0}\"未正确用=分隔配置项的key和value\n", defineParam);
                    continue;
                }
                string paramKey = paramKeyAndValue[0].Trim();
                string paramValue = paramKeyAndValue[1].Trim();

                errorString = null;
                switch (paramKey)
                {
                    case DateTimeValue.DateInputParamKey:
                        {
                            if (TableCheckHelper.CheckDateInputDefine(paramValue, out errorString) == true)
                                fieldInfo.ExtraParam[DateTimeValue.DateInputFormat] = paramValue;
                            else
                                paramDefineErrorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, paramKey, paramValue, errorString);

                            break;
                        }
                    case LuaStruct.DateToExportParamKey:
                        {
                            if (TableCheckHelper.CheckDateToLuaDefine(paramValue, out errorString) == true)
                                fieldInfo.ExtraParam[LuaStruct.DateToExportFormatKey] = paramValue;
                            else
                                paramDefineErrorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, paramKey, paramValue, errorString);

                            break;
                        }
                    case MySQLStruct.DateToExportParamKey:
                        {
                            if (TableCheckHelper.CheckDateToDatabaseDefine(paramValue, out errorString) == true)
                                fieldInfo.ExtraParam[MySQLStruct.DateToExportFormatKey] = paramValue;
                            else
                                paramDefineErrorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, paramKey, paramValue, errorString);

                            break;
                        }
                    default:
                        {
                            paramDefineErrorStringBuilder.AppendFormat("存在非法配置项key\"{0}\"\n", paramKey);
                            break;
                        }
                }
            }
            string paramDefineErrorString = paramDefineErrorStringBuilder.ToString();
            if (!string.IsNullOrEmpty(paramDefineErrorString))
            {
                errorString = string.Format("date型格式定义存在以下错误：\n{0}", paramDefineErrorString);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
        }

        // 检查date型输入格式、导出至lua文件格式、导出至MySQL数据库格式是否都已声明，没有则分别采用config文件的默认设置
        if (!fieldInfo.ExtraParam.ContainsKey(DateTimeValue.DateInputFormat))
        {
            if (DateTimeValue.DefaultDateInputFormat == null)
            {
                errorString = string.Format("未声明date型的输入格式，在config配置文件中也未定义名为\"{0}\"的默认格式配置项", DateTimeValue.APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            else
                fieldInfo.ExtraParam[DateTimeValue.DateInputFormat] = DateTimeValue.DefaultDateInputFormat;
        }
        if (!fieldInfo.ExtraParam.ContainsKey(LuaStruct.DateToExportFormatKey))
        {
            if (LuaStruct.DefaultDateToExportFormat == null)
            {
                errorString = string.Format("未声明date型导出至lua文件的格式，在config配置文件中也未定义名为\"{0}\"的默认格式配置项", LuaStruct.DefaultDateToExportFormatKey);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            else
                fieldInfo.ExtraParam[LuaStruct.DateToExportFormatKey] = LuaStruct.DefaultDateToExportFormat;
        }
        if (!fieldInfo.ExtraParam.ContainsKey(MySQLStruct.DateToExportFormatKey))
        {
            if (MySQLStruct.DefaultDateToExportFormat == null)
            {
                errorString = string.Format("未声明date型导出至MySQL数据库的格式，在config配置文件中也未定义名为\"{0}\"的默认格式配置项", MySQLStruct.DefaultDateToExportFormatKey);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            else
                fieldInfo.ExtraParam[MySQLStruct.DateToExportFormatKey] = MySQLStruct.DefaultDateToExportFormat;
        }

        DateFormatType dateFormatType = GetDateFormatType((string)fieldInfo.ExtraParam[DateTimeValue.DateInputFormat]);
        fieldInfo.Data = new List<object>();
        // 记录非法数据的行号以及数据值（key：行号， value：数据值）
        Dictionary<int, object> invalidInfo = new Dictionary<int, object>();

        if (dateFormatType == DateFormatType.FormatString)
        {
            // 用于对时间格式进行转换
            DateTimeFormatInfo dateTimeFormat = new DateTimeFormatInfo();
            dateTimeFormat.ShortDatePattern = (string)fieldInfo.ExtraParam[DateTimeValue.DateInputFormat];

            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                // 如果本行该字段的父元素标记为无效则该数据也标为无效
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                    // 忽略未填写的数据
                    if (string.IsNullOrEmpty(inputData))
                        fieldInfo.Data.Add(null);
                    else
                    {
                        try
                        {
                            DateTime dateTime = Convert.ToDateTime(inputData, dateTimeFormat);
                            fieldInfo.Data.Add(dateTime);
                        }
                        catch
                        {
                            invalidInfo.Add(row, inputData);
                        }
                    }
                }
            }
        }
        else if (dateFormatType == DateFormatType.ReferenceDateSec)
        {
            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                // 如果本行该字段的父元素标记为无效则该数据也标为无效
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                    ulong inputLongValue = 0;
                    if (ulong.TryParse(inputData, out inputLongValue) == false)
                        invalidInfo.Add(row, inputData);
                    else
                    {
                        DateTime dateTime = DateTimeValue.REFERENCE_DATE.AddSeconds((double)inputLongValue);
                        fieldInfo.Data.Add(dateTime);
                    }
                }
            }
        }
        else if (dateFormatType == DateFormatType.ReferenceDateMsec)
        {
            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                // 如果本行该字段的父元素标记为无效则该数据也标为无效
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                    ulong inputLongValue = 0;
                    if (ulong.TryParse(inputData, out inputLongValue) == false)
                        invalidInfo.Add(row, inputData);
                    else
                    {
                        DateTime dateTime = DateTimeValue.REFERENCE_DATE.AddMilliseconds((double)inputLongValue);
                        fieldInfo.Data.Add(dateTime);
                    }
                }
            }
        }
        else
        {
            errorString = "错误：用_AnalyzeDateType函数处理非法的DateFormatType类型";
            AppLog.LogErrorAndExit(errorString);
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }

        if (invalidInfo.Count > 0)
        {
            StringBuilder invalidDataInfo = new StringBuilder();
            invalidDataInfo.AppendFormat("以下行中的数据无法按指定的输入格式（{0}）进行读取\n", fieldInfo.ExtraParam[DateTimeValue.DateInputFormat]);
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

    /// <summary>
    /// 解析time型数据的定义
    /// </summary>
    private static bool _AnalyzeTimeType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        //const string DefineTimeStartString = "time";// DEFINE_START_STRING

        // 定义time型输入导出格式的key
        //const string TimeInputParamKey = "input";// INPUT_PARAM_KEY
        // const string TimeToExportParamKey = "toLua";// TO_LUA_PARAM_KEY
        // const string TimeToExportParamKey = "toDatabase";// TO_DATABASE_PARAM_KEY
        // 解析time型输入导出格式的声明
        if (!DateTimeValue.DefineTimeStartString.Equals(fieldInfo.DataTypeString, StringComparison.CurrentCultureIgnoreCase))
        {
            int leftBracketIndex = fieldInfo.DataTypeString.IndexOf('(');
            int rightBracketIndex = fieldInfo.DataTypeString.LastIndexOf(')');
            if (leftBracketIndex == -1 && rightBracketIndex == -1)
            {
                errorString = "time型格式定义错误";
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            if (!(leftBracketIndex != -1 || rightBracketIndex > leftBracketIndex))
            {
                errorString = "time型格式定义错误，括号不匹配";
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }

            // 解析声明的时间格式
            string defineString = fieldInfo.DataTypeString.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1).Trim();
            if (string.IsNullOrEmpty(defineString))
            {
                errorString = "time型格式定义错误，若要声明时间格式就必须在括号中填写，否则不要加括号，本工具会采用config配置文件中设置的默认时间格式";
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            // 通过|分隔各个时间参数
            string[] defineParams = defineString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder paramDefineErrorStringBuilder = new StringBuilder();
            const string ERROR_STRING_FORMAT = "配置项\"{0}\"设置的格式\"{1}\"错误：{2}\n";
            foreach (string defineParam in defineParams)
            {
                // 通过=分隔参数项的key和value
                string[] paramKeyAndValue = defineParam.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (paramKeyAndValue.Length != 2)
                {
                    paramDefineErrorStringBuilder.AppendFormat("配置参数\"{0}\"未正确用=分隔配置项的key和value\n", defineParam);
                    continue;
                }
                string paramKey = paramKeyAndValue[0].Trim();
                string paramValue = paramKeyAndValue[1].Trim();

                errorString = null;
                switch (paramKey)
                {
                    case DateTimeValue.TimeInputParamKey:
                        {
                            if (TableCheckHelper.CheckTimeDefine(paramValue, out errorString) == true)
                                fieldInfo.ExtraParam[DateTimeValue.TimeInputFormat] = paramValue;
                            else
                                paramDefineErrorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, paramKey, paramValue, errorString);

                            break;
                        }
                    case LuaStruct.TimeToExportParamKey:
                        {
                            if (TableCheckHelper.CheckTimeDefine(paramValue, out errorString) == true)
                                fieldInfo.ExtraParam[LuaStruct.TimeToExportFormatKey] = paramValue;
                            else
                                paramDefineErrorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, paramKey, paramValue, errorString);

                            break;
                        }
                    case MySQLStruct.TimeToExportParamKey:
                        {
                            if (TableCheckHelper.CheckTimeDefine(paramValue, out errorString) == true)
                                fieldInfo.ExtraParam[MySQLStruct.TimeToExportFormatKey] = paramValue;
                            else
                                paramDefineErrorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, paramKey, paramValue, errorString);

                            break;
                        }
                    default:
                        {
                            paramDefineErrorStringBuilder.AppendFormat("存在非法配置项key\"{0}\"\n", paramKey);
                            break;
                        }
                }
            }
            string paramDefineErrorString = paramDefineErrorStringBuilder.ToString();
            if (!string.IsNullOrEmpty(paramDefineErrorString))
            {
                errorString = string.Format("time型格式定义存在以下错误：\n{0}", paramDefineErrorString);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
        }

        // 检查time型输入格式、导出至lua文件格式、导出至MySQL数据库格式是否都已声明，没有则分别采用config文件的默认设置
        if (!fieldInfo.ExtraParam.ContainsKey(DateTimeValue.TimeInputFormat))
        {
            if (DateTimeValue.DefaultTimeInputFormat == null)
            {
                errorString = string.Format("未声明time型的输入格式，在config配置文件中也未定义名为\"{0}\"的默认格式配置项", DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            else
                fieldInfo.ExtraParam[DateTimeValue.TimeInputFormat] = DateTimeValue.DefaultTimeInputFormat;
        }
        if (!fieldInfo.ExtraParam.ContainsKey(LuaStruct.TimeToExportFormatKey))
        {
            if (LuaStruct.DefaultTimeToExportFormat == null)
            {
                errorString = string.Format("未声明time型导出至lua文件的格式，在config配置文件中也未定义名为\"{0}\"的默认格式配置项", LuaStruct.DefaultTimeToExportFormatKey);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            else
                fieldInfo.ExtraParam[LuaStruct.TimeToExportFormatKey] = LuaStruct.DefaultTimeToExportFormat;
        }
        if (!fieldInfo.ExtraParam.ContainsKey(MySQLStruct.TimeToExportFormatKey))
        {
            if (MySQLStruct.DefaultTimeToExportFormat == null)
            {
                errorString = string.Format("未声明time型导出至MySQL数据库的格式，在config配置文件中也未定义名为\"{0}\"的默认格式配置项", MySQLStruct.DefaultTimeToExportFormatKey);
                nextFieldColumnIndex = columnIndex + 1;
                return false;
            }
            else
                fieldInfo.ExtraParam[MySQLStruct.TimeToExportFormatKey] = MySQLStruct.DefaultTimeToExportFormat;
        }

        TimeFormatType timeFormatType = GetTimeFormatType((string)fieldInfo.ExtraParam[DateTimeValue.TimeInputFormat]);
        fieldInfo.Data = new List<object>();
        // 记录非法数据的行号以及数据值（key：行号， value：数据值）
        Dictionary<int, object> invalidInfo = new Dictionary<int, object>();

        if (timeFormatType == TimeFormatType.FormatString)
        {
            // 用于对时间格式进行转换
            DateTimeFormatInfo dateTimeFormat = new DateTimeFormatInfo();
            dateTimeFormat.ShortTimePattern = (string)fieldInfo.ExtraParam[DateTimeValue.TimeInputFormat];

            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                // 如果本行该字段的父元素标记为无效则该数据也标为无效
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                    // 忽略未填写的数据
                    if (string.IsNullOrEmpty(inputData))
                        fieldInfo.Data.Add(null);
                    else
                    {
                        try
                        {
                            // 此函数会将DateTime的日期部分自动赋值为当前时间
                            DateTime tempDateTime = Convert.ToDateTime(inputData, dateTimeFormat);
                            fieldInfo.Data.Add(new DateTime(DateTimeValue.REFERENCE_DATE.Year, DateTimeValue.REFERENCE_DATE.Month, DateTimeValue.REFERENCE_DATE.Day, tempDateTime.Hour, tempDateTime.Minute, tempDateTime.Second));
                        }
                        catch
                        {
                            invalidInfo.Add(row, inputData);
                        }
                    }
                }
            }
        }
        else if (timeFormatType == TimeFormatType.ReferenceTimeSec)
        {
            for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
            {
                // 如果本行该字段的父元素标记为无效则该数据也标为无效
                if (parentField != null && (bool)parentField.Data[row - ExcelTableSetting.DataFieldDataStartRowIndex] == false)
                    fieldInfo.Data.Add(null);
                else
                {
                    string inputData = dt.Rows[row][columnIndex].ToString().Trim();
                    uint inputIntValue = 0;
                    if (uint.TryParse(inputData, out inputIntValue) == false)
                        invalidInfo.Add(row, inputData);
                    else
                    {
                        if (inputIntValue >= 86400)
                            invalidInfo.Add(row, inputData);
                        else
                        {
                            int hour = (int)inputIntValue / 60 / 60;
                            int remainingSecond = (int)inputIntValue - hour * 60 * 60;
                            int minute = remainingSecond / 60;
                            remainingSecond = remainingSecond - minute * 60;
                            DateTime dateTime = new DateTime(DateTimeValue.REFERENCE_DATE.Year, DateTimeValue.REFERENCE_DATE.Month, DateTimeValue.REFERENCE_DATE.Day, hour, minute, remainingSecond);
                            fieldInfo.Data.Add(dateTime);
                        }
                    }
                }
            }
        }
        else
        {
            errorString = "错误：用_AnalyzeTimeType函数处理非法的TimeFormatType类型";
            AppLog.LogErrorAndExit(errorString);
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }

        if (invalidInfo.Count > 0)
        {
            StringBuilder invalidDataInfo = new StringBuilder();
            invalidDataInfo.AppendFormat("以下行中的数据无法按指定的输入格式（{0}）进行读取\n", fieldInfo.ExtraParam[DateTimeValue.TimeInputFormat]);
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
}