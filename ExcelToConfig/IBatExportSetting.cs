using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 某个导出类型（json，elang等）BAT导出设置
/// </summary>
interface IBatExportSetting
{
    /// <summary>
    /// 参数名，如：ExportJson
    /// </summary>
    string ExportTypeParam { set; get; }
    /// <summary>
    /// 参数名：是否需要导出，如果为空则向上找对应参数，顺序为：IExcelConfigSetting设置>IBatExportSetting等设置>IBatExportPublicSetting公共设置
    /// </summary>
    string IsExportParam { set; get; }
    /// <summary>
    /// 参数值：是否需要导出
    /// </summary>
    bool IsExport { set; get; }

    /// <summary>
    /// 参数名：导出路径
    /// </summary>
    string ExportPathParam { set; get; }
    /// <summary>
    /// 参数值：导出路径
    /// </summary>
    string ExportPath { set; get; }

    /// <summary>
    /// 参数名：导出时是否按原目录结构保存
    /// </summary>
    string ExportKeepDirectoryStructureParam { set; get; }
    /// <summary>
    /// 参数值：导出时是否按原目录结构保存
    /// </summary>
    bool ExportKeepDirectoryStructure { set; get; }

    /// <summary>
    /// 参数名：导出名字,如 item
    /// </summary>
    string ExportNameParam { set; get; }
    /// <summary>
    /// 参数值：导出名字
    /// </summary>
    string ExportName { set; get; }
    /// <summary>
    /// 参数名：excel分割符，如item-道具,取分割符前的为导出表名
    /// </summary>
    string ExcelNameSplitStringParam { set; get; }
    string ExcelNameSplitString { set; get; }
    string ExportExtensionParam { set; get; }
    string ExportExtension { set; get; }
    /// <summary>
    /// 参数名：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAddParam { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名前添加前缀,在ExportName的基础上。如tb_;cfg_
    /// </summary>
    string ExportNameBeforeAdd { set; get; }
    /// <summary>
    /// 参数名：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMarkParam { set; get; }
    /// <summary>
    /// 参数值：导出时在文件名后添加语言标识，母语通常默认为空,如_ft
    /// </summary>
    string ExportNameAfterLanguageMark { set; get; }
    /// <summary>
    /// 参数名：int long float为空时是否导出
    /// </summary>
    string ExportNullNumberParam { set; get; }
    /// <summary>
    /// 参数值：int long float为空时是否导出
    /// </summary>
    bool ExportNullNumber { set; get; }

    /// <summary>
    /// 参数名：String为空时是否导出
    /// </summary>
    string ExportNullStringParam { set; get; }
    /// <summary>
    /// 参数值：String为空时是否导出
    /// </summary>
    bool ExportNullString { set; get; }

    /// <summary>
    /// 参数名：Json为空时是否导出
    /// </summary>
    string ExportNullJsonParam { set; get; }
    /// <summary>
    /// 参数值：Json为空时是否导出
    /// </summary>
    bool ExportNullJson { set; get; }

    /// <summary>
    /// 参数名：Araay为空(-1)时是否导出
    /// </summary>
    string ExportNullArrayParam { set; get; }
    /// <summary>
    /// 参数值：Araay为空(-1)时是否导出
    /// </summary>
    bool ExportNullArray { set; get; }
    /// <summary>
    /// 参数名：Dict为空(-1)时是否导出
    /// </summary>
    string ExportNullDictParam { set; get; }
    /// <summary>
    /// 参数值：Dict为空(-1)时是否导出
    /// </summary>
    bool ExportNullDict { set; get; }
    /// <summary>
    /// 参数名：bool为空时是否导出，导出则为false
    /// </summary>
    string ExportNullBoolParam { set; get; }
    bool ExportNullBool { set; get; }
    /// <summary>
    /// 参数名：Date为空时是否导出
    /// </summary>
    string ExportNullDateParam { set; get; }
    bool ExportNullDate { set; get; }
    /// <summary>
    /// 参数名：Time为空时是否导出
    /// </summary>
    string ExportNullTimeParam { set; get; }
    bool ExportNullTime { set; get; }
    /// <summary>
    /// 参数名：Lang为空时是否导出
    /// </summary>
    string ExportNullLangParam { set; get; }
    bool ExportNullLang { set; get; }
    /// <summary>
    ///  参数名：导出时是否需要格式化
    /// </summary>
    string ExportFormatParam { set; get; }
    /// <summary>
    ///  参数值：导出时是否需要格式化
    /// </summary>
    bool ExportFormat { set; get; }

    /// <summary>
    /// 参数名：导出时在文件中生成字段注释信息
    /// </summary>
    string ExportFieldCommentParam { set; get; }
    /// <summary>
    /// 参数值：导出时在文件中生成字段注释信息
    /// </summary>
    bool ExportFieldComment { set; get; }

    /// <summary>
    /// 参数名：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWordsParam { set; get; }
    /// <summary>
    /// 参数值：导出时在文件头部额外生成指定文字
    /// </summary>
    string ExportTopWords { set; get; }

    /// <summary>
    /// 参数名：导出时的缩进符
    /// </summary>
    string ExportIndentationStringParam { set; get; }
    /// <summary>
    /// 参数值：导出时的缩进符
    /// </summary>
    string ExportIndentationString { set; get; }

    /// <summary>
    /// 参数名：导出时的间隔符
    /// </summary>
    string ExportSpaceStringParam { set; get; }
    /// <summary>
    /// 参数值：导出时的间隔符
    /// </summary>
    string ExportSpaceString { set; get; }
    /// <summary>
    /// 参数名：
    /// </summary>
    string IsExportJsonArrayFormatParam { set; get; }
    /// <summary>
    /// 导出的json文件是否生成为各行数据对应的json object包含在一个json array的形式
    /// </summary>
    bool IsExportJsonArrayFormat { set; get; }
    /// <summary>
    /// 参数名：
    /// </summary>
    string IsExportJsonMapIncludeKeyColumnValueParam { set; get; }
    /// <summary>
    /// 导出的json文件，若生成包含在一个json object的形式，是否使每行字段信息对应的json object中包含主键列对应的键值对
    /// </summary>
    bool IsExportJsonMapIncludeKeyColumnValue { set; get; }
    /// <summary>
    /// 参数名：
    /// </summary>
    string IsArrayFieldNameParam { set; get; }
    /// <summary>
    /// 导出Lua时，数组类型是否使用[1]=值  格式，默认是
    /// </summary>
    bool IsArrayFieldName { set; get; }
    /// <summary>
    /// 参数名：
    /// </summary>
    string IsTableNameStartParam { set; get; }
    /// <summary>
    /// 导出的lua文件内容是以表名开头还是以retun开启
    /// </summary>
    bool IsTableNameStart { set; get; }
    /// <summary>
    /// 参数名：
    /// </summary>
    string IsAddKeyToLuaTableParam { set; get; }
    /// <summary>
    /// 导出为lua table时，是否将主键列的值作为table中的元素
    /// </summary>
    bool IsAddKeyToLuaTable { set; get; }
    /// <summary>
    /// 参数名：
    /// </summary>
    string DateToExportFormatParam { set; get; }
    /// <summary>
    /// date型导出格式
    /// </summary>
    string DateToExportFormat { set; get; }
    /// <summary>
    /// 参数名：
    /// </summary>
    string TimeToExportFormatParam { set; get; }
    /// <summary>
    /// Time型导出格式
    /// </summary>
    string TimeToExportFormat { set; get; }
    void GetParamValue();
}
