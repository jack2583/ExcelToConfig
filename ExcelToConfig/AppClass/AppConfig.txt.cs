using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class AppConfig
{
    //Config(ConfigPat=config.txt)
    public static string AppLogParam = "AppLog";
    public static string ConfigPathParam = "ConfigPath";
    /// <summary>
    /// 配置文件（配置自定义的检查规则）的文件名
    /// </summary>
    public static string ConfigPath = FileModule.CombinePath(AppValues.ProgramFolderPath, "config.txt");

    private static void GetParamValue()
    {
        if (AppValues.BatParamInfo.ContainsKey(AppLogParam))
        {
            Dictionary<string, BatChildParam> ChildParam = AppValues.BatParamInfo[AppLogParam].ChildParam;
            ConfigPath = BatMethods.GetStringValue(ConfigPath, ChildParam, ConfigPathParam);
        }
    }
    public static void ReadConfig()
    {
        GetParamValue();
        if (File.Exists(AppConfig.ConfigPath))
        {
           string errorString = null;
            AppValues.ConfigData = TxtConfigReader.ParseTxtConfigFile(AppConfig.ConfigPath, ":", out errorString);
            if (!string.IsNullOrEmpty(errorString))
                AppLog.LogErrorAndExit(errorString);

        }
        else
            AppLog.LogWarning(string.Format("警告：找不到本工具所在路径下的{0}配置文件，请确定是否真的不需要自定义配置", AppConfig.ConfigPath));

        //检查datetime类型，并初始化
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
        if (AppValues.ConfigData.ContainsKey(DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT))
        {
            DateTimeValue.DefaultTimeInputFormat = AppValues.ConfigData[DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT].Trim();
            if (TableCheckHelper.CheckTimeDefine(DateTimeValue.DefaultTimeInputFormat, out tempErrorString) == false)
                errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT, DateTimeValue.DefaultTimeInputFormat, tempErrorString);
        }


        //if (AppValues.ConfigData.ContainsKey(DateTimeValue.APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT))
        //{
        //    DateTimeValue.DefaultDateInputFormat = AppValues.ConfigData[DateTimeValue.APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT].Trim();
        //    if (TableCheckHelper.CheckDateInputDefine(DateTimeValue.DefaultDateInputFormat, out tempErrorString) == false)
        //        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, DateTimeValue.APP_CONFIG_KEY_DEFAULT_DATE_INPUT_FORMAT, DateTimeValue.DefaultDateInputFormat, tempErrorString);
        //}
        //if (AppValues.ConfigData.ContainsKey(LuaStruct.DefaultDateToExportFormatKey))
        //{
        //    LuaStruct.DefaultDateToExportFormat = AppValues.ConfigData[LuaStruct.DefaultDateToExportFormatKey].Trim();
        //    if (TableCheckHelper.CheckDateToLuaDefine(LuaStruct.DefaultDateToExportFormat, out tempErrorString) == false)
        //        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, LuaStruct.DefaultDateToExportFormatKey, LuaStruct.DefaultDateToExportFormat, tempErrorString);
        //}
        //if (AppValues.ConfigData.ContainsKey(MySQLStruct.DefaultDateToExportFormatKey))
        //{
        //    MySQLStruct.DefaultDateToExportFormat = AppValues.ConfigData[MySQLStruct.DefaultDateToExportFormatKey].Trim();
        //    if (TableCheckHelper.CheckDateToDatabaseDefine(MySQLStruct.DefaultDateToExportFormat, out tempErrorString) == false)
        //        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, MySQLStruct.DefaultDateToExportFormatKey, MySQLStruct.DefaultDateToExportFormat, tempErrorString);
        //}
        //if (AppValues.ConfigData.ContainsKey(DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT))
        //{
        //    DateTimeValue.DefaultTimeInputFormat = AppValues.ConfigData[DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT].Trim();
        //    if (TableCheckHelper.CheckTimeDefine(DateTimeValue.DefaultTimeInputFormat, out tempErrorString) == false)
        //        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, DateTimeValue.APP_CONFIG_KEY_DEFAULT_TIME_INPUT_FORMAT, DateTimeValue.DefaultTimeInputFormat, tempErrorString);
        //}
        //if (AppValues.ConfigData.ContainsKey(LuaStruct.DefaultTimeToExportFormatKey))
        //{
        //    LuaStruct.DefaultTimeToExportFormat = AppValues.ConfigData[LuaStruct.DefaultTimeToExportFormatKey].Trim();
        //    if (TableCheckHelper.CheckTimeDefine(LuaStruct.DefaultTimeToExportFormat, out tempErrorString) == false)
        //        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, LuaStruct.DefaultTimeToExportFormatKey, LuaStruct.DefaultTimeToExportFormat, tempErrorString);
        //}
        //if (AppValues.ConfigData.ContainsKey(MySQLStruct.DefaultTimeToExportFormatKey))
        //{
        //    MySQLStruct.DefaultTimeToExportFormat = AppValues.ConfigData[MySQLStruct.DefaultTimeToExportFormatKey].Trim();
        //    if (TableCheckHelper.CheckTimeDefine(MySQLStruct.DefaultTimeToExportFormat, out tempErrorString) == false)
        //        errorStringBuilder.AppendFormat(ERROR_STRING_FORMAT, MySQLStruct.DefaultTimeToExportFormatKey, MySQLStruct.DefaultTimeToExportFormat, tempErrorString);
        //}

        string errorConfigString = errorStringBuilder.ToString();
        if (!string.IsNullOrEmpty(errorConfigString))
        {
            errorConfigString = string.Concat("配置文件中存在以下错误，请修正后重试\n", errorConfigString);
            AppLog.LogErrorAndExit(errorConfigString);
        }
    }
}
