using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

class ExportMySQL : Export
{
    // Config.txt中定义 MySQL连接字符串
    public const string connectMySQLStringParam = "connectMySQLString";

    private static MySqlConnection conn = null;
    private static string schemaName = null;
    // 导出数据前获取的数据库中已存在的表名
    private static List<string> existTableNames = new List<string>();

    // MySQL支持的用于定义Schema名的参数名 
    private static string[] schemaParam = { "Database", "Initial Catalog" };

    private const string createTableSQL = "CREATE TABLE {0} ( {1} PRIMARY KEY (`{2}`)) COMMENT='{3}' {4};";
    private const string dropTableSQL = "DROP TABLE {0};";
    private const string insertDataSQL = "INSERT INTO {0} ({1}) VALUES {2};";
    /// <summary>
    /// 导出数据到MySQL中的date型字段的默认格式 
    /// </summary>
    private const string defaultExportDateFormat = "yyyy-MM-dd";

    /// <summary>
    /// config.txt中设置
    /// 创建MySQL数据库表格时额外指定的参数字符串
    /// 导出到 MySQL 数据库进行建表时额外添加的参数字符串，
    /// 比如可以通过设置使用的编码格式，避免插入的中文变成乱码。
    /// 例如：ENGINE=InnoDB DEFAULTCHARSET=utf8mb4 COLLATE = utf8mb4_bin
    /// </summary>
    private const string createDatabaseTableExtraParam = "createDatabaseTableExtraParam";


    public static void ExportToMySQL()
    {
        string ExportType = "MySQL";
        if (AppValues.ConfigData.ContainsKey("AllExport" + ExportType))
        {
            if (string.Equals("false", AppValues.ConfigData["AllExport" + ExportType].Trim().ToLower()))
                return;
        }
        else
            return;

        string errorString = null;
        BatExportPublicSetting batExportPublicSetting = new BatExportPublicSetting();
        batExportPublicSetting.IsExport = false;
        batExportPublicSetting.ExcelNameSplitString = "-";
        batExportPublicSetting.ExportNameAfterLanguageMark = "";
        batExportPublicSetting.GetParamValue();

        BatExportSetting batExportSetting = new BatExportSetting();
        batExportSetting.ExportTypeParam = "ExportMySQL";
        batExportSetting.ExportPath = "";
        batExportSetting.IsExport = false;
        batExportSetting.IsExportKeepDirectoryStructure = false;
        batExportSetting.ExportExtension = "";
        batExportSetting.ExportNameBeforeAdd = "";
        batExportSetting.IsExportNullNumber = false;
        batExportSetting.IsExportNullString = false;//用于配置将表格导出到 MySQL 数据库时string 型字段中的空白单元格导出为数据库中的 NULL 而不是空字符串
        batExportSetting.IsExportNullJson = false;
        batExportSetting.IsExportNullArray = false;
        batExportSetting.IsExportNullDict = false;
        batExportSetting.IsExportNullBool = false;
        batExportSetting.IsExportNullDate = false;
        batExportSetting.IsExportNullTime = false;
        batExportSetting.IsExportNullLang = false;
        batExportSetting.IsExportFormat = false;
        batExportSetting.IsExportFieldComment = false;
        batExportSetting.ExportTopWords = "";
        batExportSetting.ExportIndentationString = "";
        batExportSetting.ExportSpaceString = "";
        batExportSetting.IsExportJsonArrayFormat = false;
        batExportSetting.IsExportJsonMapIncludeKeyColumnValue = false;
        batExportSetting.IsArrayFieldName = false;
        batExportSetting.IsTableNameStart = false;
        batExportSetting.IsAddKeyToLuaTable = false;
        batExportSetting.GetParamValue();

        ConnectToDatabase(out errorString);
        if (!string.IsNullOrEmpty(errorString))
            AppLog.LogErrorAndExit(string.Format("无法连接至{0}数据库：{1}\n导出至{2}数据库被迫中止，请修正错误后重试\n", ExportType, errorString, ExportType));


        foreach (KeyValuePair<string, TableInfo> kvp in AppValues.TableInfo)
        {
            TableInfo tableInfo = kvp.Value;
            errorString = null;
            ExcelConfigSetting excelConfigSetting = new ExcelConfigSetting();
            excelConfigSetting.IsExportParam = "Export" + ExportType;
            excelConfigSetting.ExportPathParam = ExportType + "ExportPath";
            excelConfigSetting.IsExportKeepDirectoryStructureParam = ExportType + "IsExportKeepDirectoryStructure";
            excelConfigSetting.ExportNameParam = ExportType + "ExportName";//用于配置当需要将表格导出到 MySQL 数据库时的表名
            excelConfigSetting.ExcelNameSplitStringParam = ExportType + "ExcelNameSplitString";
            excelConfigSetting.ExportExtensionParam = ExportType + "ExportExtension";
            excelConfigSetting.ExportNameBeforeAddParam = ExportType + "ExportNameBeforeAdd";
            excelConfigSetting.ExportNameAfterLanguageMarkParam = ExportType + "ExportNameAfterLanguageMark";
            excelConfigSetting.IsExportNullNumberParam = ExportType + "IsExportNullNumber";
            excelConfigSetting.IsExportNullStringParam = ExportType + "IsExportNullString";
            excelConfigSetting.IsExportNullJsonParam = ExportType + "IsExportNullJson";
            excelConfigSetting.IsExportNullArrayParam = ExportType + "IsExportNullArray";
            excelConfigSetting.IsExportNullDictParam = ExportType + "IsExportNullDict";
            excelConfigSetting.IsExportNullBoolParam = ExportType + "IsExportNullBool";
            excelConfigSetting.IsExportNullDateParam = ExportType + "IsExportNullDate";
            excelConfigSetting.IsExportNullTimeParam = ExportType + "IsExportNullTime";
            excelConfigSetting.IsExportNullLangParam = ExportType + "IsExportNullLang";
            excelConfigSetting.IsExportFormatParam = ExportType + "IsExportFormat";
            excelConfigSetting.IsExportFieldCommentParam = ExportType + "ExportFieldComment";
            excelConfigSetting.ExportTopWordsParam = ExportType + "ExportTopWords";//exportMySQLTableComment中声明某张表格导出到数据库中的说明信息
            excelConfigSetting.ExportIndentationStringParam = ExportType + "ExportIndentationString";
            excelConfigSetting.ExportSpaceStringParam = ExportType + "ExportSpaceString";
            excelConfigSetting.IsExportJsonArrayFormatParam = ExportType + "IsExportJsonArrayFormat";
            excelConfigSetting.IsExportJsonMapIncludeKeyColumnValueParam = ExportType + "IsExportJsonMapIncludeKeyColumnValue";
            excelConfigSetting.IsArrayFieldNameParam = ExportType + "IsArrayFieldName";
            excelConfigSetting.IsTableNameStartParam = ExportType + "IsTableNameStart";
            excelConfigSetting.IsAddKeyToLuaTableParam = ExportType + "IsAddKeyToLuaTable";
            excelConfigSetting.DateToExportFormatParam = ExportType + "DateToExportFormat";//日期导出的格式
            excelConfigSetting.TimeToExportFormatParam = ExportType + "TimeToExportFormat";
            excelConfigSetting.SpecialExportParam = "SpecialExport" + ExportType;

            if (AppValues.ConfigData.ContainsKey("Export" + ExportType))
                excelConfigSetting.IsExportParam = AppValues.ConfigData["Export" + ExportType].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportPath"))
                excelConfigSetting.ExportPathParam = AppValues.ConfigData[ExportType + "ExportPath"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportKeepDirectoryStructure"))
                excelConfigSetting.IsExportKeepDirectoryStructureParam = AppValues.ConfigData[ExportType + "IsExportKeepDirectoryStructure"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportName"))
                excelConfigSetting.ExportNameParam = AppValues.ConfigData[ExportType + "ExportName"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExcelNameSplitString"))
                excelConfigSetting.ExcelNameSplitStringParam = AppValues.ConfigData[ExportType + "ExcelNameSplitString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportExtension"))
                excelConfigSetting.ExportExtensionParam = AppValues.ConfigData[ExportType + "ExportExtension"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportNameBeforeAdd"))
                excelConfigSetting.ExportNameBeforeAddParam = AppValues.ConfigData[ExportType + "ExportNameBeforeAdd"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportNameAfterLanguageMark"))
                excelConfigSetting.ExportNameAfterLanguageMarkParam = AppValues.ConfigData[ExportType + "ExportNameAfterLanguageMark"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullNumber"))
                excelConfigSetting.IsExportNullNumberParam = AppValues.ConfigData[ExportType + "IsExportNullNumber"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullString"))
                excelConfigSetting.IsExportNullStringParam = AppValues.ConfigData[ExportType + "IsExportNullString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullJson"))
                excelConfigSetting.IsExportNullJsonParam = AppValues.ConfigData[ExportType + "IsExportNullJson"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullArray"))
                excelConfigSetting.IsExportNullArrayParam = AppValues.ConfigData[ExportType + "IsExportNullArray"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullDict"))
                excelConfigSetting.IsExportNullDictParam = AppValues.ConfigData[ExportType + "IsExportNullDict"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullBool"))
                excelConfigSetting.IsExportNullBoolParam = AppValues.ConfigData[ExportType + "IsExportNullBool"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullDate"))
                excelConfigSetting.IsExportNullDateParam = AppValues.ConfigData[ExportType + "IsExportNullDate"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullTime"))
                excelConfigSetting.IsExportNullTimeParam = AppValues.ConfigData[ExportType + "IsExportNullTime"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportNullLang"))
                excelConfigSetting.IsExportNullLangParam = AppValues.ConfigData[ExportType + "IsExportNullLang"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportFormat"))
                excelConfigSetting.IsExportFormatParam = AppValues.ConfigData[ExportType + "IsExportFormat"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportFieldComment"))
                excelConfigSetting.IsExportFieldCommentParam = AppValues.ConfigData[ExportType + "ExportFieldComment"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportTopWords"))
                excelConfigSetting.ExportTopWordsParam = AppValues.ConfigData[ExportType + "ExportTopWords"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportIndentationString"))
                excelConfigSetting.ExportIndentationStringParam = AppValues.ConfigData[ExportType + "ExportIndentationString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportSpaceString"))
                excelConfigSetting.ExportSpaceStringParam = AppValues.ConfigData[ExportType + "ExportSpaceString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "ExportLineString"))
                excelConfigSetting.ExportLineStringParam = AppValues.ConfigData[ExportType + "ExportLineString"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportJsonArrayFormat"))
                excelConfigSetting.IsExportJsonArrayFormatParam = AppValues.ConfigData[ExportType + "IsExportJsonArrayFormat"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsExportJsonMapIncludeKeyColumnValue"))
                excelConfigSetting.IsExportJsonMapIncludeKeyColumnValueParam = AppValues.ConfigData[ExportType + "IsExportJsonMapIncludeKeyColumnValue"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsArrayFieldName"))
                excelConfigSetting.IsArrayFieldNameParam = AppValues.ConfigData[ExportType + "IsArrayFieldName"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsTableNameStart"))
                excelConfigSetting.IsTableNameStartParam = AppValues.ConfigData[ExportType + "IsTableNameStart"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "IsAddKeyToLuaTable"))
                excelConfigSetting.IsAddKeyToLuaTableParam = AppValues.ConfigData[ExportType + "IsAddKeyToLuaTable"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "DateToExportFormat"))
                excelConfigSetting.DateToExportFormatParam = AppValues.ConfigData[ExportType + "DateToExportFormat"].Trim();
            if (AppValues.ConfigData.ContainsKey(ExportType + "TimeToExportFormat"))
                excelConfigSetting.TimeToExportFormatParam = AppValues.ConfigData[ExportType + "TimeToExportFormat"].Trim();

            excelConfigSetting.GetParamValue(tableInfo);

            Export export = new Export();
            export.GetValue(tableInfo, excelConfigSetting, batExportSetting, batExportPublicSetting);
            // export.GetExportName(excelConfigSetting.ExportName, tableInfo.ExcelName, export.ExcelNameSplitString);


            string m = "";
            if (AppValues.MergeTableList != null && AppValues.MergeTableList.ContainsKey(kvp.Key) && batExportSetting.IsExport == true)
            {
                export.IsExport = true;
                export.ExportName = kvp.Key;
                m = "[合并]";
            }

            if (export.IsExport == true)
            {
                AppLog.Log(string.Format("\n开始{0}导入{1}：数据库", m, ExportType), ConsoleColor.Green, false);
                AppLog.Log(string.Format("{0}并命名为{1}", tableInfo.ExcelNameTips, export.ExportName), ConsoleColor.Green);

                ExportTableToDatabase(tableInfo, export, out errorString);
                if (!string.IsNullOrEmpty(errorString))
                    AppLog.LogErrorAndExit(string.Format("导出失败：{0}\n导出至{1}数据库被迫中止，请修正错误后重试\n", errorString, ExportType));
            }
        }
    }
    //连接数据库
    private static bool ConnectToDatabase(out string errorString)
    {
        if (!AppValues.ConfigData.ContainsKey(connectMySQLStringParam))
        {
            errorString = string.Format("未在config配置文件中以名为\"{0}\"的key声明连接MySQL的字符串", connectMySQLStringParam);
            return false;
        }
        // 提取MySQL连接字符串中的Schema名
        string connectString = AppValues.ConfigData[connectMySQLStringParam];


        foreach (string legalSchemaNameParam in schemaParam)
        {
            int defineStartIndex = connectString.IndexOf(legalSchemaNameParam, StringComparison.CurrentCultureIgnoreCase);
            if (defineStartIndex != -1)
            {
                // 查找后面的等号
                int equalSignIndex = -1;
                for (int i = defineStartIndex + legalSchemaNameParam.Length; i < connectString.Length; ++i)
                {
                    if (connectString[i] == '=')
                    {
                        equalSignIndex = i;
                        break;
                    }
                }
                if (equalSignIndex == -1 || equalSignIndex + 1 == connectString.Length)
                {
                    errorString = string.Format("MySQL数据库连接字符串（\"{0}\"）中\"{1}\"后需要跟\"=\"进行Schema名声明", connectString, legalSchemaNameParam);
                    return false;
                }
                else
                {
                    // 查找定义的Schema名，在参数声明的=后面截止到下一个分号或字符串结束
                    int semicolonIndex = -1;
                    for (int i = equalSignIndex + 1; i < connectString.Length; ++i)
                    {
                        if (connectString[i] == ';')
                        {
                            semicolonIndex = i;
                            break;
                        }
                    }
                    if (semicolonIndex == -1)
                        schemaName = connectString.Substring(equalSignIndex + 1).Trim();
                    else
                        schemaName = connectString.Substring(equalSignIndex + 1, semicolonIndex - equalSignIndex - 1).Trim();
                }

                break;
            }
        }
        if (schemaName == null)
        {
            errorString = string.Format("MySQL数据库连接字符串（\"{0}\"）中不包含Schema名的声明，请在{1}中任选一个参数名进行声明", connectString, Utils.CombineString(schemaParam, ","));
            return false;
        }

        try
        {
            conn = new MySqlConnection(connectString);
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {
                // 获取已经存在的表格名
                DataTable schemaInfo = conn.GetSchema(SqlClientMetaDataCollectionNames.Tables);
                foreach (DataRow info in schemaInfo.Rows)
                    existTableNames.Add(info.ItemArray[2].ToString());

                errorString = null;
                return true;
            }
            else
            {
                errorString = "未知错误";
                return true;
            }
        }
        catch (MySqlException exception)
        {
            errorString = exception.Message;
            return false;
        }
    }


    public static bool ExportTableToDatabase(TableInfo tableInfo, Export export, out string errorString)
    {
        string tableName = export.ExportName;

        //string tableName = inputParams[0];
        // 检查数据库中是否已经存在同名表格，若存在删除旧表
        if (existTableNames.Contains(tableName))
        {
            _DropTable(tableName, out errorString);
            if (!string.IsNullOrEmpty(errorString))
            {
                errorString = string.Format("数据库中存在同名表格，但删除旧表失败，{0}", errorString);
                return false;
            }
        }
       
        string comment = export.ExportTopWords;
        _CreateTable(tableName, tableInfo, comment, export, out errorString);
        if (string.IsNullOrEmpty(errorString))
        {
            // 将Excel表格中的数据添加至数据库
            _InsertData(tableName, tableInfo, export, out errorString);
            if (string.IsNullOrEmpty(errorString))
            {
                AppLog.Log("成功");

                errorString = null;
                return true;
            }
            else
            {
                errorString = string.Format("插入数据失败，{0}", errorString);
                return false;
            }
        }
        else
        {
            errorString = string.Format("创建表格失败，{0}", errorString);
            return false;
        }
    }


    private static bool _InsertData(string tableName, TableInfo tableInfo, Export export, out string errorString)
    {
        List<FieldInfo> allDatabaseFieldInfo = GetAllDatabaseFieldInfo(tableInfo);

        // 生成所有字段名对应的定义字符串
        List<string> fileNames = new List<string>();
        foreach (FieldInfo fieldInfo in allDatabaseFieldInfo)
            fileNames.Add(string.Format("`{0}`", fieldInfo.DatabaseFieldName));

        string fieldNameDefineString = Utils.CombineString(fileNames, ", ");

       bool isWriteNullForEmptyString = export.IsExportNullString;

        // 逐行生成插入数据的SQL语句中的value定义部分
        StringBuilder valueDefineStringBuilder = new StringBuilder();
        int count = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; ++i)
            {
                List<string> values = new List<string>();
                foreach (FieldInfo fieldInfo in allDatabaseFieldInfo)
                {
                    if (fieldInfo.Data[i] == null)
                        values.Add("NULL");
                    else if (fieldInfo.DataType == DataType.Date)
                    {
                        string toDatabaseFormatDefine = fieldInfo.ExtraParam[DateTimeTypeKey.toMySQL.ToString()].ToString();
                        //string toDatabaseFormatDefine = export.DateToExportFormat;
                        DateFormatType toDatabaseFormatType = DateTimeValue.GetDateFormatType(toDatabaseFormatDefine);
                        if (toDatabaseFormatType == DateFormatType.FormatString)
                        {
                            // 注意MySQL中的时间型，datetime和time型后面可用括号进行具体设置，date型没有
                            // MySQL中的date型插入数据时不允许含有时分秒，否则会报错，故这里强制采用MySQL默认的yyyy-MM-dd格式插入
                            if (fieldInfo.DatabaseFieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase))
                                values.Add(string.Format("'{0}'", ((DateTime)(fieldInfo.Data[i])).ToString(defaultExportDateFormat)));
                            else if (fieldInfo.DatabaseFieldType.StartsWith("datetime", StringComparison.CurrentCultureIgnoreCase))
                                values.Add(string.Format("'{0}'", ((DateTime)(fieldInfo.Data[i])).ToString(DateTimeValue.APP_DEFAULT_DATE_FORMAT)));
                            // date型导出到MySQL中的其他数据类型字段如varchar，采用声明的指定格式
                            else
                                values.Add(string.Format("'{0}'", ((DateTime)(fieldInfo.Data[i])).ToString(toDatabaseFormatDefine)));
                        }
                        else if (toDatabaseFormatType == DateFormatType.ReferenceDateSec)
                            values.Add(string.Format("'{0}'", ((DateTime)fieldInfo.Data[i] - DateTimeValue.REFERENCE_DATE).TotalSeconds));
                        else if (toDatabaseFormatType == DateFormatType.ReferenceDateMsec)
                            values.Add(string.Format("'{0}'", ((DateTime)fieldInfo.Data[i] - DateTimeValue.REFERENCE_DATE).TotalMilliseconds));
                        else
                        {
                            errorString = "date型导出至MySQL的格式定义非法";
                            AppLog.LogErrorAndExit(errorString);
                            return false;
                        }
                    }
                    else if (fieldInfo.DataType == DataType.Time)
                    {
                        string toDatabaseFormatDefine = fieldInfo.ExtraParam[DateTimeTypeKey.toMySQL.ToString()].ToString();
                        //string toDatabaseFormatDefine = export.TimeToExportFormat;
                        TimeFormatType toDatabaseFormatType = DateTimeValue.GetTimeFormatType(toDatabaseFormatDefine);
                        if (toDatabaseFormatType == TimeFormatType.FormatString)
                        {
                            if (fieldInfo.DatabaseFieldType.StartsWith("time", StringComparison.CurrentCultureIgnoreCase))
                                values.Add(string.Format("'{0}'", ((DateTime)(fieldInfo.Data[i])).ToString(DateTimeValue.APP_DEFAULT_TIME_FORMAT)));
                            else
                                values.Add(string.Format("'{0}'", ((DateTime)(fieldInfo.Data[i])).ToString(toDatabaseFormatDefine)));
                        }
                        else if (toDatabaseFormatType == TimeFormatType.ReferenceTimeSec)
                            values.Add(string.Format("'{0}'", ((DateTime)fieldInfo.Data[i] - DateTimeValue.REFERENCE_DATE).TotalSeconds));
                        else
                        {
                            errorString = "time型导出至MySQL的格式定义非法";
                            AppLog.LogErrorAndExit(errorString);
                            return false;
                        }
                    }
                    else if (fieldInfo.DataType == DataType.Bool)
                    {
                        bool inputData = (bool)fieldInfo.Data[i];
                        // 如果数据库用bit数据类型表示bool型，比如要写入true，SQL语句中的1不能加单引号
                        if (fieldInfo.DatabaseFieldType.Equals("bit", StringComparison.CurrentCultureIgnoreCase) || fieldInfo.DatabaseFieldType.StartsWith("bit(", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (inputData == true)
                                values.Add("1");
                            else
                                values.Add("0");
                        }
                        else
                        {
                            // 如果数据库用tinyint(1)数据类型表示bool型，比如要写入true，SQL语句中可以写为'1'或者不带单引号的true
                            if (inputData == true)
                                values.Add("'1'");
                            else
                                values.Add("'0'");
                        }
                    }
                    else if (fieldInfo.DataType == DataType.Json)
                    {
                        // json型直接向数据库写入原始json字符串，但需要对\进行转义
                        values.Add(string.Format("'{0}'", fieldInfo.JsonString[i]).Replace("\\", "\\\\"));
                    }
                    else if (fieldInfo.DataType == DataType.MapString)
                    {
                        // mapString型也直接写入原始mapString数据字符串，并对\进行转义
                        values.Add(string.Format("'{0}'", fieldInfo.JsonString[i]).Replace("\\", "\\\\"));
                    }
                    // 这里需要自行处理数据库中某些数据类型（如datetime）中不允许插入空字符串的情况，以及用户设置的string型中空单元格导出至数据库的形式
                    else if (string.IsNullOrEmpty(fieldInfo.Data[i].ToString()))
                    {
                        if (fieldInfo.DatabaseFieldType.StartsWith("datetime", StringComparison.CurrentCultureIgnoreCase))
                            values.Add("NULL");
                        else if (fieldInfo.DataType == DataType.String && isWriteNullForEmptyString == true)
                            values.Add("NULL");
                        else
                            values.Add(string.Format("'{0}'", fieldInfo.Data[i].ToString()));
                    }
                    else
                    {
                        // 注意对\进行转义
                        values.Add(string.Format("'{0}'", fieldInfo.Data[i].ToString()).Replace("\\", "\\\\"));
                    }
                }

                valueDefineStringBuilder.AppendFormat("({0}),", Utils.CombineString(values, ","));
            }
            // 去掉末尾多余的逗号
            string valueDefineString = valueDefineStringBuilder.ToString();
            valueDefineString = valueDefineString.Substring(0, valueDefineString.Length - 1);

            string insertSqlString = string.Format(insertDataSQL, _CombineDatabaseTableFullName(tableName), fieldNameDefineString, valueDefineString);

            // 执行插入操作
            try
            {
                MySqlCommand cmd = new MySqlCommand(insertSqlString, conn);
                int insertCount = cmd.ExecuteNonQuery();
                if (insertCount < count)
                {
                    errorString = string.Format("需要插入{0}条数据但仅插入了{1}条", count, insertCount);
                    return false;
                }
                else
                {
                    errorString = null;
                    return true;
                }
            }
            catch (MySqlException exception)
            {
                errorString = exception.Message;
                return false;
            }
        }
        else
        {
            errorString = null;
            return true;
        }
    }

    private static bool _CreateTable(string tableName, TableInfo tableInfo, string comment,Export export, out string errorString)
    {

        // 将主键列作为key生成
        FieldInfo keyColumnField = tableInfo.GetKeyColumnFieldInfo();
        if (keyColumnField.DatabaseFieldName == null)
        {
            AppLog.Log("主键未设置，已忽略导入数据库！", ConsoleColor.Yellow);
            errorString = null;
            return true;
        }
        // 生成在创建数据表时所有字段的声明
        StringBuilder fieldDefineStringBuilder = new StringBuilder();
        foreach (FieldInfo fieldInfo in GetAllDatabaseFieldInfo(tableInfo))
        {
            // 在这里并不对每种本工具的数据类型是否能导出为指定的MySQL数据类型进行检查（比如本工具中的string型应该导出为MySQL中的文本类型如varchar，而不应该导出为数值类型）
            if (fieldInfo.DataType == DataType.Date)
            {
                //string toDatabaseFormatDefine = fieldInfo.ExtraParam[DateToExportFormatKey].ToString();
                string toDatabaseFormatDefine = export.DateToExportFormat;
               DateFormatType toDatabaseFormatType = DateTimeValue.GetDateFormatType(toDatabaseFormatDefine);
                if (fieldInfo.DatabaseFieldType.StartsWith("time", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorString = string.Format("date型字段\"{0}\"（列号：{1}）声明导出到MySQL中的数据类型错误，不允许为time型，如果仅需要时分秒部分，请在Excel中将该字段在本工具中的数据类型改为time型", fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1));
                    return false;
                }
                if (toDatabaseFormatType == DateFormatType.ReferenceDateSec || toDatabaseFormatType == DateFormatType.ReferenceDateMsec)
                {
                    if (fieldInfo.DatabaseFieldType.StartsWith("datetime", StringComparison.CurrentCultureIgnoreCase) || fieldInfo.DatabaseFieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase))
                    {
                        errorString = string.Format("date型字段\"{0}\"（列号：{1}）声明导出到MySQL中的形式为距1970年的时间（{2}），但所填写的导出到MySQL中的格式为时间型的{3}，请声明为MySQL中的数值型", fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1), toDatabaseFormatDefine, fieldInfo.DatabaseFieldType);
                        return false;
                    }
                }
            }
            else if (fieldInfo.DataType == DataType.Time)
            {
                //string toDatabaseFormatDefine = fieldInfo.ExtraParam[TimeToExportFormatKey].ToString();
                string toDatabaseFormatDefine = export.TimeToExportFormat;
                TimeFormatType toDatabaseFormatType = DateTimeValue.GetTimeFormatType(toDatabaseFormatDefine);
                if (fieldInfo.DatabaseFieldType.StartsWith("datetime", StringComparison.CurrentCultureIgnoreCase) || fieldInfo.DatabaseFieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorString = string.Format("time型字段\"{0}\"（列号：{1}）声明导出到MySQL中的数据类型错误，不允许为datetime或date型，如果需要年月日部分，请在Excel中将该字段在本工具中的数据类型改为date型", fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1));
                    return false;
                }
                if (toDatabaseFormatType == TimeFormatType.ReferenceTimeSec && fieldInfo.DatabaseFieldType.StartsWith("time", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorString = string.Format("time型字段\"{0}\"（列号：{1}）声明导出到MySQL中的形式为距0点的秒数（#sec），但所填写的导出到MySQL中的格式为时间型的time，请声明为MySQL中的数值型", fieldInfo.FieldName, ExcelMethods.GetExcelColumnName(fieldInfo.ColumnSeq + 1));
                    return false;
                }
            }

            fieldDefineStringBuilder.AppendFormat("`{0}` {1} COMMENT '{2}',", fieldInfo.DatabaseFieldName, fieldInfo.DatabaseFieldType, fieldInfo.Desc);
        }

        string createTableExtraParam = AppValues.ConfigData.ContainsKey(createDatabaseTableExtraParam) ? AppValues.ConfigData[createDatabaseTableExtraParam] : string.Empty;
        string createTableSql = string.Format(createTableSQL, _CombineDatabaseTableFullName(tableName), fieldDefineStringBuilder.ToString(), tableInfo.GetKeyColumnFieldInfo().DatabaseFieldName, comment, createTableExtraParam);

        try
        {
            MySqlCommand cmd = new MySqlCommand(createTableSql, conn);
            cmd.ExecuteNonQuery();
            errorString = null;
            return true;
        }
        catch (MySqlException exception)
        {
            errorString = exception.Message;
            return false;
        }
    }

    private static bool _DropTable(string tableName, out string errorString)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(string.Format(dropTableSQL, _CombineDatabaseTableFullName(tableName)), conn);
            cmd.ExecuteNonQuery();
            errorString = null;
            return true;
        }
        catch (MySqlException exception)
        {
            errorString = exception.Message;
            return false;
        }
    }

    /// <summary>
    /// 获取某张表格中对应要导出到数据库的字段集合
    /// </summary>
    public static List<FieldInfo> GetAllDatabaseFieldInfo(TableInfo tableInfo)
    {
        List<FieldInfo> allFieldInfo = new List<FieldInfo>();
        foreach (FieldInfo fieldInfo in tableInfo.GetAllFieldInfo())
            _GetOneDatabaseFieldInfo(fieldInfo, allFieldInfo);

        return allFieldInfo;
    }

    private static void _GetOneDatabaseFieldInfo(FieldInfo fieldInfo, List<FieldInfo> allFieldInfo)
    {
        if (fieldInfo.DataType == DataType.Array || fieldInfo.DataType == DataType.Dict)
        {
            foreach (FieldInfo childFieldInfo in fieldInfo.ChildField)
                _GetOneDatabaseFieldInfo(childFieldInfo, allFieldInfo);
        }
        // 忽略未配置导出到数据库信息的字段
        else if (fieldInfo.DatabaseFieldName != null)
            allFieldInfo.Add(fieldInfo);
    }

    /// <summary>
    /// 将数据库的表名连同Schema名组成形如'SchemaName'.'tableName'的字符串
    /// </summary>
    private static string _CombineDatabaseTableFullName(string tableName)
    {
        return string.Format("`{0}`.`{1}`", schemaName, tableName);
    }

}

