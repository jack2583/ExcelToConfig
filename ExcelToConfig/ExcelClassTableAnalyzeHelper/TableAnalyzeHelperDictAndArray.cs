using System;
using System.Collections.Generic;
using System.Data;

public partial class TableAnalyzeHelper
{
    private static bool _AnalyzeArrayType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        // dict或array集合类型中，如果定义列中的值填-1代表这行数据的该字段不生效，Data中用true和false代表该集合字段的某行数据是否生效
        fieldInfo.Data = _GetValidInfoForSetData(dt, columnIndex, fieldInfo.TableName);
        // 为了在多重嵌套的集合结构中快速判断出是不是在任意上层集合中已经被标为无效，这里只要在上层标为无效，在其所有下层都会进行进行标记，从而在判断数据是否有效时只要判断向上一层是否标记为无效即可
        if (parentField != null)
        {
            int dataCount = Math.Min(fieldInfo.Data.Count, parentField.Data.Count);
            for (int i = 0; i < dataCount; ++i)
            {
                if ((bool)parentField.Data[i] == false)
                    fieldInfo.Data[i] = false;
            }
        }
        // 解析array声明的子元素的数据类型和个数
        string childDataTypeString;
        DataType childDataType;
        int childCount;
        _GetArrayChildDefine(fieldInfo.DataTypeString, out childDataTypeString, out childDataType, out childCount, out errorString);
        if (errorString != null)
        {
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }
        // 存储子类型的类型以及类型定义字符串
        fieldInfo.ArrayChildDataTypeString = childDataTypeString;
        fieldInfo.ArrayChildDataType = childDataType;
        // 解析之后的几列作为array的下属元素
        fieldInfo.ChildField = new List<FieldInfo>();
        fieldInfo.ChildFieldString = new List<string>();
        nextFieldColumnIndex = columnIndex + 1;
        int tempCount = childCount;
        int seq = 1;
        while (tempCount > 0)
        {
            int nextColumnIndex = nextFieldColumnIndex;
            FieldInfo childFieldInfo = _AnalyzeOneField(dt, tableInfo, nextColumnIndex, fieldInfo, out nextFieldColumnIndex, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("array类型数据下属元素（列号：{0}）的解析存在错误\n{1}", ExcelMethods.GetExcelColumnName(nextColumnIndex + 1), errorString);
                return false;
            }
            else
            {
                // 忽略无效列
                if (childFieldInfo != null)
                {
                    // 将array下属子元素的fieldName改为顺序编号
                    childFieldInfo.FieldName = string.Format("[{0}]", seq);
                    ++seq;

                    fieldInfo.ChildField.Add(childFieldInfo);
                    fieldInfo.ChildFieldString.Add(childFieldInfo.FieldName);
                    --tempCount;
                }
            }
        }

        // 如果array的子元素为array或dict类型，当前面的子元素用-1标识为无效后，后面的数据也必须声明为无效的，比如用array[dict[3]:5]表示一场推图战斗胜利最多可获得的5种奖励物，如果某一行对应的关卡至多只有3种奖励物，则必须填在前3个子元素列中，后面2个声明为无效
        if (fieldInfo.ArrayChildDataType == DataType.Array || fieldInfo.ArrayChildDataType == DataType.Dict)
        {
            for (int i = 0; i < fieldInfo.Data.Count; ++i)
            {
                // 如果本行数据中array下属的集合型子元素已经读取到-1标识的无效数据，记录其是第几个子元素
                int invalidDataIndex = 0;
                for (int j = 0; j < fieldInfo.ChildField.Count; ++j)
                {
                    if ((bool)fieldInfo.ChildField[j].Data[i] == true)
                    {
                        if (invalidDataIndex != 0)
                        {
                            errorString = string.Format("array的子元素为array或dict类型时，当前面的子元素用-1标识为无效后，后面的数据也必须声明为无效的。而第{0}行第{1}个子元素声明为无效，而后面第{2}个子元素却有效\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, invalidDataIndex, j + 1);
                            return false;
                        }
                    }
                    else
                        invalidDataIndex = j + 1;
                }
            }
        }

        errorString = null;
        return true;
    }

    /// <summary>
    /// 解析形如array[type:n]（type为类型，n为array中元素个数）的array类型的声明字符串，获得子元素的类型以及子元素的个数
    /// </summary>
    private static bool _GetArrayChildDefine(string defineString, out string childDataTypeString, out DataType childDataType, out int childCount, out string errorString)
    {
        childDataTypeString = null;
        childDataType = DataType.Invalid;
        childCount = 0;
        errorString = null;

        int leftBracketIndex = defineString.IndexOf('[');
        int rightBracketIndex = defineString.LastIndexOf(']');
        if (leftBracketIndex != -1 && rightBracketIndex != -1 && leftBracketIndex < rightBracketIndex)
        {
            // 去掉首尾包裹的array[]
            string typeAndCountString = defineString.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1).Trim();
            childDataTypeString = typeAndCountString;
            // 通过冒号分离类型和个数（注意不能用Split分隔，可能出现array[array[int:2]:3]这种嵌套结构，必须去找最后一个冒号的位置）
            int lastColonIndex = typeAndCountString.LastIndexOf(':');
            if (lastColonIndex == -1)
            {
                errorString = string.Format("array类型数据声明不合法，请采用array[type:n]的形式，冒号分隔类型与个数的定义，你填写的类型为{0}", defineString);
                return false;
            }
            else
            {
                string typeString = typeAndCountString.Substring(0, lastColonIndex).Trim();
                string countString = typeAndCountString.Substring(lastColonIndex + 1, typeAndCountString.Length - lastColonIndex - 1).Trim();

                // 判断类型是否合法
                DataType inputChildDataType = _AnalyzeDataType(typeString);
                if (inputChildDataType == DataType.Invalid)
                {
                    errorString = string.Format("array类型数据声明不合法，子类型错误，你填写的类型为{0}", defineString);
                    return false;
                }
                childDataType = inputChildDataType;
                // 判断个数是否合法
                int count;
                if (!int.TryParse(countString, out count))
                {
                    errorString = string.Format("array类型数据声明不合法，声明的下属元素个数不是合法的数字，你填写的个数为{0}", countString);
                    return false;
                }
                if (count < 1)
                {
                    errorString = string.Format("array类型数据声明不合法，声明的下属元素个数不能低于1个，你填写的个数为{0}", countString);
                    return false;
                }
                childCount = count;
                return true;
            }
        }
        else
        {
            errorString = string.Format("array类型数据声明不合法，请采用array[type:n]的形式，其中type表示类型，n表示array中元素个数，你填写的类型为{0}", defineString);
            return false;
        }
    }

    private static bool _AnalyzeDictType(FieldInfo fieldInfo, TableInfo tableInfo, DataTable dt, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        // dict或array集合类型中，如果定义列中的值填-1代表这行数据的该字段不生效，Data中用true和false代表该集合字段的某行数据是否生效
        fieldInfo.Data = _GetValidInfoForSetData(dt, columnIndex, fieldInfo.TableName);
        // 为了在多重嵌套的集合结构中快速判断出是不是在任意上层集合中已经被标为无效，这里只要在上层标为无效，在其所有下层都会进行进行标记，从而在判断数据是否有效时只要判断向上一层是否标记为无效即可
        if (parentField != null)
        {
            int dataCount = Math.Min(fieldInfo.Data.Count, parentField.Data.Count);
            for (int i = 0; i < dataCount; ++i)
            {
                if ((bool)parentField.Data[i] == false)
                    fieldInfo.Data[i] = false;
            }
        }
        // 记录dict中子元素的字段名，dict中不允许同名字段
        List<string> inputFieldNames = new List<string>();
        // 解析dict声明的子元素个数
        int childCount;
        _GetDictChildCount(fieldInfo.DataTypeString, out childCount, out errorString);
        if (errorString != null)
        {
            nextFieldColumnIndex = columnIndex + 1;
            return false;
        }
        // 解析之后的几列作为array的下属元素
        fieldInfo.ChildField = new List<FieldInfo>();
        fieldInfo.ChildFieldString = new List<string>();
        nextFieldColumnIndex = columnIndex + 1;
        int tempCount = childCount;
        while (tempCount > 0)
        {
            int nextColumnIndex = nextFieldColumnIndex;
            FieldInfo childFieldInfo = _AnalyzeOneField(dt, tableInfo, nextColumnIndex, fieldInfo, out nextFieldColumnIndex, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("dict类型数据下属元素（列号：{0}）的解析存在错误\n{1}", ExcelMethods.GetExcelColumnName(nextColumnIndex + 1), errorString);
                return false;
            }
            else
            {
                // 忽略无效列
                if (childFieldInfo != null)
                {
                    // 检查dict子元素中是否已经存在同名的字段
                    if (inputFieldNames.Contains(childFieldInfo.FieldName))
                    {
                        errorString = string.Format("dict类型的子元素中不允许含有同名字段（{0}）\n", childFieldInfo.FieldName);
                        return false;
                    }
                    else
                        inputFieldNames.Add(childFieldInfo.FieldName);

                    fieldInfo.ChildField.Add(childFieldInfo);
                    fieldInfo.ChildFieldString.Add(childFieldInfo.FieldName);
                    --tempCount;
                }
            }
        }

        errorString = null;
        return true;
    }

    /// <summary>
    /// 解析形如dict[n]（n为子元素个数）的dict类型的声明字符串，获得子元素的个数
    /// </summary>
    private static bool _GetDictChildCount(string defineString, out int childCount, out string errorString)
    {
        childCount = 0;
        errorString = null;

        int leftBracketIndex = defineString.IndexOf('[');
        int rightBracketIndex = defineString.LastIndexOf(']');
        if (leftBracketIndex != -1 && rightBracketIndex != -1 && leftBracketIndex < rightBracketIndex)
        {
            // 取出括号中的数字，判断是否合法
            string countString = defineString.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
            int count;
            if (!int.TryParse(countString, out count))
            {
                errorString = string.Format("dict类型数据声明不合法，声明的下属元素个数不是合法的数字，你填写的个数为{0}", countString);
                return false;
            }
            if (count < 1)
            {
                errorString = string.Format("dict类型数据声明不合法，声明的下属元素个数不能低于1个，你填写的个数为{0}", countString);
                return false;
            }
            childCount = count;
            return true;
        }
        else
        {
            errorString = string.Format("dict类型数据声明不合法，请采用dict[n]的形式，其中n表示下属元素个数，你填写的类型为{0}", defineString);
            return false;
        }
    }

    /// <summary>
    /// 获取dict或array数据类型中，每一行数据的有效性（填-1表示本行的这个数据无效）
    /// </summary>
    private static List<object> _GetValidInfoForSetData(DataTable dt, int columnIndex, string tableName)
    {
        List<object> validInfo = new List<object>();

        for (int row = ExcelTableSetting.DataFieldDataStartRowIndex; row < dt.Rows.Count; ++row)
        {
            string inputData = dt.Rows[row][columnIndex].ToString().Trim();
            if ("-1".Equals(inputData))
                validInfo.Add(false);
            else if (string.IsNullOrEmpty(inputData))
                validInfo.Add(true);
            else
            {
                AppLog.LogWarning(string.Format("警告：表格{0}的第{1}列的第{2}行数据有误，array或dict类型中若某行不需要此数据请填-1，否则留空，你填写的为\"{3}\"，本工具按有效值进行处理，但请按规范更正", tableName, ExcelMethods.GetExcelColumnName(columnIndex + 1), row, inputData));
                validInfo.Add(true);
            }
        }

        return validInfo;
    }

    /// <summary>
    /// 当表格存在错误无法继续时，输出内容前统一加上表格名和列名
    /// </summary>
    private static string _GetTableAnalyzeErrorString(string tableName, string sheetName, int columnIndex)
    {
        return string.Format("表格{0}-{1}中列号为{2}的字段存在以下严重错误，导致无法继续，请修正错误后重试\n", tableName, sheetName, ExcelMethods.GetExcelColumnName(columnIndex + 1));
    }
}