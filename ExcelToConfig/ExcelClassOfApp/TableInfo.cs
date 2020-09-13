using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

/// <summary>
/// 将一张Excel表格解析为本工具所需的数据结构
/// </summary>
public class TableInfo
{
    /// <summary>
    /// 完成Excel文件目录,路径+名字+中文+后缀
    /// 如：C:\Users\Administrator\Desktop\text.xlsx
    /// </summary>
    public string ExcelFilePath { get; set; }

    /// <summary>
    ///带中文但不带后缀的Excel文件名，如 item_ft-物品
    /// Path.GetFileNameWithoutExtension(ExcelFilePath)
    /// </summary>
    public string ExcelName { get; set; }

    /// <summary>
    /// 不带中文且不组后缀的Excel文件名，即数据表名，Excel文件不带后缀的名称，如 item
    /// 如果是多语言则会同时去掉多语言后缀
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Excel文件所在的目录，
    /// 如：C:\Users\Administrator\Desktop
    /// Path.GetDirectoryName(ExcelFilePath)
    /// </summary>
    //public string ExcelDirectoryName { get; set; }
    public string ExcelDirectory { get; set; }
    public string ExcelNameTips
    {
        get
        {
            return ExcelDirectory + "/" + TableName;
        }
    }


    /// <summary>
    /// Excel文件的名称含有后缀,
    ///如：text.xlsx
    ///Path.GetFileName(ExcelFilePath)
    /// </summary>
    //public string ExcelFileName { get; set; }

    // 表格配置参数
    public Dictionary<string, string> TableConfigData { get; set; }
    // 表格配置参数
    public Dictionary<string, List<string>> TableConfigData2 { get; set; }

    public DataTable TableConfig { get; set; }

    // 存储每个字段的信息以及按字段存储的所有数据
    private List<FieldInfo> _fieldInfo = new List<FieldInfo>();


    private List<string> _fieldName = new List<string>();

    // 用于将字段名对应到_fieldInfo中的下标位置，目的是当其他表格进行ref检查规则时无需遍历快速找到指定字段列信息和数据（key：fieldName， value：index），但忽略array或dict的子元素列
    private Dictionary<string, int> _indexForFieldNameToColumnSeq = new Dictionary<string, int>();

    /// <summary>
    /// 添加字段
    /// </summary>
    /// <param name="fieldInfo"></param>
    public void AddField(FieldInfo fieldInfo)
    {
        _fieldInfo.Add(fieldInfo);
        _indexForFieldNameToColumnSeq.Add(fieldInfo.FieldName, _fieldInfo.Count - 1);
        _fieldName.Add(fieldInfo.FieldName);
    }

    /// <summary>
    /// 根据字段名获取该字段
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public FieldInfo GetFieldInfoByFieldName(string fieldName)
    {
        if (_indexForFieldNameToColumnSeq.ContainsKey(fieldName))
            return _fieldInfo[_indexForFieldNameToColumnSeq[fieldName]];
        else
            return null;
    }

    /// <summary>
    /// 获取所有字段信息
    /// </summary>
    /// <returns></returns>
    public List<FieldInfo> GetAllFieldInfo()
    {
        return _fieldInfo;
    }

    /// <summary>
    /// 获取所有字段名字
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllFieldName()
    {
        return _fieldName;
    }
    /// <summary>
    /// 是否存在指定名字的字段
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public bool IsContainField(string fieldName)
    {
        return _fieldName.Contains(fieldName);
    }
    /// <summary>
    /// 获取所有【客户端】字段，需要进行客户端lua、csv、json等方式导出的字段
    /// </summary>
    public List<FieldInfo> GetAllClientFieldInfo()
    {
        List<FieldInfo> allClientFieldInfo = new List<FieldInfo>();
        foreach (FieldInfo fieldInfo in _fieldInfo)
        {
            if (fieldInfo.IsIgnoreClientExport == false)
                allClientFieldInfo.Add(fieldInfo);
        }

        return allClientFieldInfo;
    }
    /// <summary>
    /// 获取所有【服务端】字段
    /// </summary>
    public List<FieldInfo> GetAllServerFieldInfo()
    {
        List<FieldInfo> allServerFieldInfo = new List<FieldInfo>();
        foreach (FieldInfo fieldInfo in _fieldInfo)
        {
            if (fieldInfo.DatabaseFieldName != null)
                allServerFieldInfo.Add(fieldInfo);
        }

        return allServerFieldInfo;
    }
    /// <summary>
    /// 获取主键字段
    /// </summary>
    /// <returns></returns>
    public FieldInfo GetKeyColumnFieldInfo()
    {
        if (_fieldInfo.Count > 0)
            return _fieldInfo[0];
        else
            return null;
    }

    /// <summary>
    /// 获取依次排列的表格中各字段信息，但无视array、dict型的嵌套结构，将其下属子元素作为独立字段
    /// </summary>
    public List<FieldInfo> GetAllClientFieldInfoIgnoreSetDataStructure()
    {
        List<FieldInfo> allFieldInfo = new List<FieldInfo>();
        foreach (FieldInfo fieldInfo in _fieldInfo)
            _AddClientFieldInfoFromOneField(fieldInfo, allFieldInfo);

        return allFieldInfo;
    }

    public void _AddClientFieldInfoFromOneField(FieldInfo fieldInfo, List<FieldInfo> allFieldInfo)
    {
        if (fieldInfo.DataType == DataType.Array || fieldInfo.DataType == DataType.Dict)
        {
            allFieldInfo.Add(fieldInfo);
            foreach (FieldInfo childField in fieldInfo.ChildField)
                _AddClientFieldInfoFromOneField(childField, allFieldInfo);
        }
        else if (fieldInfo.IsIgnoreClientExport == false)
            allFieldInfo.Add(fieldInfo);
    }

    /// <summary>
    /// 指定TableInfo与当前TableInfo合并
    /// </summary>
    /// <param name="tableInfoList"></param>
    public static TableInfo Merge(string newTableName, List<TableInfo> tableInfoList, out string errorString)
    {
        errorString = null;
        StringBuilder stringBuilder = new StringBuilder();
        //foreach(TableInfo t in tableInfoList)
        //{
        //    stringBuilder.AppendLine(t.ExcelName);
        //}
        AppLog.Log(string.Format("\n开始合并表格{0}", newTableName));
      //  AppLog.Log(string.Format("{0}",stringBuilder.ToString()), ConsoleColor.Green);
        TableInfo tableInfoFirst = new TableInfo();
        tableInfoFirst.ExcelFilePath = tableInfoList[0].ExcelFilePath;
        tableInfoFirst.ExcelName = tableInfoList[0].ExcelName;
        tableInfoFirst.TableName = newTableName;
        tableInfoFirst.ExcelDirectory = tableInfoList[0].ExcelDirectory;
        tableInfoFirst.TableConfigData = tableInfoList[0].TableConfigData;
        tableInfoFirst.TableConfigData2 = tableInfoList[0].TableConfigData2;
        tableInfoFirst.TableConfig = tableInfoList[0].TableConfig;



        FieldInfo fieldInfoTemp;
        List<FieldInfo> allFieldInfo = null;
        List<FieldInfo> allFieldInfoA = null;
        List<string> fieldName = new List<string>();

        //逐个表合并
        foreach (TableInfo tableInfo in tableInfoList)
        {
           
            FieldInfo tableInfoKeyField = tableInfo.GetKeyColumnFieldInfo();
            FieldInfo tableInfoFirstKeyField = tableInfoFirst.GetKeyColumnFieldInfo();

            //检查主键是否相同
            if (tableInfoFirstKeyField != null)
            {
                if (!(string.Equals(tableInfoFirstKeyField.FieldName, tableInfoKeyField.FieldName)
                    && string.Equals(tableInfoFirstKeyField.DataType, tableInfoKeyField.DataType)
                    && string.Equals(tableInfoFirstKeyField.DatabaseFieldName, tableInfoKeyField.DatabaseFieldName)
                    && string.Equals(tableInfoFirstKeyField.DatabaseFieldType, tableInfoKeyField.DatabaseFieldType)))
                {

                    AppLog.LogErrorAndExit(string.Format("合并遇到错误：合并表格的主键名必须一致，表{0}的主键与其他表格主键不同", tableInfo.ExcelName));
                }
            }

            //合并字段信息
            allFieldInfo = new List<FieldInfo>();
            allFieldInfo = tableInfo.GetAllFieldInfo();
            foreach (FieldInfo fieldInfo in allFieldInfo)
            {
                if (!fieldName.Contains(fieldInfo.FieldName))
                {
                    fieldInfoTemp = new FieldInfo();
                    fieldInfoTemp.TableName = fieldInfo.TableName;
                    fieldInfoTemp.SheetName = fieldInfo.SheetName;
                    //Excel表第1行信息
                    fieldInfoTemp.Desc = fieldInfo.Desc;
                    //Excel表第2行信息
                    fieldInfoTemp.FieldName = fieldInfo.FieldName;
                    //Excel表第3行信息
                    fieldInfoTemp.DataType = fieldInfo.DataType;
                    fieldInfoTemp.DataTypeString = fieldInfo.DataTypeString;
                    fieldInfoTemp.ExtraParam = fieldInfo.ExtraParam;//类似：date(input=#1970sec|toLua=yyyy年MM月dd日 HH时mm分ss秒)
                                                                    //Excel表第4行信息
                    fieldInfoTemp.CheckRule = fieldInfo.CheckRule;
                    //Excel表第5行信息
                    fieldInfoTemp.DatabaseFieldName = fieldInfo.DatabaseFieldName;
                    fieldInfoTemp.DatabaseFieldType = fieldInfo.DatabaseFieldType;

                    //其他信息
                    fieldInfoTemp.ArrayChildDataType = fieldInfo.ArrayChildDataType;
                    fieldInfoTemp.ArrayChildDataTypeString = fieldInfo.ArrayChildDataTypeString;
                    fieldInfoTemp.ColumnSeq = fieldInfo.ColumnSeq;
                    fieldInfoTemp.IsIgnoreClientExport = fieldInfo.IsIgnoreClientExport;
                    fieldInfoTemp.TableStringFormatDefine = fieldInfo.TableStringFormatDefine;
                    fieldInfoTemp.MapStringFormatDefine = fieldInfo.MapStringFormatDefine;
                    fieldInfoTemp.ChildField = fieldInfo.ChildField;
                    fieldInfoTemp.ParentField = fieldInfo.ParentField;

                    tableInfoFirst.AddField(fieldInfoTemp);
                    fieldName.Add(fieldInfo.FieldName);
                }
                else
                {
                    FieldInfo tableInfoFirstField = tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName);
                    if (!(String.Equals(tableInfoFirstField.FieldName, fieldInfo.FieldName)
                        && String.Equals(tableInfoFirstField.DataType, fieldInfo.DataType)
                        && String.Equals(tableInfoFirstField.DatabaseFieldName, fieldInfo.DatabaseFieldName)
                        && String.Equals(tableInfoFirstField.DatabaseFieldType, fieldInfo.DatabaseFieldType)))
                    {

                        AppLog.LogErrorAndExit(string.Format("合并遇到错误：合并表格的字段必须一致，表{0}的字段{1}与其他表格不同", tableInfo.ExcelName, fieldInfo.FieldName));
                    }
                }

            }


        }
        List<FieldInfo> allFieldInfo2 = new List<FieldInfo>();
        allFieldInfo2 = tableInfoFirst.GetAllFieldInfo();
        List<string> fieldNameB = new List<string>();
        //所有字段先赋值
        foreach (FieldInfo fieldInfo in allFieldInfo2)
        {
            if (tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).Data == null)
                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).Data = new List<object>();

            if (tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys == null)
                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys = new List<object>();

            if (tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString == null)
                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString = new List<string>();
        }

        //合并字段的值
        //Dictionary<主键, 源自哪个Excel表>
        Dictionary<string, string> KeyColumn = new Dictionary<string, string>();
        string KeyColumnName = tableInfoList[0].GetKeyColumnFieldInfo().FieldName;
        StringBuilder sbkey = new StringBuilder();
        foreach (TableInfo tableInfo in tableInfoList)
        {
            AppLog.Log(string.Format("将表[{0}]合并到[{1}]", tableInfo.ExcelName, newTableName), ConsoleColor.Green);
            int count = tableInfo.GetAllFieldInfo()[0].Data.Count;
            if (count > 0)
            {
                allFieldInfoA = new List<FieldInfo>();
                allFieldInfoA = tableInfo.GetAllFieldInfo();
                for (int i = 0; i < count; ++i)
                {
                    foreach (FieldInfo fieldInfo in allFieldInfo2)
                    {
                        if (tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName) != null)
                        {
 
                            object data = tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).Data[i];
                            if (string.Equals(fieldInfo.FieldName, KeyColumnName))
                            {
                                if (!KeyColumn.ContainsKey(data.ToString()))
                                    KeyColumn.Add(data.ToString(), tableInfo.ExcelName);
                                else
                                    sbkey.AppendLine(string.Format("错误：表{0}使用了已存在主键值{1}", tableInfo.ExcelName, data.ToString()));
                                   // AppLog.LogErrorAndExit(string.Format("错误：表{0}使用了已存在主键值{1}", tableInfo.ExcelName, data.ToString()));
                            }
                            if (data != null)
                                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).Data.Add(data);
                            else
                                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).Data.Add(null);

                            //object langkeys = tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys[i];
                            if (tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys != null)
                                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys.Add(tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys[i]);
                            else
                                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys.Add(null);

                            // string jsonstring = tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString[i];
                            if (tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString != null)
                                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString.Add(tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString[i]);
                            else
                                tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString.Add(null);
                        }
                        else
                        {
                            tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).Data.Add(null);
                            tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys.Add(null);
                            tableInfoFirst.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString.Add(null);
                        }
                    }
                }
            }
            AppLog.Log("合并成完");
        }

        if(sbkey.Length>0)
            AppLog.LogErrorAndExit(sbkey.ToString());
        // 唯一性检查
        //FieldCheckRule uniqueCheckRule = new FieldCheckRule();
        //uniqueCheckRule.CheckType = TableCheckType.Unique;
        //uniqueCheckRule.CheckRuleString = "unique";
        //TableCheckHelper.CheckUnique(tableInfoFirst.GetKeyColumnFieldInfo(), uniqueCheckRule, out errorString);
        //if (errorString != null)
        //{
        //    //string error = string.Format("表格{0}-{1}中列号为{2}的字段存在以下严重错误，导致无法继续，请修正错误后重试\n", newTableName, "", ExcelMethods.GetExcelColumnName(0 + 1));
        //    // errorString = "主键列存在重复错误\n" + errorString;
        //    AppLog.LogErrorAndExit(string.Format("错误：合并{0}失败,因为主键{1},{2}", newTableName, tableInfoFirst.GetKeyColumnFieldInfo().FieldName, errorString));
        //    //AppLog.LogErrorAndExit(string.Format("错误：存在多个以{0}为名的表格,且合并时发生错误失败\n{1}", tableName, errorString));
        //}

        return tableInfoFirst;
    }
    //    catch(Exception e)
    //    {
    //        AppLog.LogErrorAndExit(string.Format("合并表格失败，因为{0}",e));
    //    }
    //    return newTableInfo;
    //}
    /// <summary>
    /// 合并表格
    /// </summary>
    /// <param name="newTableName">新表的名字，如：item</param>
    /// <param name="tableInfoList">待合并的表，TableInfo </param>
    /// <param name="errorString">错误输出</param>
    /// <returns></returns>
    public static TableInfo MergeTable(string newTableName, List<TableInfo> tableInfoList, out string errorString)
    {
        AppLog.Log(string.Format("开始合并表格：{0}", newTableName));
        errorString = null;
        TableInfo newTableInfo = new TableInfo();
        TableInfo oldTableInfo = tableInfoList[0];
        //新表的基本信息
        newTableInfo.ExcelFilePath = oldTableInfo.ExcelFilePath;
        newTableInfo.ExcelName = oldTableInfo.ExcelName;
        newTableInfo.TableName = newTableName;
        newTableInfo.ExcelDirectory = oldTableInfo.ExcelDirectory;
        newTableInfo.TableConfigData = oldTableInfo.TableConfigData;
        newTableInfo.TableConfigData2 = oldTableInfo.TableConfigData2;
        newTableInfo.TableConfig = oldTableInfo.TableConfig;


        FieldInfo newPrimaryKeyField = new FieldInfo();
        FieldInfo oldPrimaryKeyField = oldTableInfo.GetKeyColumnFieldInfo();
        newPrimaryKeyField.TableName = oldPrimaryKeyField.TableName;
        newPrimaryKeyField.SheetName = oldPrimaryKeyField.SheetName;
        //Excel表第1行信息
        newPrimaryKeyField.Desc = oldPrimaryKeyField.Desc;
        //Excel表第2行信息
        newPrimaryKeyField.FieldName = oldPrimaryKeyField.FieldName;
        //Excel表第3行信息
        newPrimaryKeyField.DataType = oldPrimaryKeyField.DataType;
        newPrimaryKeyField.DataTypeString = oldPrimaryKeyField.DataTypeString;
        newPrimaryKeyField.ExtraParam = oldPrimaryKeyField.ExtraParam;//类似：date(input=#1970sec|toLua=yyyy年MM月dd日 HH时mm分ss秒)
        //Excel表第4行信息
        newPrimaryKeyField.CheckRule = oldPrimaryKeyField.CheckRule;
        //Excel表第5行信息
        newPrimaryKeyField.DatabaseFieldName = oldPrimaryKeyField.DatabaseFieldName;
        newPrimaryKeyField.DatabaseFieldType = oldPrimaryKeyField.DatabaseFieldType;
        newPrimaryKeyField.DatabaseInfoString = oldPrimaryKeyField.DatabaseInfoString;


        //合并主键及数据
        StringBuilder tempMergeTable1 = new StringBuilder();
        foreach (TableInfo tableInfoTemp in tableInfoList)
        {
            FieldInfo fieldInfoTemp = tableInfoTemp.GetKeyColumnFieldInfo();
            if (newPrimaryKeyField.FieldName.Equals(fieldInfoTemp.FieldName) == false)
            {
                AppLog.LogErrorAndExit(string.Format("合并遇到错误：合并表格的主键名必须一致，表{0}的主键名为{1}与其他表格主键为{2}不同,其他表格包括：{3}", tableInfoTemp.ExcelName, fieldInfoTemp.FieldName, newPrimaryKeyField.FieldName, tempMergeTable1.ToString()));
            }

            if (newPrimaryKeyField.DataType.Equals(fieldInfoTemp.DataType) == false)
            {
                AppLog.LogErrorAndExit(string.Format("合并遇到错误：合并表格的主键名及类型必须一致，表{0}的主键名为{1}(类型为：{2})与其他表格主键类型为{3},其他表格包括：{4}", tableInfoTemp.ExcelName, fieldInfoTemp.FieldName, fieldInfoTemp.DataType, newPrimaryKeyField.DataType, tempMergeTable1.ToString()));
            }

            //if (newPrimaryKeyField.DatabaseInfoString.Equals(fieldInfoTemp.DatabaseInfoString) == false)
            //{
            //    AppLog.LogErrorAndExit(string.Format("合并遇到错误：合并表格的主键导入数据库（Excel第{0}行）必须一致：表{1}的导入数据库{2}与其他表格主键不同,其他表格包括：{3}", ExcelTableSetting.DataFieldExportDataBaseFieldInFoRowIndex, tableInfoTemp.ExcelName, fieldInfoTemp.DatabaseInfoString, tempMergeTable1.ToString()));
            //}

            newPrimaryKeyField = FieldInfo.AddDataByOtherField(newPrimaryKeyField, fieldInfoTemp, fieldInfoTemp.Data.Count, 0, out errorString);
            if (errorString != null)
            {
                AppLog.LogErrorAndExit("合并遇到错误：" + errorString);
            }
        }
        // 唯一性检查
        FieldCheckRule uniqueCheckRule = new FieldCheckRule();
        uniqueCheckRule.CheckType = TableCheckType.Unique;
        uniqueCheckRule.CheckRuleString = "unique";
        TableCheckHelper.CheckUnique(newPrimaryKeyField, uniqueCheckRule, out errorString);
        if (errorString != null)
        {
            AppLog.LogErrorAndExit(string.Format("合并后名为{0}的表格主键存在重复值：{1}", newTableName, errorString));
        }
        newTableInfo.AddField(newPrimaryKeyField);


        // 合并其他字段及字段类型，但不合并数据
        List<string> newTableFieldNames = new List<string>();
        newTableFieldNames.Add(newPrimaryKeyField.FieldName);
        StringBuilder tempMergeTable2 = new StringBuilder();
        foreach (TableInfo tableInfoTemp in tableInfoList)
        {
            List<FieldInfo> listFieldInfoTemp = tableInfoTemp.GetAllFieldInfo();
            foreach (FieldInfo tempFieldInfo in listFieldInfoTemp)
            {
                //如果是主键就跳过
                if (tempFieldInfo.FieldName.Equals(newPrimaryKeyField.FieldName))
                    continue;

                //如果已合并过该字段，则进行检查字段类型、数据库类型是否相同
                if (newTableFieldNames.Contains(tempFieldInfo.FieldName))
                {
                    FieldInfo tempNewTableInfoFieldInfo = newTableInfo.GetFieldInfoByFieldName(tempFieldInfo.FieldName);
                    if (tempFieldInfo.DataType.Equals(tempNewTableInfoFieldInfo.DataType) == false)
                    {
                        AppLog.LogErrorAndExit(string.Format("合并遇到错误：合并表格的字段名与类型必须一致，表{0}中为{1}的字段(类型为：{2})与其他表格同名字段类型为{3},其他表格包括：{4}", tableInfoTemp.ExcelName, tempFieldInfo.FieldName, tempFieldInfo.DataType, tempNewTableInfoFieldInfo.DataType, tempMergeTable2.ToString()));
                    }

                    //if (tempFieldInfo.DatabaseInfoString.Equals(tempNewTableInfoFieldInfo.DatabaseInfoString) == false)
                    //{
                    //    AppLog.LogErrorAndExit(string.Format("合并遇到错误：合并表格的字段名与类型必须一致，表{0}中为{1}的字段(导出入数据类型为：{2})与其他表格同名字段导入数据库类型为{3},其他表格包括：{4}", tableInfoTemp.ExcelName, tempFieldInfo.FieldName, tempFieldInfo.DatabaseInfoString, tempNewTableInfoFieldInfo.DatabaseInfoString, tempMergeTable2.ToString()));
                    //}

                    if (tempNewTableInfoFieldInfo.ChildField == null)
                    {
                        if (tempFieldInfo.ChildField != null)
                        {
                            tempNewTableInfoFieldInfo.ChildField = tempFieldInfo.ChildField;
                        }
                    }
                    else
                    {
                        if (tempNewTableInfoFieldInfo.ChildField.Count == tempFieldInfo.ChildField.Count)
                        {
                            //继续判断其他条件
                        }
                        else
                        {
                            AppLog.LogErrorAndExit(string.Format("合并遇到错误:表{0}中为{1}的字段的ChildField数量与其他表格不同，其他表格包括：{2}", tableInfoTemp.ExcelName, tempFieldInfo.FieldName, tempMergeTable2.ToString()));
                        }
                    }
                }
                else//如果未合并，则直接合并
                {
                    FieldInfo newField = new FieldInfo();
                    //Excel表第1行信息
                    newField.Desc = tempFieldInfo.Desc;
                    //Excel表第2行信息
                    newField.FieldName = tempFieldInfo.FieldName;
                    //Excel表第3行信息
                    newField.DataType = tempFieldInfo.DataType;
                    newField.DataTypeString = tempFieldInfo.DataTypeString;
                    newField.ExtraParam = tempFieldInfo.ExtraParam;//类似：date(input=#1970sec|toLua=yyyy年MM月dd日 HH时mm分ss秒)
                                                                   //Excel表第4行信息
                    newField.CheckRule = tempFieldInfo.CheckRule;
                    //Excel表第5行信息
                    newField.DatabaseFieldName = tempFieldInfo.DatabaseFieldName;
                    newField.DatabaseFieldType = tempFieldInfo.DatabaseFieldType;
                    newField.DatabaseInfoString = tempFieldInfo.DatabaseInfoString;

                    newField.ChildField = tempFieldInfo.ChildField;
                    newField.ChildFieldString = tempFieldInfo.ChildFieldString;

                    newTableFieldNames.Add(newField.FieldName);
                    newTableInfo.AddField(newField);
                }
            }
            tempMergeTable2.Append(tableInfoTemp.ExcelName);
        }

        List<FieldInfo> listFieldInfoTemp2 = newTableInfo.GetAllFieldInfo();
        //移除各字段中ChildField字段的数据
        foreach (FieldInfo tempFieldInfo in listFieldInfoTemp2)
        {
            if (tempFieldInfo.ChildField == null)
                continue;

            foreach (FieldInfo ChildFieldInfo1 in tempFieldInfo.ChildField)
            {
                if (ChildFieldInfo1.ChildField == null)
                {
                    ChildFieldInfo1.Data = null;
                    continue;
                }
                else
                {
                    foreach (FieldInfo ChildFieldInfo2 in tempFieldInfo.ChildField)
                    {
                        if (ChildFieldInfo2.ChildField == null)
                        {
                            ChildFieldInfo2.Data = null;
                            continue;
                        }
                        else
                        {
                            foreach (FieldInfo ChildFieldInfo3 in tempFieldInfo.ChildField)
                            {
                                if (ChildFieldInfo3.ChildField == null)
                                {
                                    ChildFieldInfo3.Data = null;
                                    continue;
                                }
                                else
                                {
                                    foreach (FieldInfo ChildFieldInfo4 in tempFieldInfo.ChildField)
                                    {
                                        if (ChildFieldInfo4.ChildField == null)
                                        {
                                            ChildFieldInfo4.Data = null;
                                            continue;
                                        }
                                        else
                                        {
                                            foreach (FieldInfo ChildFieldInfo5 in tempFieldInfo.ChildField)
                                            {
                                                if (ChildFieldInfo5.ChildField == null)
                                                {
                                                    ChildFieldInfo5.Data = null;
                                                    continue;
                                                }
                                                else
                                                {
                                                    //暂时只支持5层ChildField，后续再考虑递归
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //合并其他字段数据,遍历新表所有字段
        foreach (FieldInfo tempFieldInfo in listFieldInfoTemp2)
        {
            FieldInfo fieldA = tempFieldInfo;
            //如果是主键就跳过
            if (fieldA.FieldName.Equals(newPrimaryKeyField.FieldName))
                continue;

            FieldInfo fieldB;
            FieldInfo FieldReturn = new FieldInfo();
            foreach (TableInfo tableInfoTemp in tableInfoList)//遍历所有待合并表
            {
                if (tableInfoTemp.IsContainField(fieldA.FieldName))//如果存在字段就获取到该字段
                {
                    fieldB = tableInfoTemp.GetFieldInfoByFieldName(fieldA.FieldName);
                }
                else
                {
                    fieldB = new FieldInfo();
                    fieldB.Data = null;
                }
                fieldA = FieldInfo.AddDataByOtherField(fieldA, fieldB, tableInfoTemp.GetKeyColumnFieldInfo().Data.Count, 0, out errorString);

            }
            //newTableInfo.AddField(fieldA);
        }
        return newTableInfo;
    }
}