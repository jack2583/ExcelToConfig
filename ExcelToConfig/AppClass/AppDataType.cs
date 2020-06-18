/// <summary>
/// 本工具所的数据类型的定义
/// </summary>
public enum DataType
{
    /// <summary>
    /// 无效类型
    /// </summary>
    Invalid,

    Int,
    Long,
    Float,
    Bool,
    String,
    Lang,
    Date,
    Time,
    Json,
    TableString,
    Array,
    Dict,
    MapString,
}
/// <summary>
/// 本工具所支持的导出类型定义
/// </summary>
public enum ExportType
{
    /// <summary>
    /// 无效类型
    /// </summary>
    Invalid,

    txt,
    lua,
    lua2,
    lua3,
    lua4,
    lua5,
    json,
    json2,
    json3,
    json4,
    json5,
    erlang,
    MySql,
    Sqlite,

}