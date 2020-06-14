using System.Collections.Generic;

/// <summary>
/// 一张表格中一个字段的信息，包含这个字段名称、数据类型、检查规则的定义，以及所有行的数据
/// </summary>
public class FieldInfo
{
    public FieldInfo()
    {
        ExtraParam = new Dictionary<string, object>();
        IsIgnoreClientExport = false;
    }

    /// <summary>
    /// 表名， 该字段所在表格,如item
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Sheet表名
    /// </summary>
    public string SheetName { get; set; }

    /// <summary>
    /// 第1行，字段描述，即中文字段名称
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 第2行，英文字段名
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    /// 第3行，lua等客户端字段数据库类型
    /// </summary>
    public DataType DataType { get; set; }

    /// <summary>
    /// 第4行 声明字段检查规则的字符串
    /// </summary>
    public string CheckRule { get; set; }

    /// <summary>
    /// 第5行，导出到MySQL数据库中对应的字段名
    /// </summary>
    public string DatabaseFieldName { get; set; }

    /// <summary>
    /// 第5行，导出到MySQL数据库中对应的字段数据类型
    /// </summary>
    public string DatabaseFieldType { get; set; }

    /// <summary>
    /// 第5行，导出到字段数据类型字符串
    /// </summary>
    public string DatabaseInfoString { get; set; }

    // 声明字段数据类型的字符串
    public string DataTypeString { get; set; }

    // array类型的子元素的数据类型
    public DataType ArrayChildDataType { get; set; }

    // array类型的子元素的数据类型字符串
    public string ArrayChildDataTypeString { get; set; }

    // 该字段在表格中的列号（从0计）
    public int ColumnSeq { get; set; }

    // 是否忽略进行lua、csv、json等客户端方式导出（未填写字段名但填写了数据库导出信息的字段，仅进行数据库导出）
    public bool IsIgnoreClientExport { get; set; }

    // 存储额外属性，比如date类型的输入、导出选项等
    public Dictionary<string, object> ExtraParam { get; set; }

    // 如果该字段为tableString型，存储解析之后的格式定义
    public TableStringFormatDefine TableStringFormatDefine { get; set; }

    // 如果该字段为mapString型，存储解析之后的格式定义
    public MapStringInfo MapStringFormatDefine { get; set; }

    /// <summary>
    /// 如果该字段是array或dict类型，其下属的字段信息存放在该变量中
    /// </summary>
    public List<FieldInfo> ChildField { get; set; }
    /// <summary>
    /// 如果该字段是array或dict类型，其下属的字段name存放在该变量中
    /// </summary>
    public List<string> ChildFieldString { get; set; }

    /// <summary>
    /// 如果该字段是array或dict的子元素，存储其父元素的引用
    /// </summary>
    public FieldInfo ParentField { get; set; }

    /// <summary>
    /// 如果该字段不是集合类型，直接依次存储该字段下的所有行的数据，否则存储每行定义的该集合数据是否有效
    /// </summary>
    public List<object> Data { get; set; }

    // 如果该字段为lang型，LangKeys中额外存储所填的所有key名，对应的键值则存储在Data中
    public List<object> LangKeys { get; set; }

    // 如果该字段为json型，JsonString中存储所填的所有json字符串，对应解析后的JsonData存储在Data中
    public List<string> JsonString { get; set; }

    //导出类型
    public ExportTableType ExportTable { get; set; }

    /// <summary>
    /// 将字段B的数据追加到字段A中，如果字段B数据不存在，以追加等数量的字段B所在表的主键数量null或false
    /// </summary>
    /// <param name="FieldA"></param>
    /// <param name="FieldB"></param>
    /// <param name="FieldBPrimaryKeyCount"></param>
    /// <param name="errorString"></param>
    /// <returns></returns>
    public static FieldInfo AddDataByOtherField(FieldInfo FieldA, FieldInfo FieldB, int FieldBPrimaryKeyCount,int row, out string errorString)
    {
        FieldInfo FieldReturn = FieldA;
        if (FieldReturn.DataType.Equals(FieldB.DataType) == false)
        {
            errorString=string.Format("错误：数据追加的字段类型必须一致，字段为{0}的类型为{1}(Excel表第：{2}行)与字段为{3}的类型为{4}不同", FieldReturn.FieldName, FieldReturn.DataType, ExcelTableSetting.DataFieldDataTypeRowIndex, FieldB.FieldName, FieldB.DataType);
            return FieldReturn;
        }

        if (FieldReturn.Data == null)
            FieldReturn.Data = new List<object>();

        if (FieldReturn.ChildField == null && FieldB.ChildField !=null)
            FieldReturn.ChildField = new List<FieldInfo>();

        if (FieldB.Data==null || FieldB.Data.Count==0)
        {
            //如果为bool dict array 则默认为fasel否则默认为null
            if(FieldReturn.DataType==DataType.Bool || FieldReturn.DataType==DataType.Dict || FieldReturn.DataType==DataType.Array)
            {
                for (int i = 0; i < FieldBPrimaryKeyCount; ++i)
                {
                    FieldReturn.Data.Add(false);
                    //FieldA.ChildField.Add(null);
                   
                }
            }
            else
            {
                for (int i = 0; i < FieldBPrimaryKeyCount; ++i)
                {
                    FieldReturn.Data.Add(null);
                    //FieldA.ChildField.Add(null);
                   
                }
            }
        }
        else
        {
            int iRow = row;
            for (int i = row; i < FieldBPrimaryKeyCount; ++i)
            {
                FieldReturn.Data.Add(FieldB.Data[i]);
                if(FieldReturn.ChildField!=null)
                {
                    if(FieldB.ChildField != null)
                    {
                        if (FieldReturn.ChildField.Count == FieldB.ChildField.Count)
                        {
                            foreach (FieldInfo ChildFieldInfo1 in FieldReturn.ChildField)
                            {
                                FieldInfo fieldAA = ChildFieldInfo1;
                               if (FieldB.ChildFieldString.Contains(ChildFieldInfo1.FieldName))
                                {
                                    int index = FieldB.ChildFieldString.IndexOf(ChildFieldInfo1.FieldName);
                                    FieldInfo FieldReturnA= AddDataByOtherField(fieldAA, FieldB.ChildField[index], FieldBPrimaryKeyCount, row, out errorString);
                                    fieldAA = FieldReturnA;
                                }
                               else
                                {
                                    errorString = string.Format("出现错误,因为字段{0}的ChildField:{1}无法合并", FieldReturn, FieldB);
                                    return FieldReturn;
                                }
                                int indexA = FieldReturn.ChildFieldString.IndexOf(fieldAA.FieldName);
                                FieldReturn.ChildField[indexA].Data = fieldAA.Data;
                            }
                        }
                        else
                        {
                            errorString = string.Format("出现错误,因为字段{0}与{1}的ChildField数量不同", FieldReturn, FieldB);
                            return FieldReturn;
                        }
                    }
                }
                else
                {
                    if (FieldB.ChildField != null)
                    {
                        errorString = string.Format("出现错误,因为字段{0}不存在ChildField，但在字段{1}中存在ChildField", FieldReturn, FieldB);
                        return FieldReturn;
                    }
                }
                iRow++;
            }
        }
        errorString = null;
        return FieldReturn;
    }
    /// <summary>
    /// 导出时的特殊处理
    /// </summary>
    public enum ExportTableType
    {
        Null,
        ToErlang,    //
        ToJson,    //
    }
}