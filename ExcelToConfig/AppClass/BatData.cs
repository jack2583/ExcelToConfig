using System;
using System.Collections.Generic;

/// <summary>
/// "MergeTable(IsMerge=true|item=item100,item101)"
/// </summary>
public class BatParamInfo
{
    public BatParamInfo(string arg,int i)
    {
        Arg = arg;
        SetChildParam(i);
    }
   public string Arg { get; set; }
    /// <summary>
    /// 左括号(前面的内容，如：MergeTable
    /// </summary>
    public string ParamName { get; set; }
    public char ParamNameLeft { get; set; } = '(';
    public char ParamNameRight { get; set; } = ')';
    public char ChildParamSplit { get; set; } = '|';
    public char ChildParamValueSplit { get; set; } = '=';
    public char ChildParamListSplit { get; set; } = ',';

    public Dictionary<string, BatChildParam> ChildParam = new Dictionary<string, BatChildParam>();
    
    //void SetParamData()
    //{
    //   // SetParamName();
    //    SetChildParam();
    //}

    private void SetParamName()
    {
        if (Arg == null)
            return;

        if (Arg.Length >= 2)//形式如：a(b)  最少是4个字符
        {
            if (Arg.StartsWith("“") || Arg.StartsWith("”"))
            {
                Arg = Arg.Substring(1);
            }
            if (Arg.EndsWith("“") || Arg.EndsWith("”"))
            {
                Arg = Arg.Substring(0, Arg.Length - 1);
            }
            int leftBracketIndex = Arg.IndexOf(ParamNameLeft);
            // int rightBracketIndex = paramString.LastIndexOf(')');

            ParamName = Arg.Substring(0, leftBracketIndex);
        }
        else
            ParamName = null;
    }
    private void SetChildParam(int i)
    {
        if (Arg == null)
            return;

        AppLog.Log(string.Format("检查参数：{0}", Arg),ConsoleColor.Green);

        if (Arg.Length >= 2)//形式如：a(b)  最少是4个字符
        {
            if (Arg.StartsWith("“") || Arg.StartsWith("”"))
            {
                Arg = Arg.Substring(1);
            }
            if (Arg.EndsWith("“") || Arg.EndsWith("”"))
            {
                Arg = Arg.Substring(0, Arg.Length - 1);
            }
            int leftBracketIndex = Arg.IndexOf(ParamNameLeft);
            // int rightBracketIndex = paramString.LastIndexOf(')');

            ParamName = Arg.Substring(0, leftBracketIndex);
        }
        else
        {
            ParamName = null;
            AppLog.LogErrorAndExit(string.Format("第{0}个参数错误，{1}", i, Arg));

        }


        if (Arg.Length >= 4)//形式如：a(b)  最少是4个字符
        {
            int leftBracketIndex = Arg.IndexOf(ParamNameLeft);
            int rightBracketIndex = Arg.LastIndexOf(ParamNameRight);
            if (rightBracketIndex == -1)
            {
                AppLog.LogErrorAndExit(string.Format("参数{0}错误，应形如：类型(参数1=值1)", Arg));
            }

            string paramStringAll = Arg.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);

            // 通过|分隔各个参数，但因为用户设置的TXT文件中的字段分隔符本身可能为|，本工具采用\|配置进行转义，故需要自行从头遍历查找真正的参数分隔符
            // 记录参数分隔符的下标位置
            List<int> splitParamCharIndex = new List<int>();

            for (int index = 0; index < paramStringAll.Length; ++index)
            {
                char c = paramStringAll[index];
                if (c == ChildParamSplit && (index < 1 || (index > 1 && paramStringAll[index - 1] != '\\')))
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


            string[] param = null;
            foreach (string s in paramStringList)
            {
                if (!s.Contains(ChildParamValueSplit.ToString()))
                    AppLog.LogErrorAndExit(string.Format("错误：BAT参数{0}\n中的{1}设置有错误，应该是形如：参数1=值1", Arg, s));

                param = s.Split(new char[] { ChildParamValueSplit }, StringSplitOptions.RemoveEmptyEntries);
                //是否需要判断param[0], param[1]不为空？？？

                if (param.Length < 2)
                    AppLog.LogErrorAndExit(string.Format("错误：BAT参数{0}\n中的{1}设置有错误，应该是形如：参数1=值1", Arg, s));

                BatChildParam batChildParam = new BatChildParam(param[1].Trim(), ChildParamListSplit);

                if (!ChildParam.ContainsKey(param[0].Trim()))
                {
                    ChildParam.Add(param[0].Trim(), batChildParam);
                }
                else
                    AppLog.LogErrorAndExit(string.Format("bat参数错误,存在2个以上同名子参数，{0}", Arg));
            }
        }
        else
            AppLog.LogErrorAndExit(string.Format("bat参数错误,应该为形式如：a(b)  最少是4个字符，{0}", Arg));

        AppLog.Log("检查完成，正确");

    }
}
public class BatChildParam
{
    public BatChildParam()
    { }
    public BatChildParam(string param, char SplitChar)
    {
        if (param == null)
            return;
        if (param.Length == 0)
            return;
        ChildParamValue = param;

        if (param.Contains(new string(new char[] { SplitChar })))
        {
            ChildParamValueList = param.Split(new char[] { SplitChar }, StringSplitOptions.RemoveEmptyEntries);
        }
        else
            ChildParamValueList = new string[] { param };

    }
    /// <summary>
    /// true或item100,item101
    /// </summary>
    public string ChildParamValue { get; set; }
    /// <summary>
    /// item100,item101
    /// </summary>
    public string[] ChildParamValueList { get; set; }
}
public class BatData
{
    /// <summary>
    /// 获取Bat参数的名称
    /// </summary>
    /// <param name="paramString">bat参数字符</param>
    /// <param name="errorString"></param>
    /// <returns></returns>
    public static string GetParamName(string paramString, out string errorString)
    {
        string paramName = "paramNameError";
        if (paramString.Length < 4)
        {
            errorString = "错误：声明导出格式必须为：在ParamName后面英文小括号内声明各个具体参数，\n如：ExportLua(IsExport=ture|ExportPath=Save/Lua \n或 ExceptExportExcel(item|skill)";
            return paramName;
        }
        if (paramString.StartsWith("“") || paramString.StartsWith("”"))
        {
            paramString = paramString.Substring(1);
        }
        if (paramString.EndsWith("“") || paramString.EndsWith("”"))
        {
            paramString = paramString.Substring(0, paramString.Length - 1);
        }
        int leftBracketIndex = paramString.IndexOf('(');
        // int rightBracketIndex = paramString.LastIndexOf(')');
        if (leftBracketIndex == -1)
        {
            errorString = string.Format("参数{0}错误，应形如：类型(参数1=值1)", paramString);
            return null;
        }
        else
        {
            paramName = paramString.Substring(0, leftBracketIndex);
            errorString = null;
            return paramName;
        }

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
    public static Dictionary<string, string> GetParam2(List<string> param)
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
