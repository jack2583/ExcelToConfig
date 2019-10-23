using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BatData
{
    /// <summary>
    /// 获取Bat参数的名称
    /// </summary>
    /// <param name="paramString">bat参数字符</param>
    /// <param name="errorString"></param>
    /// <returns></returns>
    public static string GetParamName(string paramString,out string errorString)
    {
        string paramName = "paramNameError";
        if(paramString.Length<4)
        {
            errorString = "错误：声明导出格式必须为：在ParamName后面英文小括号内声明各个具体参数，\n如：ExportLua(IsExport=ture|ExportPath=Save/Lua \n或 ExceptExportExcel(item|skill)";
            return paramName;
        }
        if(paramString.StartsWith("“")|| paramString.StartsWith("”"))
        {
            paramString = paramString.Substring(1);
        }
        if (paramString.EndsWith("“") || paramString.EndsWith("”"))
        {
            paramString = paramString.Substring(0, paramString.Length-1);
        }
        int leftBracketIndex = paramString.IndexOf('(');
       // int rightBracketIndex = paramString.LastIndexOf(')');

        paramName = paramString.Substring(0, leftBracketIndex);
        errorString = null;
        return paramName;
    }
    /// <summary>
    /// 获取Bat参数的内容，如item|skill或IsExport=ture|ExportPath=Save/Lua
    /// </summary>
    /// <param name="paramString"></param>
    /// <returns></returns>
    public static List<string> GetParam(string paramString)
    {
        int leftBracketIndex = paramString.IndexOf('(');
        int rightBracketIndex = paramString.LastIndexOf(')');

        string paramStringAll = paramString.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);

        // 通过|分隔各个参数，但因为用户设置的TXT文件中的字段分隔符本身可能为|，本工具采用\|配置进行转义，故需要自行从头遍历查找真正的参数分隔符
        // 记录参数分隔符的下标位置
        List<int> splitParamCharIndex = new List<int>();

        for (int index = 0; index < paramStringAll.Length; ++index)
        {
            char c = paramStringAll[index];
            if (c == '|' && (index < 1 || (index > 1 && paramStringAll[index - 1] != '\\')))
                splitParamCharIndex.Add(index);
        }

        // 通过识别的参数分隔符，分隔各个参数
        List<string> paramStringList = new List<string>();
        int lastSplitParamChatIndex = -1;
        foreach (int index in splitParamCharIndex)
        {
            paramStringList.Add(paramStringAll.Substring(lastSplitParamChatIndex + 1, index - lastSplitParamChatIndex - 1));
            lastSplitParamChatIndex = index;
        }
        // 还要加上最后一个|后面的参数
        if (lastSplitParamChatIndex == -1)
            paramStringList.Add(paramStringAll);
        else if (lastSplitParamChatIndex + 1 < paramStringAll.Length - 1)
            paramStringList.Add(paramStringAll.Substring(lastSplitParamChatIndex + 1));

        return paramStringList;
    }
    /// <summary>
    /// 获取Bat参数的内容以=的key和value
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static Dictionary<string,string> GetParam2(List<string> param)
    {
        Dictionary<string, string> param2 = new Dictionary<string, string>();
       // bool isContains = false;
        string[] fileNames = null;
        foreach (string p in param)
        {
           // isContains = p.ToLower().Contains("=".ToLower());//true
            fileNames = p.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
             param2.Add(fileNames[0], fileNames[1]);
        }

         return param2;
    }
    /// <summary>
    /// 获取Bat参数的内容以指定分隔符的string[]集合
    /// </summary>
    /// <param name="parm"></param>
    /// <param name="splitStr"></param>
    /// <returns></returns>
    public static string[] GetParam3(string parm)
    {
        
        string[] parms = null;
        parms = parm.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        return parms;

    }
}

