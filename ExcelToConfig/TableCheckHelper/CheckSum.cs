using System;
using System.Collections.Generic;
using System.Text;

public partial class TableCheckHelper
{
    /// <summary>
    /// 用于int、long型字段组合成ID，sum:main_type*1000000+sub_type*10000+color*1000+lvl
    /// main_type*1000000,sub_type*10000,color*1000,lvl。等价于该字段值必须是同一行的这些值之和：main_type*1000000+sub_type*10000+color*1000+lvl
    /// </summary>
    public static bool CheckSum(FieldInfo fieldInfo, FieldCheckRule checkRule, out string errorString)
    {
        string temp = null;

        // 首先要求字段类型只能为int、long型
        if (!(fieldInfo.DataType == DataType.Int || fieldInfo.DataType == DataType.Long))
        {
            errorString = string.Format("sum检查规则只适用于int和long类型的字段，要检查的这列类型为{0}\n", fieldInfo.DataType.ToString());
            return false;
        }

        // 解析sum规则中目标列所在表格以及字段名
        const string START_STRING = "sum:";
        if (!checkRule.CheckRuleString.StartsWith(START_STRING, StringComparison.CurrentCultureIgnoreCase))
        {
            errorString = string.Format("sum检查规则声明错误，必须以\"{0}\"开头，后面跟字段名*倍数\n", START_STRING);
            return false;
        }

        temp = checkRule.CheckRuleString.Substring(START_STRING.Length).Trim();//去掉前面的sum:字符
        if (string.IsNullOrEmpty(temp))
        {
            errorString = string.Format("sum检查规则声明错误，\"{0}\"的后面必须跟字段名*倍数\n", START_STRING);
            return false;
        }

        string[] values = temp.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
        // 存储引用字段的信息（字段名，倍数）
        Dictionary<string, long> RuleValues = new Dictionary<string, long>();
        TableInfo thisTableInfo = AppValues.TableInfo[fieldInfo.TableName];
        for (int i = 0; i < values.Length; ++i)
        {
            string oneValueString = values[i].Trim();
            string[] oneValue = oneValueString.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            if(oneValue.Length==0)
            {
                errorString = "sum检查定义中出现了错误，，因为定义为空值";
                return false;
            }
            else
            {
                FieldInfo targetFieldInfo = GetFieldByIndexDefineString(oneValue[0].Trim(), thisTableInfo, out errorString);
                if (errorString != null)
                {
                    errorString = string.Format("sum检查规则声明错误，表格\"{0}\"中无法根据索引字符串\"{1}\"找到要参考的字段，错误信息为：{2}\n", thisTableInfo, oneValue[0].Trim(), errorString);
                    return false;
                }

                DataType dt= thisTableInfo.GetFieldInfoByFieldName(oneValue[0].Trim()).DataType;
                if (!(dt == DataType.Int || dt == DataType.Long))
                {
                    errorString = string.Format("sum检查规则只适用于int和long类型的字段，而字段{0}的类型为{1}\n", oneValue[0].Trim(), dt.ToString());
                    return false;
                }
            }

            if(oneValue.Length==1)
            {
                RuleValues.Add(oneValue[0].Trim(), 1);
            }
            else if (oneValue.Length == 2)
            {
                RuleValues.Add(oneValue[0].Trim(), Convert.ToInt64(oneValue[1].Trim()));
            }
            else
            {
                errorString = string.Format("sum检查定义中出现了错误，请检查，其定义为\"{0}\"\n", checkRule);
                return false;
            }
        }


        long temNum;
        // 存储不符合sum规则的数据信息（key：行号， value：填写的数据）
        Dictionary<int, string> unreferencedInfo = new Dictionary<int, string>();
        for (int i = 0; i < fieldInfo.Data.Count; ++i)
        {
            // 忽略无效集合元素下属子类型的空值或本身为空值
            if (fieldInfo.Data[i] == null)
                continue;

            temNum = 0;
            foreach (KeyValuePair<string,long> kvp in RuleValues)
            {
                temNum = temNum+ Convert.ToInt64(thisTableInfo.GetFieldInfoByFieldName(kvp.Key).Data[i])* kvp.Value;
            }
            
            if(Convert.ToInt64(fieldInfo.Data[i])== temNum)
            {
            }
            else
            {
                unreferencedInfo.Add(i,string.Format("{0}不符合sum组合规则,正确应为：{1}", fieldInfo.Data[i].ToString(),temNum));
            }

        }

        if (unreferencedInfo.Count > 0)
        {
            StringBuilder errorStringBuild = new StringBuilder();
            errorStringBuild.AppendLine("存在以下不符合sum关系的数据行：");
            foreach (var item in unreferencedInfo)
                errorStringBuild.AppendFormat("第{0}行数据\"{1}\"\n", item.Key + ExcelTableSetting.DataFieldDataStartRowIndex + 1, item.Value);

            errorString = errorStringBuild.ToString();
            return false;
        }
        else
        {
            errorString = null;
            return true;
        }

    }
}
