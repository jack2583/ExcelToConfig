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
         * 公共参数格式：PublicSetting(IsIncludeSubfolder=true|IsAllowedNullNumber=true|IsNeedCheck=false)
         * 公共参数格式：MergeTable()
         * 公共参数格式：MergeTable(IsMerge=true|IsMergeSingle=false|item=item100,item101|monster=monster301,monster306)
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

         * AppLog参数格式：AppLog(IsPrintLog=true|IsPrintLogWarning=true|IsPrintLogError=true|IsSaveLog=true|IsSaveLogWarning=true|IsSaveLogError=true)
         */

        private static void Main(string[] args)
        {
            LoadResourceDll.RegistDLL();
            try
            {
                //if(!StringModule.GetDotNetVersion("4.0"))
                //{
                //    AppLog.LogWarning("警告：你可能未安装framework4.0，请下载安装：\n"+"https://microsoft-net-framework-3-0.updatestar.com", ConsoleColor.Yellow);
                //}

                AppLog.Log("检查传入参数...", ConsoleColor.White);
                if (args.Length < 1)
                    AppLog.LogErrorAndExit("错误：未输入Excel表格所在目录,第1个参数必须为Excel所在目录");
                if (!Directory.Exists(args[0]))
                    AppLog.LogErrorAndExit(string.Format("错误：输入的Excel表格所在目录不存在，路径为{0}", args[0]));

                ExcelFolder.ExcelPath = Path.GetFullPath(args[0]);

                //从第2个参数起，检查可选参数
                for (int i = 1; i < args.Length; ++i)
                {
                    BatParamInfo batParamInfo = new BatParamInfo(args[i], i);
                    AppValues.BatParamInfo.Add(batParamInfo.ParamName, batParamInfo);
                }

                AppLog.GetParamValue();
                AppLang.ReadLangData();
                AppConfig.ReadConfig();

                AppLanguage.GetParamValue();
                ExcelFolder.GetExportTables();

                
                AppLanguage.GetLanguageDictData();


                //解析Excel表格并替换翻译
                TableAnalyzeHelper.AnalyzeAllTable(ExcelFolder.ExportTables);

                //多语言下，创建多语言集合，即将简体文本提出取来待翻译
                AppLanguage.CreateLanguageDictFile();

                //合并表格
                BatExportMergeSetting batExportMergeSetting = new BatExportMergeSetting();
                AppValues.MergeTableList = batExportMergeSetting.MergeTableList;


                //检查表格
                AppLog.Log("开始检查表格...");
                CheckTableInfo.CheckTable();

                //开始导出 AppValues.TableInfo

                ExportTxt.ExportToTxt();
                ExportJson.ExportToJson();
                ExportErlang.ExportToErlang();
                ExportServerJson.ExportToJson();
                ExportLua.ExportToLua();
                ExportMySQL.ExportToMySQL();
                


                AppLog.Log("\n全部文件导出成功");
#if DEBUG
                Console.ReadKey();
#endif
            }
            catch (Exception e)
            {

                if (AppValues.App_Config_Error == true)
                    AppLog.LogErrorAndExit(e.ToString());
                else
                {
#if DEBUG
                    AppLog.LogErrorAndExit(e.ToString());
#endif
                    //  AppLog.LogErrorAndExit("\n出现错误，请检查配置表");
                }

            }
        }
    }
}