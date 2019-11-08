using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using LitJson;
using System.Diagnostics;

public partial class TableAnalyzeHelper
{
    public static TableInfo AnalyzeTable(DataTable dt, string filePath, out string errorString)
    {
        if (dt.Rows.Count < ExcelTableSetting.DataFieldDataStartRowIndex)
        {
            errorString = "表格格式不符合要求，必须在表格前五行中依次声明字段描述、字段名、数据类型、检查规则、导出到MySQL数据库中的配置";
            return null;
        }
        if (dt.Columns.Count < 2)
        {
            errorString = "表格中至少需要配置2个字段";
            return null;
        }

        TableInfo tableInfo = new TableInfo();
        tableInfo.ExcelFilePath = filePath;
        tableInfo.ExcelName = Path.GetFileNameWithoutExtension(filePath);
        tableInfo.TableName = ExcelMethods.GetTableName(tableInfo.ExcelName);

        string tableName = tableInfo.TableName;

        // 当前解析到的列号
        int curColumnIndex = 0;

        // 解析第一列（主键列，要求数据类型为int、long或string且数据非空、唯一）
        DataType primaryKeyColumnType = _AnalyzeDataType(dt.Rows[ExcelTableSetting.DataFieldDataTypeRowIndex][0].ToString().Trim());
        if (!(primaryKeyColumnType == DataType.Int || primaryKeyColumnType == DataType.Long || primaryKeyColumnType == DataType.String))
        {
            errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, 0) + "主键列的类型只能为int、long或string";
            return null;
        }
        else
        {
            // 解析出主键列，然后检查是否非空、唯一，如果是string型主键还要检查是否符合变量名的规范（只能由英文字母、数字、下划线组成）
            FieldInfo primaryKeyField = _AnalyzeOneField(dt, tableInfo, 0, null, out curColumnIndex, out errorString);
            if (errorString != null)
            {
                errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, 0) + "主键列解析错误\n" + errorString;
                return null;
            }
            // 主键列字段名不允许为空
            else if (primaryKeyField.IsIgnoreClientExport == true)
            {
                errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, 0) + "主键列必须指定字段名\n" + errorString;
                return null;
            }
            else
            {
                // 唯一性检查
                FieldCheckRule uniqueCheckRule = new FieldCheckRule();
                uniqueCheckRule.CheckType = TableCheckType.Unique;
                uniqueCheckRule.CheckRuleString = "unique";
                TableCheckHelper.CheckUnique(primaryKeyField, uniqueCheckRule, out errorString);
                if (errorString != null)
                {
                    errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, 0) + "主键列存在重复错误\n" + errorString;
                    return null;
                }

                // string型主键检查是否符合变量名的规范
                if (primaryKeyColumnType == DataType.String)
                {
                    StringBuilder errorStringBuilder = new StringBuilder();
                    for (int row = 0; row < primaryKeyField.Data.Count; ++row)
                    {
                        string tempError = null;
                        TableCheckHelper.CheckFieldName(primaryKeyField.Data[row].ToString(), out tempError);
                        if (tempError != null)
                            errorStringBuilder.AppendFormat("第{0}行所填主键{1}\n", row + ExcelTableSetting.DataFieldDataStartRowIndex + 1, tempError);
                    }
                    if (!string.IsNullOrEmpty(errorStringBuilder.ToString()))
                    {
                        errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, 0) + "string型主键列存在非法数据\n" + errorStringBuilder.ToString();
                        return null;
                    }
                }

                // 非空检查（因为string型检查是否符合变量名规范时以及未声明数值型字段中允许空时，均已对主键列进行过非空检查，这里只需对数据值字段且声明数值型字段中允许空的情况下进行非空检查）
                if (CheckStruct.IsAllowedNullNumber == true && (primaryKeyColumnType == DataType.Int || primaryKeyColumnType == DataType.Long))
                {
                    FieldCheckRule notEmptyCheckRule = new FieldCheckRule();
                    uniqueCheckRule.CheckType = TableCheckType.NotEmpty;
                    uniqueCheckRule.CheckRuleString = "notEmpty";
                    TableCheckHelper.CheckNotEmpty(primaryKeyField, notEmptyCheckRule, out errorString);
                    if (errorString != null)
                    {
                        errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, 0) + "主键列存在非空错误\n" + errorString;
                        return null;
                    }
                }

                tableInfo.AddField(primaryKeyField);
            }
        }

        // 存储定义过的字段名，不允许有同名字段（key：字段名， value：列号）
        Dictionary<string, int> fieldNames = new Dictionary<string, int>();
        // 先加入主键列
        fieldNames.Add(tableInfo.GetKeyColumnFieldInfo().FieldName, 0);
        // 解析剩余的列
        while (curColumnIndex < dt.Columns.Count)
        {
            int nextColumnIndex = curColumnIndex;
            FieldInfo oneField = _AnalyzeOneField(dt, tableInfo, nextColumnIndex, null, out curColumnIndex, out errorString);
            if (errorString != null)
            {
                errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, nextColumnIndex) + errorString;
                return null;
            }
            else
            {
                // 跳过未声明变量名以及数据库导出信息的无效列
                if (oneField != null)
                {
                    // 检查字段名是否重复
                    if (fieldNames.ContainsKey(oneField.FieldName))
                    {
                        errorString = _GetTableAnalyzeErrorString(tableName, dt.TableName, nextColumnIndex) + string.Format("表格中存在字段名同为{0}的字段，分别位于第{1}列和第{2}列", oneField.FieldName, ExcelMethods.GetExcelColumnName(fieldNames[oneField.FieldName] + 1), ExcelMethods.GetExcelColumnName(oneField.ColumnSeq + 1));
                        return null;
                    }
                    else
                    {
                        tableInfo.AddField(oneField);
                        fieldNames.Add(oneField.FieldName, oneField.ColumnSeq);
                    }
                }
            }
        }

        errorString = null;
        return tableInfo;
    }
    /// <summary>
    /// 解析所有配置表
    /// </summary>
    /// <param name="ExportTables"></param>
    public static void AnalyzeAllTable(Dictionary<string, string> ExportTables)
    {
        AppLog.Log("开始解析Excel文件：");
        Stopwatch stopwatch = new Stopwatch();//计算运行时间
        foreach (KeyValuePair <string, string> kvp in ExportTables )
        {
            AppLog.Log(string.Format("解析表格\"{0}\"：", kvp.Key), ConsoleColor.Green);
            string excelName = Path.GetFileNameWithoutExtension(kvp.Key);
            /*
            if (AppValues.IsMoreLanguage==true)//指定是多语言情况下才处理
            {
                if (AppValues.NeedLanguage == null)
                {
                    bool Current_Language = true;
                    if(AppValues.OtherLanguage!=null)
                    {
                        foreach (string str in AppValues.OtherLanguage)
                        {
                            if (excelName.EndsWith(str))
                            {
                                Current_Language = false;
                                break;
                            }
                        }
                        if (Current_Language == false)
                        {
                            continue;
                        }
                    }
                }
                else// 
                {
                    bool Current_Language = true;
                    if (AppValues.OtherLanguage != null)
                    {
                        foreach (string str in AppValues.OtherLanguage)
                        {
                            if (excelName.EndsWith(str))
                            {
                                Current_Language = false;
                                break;
                            }
                        }
                    }

                    if (Current_Language == false)//如果是属于多语言
                    {
                        if (excelName.EndsWith(AppValues.NeedLanguage))//是指定的多语言
                        {
                            excelName.Substring(0, excelName.Length - AppValues.NeedLanguage.Length);
                            Current_Language = true;
                            break;
                        }
                    }

                    if (Current_Language == false)//如果不是默认语言或指定的多语言则忽略，不解析
                    {
                        continue;
                    }
                }
            }
           */
            stopwatch.Reset();//时间重置
            stopwatch.Start();

            string errorString = null;
          
            string tableName = null;
            DataSet ds = ReadExcelHelper.ReadXlsxFileForOleDb(kvp.Value, excelName,ref tableName, out errorString);

            if (string.IsNullOrEmpty(errorString))
            {
                int tbNum = 0;
                foreach(DataTable tb in ds.Tables)
                {
                    if(tb.TableName!=ExcelTableSetting.ExcelConfigSheetName)
                    {
                        TableInfo tableInfo = AnalyzeTable(tb, kvp.Value, out errorString);
                        if (errorString != null)
                            AppLog.LogErrorAndExit(string.Format("错误：解析{0}失败\n{1}", kvp.Value, errorString));
                        else
                        {
                            tbNum++;
                            //tableInfo.ExcelFilePath = kvp.Key;
                            //tableInfo.ExcelName = excelName;
                            //tableInfo.TableName = tableName;
                            // 如果有表格配置进行解析
                            if (ds.Tables[ExcelTableSetting.ExcelConfigSheetName] != null)
                            {
                                tableInfo.TableConfig = GetTableConfigOfFirstColumn(ds.Tables[ExcelTableSetting.ExcelConfigSheetName], out errorString);
                                if (!string.IsNullOrEmpty(errorString))
                                {
                                    AppLog.LogErrorAndExit(string.Format("错误：解析表格{0}的配置失败\n{1}", kvp.Key, errorString));
                                }
                                else
                                {
                                    tableInfo.TableConfigData = ds.Tables[ExcelTableSetting.ExcelConfigSheetName];
                                }
                            }
                            if (!AppValues.TableInfoList.ContainsKey(tableName))
                                AppValues.TableInfoList.Add(tableName, new List<TableInfo> { tableInfo });
                            else
                                AppValues.TableInfoList[tableName].Add(tableInfo);
                        }
                    }
                }
                TableInfo tableInfo2 ;

                if (tbNum > 1)
                {
                    tableInfo2 = TableInfo.Merge(AppValues.TableInfoList[tableName]);
                    // 唯一性检查
                    FieldCheckRule uniqueCheckRule = new FieldCheckRule();
                    uniqueCheckRule.CheckType = TableCheckType.Unique;
                    uniqueCheckRule.CheckRuleString = "unique";
                    TableCheckHelper.CheckUnique(tableInfo2.GetKeyColumnFieldInfo(), uniqueCheckRule, out errorString);
                    if (errorString != null)
                    {
                        errorString = _GetTableAnalyzeErrorString(tableName, "", 0) + "主键列存在重复错误\n" + errorString;
                        AppLog.LogErrorAndExit(string.Format("错误：合并{0}失败\n{1}", tableName, errorString));
                    }

                    AppValues.TableInfo.Add(tableName, tableInfo2);
                }
                else
                {
                    if(!AppValues.TableInfo.ContainsKey(tableName))
                        AppValues.TableInfo.Add(tableName,AppValues.TableInfoList[tableName][0]);
                    else
                    {
                        AppLog.LogErrorAndExit(string.Format("错误：存在多个以{0}为名的表格，请检查配置\n", tableName));
                    }
                }
               
                stopwatch.Stop();
                AppLog.Log(string.Format("成功，耗时：{0}毫秒", stopwatch.ElapsedMilliseconds));
            }
            else
            {
                AppLog.LogErrorAndExit(string.Format("错误：读取{0}失败\n{1}", kvp.Value, errorString));
            }

            //导出txt用
            AppValues.ExcelDataSet.Add(kvp.Value, ds);
        }
        AppLog.Log("所有配置表解析完毕！！！");
    }
    /// <summary>
    /// 解析一列的数据结构及数据，返回FieldInfo
    /// </summary>
    private static FieldInfo _AnalyzeOneField(DataTable dt, TableInfo tableInfo, int columnIndex, FieldInfo parentField, out int nextFieldColumnIndex, out string errorString)
    {
        // 判断列号是否越界
        if (columnIndex >= dt.Columns.Count)
        {
            errorString = "需要解析的列号越界，可能因为dict或array中实际的子元素个数低于声明的个数";
            nextFieldColumnIndex = columnIndex + 1;
            return null;
        }

        FieldInfo fieldInfo = new FieldInfo();
        // 所在表格
        fieldInfo.TableName = tableInfo.TableName;
        // 字段描述中的换行全替换为空格
        fieldInfo.Desc = dt.Rows[ExcelTableSetting.DataFieldDescRowIndex][columnIndex].ToString().Trim().Replace(System.Environment.NewLine, " ").Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');
        // 字段所在列号
        fieldInfo.ColumnSeq = columnIndex;
        // 检查规则字符串
        string checkRuleString = null;
        if(ExcelTableSetting.DataFieldCheckRuleRowIndex>=0)
            checkRuleString = dt.Rows[ExcelTableSetting.DataFieldCheckRuleRowIndex][columnIndex].ToString().Trim().Replace(System.Environment.NewLine, " ").Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');
        fieldInfo.CheckRule = string.IsNullOrEmpty(checkRuleString) ? null : checkRuleString;
        // 导出到数据库中的字段名及类型
        if(ExcelTableSetting.DataFieldExportDataBaseFieldInFoRowIndex>=0)
            fieldInfo.DatabaseInfoString = dt.Rows[ExcelTableSetting.DataFieldExportDataBaseFieldInFoRowIndex][columnIndex].ToString().Trim();
        
        // 引用父FileInfo
        fieldInfo.ParentField = parentField;

        // 如果该字段是array类型的子元素，则不填写变量名（array下属元素的变量名自动顺序编号）
        // 并且如果子元素不是集合类型，也不需配置数据类型（直接使用array声明的子元素数据类型，子元素列不再单独配置），直接依次声明各子元素列
        string inputFieldName = dt.Rows[ExcelTableSetting.DataFieldNameRowIndex][columnIndex].ToString().Trim();
        if (parentField != null && parentField.DataType == DataType.Array)
        {
            // array的子元素如果为array或dict，则必须像array、dict定义格式那样通过一列声明子元素类型（目的为了校验，防止类似array声明子元素为dict[2]，而实际子元素为dict[3]，导致程序仍能正确解析之后的字段，但逻辑上却把后面的独立字段误认为是声明的array的子元素），但变量名仍旧不需填写
            if (parentField.ArrayChildDataType == DataType.Array)
            {
                string inputDataTypeString = dt.Rows[ExcelTableSetting.DataFieldDataTypeRowIndex][columnIndex].ToString().Trim();
                if (string.IsNullOrEmpty(inputDataTypeString))
                {
                    errorString = "array的子元素若为array型，则必须在每个子元素声明列中声明具体的array类型";
                    nextFieldColumnIndex = columnIndex + 1;
                    return null;
                }
                // 判断本列所声明的类型是否为array型
                DataType inputDataType = _AnalyzeDataType(inputDataTypeString);
                if (inputDataType != DataType.Array)
                {
                    errorString = string.Format("父array声明的子元素类型为array，但第{0}列所填的类型为{1}", ExcelMethods.GetExcelColumnName(columnIndex + 1), inputDataType.ToString());
                    nextFieldColumnIndex = columnIndex + 1;
                    return null;
                }
                // 解析本列实际填写的array的子元素数据类型定义
                string childDataTypeString;
                DataType childDataType;
                int childCount;
                _GetArrayChildDefine(inputDataTypeString, out childDataTypeString, out childDataType, out childCount, out errorString);
                if (errorString != null)
                {
                    errorString = string.Format("解析第{0}列定义的array类型声明错误，{1}", ExcelMethods.GetExcelColumnName(columnIndex + 1), errorString);
                    nextFieldColumnIndex = columnIndex + 1;
                    return null;
                }
                // 解析父array声明的子类型数据类型定义（之前经过了检测，这里无需进行容错）
                string defineDataTypeString;
                DataType defineDataType;
                int defineCount;
                _GetArrayChildDefine(parentField.ArrayChildDataTypeString, out defineDataTypeString, out defineDataType, out defineCount, out errorString);
                if (childDataType != defineDataType)
                {
                    errorString = string.Format("父array的array类型子元素所声明的子元素为{0}型，但你填写的经过解析为{1}型", defineDataType.ToString(), childDataType.ToString());
                    nextFieldColumnIndex = columnIndex + 1;
                    return null;
                }
                if (childCount != defineCount)
                {
                    errorString = string.Format("父array的array类型子元素所声明的子元素共有{0}个，但你填写的经过解析为{1}个", defineCount, childCount);
                    nextFieldColumnIndex = columnIndex + 1;
                    return null;
                }
            }
            else if (parentField.ArrayChildDataType == DataType.Dict)
            {
                string inputDataTypeString = dt.Rows[ExcelTableSetting.DataFieldDataTypeRowIndex][columnIndex].ToString().Trim();
                // dict类型子元素定义列中允许含有无效列
                if (string.IsNullOrEmpty(inputDataTypeString))
                {
                    errorString = null;
                    nextFieldColumnIndex = columnIndex + 1;
                    return null;
                }
                else
                {
                    // 判断本列所声明的类型是否为dict型
                    DataType inputDataType = _AnalyzeDataType(inputDataTypeString);
                    if (inputDataType != DataType.Dict)
                    {
                        errorString = string.Format("父array声明的子元素类型为dict，但第{0}列所填的类型为{1}", ExcelMethods.GetExcelColumnName(columnIndex + 1), inputDataType.ToString());
                        nextFieldColumnIndex = columnIndex + 1;
                        return null;
                    }
                    // 解析本列实际填写的dict的子元素格式
                    int childCount;
                    _GetDictChildCount(inputDataTypeString, out childCount, out errorString);
                    if (errorString != null)
                    {
                        errorString = string.Format("解析第{0}列定义的dict类型声明错误，{1}", ExcelMethods.GetExcelColumnName(columnIndex + 1), errorString);
                        nextFieldColumnIndex = columnIndex + 1;
                        return null;
                    }
                    // 解析父array声明的dict子元素个数（之前经过了检测，这里无需进行容错）
                    int defineCount;
                    _GetDictChildCount(parentField.ArrayChildDataTypeString, out defineCount, out errorString);
                    if (childCount != defineCount)
                    {
                        errorString = string.Format("父array的dict类型子元素所声明的子元素共有{0}个，但你填写的经过解析为{1}个", defineCount, childCount);
                        nextFieldColumnIndex = columnIndex + 1;
                        return null;
                    }
                }
            }

            // 检查array下属子元素列，不允许填写变量名（如果不进行此强制限制，当定表时没有空出足够的array子元素列数，并且后面为独立字段时，解析array型时会把后面的独立字段当成array子元素声明列，很可能造成格式上可以正确解析，但逻辑上和定表者想法不符但不易发觉的问题）
            if (!string.IsNullOrEmpty(inputFieldName))
            {
                errorString = string.Format("array下属的子元素列不允许填写变量名，第{0}列错误地填写了变量名{1}", ExcelMethods.GetExcelColumnName(columnIndex + 1), inputFieldName);
                nextFieldColumnIndex = columnIndex + 1;
                return null;
            }
            fieldInfo.FieldName = null;
            fieldInfo.DataType = parentField.ArrayChildDataType;
            // array类型的ArrayChildDataTypeString中还包含了子元素个数，需要去除
            int lastColonIndex = parentField.ArrayChildDataTypeString.LastIndexOf(':');
            if (lastColonIndex == -1)
            {
                AppLog.LogErrorAndExit("错误：用_AnalyzeOneField函数解析array子元素类型时发现array类型的ArrayChildDataTypeString中不含有子元素个数");
                errorString = null;
                nextFieldColumnIndex = columnIndex + 1;
                return null;
            }
            else
                fieldInfo.DataTypeString = parentField.ArrayChildDataTypeString.Substring(0, lastColonIndex);
        }
        else if (string.IsNullOrEmpty(inputFieldName))
        {
            // dict下属字段的变量名为空的列，视为无效列，直接忽略
            if (fieldInfo.ParentField != null && fieldInfo.ParentField.DataType == DataType.Dict)
            {
                // AppLog.LogWarning(string.Format("警告：第{0}列为dict下属字段，但未填写变量名，将被视为无效列而忽略", ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1)));
                errorString = null;
                nextFieldColumnIndex = columnIndex + 1;
                return null;
            }
            // 独立字段未填写字段名以及导出数据库信息，视为无效列，直接忽略
            else if (string.IsNullOrEmpty(fieldInfo.DatabaseInfoString))
            {
                //  AppLog.LogWarning(string.Format("警告：第{0}列未填写变量名，也未填写导出数据库信息，将被视为无效列而忽略", ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1)));
                errorString = null;
                nextFieldColumnIndex = columnIndex + 1;
                return null;
            }
            // 未填写字段名但填写了导出数据库的信息，视为只进行MySQL导出的字段，自动进行字段命名
            else
            {
                //  AppLog.LogWarning(string.Format("警告：第{0}列未填写变量名，仅填写了导出数据库信息，将不会进行lua、csv、json等客户端形式导出", ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1)));
                fieldInfo.FieldName = string.Concat(DataBase.AutoFieldNamePrefix, fieldInfo.ColumnSeq);
                fieldInfo.IsIgnoreClientExport = true;

                // 读取填写的数据类型
                fieldInfo.DataTypeString = dt.Rows[ExcelTableSetting.DataFieldDataTypeRowIndex][columnIndex].ToString().Trim();
                fieldInfo.DataType = _AnalyzeDataType(fieldInfo.DataTypeString);
            }
        }
        else
        {
            // 检查字段名是否合法
            if (!TableCheckHelper.CheckFieldName(inputFieldName, out errorString))
            {
                nextFieldColumnIndex = columnIndex + 1;
                return null;
            }
            else
                fieldInfo.FieldName = inputFieldName;

            // 读取填写的数据类型
            fieldInfo.DataTypeString = dt.Rows[ExcelTableSetting.DataFieldDataTypeRowIndex][columnIndex].ToString().Trim();
            fieldInfo.DataType = _AnalyzeDataType(fieldInfo.DataTypeString);
        }

        switch (fieldInfo.DataType)
        {
            case DataType.Int:
                {
                    _AnalyzeIntType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Long:
                {
                    _AnalyzeLongType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Float:
                {
                    _AnalyzeFloatType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Bool:
                {
                    _AnalyzeBoolType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.String:
                {
                    _AnalyzeStringType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Lang:
                {
                    _AnalyzeLangType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Date:
                {
                    _AnalyzeDateType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Time:
                {
                    _AnalyzeTimeType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Json:
                {
                    _AnalyzeJsonType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.TableString:
                {
                    _AnalyzeTableStringType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.MapString:
                {
                    _AnalyzeMapStringType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Array:
                {
                    _AnalyzeArrayType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            case DataType.Dict:
                {
                    _AnalyzeDictType(fieldInfo, tableInfo, dt, columnIndex, parentField, out nextFieldColumnIndex, out errorString);
                    break;
                }
            default:
                {
                    errorString = string.Format("数据类型无效，填写的为{0}", fieldInfo.DataTypeString);
                    nextFieldColumnIndex = columnIndex + 1;
                    break;
                }
        }

        if (errorString == null)
            return fieldInfo;
        else
            return null;
    }

}
