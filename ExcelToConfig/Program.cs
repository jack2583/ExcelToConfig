using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExcelToConfig
{
    internal class Program
    {
        //第1个参数，Excel所在路径格式：Excel
        //第2个以后参数可选参数
        /*
         * 公共参数格式：PublicSetting(是否包含子目录=true|是否充许数字型为空=true|是否需要检查=false|ClientPath=)
         * 公共参数格式：PublicSetting(IsIncludeSubfolder=true|IsAllowedNullNumber=true|IsNeedCheck=false|IsCopy=false|CopyBatName=copy.bat)
         * 多语言：MoreLanguage(IsMoreLanguage=false|NeedLanguage=_ft|OtherLanguage=_yn,_English|IsAddSaveType=false|IsGetSourceTextFile=false)
         * lang参数：Lang(IsLang=true|LangPath=lang.txt|IsLangNull=false)
         * config参数：Config(ConfigPat=config.txt)
         * 只导出部分表格式（优先判定）：OnlyExportPartExcel(item|skill)
         * 指定不导出哪些表格式（后判定）：ExceptExportExcel(item|skill)
         * 导出Lua参数格式：ExportLua(是否导出Lua=true|导出路径=Save/Lua|是否按原目录结构保存=ture|是否需要生成头部信息|数组类型是否需要[1]=值格式|导出文件内容是否以文件名开头|导出时在文件名前添加前缀)
         * 导出Lua参数格式：ExportLua(IsExport=true|ExportPath=Save/Lua|IsExportKeepDirectoryStructure=ture|IsNeedColumnInfo=ture|IsArrayFieldName=true|IsTableNameStart=false|IsFormat=true|ExportNameBeforeAdd=cfg_)
         * 导出json参数格式：ExportJson(是否导出Lua=true|导出路径=Save/Lua|是否按原目录结构保存=true|包含在一个json array的形式=false|是否包含主键列对应的键值对=true|是否格式化=true)
         * 导出json参数格式：ExportJson(IsExport=true|ExportPath=Save/Json|IsExportKeepDirectoryStructure=true|IsArrayFormat=false|IsKeyColumnValue=true|IsFormat=true|ExportNameBeforeAdd=tb_)
         * 导出txt参数格式：ExportTxt(IsExport=true|ExportPath=Save/Txt|IsExportKeepDirectoryStructure=true|ExportNameBeforeAdd=tb_)
         * 导出mysql参数格式：ExportMySQL(IsExport=true|Server=127.0.0.1|Port=3306|Uid=root|Password=root|Database=mydb|Charset=utf8)
         * 导出sqlite参数格式：
         * 导出erlang参数格式：ExportErlang(IsExport=true|ExportPath=Save/Erl|IsExportKeepDirectoryStructure=true|IsFormat=true|ExportNameBeforeAdd=tb_)
         * 导出hrl参数格式：ExportHrl(IsExport=true|ExportPath=Save/hrl|IsExportKeepDirectoryStructure=true|ExportNameBeforeAdd=tb_)
         * 导出luafile参数格式：ExportLuaFile(IsExport=true|ExportPath=Save/luafile|IsExportKeepDirectoryStructure=true)
         * 导出sqvn参数格式：Svn(SvnPath=|IsUpdateSvn=false|UpdateSvnCloseonend=2|IsCommitSvn=false|CommitSvnCloseonend=0)
         * AppLog参数格式：AppLog(IsPrintLog=true|IsPrintLogWarning=true|IsPrintLogError=true|IsSaveLog=true|IsSaveLogWarning=true|IsSaveLogError=true)
         */

        private static void Main(string[] args)
        {
            try
            {
                /*↓↓↓↓↓↓↓↓↓读取参数↓↓↓↓↓↓↓↓↓*/
                //AppLog.Log("检查传入参数...", ConsoleColor.White);
                if (args.Length < 1)
                    AppLog.LogErrorAndExit("错误：未输入Excel表格所在目录,第1个参数必须为Excel所在目录");
                if (!Directory.Exists(args[0]))
                    AppLog.LogErrorAndExit(string.Format("错误：输入的Excel表格所在目录不存在，路径为{0}", args[0]));

                ExcelFolder.ExcelFolderPath = Path.GetFullPath(args[0]);

                string paramName = null;
                List<string> param = new List<string>();
                Dictionary<string, string> param2 = new Dictionary<string, string>();
                string errorString = null;
                //从第2个参数起，检查可选参数
                for (int i = 1; i < args.Length; ++i)
                {
                    paramName = BatData.GetParamName(args[i], out errorString);
                    if (errorString != null)
                    {
                        AppLog.LogError(string.Format("第{0}个参数错误，{1}", i, errorString));
                    }
                    param = BatData.GetParam(args[i]);
                    param2 = BatData.GetParam2(param);
                    //公共通用设置
                    if (paramName == AppValues.Public_Config_PublicSetting)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == AppValues.Public_Config_IsIncludeSubfolder)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    ExcelFolder.IsIncludeSubfolder = true;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    ExcelFolder.IsIncludeSubfolder = false;
                                }

                                continue;
                            }
                            else if (kvp.Key == AppValues.Public_Config_IsAllowedNullNumber)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    // 允许int、float型字段中存在空值
                                    CheckStruct.IsAllowedNullNumber = true;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    CheckStruct.IsAllowedNullNumber = false;
                                }

                                continue;
                            }
                            else if (kvp.Key == AppValues.Public_Config_IsNeedCheck)
                            {
                                if (kvp.Value.ToLower() == "false")
                                {
                                    CheckStruct.IsNeedCheck = false;
                                }
                                else if (kvp.Value.ToLower() == "true")
                                {
                                    CheckStruct.IsNeedCheck = true;
                                }

                                continue;
                            }
                            else if (kvp.Key == AppValues.Public_Config_IsCopy)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    AppValues.App_Config_IsCopy = true;
                                    continue;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    AppValues.App_Config_IsCopy = false;
                                    break;
                                }
                            }
                            else if (kvp.Key == AppValues.Public_Config_CopyBatName)
                            {
                                AppValues.App_Config_CopyBatName = kvp.Value;
                                continue;
                            }
                            else if (kvp.Key == AppValues.Public_Config_ClientPath)
                            {
                                AppValues.App_Config_ClientPath = Path.GetFullPath(kvp.Value);
                                continue;
                            }
                            else if (kvp.Key == AppValues.Public_Config_ReadExcelType)
                            {
                                string kvptemp= kvp.Value;
                                if(kvptemp== "ExcelDataReader")
                                    AppValues.App_Config_ReadExcelType = kvptemp;
                                if (kvptemp == "OleDb")
                                    AppValues.App_Config_ReadExcelType = kvptemp;
                                continue;
                            }
                            else if (kvp.Key == AppValues.Public_Config_MergeTable)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    AppValues.App_Config_MergeTable = true;
                                    continue;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    AppValues.App_Config_MergeTable = false;
                                    break;
                                }
                            }
                        }
                    }
                    //公共lang设置
                    if (paramName == AppLang.Public_Config_Lang)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == AppLang.Public_Config_IsLang)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    AppLang.IsLang = true;
                                    continue;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    AppLang.IsLang = false;
                                    break;
                                }
                            }
                            else if (kvp.Key == AppLang.Public_Config_LangPath)
                            {
                                AppLang.LangPath = Path.GetFullPath(kvp.Value);
                                continue;
                            }
                            else if (kvp.Key == AppLang.Public_Config_IsLangNull)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    AppLang.IsLangNull = true;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    AppLang.IsLangNull = false;
                                }

                                continue;
                            }
                        }
                    }
                    //公共Config设置
                    if (paramName == AppValues.Public_Config_Config)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == AppValues.Public_Config_ConfigPath)
                            {
                                AppValues.ConfigPath = Path.GetFullPath(kvp.Value);
                                continue;
                            }
                        }
                    }
                    //多语言设置
                    if (paramName == AppLanguage.Public_Config_MoreLanguage)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == AppLanguage.Public_Config_IsMoreLanguage)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    AppLanguage.IsMoreLanguage = true;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    AppLanguage.IsMoreLanguage = false;
                                }
                            }
                            else if (kvp.Key == AppLanguage.Public_Config_IsGetSourceTextFile)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    AppLanguage.IsGetSourceTextFile = true;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    AppLanguage.IsGetSourceTextFile = false;
                                }
                            }
                            else if (kvp.Key == AppLanguage.Public_Config_NeedLanguage)
                            {
                                if (kvp.Value.ToLower() != "null")
                                    AppLanguage.NeedLanguage = kvp.Value;
                            }
                            else if (kvp.Key == AppLanguage.Public_Config_OtherLanguage)
                            {
                                if (kvp.Value.ToLower() != "null")
                                    AppLanguage.OtherLanguage = BatData.GetParam3(kvp.Value);
                            }
                            else if (kvp.Key == AppLanguage.Public_Config_IsAddSaveType)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    AppLanguage.IsAddSaveType = true;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    AppLanguage.IsAddSaveType = false;
                                }
                            }
                            //else if (kvp.Key == AppLanguage.Public_Config_LanguageDictPath)
                            //{
                            //   AppLanguage.LanguageDictPath= Path.GetFullPath(kvp.Value);
                            //}
                        }
                    }
                    //公共Lua设置
                    else if (paramName == LuaStruct.Public_Config_Export)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == LuaStruct.Public_Config_IsExport)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    LuaStruct.IsExport = true;
                                    continue;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    LuaStruct.IsExport = false;
                                    continue;
                                }
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_ExportPath)
                            {
                                LuaStruct.SavePath = Path.GetFullPath(kvp.Value);
                                continue;
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_IsExportKeepDirectoryStructure)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    LuaStruct.IsExportKeepDirectoryStructure = false;
                                else if (kvp.Value.ToLower() == "true")
                                    LuaStruct.IsExportKeepDirectoryStructure = true;

                                continue;
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_IsNeedColumnInfo)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    LuaStruct.IsNeedColumnInfo = true;
                                else if (kvp.Value.ToLower() == "false")
                                    LuaStruct.IsNeedColumnInfo = false;

                                continue;
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_ExportNameBeforeAdd)
                            {
                                LuaStruct.ExportNameBeforeAdd = kvp.Value;
                                continue;
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_IsArrayFieldName)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    LuaStruct.IsArrayFieldName = true;
                                else if (kvp.Value.ToLower() == "false")
                                    LuaStruct.IsArrayFieldName = false;

                                continue;
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_IsTableNameStart)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    LuaStruct.IsTableNameStart = true;
                                else if (kvp.Value.ToLower() == "false")
                                    LuaStruct.IsTableNameStart = false;

                                continue;
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_IsFormat)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    LuaStruct.ExportLuaIsFormat = true;
                                else if (kvp.Value.ToLower() == "false")
                                    LuaStruct.ExportLuaIsFormat = false;
                            }
                            else if (kvp.Key == LuaStruct.Public_Config_NotExportLuaNil)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    LuaStruct.IsExportLuaNilConfig = true;
                                else if (kvp.Value.ToLower() == "false")
                                    LuaStruct.IsExportLuaNilConfig = false;
                            }
                        }
                    }
                    //公共Json设置
                    else if (paramName == JsonStruct.Public_Config_Export)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == JsonStruct.Public_Config_IsExport)
                            {
                                if (kvp.Value.ToLower() == "true")
                                {
                                    JsonStruct.IsExport = true;
                                    continue;
                                }
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    JsonStruct.IsExport = false;
                                    continue;
                                }
                            }
                            else if (kvp.Key == JsonStruct.Public_Config_ExportPath)
                            {
                                JsonStruct.SavePath = Path.GetFullPath(kvp.Value);
                            }
                            else if (kvp.Key == JsonStruct.Public_Config_IsExportKeepDirectoryStructure)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    JsonStruct.IsExportKeepDirectoryStructure = false;
                                else if (kvp.Value.ToLower() == "true")
                                    JsonStruct.IsExportKeepDirectoryStructure = true;
                            }
                            else if (kvp.Key == JsonStruct.Public_Config_ExportNameBeforeAdd)
                            {
                                JsonStruct.ExportNameBeforeAdd = kvp.Value;
                                continue;
                            }
                            else if (kvp.Key == JsonStruct.Public_Config_IsArrayFormat)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    JsonStruct.ExportJsonIsExportJsonArrayFormat = true;
                                else if (kvp.Value.ToLower() == "false")
                                    JsonStruct.ExportJsonIsExportJsonArrayFormat = false;
                            }
                            else if (kvp.Key == JsonStruct.Public_Config_IsKeyColumnValue)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    JsonStruct.ExportJsonIsExportJsonMapIncludeKeyColumnValue = false;
                                else if (kvp.Value.ToLower() == "true")
                                    JsonStruct.ExportJsonIsExportJsonMapIncludeKeyColumnValue = true;
                            }
                            else if (kvp.Key == JsonStruct.Public_Config_IsFormat)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    JsonStruct.ExportJsonIsFormat = false;
                                else if (kvp.Value.ToLower() == "true")
                                    JsonStruct.ExportJsonIsFormat = true;
                            }
                            else if (kvp.Key == JsonStruct.Public_Config_NotExportJsonNull)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    JsonStruct.IsExportJsonNullConfig = true;
                                else if (kvp.Value.ToLower() == "false")
                                    JsonStruct.IsExportJsonNullConfig = false;
                            }
                        }
                    }
                    //公共Erlang设置
                    else if (paramName == ErlangStruct.Public_Config_Export)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == ErlangStruct.Public_Config_IsExport)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    ErlangStruct.IsExport = true;
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    ErlangStruct.IsExport = false;
                                    continue;
                                }
                            }
                            else if (kvp.Key == ErlangStruct.Public_Config_ExportPath)
                            {
                                ErlangStruct.SavePath = Path.GetFullPath(kvp.Value);
                            }
                            else if (kvp.Key == ErlangStruct.Public_Config_IsExportKeepDirectoryStructure)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    ErlangStruct.IsExportKeepDirectoryStructure = false;
                                else if (kvp.Value.ToLower() == "true")
                                    ErlangStruct.IsExportKeepDirectoryStructure = true;
                            }
                            else if (kvp.Key == ErlangStruct.Public_Config_ExportNameBeforeAdd)
                            {
                                ErlangStruct.ExportNameBeforeAdd = kvp.Value;
                                continue;
                            }
                            else if (kvp.Key == ErlangStruct.Public_Config_IsFormat)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    ErlangStruct.ExportErlangIsFormat = true;
                                else if (kvp.Value.ToLower() == "false")
                                    ErlangStruct.ExportErlangIsFormat = false;
                            }
                            else if (kvp.Key == ErlangStruct.Public_Config_NotExportErlangNull)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    ErlangStruct.IsExportErlangNullConfig = true;
                                else if (kvp.Value.ToLower() == "false")
                                    ErlangStruct.IsExportErlangNullConfig = false;
                            }
                        }
                    }
                    //公共hrl设置
                    else if (paramName == HrlStruct.Public_Config_Export)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == HrlStruct.Public_Config_IsExport)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    HrlStruct.IsExport = true;
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    HrlStruct.IsExport = false;
                                    continue;
                                }
                            }
                            else if (kvp.Key == HrlStruct.Public_Config_ExportPath)
                            {
                                HrlStruct.SavePath = Path.GetFullPath(kvp.Value);
                            }
                            else if (kvp.Key == HrlStruct.Public_Config_IsExportKeepDirectoryStructure)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    HrlStruct.IsExportKeepDirectoryStructure = false;
                                else if (kvp.Value.ToLower() == "true")
                                    HrlStruct.IsExportKeepDirectoryStructure = true;
                            }
                            else if (kvp.Key == HrlStruct.Public_Config_ExportNameBeforeAdd)
                            {
                                HrlStruct.ExportNameBeforeAdd = kvp.Value;
                                continue;
                            }
                        }
                    }
                    //公共Txt设置
                    else if (paramName == TxtStruct.Public_Config_Export)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == TxtStruct.Public_Config_IsExport)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    TxtStruct.IsExport = true;
                                else if (kvp.Value.ToLower() == "false")
                                {
                                    TxtStruct.IsExport = false;
                                    continue;
                                }
                            }
                            else if (kvp.Key == TxtStruct.Public_Config_ExportPath)
                            {
                                TxtStruct.SavePath = Path.GetFullPath(kvp.Value);
                            }
                            else if (kvp.Key == TxtStruct.Public_Config_IsExportKeepDirectoryStructure)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    TxtStruct.IsExportKeepDirectoryStructure = false;
                                else if (kvp.Value.ToLower() == "true")
                                    TxtStruct.IsExportKeepDirectoryStructure = true;
                            }
                            else if (kvp.Key == TxtStruct.Public_Config_ExportNameBeforeAdd)
                            {
                                TxtStruct.ExportNameBeforeAdd = kvp.Value;
                                continue;
                            }
                        }
                    }
                    //公共LuaFile设置
                    else if (paramName == LuaFileStruct.Public_Config_Export)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == LuaFileStruct.Public_Config_IsExport)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    LuaFileStruct.IsExport = true;
                                else
                                    continue;
                            }
                            else if (kvp.Key == LuaFileStruct.Public_Config_ExportPath)
                            {
                                LuaFileStruct.SavePath = Path.GetFullPath(kvp.Value);
                            }
                            else if (kvp.Key == LuaFileStruct.Public_Config_IsExportKeepDirectoryStructure)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    LuaFileStruct.IsExportKeepDirectoryStructure = false;
                            }
                        }
                    }
                    //公共Svn设置
                    else if (paramName == SvnStruct.Public_Config_Svn)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == SvnStruct.Public_Config_SvnPath)
                            {
                                SvnStruct.SvnPath = Path.GetFullPath(kvp.Value);
                            }
                            else if (kvp.Key == SvnStruct.Public_Config_IsUpdateSvn)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    SvnStruct.IsUpdateSvn = false;
                                else
                                    continue;
                            }
                            else if (kvp.Key == SvnStruct.Public_Config_UpdateSvnCloseonend)
                            {
                                SvnStruct.UpdateSvnCloseonend = int.Parse(kvp.Value);
                            }
                            else if (kvp.Key == SvnStruct.Public_Config_IsCommitSvn)
                            {
                                if (kvp.Value.ToLower() == "false")
                                    SvnStruct.IsCommitSvn = false;
                                else
                                    continue;
                            }
                            else if (kvp.Key == SvnStruct.Public_Config_CommitSvnCloseonend)
                            {
                                SvnStruct.CommitSvnCloseonend = int.Parse(kvp.Value);
                            }
                        }
                    }
                    //AppLog设置
                    else if (paramName == AppLog.Public_Config_AppLog)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == AppLog.Public_Config_IsPrintLog)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    AppLog.IsPrintLog = true;
                                else if (kvp.Value.ToLower() == "false")
                                    AppLog.IsPrintLog = false;
                            }
                            else if (kvp.Key == AppLog.Public_Config_IsPrintLogWarning)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    AppLog.IsPrintLogWarning = true;
                                else if (kvp.Value.ToLower() == "false")
                                    AppLog.IsPrintLogWarning = false;
                            }
                            else if (kvp.Key == AppLog.Public_Config_IsPrintLogError)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    AppLog.IsPrintLogError = true;
                                else if (kvp.Value.ToLower() == "false")
                                    AppLog.IsPrintLogError = false;
                            }
                            else if (kvp.Key == AppLog.Public_Config_IsSaveLog)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    AppLog.IsSaveLog = true;
                                else if (kvp.Value.ToLower() == "false")
                                    AppLog.IsSaveLog = false;
                            }
                            else if (kvp.Key == AppLog.Public_Config_IsSaveLogWarning)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    AppLog.IsSaveLogWarning = true;
                                else if (kvp.Value.ToLower() == "false")
                                    AppLog.IsSaveLogWarning = false;
                            }
                            else if (kvp.Key == AppLog.Public_Config_IsSaveLogError)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    AppLog.IsSaveLogError = true;
                                else if (kvp.Value.ToLower() == "false")
                                    AppLog.IsSaveLogError = false;
                            }
                        }
                    }
                    //公共MySQL设置
                    else if (paramName == MySQLStruct.Public_Config_Export)
                    {
                        foreach (KeyValuePair<string, string> kvp in param2)
                        {
                            if (kvp.Key == MySQLStruct.Public_Config_IsExport)
                            {
                                if (kvp.Value.ToLower() == "true")
                                    MySQLStruct.IsExport = true;
                                else
                                {
                                    MySQLStruct.IsExport = false;
                                    break;
                                }
                            }
                            else if (kvp.Key == MySQLStruct.Public_Config_Server)
                            {
                                MySQLStruct.Server = kvp.Value;
                            }
                            else if (kvp.Key == MySQLStruct.Public_Config_Port)
                            {
                                MySQLStruct.Port = kvp.Value;
                            }
                            else if (kvp.Key == MySQLStruct.Public_Config_Uid)
                            {
                                MySQLStruct.Uid = kvp.Value;
                            }
                            else if (kvp.Key == MySQLStruct.Public_Config_PassWord)
                            {
                                MySQLStruct.PassWord = kvp.Value;
                            }
                            else if (kvp.Key == MySQLStruct.Public_Config_DataBase)
                            {
                                MySQLStruct.DataBase = kvp.Value;
                            }
                            else if (kvp.Key == MySQLStruct.Public_Config_Charset)
                            {
                                MySQLStruct.Charset = kvp.Value;
                            }
                        }
                    }
                }

                /*↑↑↑↑↑↑↑↑↑读取参数↑↑↑↑↑↑↑↑↑*/

                /*↓↓↓↓↓↓↓↓↓公共参数设定↓↓↓↓↓↓↓↓↓*/

                //判断是否更新SVN
                if (SvnStruct.IsUpdateSvn)
                    SVN.UpdateSvnDirectory(SvnStruct.SvnPath, SvnStruct.UpdateSvnCloseonend);

                //解析Config。txt数据
                if (File.Exists(AppValues.ConfigPath))
                {
                    errorString = null;
                    AppValues.ConfigData = TxtConfigReader.ParseTxtConfigFile(AppValues.ConfigPath, ":", out errorString);
                    if (!string.IsNullOrEmpty(errorString))
                        AppLog.LogErrorAndExit(errorString);
                }
                else
                    AppLog.LogWarning(string.Format("警告：找不到本工具所在路径下的{0}配置文件，请确定是否真的不需要自定义配置", AppValues.ConfigPath));

                // 读取部分配置项并进行检查
                const string ERROR_STRING_FORMAT = "配置项\"{0}\"所设置的值\"{1}\"非法：{2}\n";
                StringBuilder errorStringBuilder = new StringBuilder();
                string tempErrorString = null;
                if (AppValues.ConfigData.ContainsKey(DateTimeValue.APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT))
                {
                    DateTimeValue.DefaultDateInputFormat = AppValues.ConfigData[DateTimeValue.APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT].Trim();
                    if (TableCheckHelper.CheckDateInputDefine(DateTimeValue.DefaultDateInputFormat, out tempErrorString) == false)
                        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, DateTimeValue.APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT, DateTimeValue.DefaultDateInputFormat, tempErrorString);
                }
                if (AppValues.ConfigData.ContainsKey(LuaStruct.DefaultDateToExportFormatKey))
                {
                    LuaStruct.DefaultDateToExportFormat = AppValues.ConfigData[LuaStruct.DefaultDateToExportFormatKey].Trim();
                    if (TableCheckHelper.CheckDateToLuaDefine(LuaStruct.DefaultDateToExportFormat, out tempErrorString) == false)
                        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, LuaStruct.DefaultDateToExportFormatKey, LuaStruct.DefaultDateToExportFormat, tempErrorString);
                }
                if (AppValues.ConfigData.ContainsKey(MySQLStruct.DefaultDateToExportFormatKey))
                {
                    MySQLStruct.DefaultDateToExportFormat = AppValues.ConfigData[MySQLStruct.DefaultDateToExportFormatKey].Trim();
                    if (TableCheckHelper.CheckDateToDatabaseDefine(MySQLStruct.DefaultDateToExportFormat, out tempErrorString) == false)
                        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, MySQLStruct.DefaultDateToExportFormatKey, MySQLStruct.DefaultDateToExportFormat, tempErrorString);
                }
                if (AppValues.ConfigData.ContainsKey(DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT))
                {
                    DateTimeValue.DefaultTimeInputFormat = AppValues.ConfigData[DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT].Trim();
                    if (TableCheckHelper.CheckTimeDefine(DateTimeValue.DefaultTimeInputFormat, out tempErrorString) == false)
                        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT, DateTimeValue.DefaultTimeInputFormat, tempErrorString);
                }
                if (AppValues.ConfigData.ContainsKey(LuaStruct.DefaultTimeToExportFormatKey))
                {
                    LuaStruct.DefaultTimeToExportFormat = AppValues.ConfigData[LuaStruct.DefaultTimeToExportFormatKey].Trim();
                    if (TableCheckHelper.CheckTimeDefine(LuaStruct.DefaultTimeToExportFormat, out tempErrorString) == false)
                        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, LuaStruct.DefaultTimeToExportFormatKey, LuaStruct.DefaultTimeToExportFormat, tempErrorString);
                }
                if (AppValues.ConfigData.ContainsKey(MySQLStruct.DefaultTimeToExportFormatKey))
                {
                    MySQLStruct.DefaultTimeToExportFormat = AppValues.ConfigData[MySQLStruct.DefaultTimeToExportFormatKey].Trim();
                    if (TableCheckHelper.CheckTimeDefine(MySQLStruct.DefaultTimeToExportFormat, out tempErrorString) == false)
                        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, MySQLStruct.DefaultTimeToExportFormatKey, MySQLStruct.DefaultTimeToExportFormat, tempErrorString);
                }

                string errorConfigString = errorStringBuilder.ToString();
                if (!string.IsNullOrEmpty(errorConfigString))
                {
                    errorConfigString = string.Concat("配置文件中存在以下错误，请修正后重试\n", errorConfigString);
                    AppLog.LogErrorAndExit(errorConfigString);
                }

                if (AppLang.IsLang)
                {
                    errorString = null;
                    AppLang.LangData = TxtConfigReader.ParseTxtConfigFile(AppLang.LangPath, ":", out errorString);
                    if (!string.IsNullOrEmpty(errorString))
                        AppLog.LogErrorAndExit(errorString);
                }

                AppLanguage.GetLanguageDictData();

                //判断指定的Excel文件是否存在
                if (!Directory.Exists(ExcelFolder.ExcelFolderPath))
                    AppLog.LogErrorAndExit(string.Format("错误!!! 输入的Excel表格所在目录不存在，路径为:{0}", ExcelFolder.ExcelFolderPath));

                ExcelFolder.ExcelFolderPath = Path.GetFullPath(ExcelFolder.ExcelFolderPath);
                AppLog.Log(string.Format("提示: 您选择的Excel所在路径：{0}", ExcelFolder.ExcelFolderPath));
                ExcelFolder.IsIncludeSubfolder = true;
                ExcelFolder.AllExcelPaths = ExcelFolder.getAllExcelPaths(ExcelFolder.ExcelFolderPath, "xlsx", ExcelFolder.IsIncludeSubfolder);
                ExcelFolder.AllExcelPaths = FileModule.RemoveTempFile(ExcelFolder.AllExcelPaths, AppLanguage.IsMoreLanguage, ExcelTableSetting.ExcelTempFileFileNameStartString);

                FileModule.CheckSameName(ExcelFolder.AllExcelPaths, "xlsx");

                ExcelFolder.ExportPart = null;
                ExcelFolder.ExportExcept = null;
                ExcelFolder.ExportTables = ExcelFolder.getExportTables(ExcelFolder.AllExcelPaths, ExcelFolder.ExportPart, ExcelFolder.ExportExcept);

                /*↑↑↑↑↑↑↑↑↑公共参数设定↑↑↑↑↑↑↑↑↑*/

                //检测Excel是否打开
                ReadExcelHelper.GetFileState(ExcelFolder.ExportTables);

                //解析Excel表格
                TableAnalyzeHelper.AnalyzeAllTable(ExcelFolder.ExportTables);

                AppLanguage.CreateLanguageDictFile();
                //检查表格
                if (CheckStruct.IsNeedCheck == true)
                {
                    foreach (KeyValuePair<string, TableInfo> kvp in AppValues.TableInfo)
                    {
                        TableInfo tableInfo = kvp.Value;
                        AppLog.Log(string.Format("检查表格\"{0}\"：", tableInfo.TableName), ConsoleColor.Green);
                        errorString = null;

                        TableCheckHelper.CheckTable(tableInfo, out errorString);
                        if (errorString != null)
                        {
                            AppLog.LogError(string.Format("{0}表格检查存在以下错误：\n{1}", tableInfo.TableName, errorString));
                        }
                        else
                            AppLog.Log(string.Format("{0}表格检查正确", tableInfo.TableName));
                    }

                    if (AppLog.LogErrorContent.Length > 0)
                    {
                        AppLog.SaveErrorInfoToFile("错误日志");
                        AppLog.LogErrorAndExit("\n按任意键继续");
                    }
                }

                //开始导出 AppValues.TableInfo
                foreach (KeyValuePair<string, TableInfo> kvp in AppValues.TableInfo)
                {
                    TableInfo tableInfo = kvp.Value;
                    AppLog.Log(string.Format("导出表格\"{0}\"：", tableInfo.TableName), ConsoleColor.Green);
                    errorString = null;
                    if (AppLanguage.IsMoreLanguage == true && AppLanguage.NeedLanguage != null)
                    {
                        if (!tableInfo.TableName.EndsWith(AppLanguage.NeedLanguage))
                        {
                            if (AppValues.TableInfo.ContainsKey(tableInfo.ExcelName))
                            {
                                AppLog.LogWarning(string.Format("已忽备导出{0},因为存在{1}的对应文件", tableInfo.ExcelName, AppLanguage.NeedLanguage));
                                continue;
                            }
                        }
                    }

                    ExportToLuaHelper.ExportToLua(tableInfo);

                    ExportToJsonHelper.ExportToJson(tableInfo);

                    ExportToTxtHelper.ExportToTxt(tableInfo);

                    ExportToErlangHelper.ExportToErlang(tableInfo);
                }
                ExportToMySQLHelper.ExportToMySQL();

                AppLog.Log("全部文件导出成功");
#if DEBUG
                Console.ReadKey();
#endif
            }
            catch (Exception e)
            {
#if DEBUG
                AppLog.LogErrorAndExit(e.ToString());
#else
                AppLog.LogErrorAndExit("出现错误，请检查配置表");
#endif
            }
        }
    }
}