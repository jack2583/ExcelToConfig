using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

public partial class TableCheckHelper
{
    /// <summary>
    /// 用于int、long、float或string型取值必须在另一字段（可能还是这张表格也可能跨表）中有对应值的检查
    /// jsonString:(2|ref:item-id|noCheck|[2,5])
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
        if (checkRule.CheckRuleString.StartsWith("jsonString:("))
            isIncludeFloor = true;
        else
        {
            errorString = "jsonref检查定义错误：必须用jsonString:(开头，以|分割\n";
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
        string temp = checkRule.CheckRuleString.Substring(12, checkRule.CheckRuleString.Length - 13);
        // 通过英文逗号分隔上下限
        string[] floorAndCeilString = temp.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        int floorandCeilStrinLength = floorAndCeilString.Length;
        if (floorandCeilStrinLength < 2)
        {
            errorString = "值范围检查定义错误：必须用一个|分隔,如：jsonref:(2|ref:item-id|noCheck|[2,5])\n";
            return false;
        }
        int floorString = int.Parse(floorAndCeilString[0].Trim());
        string ceilString = floorAndCeilString[1].Trim();
        string ceilString2 = "";
        string ceilString3 = "";
        string ceilString4 = "";
        string ceilString5 = "";

        if (floorandCeilStrinLength > 2)
            ceilString2 = floorAndCeilString[2].Trim();

        if (floorandCeilStrinLength > 3)
            ceilString3 = floorAndCeilString[3].Trim();

        if (floorandCeilStrinLength > 4)
            ceilString4 = floorAndCeilString[4].Trim();

        if (floorandCeilStrinLength > 5)
            ceilString5 = floorAndCeilString[5].Trim();

        //对定义的检查字符串进行解析
        if (ceilString.Length > 0 && ceilString.StartsWith("$"))
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

        
        CheckType checkTypeceilString=CheckType.Invalid;
        object[] RulevalueceilString=null;
        if(GetCheckType(ceilString,ref checkTypeceilString,ref RulevalueceilString,out errorString) ==false)
        {
            return false;
        }
        else
        {
            if ( RulevalueceilString[1] == null)
            {
                if (AppValues.IsRefCheckNotTable == false)
                {
                    AppLog.Log(string.Format("找不到名为{0}的表格，已忽略字段{1}({2})这个检查:{3}\n", RulevalueceilString[0], fieldInfo.FieldName, fieldInfo.Desc, checkRule.CheckRuleString),ConsoleColor.Yellow);
                    errorString = null;
                    return true;
                }
                else
                    return false;
            }
        }

        CheckType checkTypeceilString2 = CheckType.Invalid;
        object[] RulevalueceilString2 = null;
        if(ceilString2.Length > 0)
        {
            if (GetCheckType(ceilString2, ref checkTypeceilString2, ref RulevalueceilString2, out errorString) == false)
            {
                return false;
            }
        }

        CheckType checkTypeceilString3 = CheckType.Invalid;
        object[] RulevalueceilString3 = null;
        if (ceilString3.Length > 0)
        {
            if (GetCheckType(ceilString3, ref checkTypeceilString3, ref RulevalueceilString3, out errorString) == false)
            {
                return false;
            }
        }

        CheckType checkTypeceilString4 = CheckType.Invalid;
        object[] RulevalueceilString4 = null;
        if (ceilString4.Length > 0)
        {
            if (GetCheckType(ceilString4, ref checkTypeceilString4, ref RulevalueceilString4, out errorString) == false)
            {
                return false;
            }
        }

        CheckType checkTypeceilString5 = CheckType.Invalid;
        object[] RulevalueceilString5 = null;
        if (ceilString5.Length > 0)
        {
            if (GetCheckType(ceilString5, ref checkTypeceilString5, ref RulevalueceilString5, out errorString) == false)
            {
                return false;
            }
        }
        // 进行检查
        // 存储检查出的非法值（key：数据索引， value：填写值）
        Dictionary<int, object> illegalValue = new Dictionary<int, object>();
        StringBuilder stringBuilder = new StringBuilder();
        if (floorString == 1)
        {
            for (int i = 0; i < fieldInfo.Data.Count; ++i)
            {
                if (fieldInfo.Data[i] == null)
                    continue;
                string fieldInfoDataString = fieldInfo.JsonString[i];
                if (fieldInfoDataString == "[]")
                    continue;

                if (!fieldInfoDataString.StartsWith("["))
                    stringBuilder.AppendLine(string.Format("第{0}行首字符不合要求，应为：[[  而填入值为{1}", i, fieldInfoDataString.Substring(0, 2)));

                if (!fieldInfoDataString.EndsWith("]"))
                    stringBuilder.AppendLine(string.Format("第{0}行末尾字符不合要求，应为：]]  而填入值为{1}", i, fieldInfoDataString.Substring(fieldInfoDataString.Length - 2, 2)));

                JsonData jsonData = fieldInfo.Data[i] as JsonData;
                if (jsonData != null)
                {
                    for (int j = 0; j < jsonData.Count; ++j)
                    {
                        JsonData jsonData3 = jsonData[j] as JsonData;

                            if (jsonData3.IsInt == true || jsonData3.IsLong == true || jsonData3.IsDouble == true)
                            {
                            switch (checkTypeceilString)
                            {
                                case CheckType.CheckRef:
                                    {
                                        TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString[0].ToString()];
                                        FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString[1].ToString(), targetTableInfo, out errorString);
                                        object obj;

                                        if(targetFieldInfo.DataType==DataType.Int)
                                        {
                                            obj = int.Parse(jsonData3.ToString());
                                            if ((int)obj == 0)
                                                break;
                                        }
                                        else if (targetFieldInfo.DataType == DataType.Long)
                                        {
                                            obj = long.Parse(jsonData3.ToString());
                                            if ((long)obj == 0)
                                                break;
                                        }
                                        else if (targetFieldInfo.DataType == DataType.Float)
                                        {
                                            obj = double.Parse(jsonData3.ToString());
                                            if ((double)obj == 0)
                                                break;
                                        }
                                        else if (targetFieldInfo.DataType == DataType.String)
                                        {
                                            obj = jsonData3.ToString();
                                            if (obj == null)
                                                break;
                                        }
                                        else
                                        {
                                            obj = null;
                                            break;
                                        }


                                        if (!targetFieldInfo.Data.Contains(obj))
                                        {
                                            stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString[0].ToString(), RulevalueceilString[1].ToString()));
                                        }
                                        break;
                                    }
                                case CheckType.CheckRange:
                                    {
                                        if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString[1])
                                        {
                                            stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, (double)RulevalueceilString[0], (double)RulevalueceilString[0], (double)RulevalueceilString[1]));
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            /*
                            if (j == 0)
                                {
                                    switch (checkTypeceilString)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (jsonData3.IsInt == true)
                                                    obj = int.Parse(jsonData3.ToString());
                                                else if (jsonData3.IsLong == true)
                                                    obj = Int64.Parse(jsonData3.ToString());
                                                else
                                                    obj = double.Parse(jsonData3.ToString());

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1,  Double.Parse(jsonData3.ToJson()), RulevalueceilString[0].ToString(), RulevalueceilString[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, (double)RulevalueceilString[0], (double)RulevalueceilString[0], (double)RulevalueceilString[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 1)
                                {
                                    switch (checkTypeceilString2)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString2[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString2[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (jsonData3.IsInt == true)
                                                    obj = int.Parse(jsonData3.ToString());
                                                else if (jsonData3.IsLong == true)
                                                    obj = Int64.Parse(jsonData3.ToString());
                                                else
                                                    obj = double.Parse(jsonData3.ToString());

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString2[0].ToString(), RulevalueceilString2[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString2[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString2[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString2[0], (double)RulevalueceilString2[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 2)
                                {
                                    switch (checkTypeceilString3)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString3[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString3[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (jsonData3.IsInt == true)
                                                    obj = int.Parse(jsonData3.ToString());
                                                else if (jsonData3.IsLong == true)
                                                    obj = Int64.Parse(jsonData3.ToString());
                                                else
                                                    obj = double.Parse(jsonData3.ToString());

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString3[0].ToString(), RulevalueceilString3[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString3[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString3[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString3[0], (double)RulevalueceilString3[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 3)
                                {
                                    switch (checkTypeceilString4)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString4[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString4[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (jsonData3.IsInt == true)
                                                    obj = int.Parse(jsonData3.ToString());
                                                else if (jsonData3.IsLong == true)
                                                    obj = Int64.Parse(jsonData3.ToString());
                                                else
                                                    obj = double.Parse(jsonData3.ToString());

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合引用表字段[{3}-{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString4[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString4[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString4[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1,  Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString4[0], (double)RulevalueceilString4[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 4)
                                {
                                    switch (checkTypeceilString5)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString5[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString5[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (jsonData3.IsInt == true)
                                                    obj = int.Parse(jsonData3.ToString());
                                                else if (jsonData3.IsLong == true)
                                                    obj = Int64.Parse(jsonData3.ToString());
                                                else
                                                    obj = double.Parse(jsonData3.ToString());

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString5[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString5[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString5[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString5[0], (double)RulevalueceilString5[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                */
                        }
                         else if (jsonData.IsString == true)
                            {
                                if (j == 0)
                                {
                                    switch (checkTypeceilString)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString[0].ToString(), RulevalueceilString[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 1)
                                {
                                    switch (checkTypeceilString2)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString2[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString2[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString2[0].ToString(), RulevalueceilString2[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 2)
                                {
                                    switch (checkTypeceilString3)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString3[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString3[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString3[0].ToString(), RulevalueceilString3[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 3)
                                {
                                    switch (checkTypeceilString4)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString4[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString4[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString4[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (j == 4)
                                {
                                    switch (checkTypeceilString5)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString5[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString5[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}个值为：{2}不符合范围[{3},{4}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString5[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                            }


                        
                    }
                }
            }

        }
        if (floorString == 2)
        {
            for (int i = 0; i < fieldInfo.Data.Count; ++i)
            {
                if (fieldInfo.Data[i] == null)
                    continue;
                string fieldInfoDataString = fieldInfo.JsonString[i];
                if (fieldInfoDataString == "[]")
                    continue;

                if (!fieldInfoDataString.StartsWith("[["))
                    stringBuilder.AppendLine(string.Format("第{0}行首字符不合要求，应为：[[  而填入值为{1}", i, fieldInfoDataString.Substring(0, 2)));

                if (!fieldInfoDataString.EndsWith("]]"))
                    stringBuilder.AppendLine(string.Format("第{0}行末尾字符不合要求，应为：]]  而填入值为{1}", i, fieldInfoDataString.Substring(fieldInfoDataString.Length-2, 2)));

                JsonData jsonData = fieldInfo.Data[i] as JsonData;
                if (jsonData != null)
                {
                    for(int j=0;j<jsonData.Count;++j)
                    {
                        JsonData jsonData2 = jsonData[j] as JsonData;
                        for (int z=0;z< jsonData2.Count;++z)
                        {
                            JsonData jsonData3 = jsonData2[z] as JsonData;
                            if ( jsonData3.IsInt == true || jsonData3.IsLong == true || jsonData3.IsDouble == true)
                            {
                                if (z == 0)
                                {
                                    switch (checkTypeceilString)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (targetFieldInfo.DataType == DataType.Int)
                                                {
                                                    obj = int.Parse(jsonData3.ToString());
                                                    if ((int)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Long)
                                                {
                                                    obj = long.Parse(jsonData3.ToString());
                                                    if ((long)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Float)
                                                {
                                                    obj = double.Parse(jsonData3.ToString());
                                                    if ((double)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.String)
                                                {
                                                    obj = jsonData3.ToString();
                                                    if (obj == null)
                                                        break;
                                                }
                                                else
                                                {
                                                    obj = null;
                                                    break;
                                                }

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j+1, z+1, Double.Parse(jsonData3.ToJson()), RulevalueceilString[0].ToString(), RulevalueceilString[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if(Double.Parse(jsonData3.ToJson())<(double)RulevalueceilString[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合范围[{4},{5}]要求", i+ ExcelTableSetting.DataFieldDataStartRowIndex + 1, j+1,z+1, (double)RulevalueceilString[0], (double)RulevalueceilString[0], (double)RulevalueceilString[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 1)
                                {
                                    switch (checkTypeceilString2)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString2[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString2[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (targetFieldInfo.DataType == DataType.Int)
                                                {
                                                    obj = int.Parse(jsonData3.ToString());
                                                    if ((int)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Long)
                                                {
                                                    obj = long.Parse(jsonData3.ToString());
                                                    if ((long)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Float)
                                                {
                                                    obj = double.Parse(jsonData3.ToString());
                                                    if ((double)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.String)
                                                {
                                                    obj = jsonData3.ToString();
                                                    if (obj == null)
                                                        break;
                                                }
                                                else
                                                {
                                                    obj = null;
                                                    break;
                                                }

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString2[0].ToString(), RulevalueceilString2[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString2[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString2[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合范围[{4},{5}]要求", i+ ExcelTableSetting.DataFieldDataStartRowIndex + 1, j+1, z+1, Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString2[0], (double)RulevalueceilString2[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 2)
                                {
                                    switch (checkTypeceilString3)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString3[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString3[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (targetFieldInfo.DataType == DataType.Int)
                                                {
                                                    obj = int.Parse(jsonData3.ToString());
                                                    if ((int)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Long)
                                                {
                                                    obj = long.Parse(jsonData3.ToString());
                                                    if ((long)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Float)
                                                {
                                                    obj = double.Parse(jsonData3.ToString());
                                                    if ((double)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.String)
                                                {
                                                    obj = jsonData3.ToString();
                                                    if (obj == null)
                                                        break;
                                                }
                                                else
                                                {
                                                    obj = null;
                                                    break;
                                                }

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString3[0].ToString(), RulevalueceilString3[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString3[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString3[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合范围[{4},{5}]要求", i+ ExcelTableSetting.DataFieldDataStartRowIndex + 1, j+1, z+1, Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString3[0], (double)RulevalueceilString3[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 3)
                                {
                                    switch (checkTypeceilString4)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString4[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString4[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (targetFieldInfo.DataType == DataType.Int)
                                                {
                                                    obj = int.Parse(jsonData3.ToString());
                                                    if ((int)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Long)
                                                {
                                                    obj = long.Parse(jsonData3.ToString());
                                                    if ((long)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Float)
                                                {
                                                    obj = double.Parse(jsonData3.ToString());
                                                    if ((double)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.String)
                                                {
                                                    obj = jsonData3.ToString();
                                                    if (obj == null)
                                                        break;
                                                }
                                                else
                                                {
                                                    obj = null;
                                                    break;
                                                }

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString4[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString4[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString4[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合范围[{4},{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j+1, z+1, Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString4[0], (double)RulevalueceilString4[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 4)
                                {
                                    switch (checkTypeceilString5)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString5[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString5[1].ToString(), targetTableInfo, out errorString);
                                                object obj;
                                                if (targetFieldInfo.DataType == DataType.Int)
                                                {
                                                    obj = int.Parse(jsonData3.ToString());
                                                    if ((int)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Long)
                                                {
                                                    obj = long.Parse(jsonData3.ToString());
                                                    if ((long)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.Float)
                                                {
                                                    obj = double.Parse(jsonData3.ToString());
                                                    if ((double)obj == 0)
                                                        break;
                                                }
                                                else if (targetFieldInfo.DataType == DataType.String)
                                                {
                                                    obj = jsonData3.ToString();
                                                    if (obj == null)
                                                        break;
                                                }
                                                else
                                                {
                                                    obj = null;
                                                    break;
                                                }

                                                if (!targetFieldInfo.Data.Contains(obj))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString5[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        case CheckType.CheckRange:
                                            {
                                                if (Double.Parse(jsonData3.ToJson()) < (double)RulevalueceilString5[0] || Double.Parse(jsonData3.ToJson()) > (double)RulevalueceilString5[1])
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合范围[{4},{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j+1, z+1, Double.Parse(jsonData3.ToJson()), (double)RulevalueceilString5[0], (double)RulevalueceilString5[1]));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                            }
                            else if(jsonData.IsString == true)
                            {
                                if (z == 0)
                                {
                                    switch (checkTypeceilString)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString[0].ToString(), RulevalueceilString[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 1)
                                {
                                    switch (checkTypeceilString2)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString2[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString2[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex+1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString2[0].ToString(), RulevalueceilString2[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 2)
                                {
                                    switch (checkTypeceilString3)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString3[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString3[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString3[0].ToString(), RulevalueceilString3[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 3)
                                {
                                    switch (checkTypeceilString4)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString4[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString4[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString4[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else if (z == 4)
                                {
                                    switch (checkTypeceilString5)
                                    {
                                        case CheckType.CheckRef:
                                            {
                                                TableInfo targetTableInfo = AppValues.TableInfo[RulevalueceilString5[0].ToString()];
                                                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(RulevalueceilString5[1].ToString(), targetTableInfo, out errorString);

                                                if (!targetFieldInfo.Data.Contains(jsonData3.ToString()))
                                                {
                                                    stringBuilder.AppendLine(string.Format("第{0}行第{1}组第{2}个值为：{3}不符合引用表字段[{4}-{5}]要求", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, j + 1, z + 1, Double.Parse(jsonData3.ToJson()), RulevalueceilString5[0].ToString(), RulevalueceilString5[1].ToString()));
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                            }


                        }
                    }
                }
            }

        }



        if (!(stringBuilder==null))
        {
           if(stringBuilder.ToString().Length>0)
            {
                errorString = stringBuilder.ToString();
                return false;
            }
           else
            {
                errorString = null;
                return true;
            }
            
        }
        else
        {
            errorString = null;
            return true;
        }
    }
}