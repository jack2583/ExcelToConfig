using System;

public partial class TableCheckHelper
{
    public static bool GetCheckType(string checkRuleStr, ref CheckType checkType, ref object[] Rulevalue,  out string errorString)
    {
        errorString = null;
        // 规则首位必须为方括号或者圆括号
        if (checkRuleStr.StartsWith("ref:"))
        {
            checkType = CheckType.CheckRef;
            string temp;
            string tableName;//所引用的表名
            string fieldIndexDefine;//所引的字段名 表名-字段名
            FieldInfo targetFieldInfo;
            temp = checkRuleStr.Substring(4).Trim();//去掉前面的fef:字符
            if (string.IsNullOrEmpty(temp))
            {
                errorString = string.Format("值引用检查规则声明错误，\"{0}\"的后面必须跟表格名-字段名\n", "ref:");
                return false;
            }
            else
            {
                // 解析参考表名、列名声明

                int hyphenIndex2 = temp.LastIndexOf('-');
                if (hyphenIndex2 == -1)
                {
                    tableName = temp;
                    fieldIndexDefine = null;
                }
                else
                {
                    tableName = temp.Substring(0, hyphenIndex2).Trim();
                    fieldIndexDefine = temp.Substring(hyphenIndex2 + 1, temp.Length - hyphenIndex2 - 1);
                }

                if (!AppValues.TableInfo.ContainsKey(tableName))
                {
                    errorString = string.Format("值引用检查规则声明错误，找不到名为{0}的表格\n", tableName);
                    Rulevalue = new object[] { tableName, null };
                    return true;
                }

                if (string.IsNullOrEmpty(fieldIndexDefine))
                {
                    //  targetFieldInfo = AppValues.TableInfo[tableName].GetKeyColumnFieldInfo();
                    errorString = string.Format("值引用检查规则声明错误，必须以 表名-字段名 的形式进行声明，而{0}的字段名为空\n", checkRuleStr);
                    return false;
                }
                else
                {
                    TableInfo targetTableInfo = AppValues.TableInfo[tableName];
                    targetFieldInfo = GetFieldByIndexDefineString(fieldIndexDefine, targetTableInfo, out errorString);
                    if (errorString != null)
                    {
                        errorString = string.Format("值引用检查规则声明错误，表格\"{0}\"中无法根据索引字符串\"{1}\"找到要参考的字段，错误信息为：{2}\n", tableName, fieldIndexDefine, errorString);
                        return false;
                    }
                }
            }
            Rulevalue = new object[] { tableName, fieldIndexDefine };
            return true;
        }
        // 规则末位必须为方括号或者圆括号
        else if (checkRuleStr.StartsWith("["))
        {
            checkType = CheckType.CheckRange;
            double floorValue = 0;
            double ceilValue = 0;
            if (!checkRuleStr.EndsWith("]"))
            {
                errorString = "值范围检查规则声明错误：必须用以[x,y]区间的形式进行声明\n";
                return false;
            }
            else
            {
                // 去掉首尾的括号
                string temp = checkRuleStr.Substring(1, checkRuleStr.Length - 2);
                // 通过英文逗号分隔上下限
                string[] floorAndCeilString = temp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (floorAndCeilString.Length != 2)
                {
                    errorString = "值范围检查定义错误：必须用一个英文逗号分隔值范围的上下限\n";
                    return false;
                }
                string floorString = floorAndCeilString[0].Trim();
                string ceilString = floorAndCeilString[1].Trim();


                if (double.TryParse(floorString, out floorValue) == false)
                {
                    errorString = string.Format("值范围检查定义错误：下限不是合法的数字，你输入的为{0}\n", floorString);
                    return false;
                }

                if (double.TryParse(ceilString, out ceilValue) == false)
                {
                    errorString = string.Format("值范围检查定义错误：上限不是合法的数字，你输入的为{0}\n", ceilString);
                    return false;
                }

                if(floorValue> ceilValue)
                {
                    errorString = string.Format("值范围检查定义错误：下限应该<=上限，你输入的下限为{0}，上限为{1}\n", floorValue, ceilValue);
                    return false;
                }
            }
            Rulevalue = new object[] { floorValue, ceilValue };
            return true;
        }
        else if(checkRuleStr.StartsWith("noCheck"))
        {
            checkType = CheckType.Invalid;
            return true;
        }
        else
        {
            checkType = CheckType.Invalid;
            errorString = string.Format("jsonref检查的子项声明定义错误，仅支持 ref:表名-字段名 或 [2,5] 的形式，而此处声明为{0}\n", checkRuleStr);
            return false;
        }
    }
}

/// <summary>
/// 本工具所有的检查类型的定义
/// </summary>
public enum CheckType
{
    /// <summary>
    /// 无效类型
    /// </summary>
    Invalid,

    CheckRef,
    CheckRange
}