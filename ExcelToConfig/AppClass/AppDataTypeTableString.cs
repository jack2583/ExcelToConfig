using System.Collections.Generic;

/// <summary>
/// tableString代表的table中的key类型
/// </summary>
public enum TableStringKeyType
{
    Seq,             // 按数据顺序自动编号
    DataInIndex,     // 以数据组中指定索引位置的数据为key
}

/// <summary>
/// tableString代表的table中的value类型
/// </summary>
public enum TableStringValueType
{
    True,            // 值始终为true
    DataInIndex,     // 以数据组中指定索引位置的数据为value
    Table,           // value是含有复杂数据的table
}

/// <summary>
/// 形如#1(int)这样对指定数据类型和索引位置的数据的定义
/// </summary>
public struct DataInIndexDefine
{
    // 数据类型
    public DataType DataType;

    // 数据所在数据组的索引位置
    public int DataIndex;
}

/// <summary>
/// 形如type=#1(int)这样对table型value中一个键值对的定义
/// </summary>
public struct TableElementDefine
{
    public string KeyName;
    public DataInIndexDefine DataInIndexDefine;
}

/// <summary>
/// tableString代表的table中对key的定义描述
/// </summary>
public struct TableStringKeyDefine
{
    // key的类型
    public TableStringKeyType KeyType;

    // 如果key为DATA_IN_INDEX类型，存储其定义
    public DataInIndexDefine DataInIndexDefine;
}

/// <summary>
/// tableString代表的table中对value的定义描述
/// </summary>
public struct TableStringValueDefine
{
    // value的类型
    public TableStringValueType ValueType;

    // 如果value为DATA_IN_INDEX类型，存储其定义
    public DataInIndexDefine DataInIndexDefine;

    // 如果value为TABLE类型，则需要存储table中每个元素的定义
    public List<TableElementDefine> TableValueDefineList;
}

/// <summary>
/// 将tableString定义的字符串转为的格式定义
/// </summary>
public struct TableStringFormatDefine
{
    public TableStringKeyDefine KeyDefine;
    public TableStringValueDefine ValueDefine;
}