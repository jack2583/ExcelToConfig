using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// mapString类型的配置信息
/// </summary>
public class MapStringInfo
{
    // 下属参数配置信息（key：参数变量名， value：此参数的配置信息）
    public Dictionary<string, MapStringParamInfo> ParamInfo { get; set; }

    public MapStringInfo()
    {
        ParamInfo = new Dictionary<string, MapStringParamInfo>();
    }
}

/// <summary>
/// mapString类型下属的参数配置信息
/// </summary>
public class MapStringParamInfo
{
    // 参数名
    public string ParamName { get; set; }
    // 参数的数据类型
    public DataType DataType { get; set; }
    // 如果此参数是mapString型，存储下属参数信息
    public MapStringInfo MapStringInfo { get; set; }
}
