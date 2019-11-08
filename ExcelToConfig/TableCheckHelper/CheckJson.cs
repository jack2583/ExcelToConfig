using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;


public partial class TableCheckHelper
{
    /// <summary>
    /// 用于int、long、float或string型取值必须在另一字段（可能还是这张表格也可能跨表）中有对应值的检查
    /// jsonref:(2|ref:item-id|noCheck|[2,5])
    /// 前面数字为json层数，[]为1，[[]]为2
    /// </summary>
    public static bool CheckJson(FieldInfo fieldInfo, FieldCheckRule checkRule, out string errorString)
    {
        errorString = null;
        bool isNumberDataType = fieldInfo.DataType == DataType.Int || fieldInfo.DataType == DataType.Long || fieldInfo.DataType == DataType.Float;
        bool isTimeDataType = fieldInfo.DataType == DataType.Date || fieldInfo.DataType == DataType.Time;
        bool isStringDataType = fieldInfo.DataType == DataType.String || fieldInfo.DataType == DataType.Lang;
        if (fieldInfo.DataType != DataType.Json)
        {
            errorString = string.Format("jsonref检查只能用于json的字段值的引用检查，而该字段为{0}型\n", fieldInfo.DataType);
            return false;
        }
        // 检查填写的检查规则是否正确
        bool isIncludeFloor;
        bool isIncludeCeil;
        bool isCheckFloor;
        bool isCheckCeil;
        double floorValue = 0;
        double ceilValue = 0;
        DateTime floorDateTime = DateTimeValue.REFERENCE_DATE;
        DateTime ceilDateTime = DateTimeValue.REFERENCE_DATE;
        // 规则首位必须为方括号或者圆括号
        if (checkRule.CheckRuleString.StartsWith("jsonref:("))
            isIncludeFloor = true;
        else
        {
            errorString = "jsonref检查定义错误：必须用jsonref:(开头，以|分割\n";
            return false;
        }
        // 规则末位必须为方括号或者圆括号
        if (checkRule.CheckRuleString.EndsWith(")"))
            isIncludeCeil = true;
        else
        {
            errorString = "jsonref检查定义错误：必须用英文)，中间部分以|分割\n";
            return false;
        }
        // 去掉首尾的括号
        string temp = checkRule.CheckRuleString.Substring(9, checkRule.CheckRuleString.Length - 11);
        // 通过英文逗号分隔上下限
        string[] floorAndCeilString = temp.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        int floorandCeilStrinLength = floorAndCeilString.Length;
        if (floorandCeilStrinLength < 2)
        {
            errorString = "值范围检查定义错误：必须用一个|分隔,如：jsonref:(2|ref:item-id|noCheck|[2,5])\n";
            return false;
        }
        int floorString =int.Parse(floorAndCeilString[0].Trim());
        string ceilString = floorAndCeilString[1].Trim();
        string ceilString2 = "";
        string ceilString3 = "";
        string ceilString4 = "";
        string ceilString5 = "";

        if (floorandCeilStrinLength > 2)
            ceilString2= floorAndCeilString[2].Trim();

        if (floorandCeilStrinLength > 3)
            ceilString3 = floorAndCeilString[3].Trim();

        if (floorandCeilStrinLength > 4)
            ceilString4 = floorAndCeilString[4].Trim();

        if (floorandCeilStrinLength > 5)
            ceilString5 = floorAndCeilString[5].Trim();

        //对定义的检查字符串进行解析
        if(ceilString.Length>0 && ceilString.StartsWith("$"))
        {
            ceilString = AppValues.ConfigData[ceilString];
        }
        if (ceilString2.Length > 0 && ceilString2.StartsWith("$"))
        {
            ceilString2 = AppValues.ConfigData[ceilString2];
        }
        if (ceilString3.Length > 0 && ceilString3.StartsWith("$"))
        {
            ceilString3 = AppValues.ConfigData[ceilString3];
        }
        if (ceilString4.Length > 0 && ceilString4.StartsWith("$"))
        {
            ceilString4 = AppValues.ConfigData[ceilString4];
        }
        if (ceilString5.Length > 0 && ceilString5.StartsWith("$"))
        {
            ceilString5 = AppValues.ConfigData[ceilString5];
        }




        // 进行检查
        // 存储检查出的非法值（key：数据索引， value：填写值）
        Dictionary<int, object> illegalValue = new Dictionary<int, object>();
        for (int i = 0; i < fieldInfo.Data.Count; ++i)
        {
            if (fieldInfo.Data[i] == null)
                continue;

            if (isNumberDataType == true)
            {
                double inputValue = Convert.ToDouble(fieldInfo.Data[i]);
                if (inputValue < floorValue || inputValue > ceilValue)
                    illegalValue.Add(i, fieldInfo.Data[i]);
            }
            else if (isStringDataType == true)
            {
                int lengh = System.Text.Encoding.Default.GetBytes(fieldInfo.Data[i].ToString().ToCharArray()).Length;
                if (lengh < floorValue || lengh > ceilValue)
                    illegalValue.Add(i, fieldInfo.Data[i]);
            }

            if (illegalValue.Count > 0)
            {
                StringBuilder illegalValueInfo = new StringBuilder();
                if (isNumberDataType == true || isStringDataType == true)
                {
                    foreach (var item in illegalValue)
                        illegalValueInfo.AppendFormat("第{0}行数据\"{1}\"不满足要求\n", item.Key + ExcelTableSetting.DataFieldDataStartRowIndex + 1, item.Value);
                }
                else if (fieldInfo.DataType == DataType.Date)
                {
                    foreach (var item in illegalValue)
                        illegalValueInfo.AppendFormat("第{0}行数据\"{1}\"不满足要求\n", item.Key + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ((DateTime)(item.Value)).ToString(DateTimeValue.APP_DEFAULT_DATE_FORMAT));
                }
                else if (fieldInfo.DataType == DataType.Time)
                {
                    foreach (var item in illegalValue)
                        illegalValueInfo.AppendFormat("第{0}行数据\"{1}\"不满足要求\n", item.Key + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ((DateTime)(item.Value)).ToString(DateTimeValue.APP_DEFAULT_TIME_FORMAT));
                }

                errorString = illegalValueInfo.ToString();
                return false;
            }
            else
            {
                errorString = null;
                return true;
            }
        }
        return true;
    }
}