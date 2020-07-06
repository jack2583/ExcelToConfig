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
    Txt,
    Lua,
    Lua2,
    Lua3,
    Lua4,
    Lua5,
    Json,
    Json2,
    Json3,
    Json4,
    Json5,
    Erlang,
    MySql,
    Sqlite,

}