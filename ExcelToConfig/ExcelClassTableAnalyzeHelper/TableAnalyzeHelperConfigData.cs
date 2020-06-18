using System;
using System.Collections.Generic;
using System.Data;

public partial class TableAnalyzeHelper
{
    /// <summary>
    /// 获取某张表格以第1列为key，第3列起来value的方式的配置参数（key：参数名， value：参数列表）
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="errorString"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetTableConfigData(DataTable dt, out string errorString)
    {
        Dictionary<string, string> config = new Dictionary<string, string>();
        int rowCount = dt.Rows.Count;
        int columnCount = dt.Columns.Count;

        for (int row = 1; row < rowCount; ++row)
        {
            // 取参数名
            string paramName = dt.Rows[row][0].ToString().Trim();
            if (string.IsNullOrEmpty(paramName))
                continue;
            else
            {
                if (config.Count > 0)
                {
                    if (config.ContainsKey(paramName))
                    {
                        errorString = string.Format("错误：表格{0}的配置表中存在相同的参数名{1}，请修正错误后重试\n", dt.TableName, paramName);
                        return null;
                    }
                }

                config.Add(paramName, dt.Rows[row][2].ToString().Trim());
            }
        }

        errorString = null;
        if (config.Count > 0)
            return config;
        else
            return null;
    }

    /// <summary>
    /// 获取某张表格以第1行为key，第2行起为value的方式的配置参数（key：参数名， value：参数列表）
    /// </summary>
    public static Dictionary<string, List<string>> GetTableConfigOfFirstRow(DataTable dt, out string errorString)
    {
        Dictionary<string, List<string>> config = new Dictionary<string, List<string>>();
        int rowCount = dt.Rows.Count;
        int columnCount = dt.Columns.Count;

        for (int column = 0; column < columnCount; ++column)
        {
            // 取参数名
            string paramName = dt.Rows[ExcelTableSetting.ConfigFieldDefingRowIndex][column].ToString().Trim();
            if (string.IsNullOrEmpty(paramName))
                continue;
            else
            {
                if (config.ContainsKey(paramName))
                {
                    errorString = string.Format("错误：表格{0}的配置表中存在相同的参数名{1}，请修正错误后重试\n");
                    return null;
                }
                else
                    config.Add(paramName, new List<string>());
            }

            // 取具体参数配置
            List<string> inputParams = config[paramName];
            for (int row = ExcelTableSetting.ConfigFieldParamStartRowIndex; row < rowCount; ++row)
            {
                string param = dt.Rows[row][column].ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    if (inputParams.Contains(param))
                        AppLog.LogWarning(string.Format("警告：配置项（参数名为{0}）中存在相同的参数\"{1}\"，请确认是否填写错误\n", paramName, param));

                    inputParams.Add(param);
                }
            }
        }

        errorString = null;
        if (config.Count > 0)
            return config;
        else
            return null;
    }

    /// <summary>
    /// 获取某张表格以以第1列为key，第3列起来value的方式的配置参数（key：参数名， value：参数列表）
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="errorString"></param>
    /// <returns></returns>
    public static Dictionary<string, List<string>> GetTableConfigOfFirstColumn(DataTable dt, out string errorString)
    {
        Dictionary<string, List<string>> config = new Dictionary<string, List<string>>();
        int rowCount = dt.Rows.Count;
        int columnCount = dt.Columns.Count;

        for (int row = 1; row < rowCount; ++row)
        {
            // 取参数名
            string paramName = dt.Rows[row][0].ToString().Trim();
            if (string.IsNullOrEmpty(paramName))
                continue;
            else
            {
                if (config.Count > 0)
                {
                    if (config.ContainsKey(paramName))
                    {
                        errorString = string.Format("错误：表格{0}的配置表中存在相同的参数名{1}，请修正错误后重试\n", dt.TableName, paramName);
                        return null;
                    }
                    else
                        config.Add(paramName, new List<string>());
                }
                else
                    config.Add(paramName, new List<string>());
            }

            // 取具体参数配置

            List<string> inputParams = config[paramName];
            for (int column = 2; column < columnCount; ++column)
            {
                string param = dt.Rows[row][column].ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    if (inputParams.Count > 0)
                    {
                        if (inputParams.Contains(param))
                            AppLog.LogWarning(string.Format("警告：配置项（参数名为{0}）中存在相同的参数\"{1}\"，请确认是否填写错误\n", paramName, param));
                    }

                    inputParams.Add(param);
                }
            }
        }

        errorString = null;
        if (config.Count > 0)
            return config;
        else
            return null;
    }

    public static bool GetOneConfigData(TableInfo tableInfo, string configKey)
    {
        return true;
    }

    /// <summary>
    /// 获取一个config配置的bool值，如果找不到key则不到传入的bool值进行修改
    /// </summary>
    /// <param name="tableInfo"></param>
    /// <param name="configKey"></param>
    /// <param name="configValue"></param>
    /// <returns></returns>
    public static bool GetOneConfigData(TableInfo tableInfo, string configKey, ref bool configValue)
    {
        if (tableInfo.TableConfigData2 != null && tableInfo.TableConfigData2.ContainsKey(configKey))
        {
            if (tableInfo.TableConfigData2[configKey].Count > 0)
            {
                if ("false".Equals(tableInfo.TableConfigData2[configKey][0], StringComparison.CurrentCultureIgnoreCase))
                {
                    configValue = false;
                    return true;
                }
                else if ("true".Equals(tableInfo.TableConfigData2[configKey][0], StringComparison.CurrentCultureIgnoreCase))
                {
                    configValue = true;
                    return true;
                }
                else
                    return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    /// <summary>
    /// 获取一个config配置的list值，如特殊导出的嵌套，list则为多个嵌套方案
    /// </summary>
    /// <param name="tableInfo"></param>
    /// <param name="configKey"></param>
    /// <param name="configValue"></param>
    /// <returns></returns>
    public static bool GetOneConfigData(TableInfo tableInfo, string configKey, ref List<string> configValue)
    {
        if (tableInfo.TableConfigData2 != null && tableInfo.TableConfigData2.ContainsKey(configKey))
        {
            if (tableInfo.TableConfigData2[configKey].Count > 0)
            {
                configValue = tableInfo.TableConfigData2[configKey];
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    /// <summary>
    /// 获取一个config配置的string值，
    /// </summary>
    /// <param name="tableInfo"></param>
    /// <param name="configKey"></param>
    /// <param name="configValue"></param>
    /// <returns></returns>
    public static bool GetOneConfigData(TableInfo tableInfo, string configKey, ref string configValue)
    {
        if (tableInfo.TableConfigData2 != null && tableInfo.TableConfigData2.ContainsKey(configKey))
        {
            if (tableInfo.TableConfigData2[configKey].Count > 0)
            {
                configValue = tableInfo.TableConfigData2[configKey][0];
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
}