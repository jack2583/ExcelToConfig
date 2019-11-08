using System.Collections.Generic;
using System.Data;

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
    /// 不带中文且不组后缀的Excel文件名，即数据表名，Excel文件不带后缀的名称，如 item_ft
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Excel文件所在的目录，
    /// 如：C:\Users\Administrator\Desktop
    /// Path.GetDirectoryName(ExcelFilePath)
    /// </summary>
    public string ExcelDirectoryName { get; set; }

    /// <summary>
    /// Excel文件的名称含有后缀,
    ///如：text.xlsx
    ///Path.GetFileName(ExcelFilePath)
    /// </summary>
    public string ExcelFileName { get; set; }

    // 表格配置参数
    public Dictionary<string, List<string>> TableConfig { get; set; }

    public DataTable TableConfigData { get; set; }

    // 存储每个字段的信息以及按字段存储的所有数据
    private List<FieldInfo> _fieldInfo = new List<FieldInfo>();

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
    /// 获取所有指定了字段名，需要进行客户端lua、csv、json等方式导出的字段
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
    public static TableInfo Merge(List<TableInfo> tableInfoList)
    {
        TableInfo tableInfo2 = new TableInfo();
        tableInfo2.ExcelFilePath = tableInfoList[0].ExcelFilePath;
        tableInfo2.ExcelName = tableInfoList[0].ExcelName;
        tableInfo2.TableName = tableInfoList[0].TableName;
        tableInfo2.TableConfig = tableInfoList[0].TableConfig;
        tableInfo2.TableConfigData = tableInfoList[0].TableConfigData;

        FieldInfo fieldInfoTemp;
        List<FieldInfo> allFieldInfo = null;
        List<FieldInfo> allFieldInfoA = null;
        List<string> fieldName = new List<string>();
        foreach (TableInfo tableInfo in tableInfoList)
        {
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

                    tableInfo2.AddField(fieldInfoTemp);
                    fieldName.Add(fieldInfo.FieldName);
                }
            }
        }
        List<FieldInfo> allFieldInfo2 = new List<FieldInfo>();
        allFieldInfo2 = tableInfo2.GetAllFieldInfo();
        List<string> fieldNameB = new List<string>();
        foreach (FieldInfo fieldInfo in allFieldInfo2)
        {
            if (tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).Data == null)
                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).Data = new List<object>();

            if (tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys == null)
                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys = new List<object>();

            if (tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString == null)
                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString = new List<string>();
        }
        foreach (TableInfo tableInfo in tableInfoList)
        {
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
                            if (data != null)
                                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).Data.Add(data);
                            else
                                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).Data.Add(null);

                            //object langkeys = tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys[i];
                            if (tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys != null)
                                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys.Add(tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys[i]);
                            else
                                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys.Add(null);

                            // string jsonstring = tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString[i];
                            if (tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString != null)
                                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString.Add(tableInfo.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString[i]);
                            else
                                tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString.Add(null);
                        }
                        else
                        {
                            tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).Data.Add(null);
                            tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).LangKeys.Add(null);
                            tableInfo2.GetFieldInfoByFieldName(fieldInfo.FieldName).JsonString.Add(null);
                        }
                    }
                }
            }
        }

        return tableInfo2;
    }
}