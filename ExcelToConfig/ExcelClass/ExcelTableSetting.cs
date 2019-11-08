public class ExcelTableSetting
{
    /// <summary>
    /// Excel临时文件的文件名前缀
    /// </summary>
    public const string ExcelTempFileFileNameStartString = "~$";

    #region Data数据sheet表格式要求

    /// <summary>
    /// Excel文件中存放数据的工作簿Sheet名。除预设功能的特殊Sheet表之外，其余Sheet表可自定义内容，不会被本工具导出
    /// </summary>
    public const string ExcelDataSheetName = "data$";

    /// <summary>
    /// 字段描述（中文），配置在第1行（行号从0开始）
    /// </summary>
    public static int DataFieldDescRowIndex = 0;//0

    /// <summary>
    /// 字段名（英文），配置在第2行（行号从0开始）
    /// </summary>
    public static int DataFieldNameRowIndex = 1;//1

    /// <summary>
    /// 字段数据类型（指本工具所使用的数据类型），配置在第3行（行号从0开始）
    /// </summary>
    public static int DataFieldDataTypeRowIndex = 2;//2

    /// <summary>
    /// 字段检查规制（可留空不配置），配置在第4行（行号从0开始）
    /// </summary>
    public static int DataFieldCheckRuleRowIndex = 3;//3

    /// <summary>
    /// 导出到数据库（如mysql、sqlite等）中对应数据库所使用的字段名及字段类型，配置格式为：FileName(FileType)，配置在第5行（行号从0开始）
    /// </summary>
    public static int DataFieldExportDataBaseFieldInFoRowIndex = 4;//4

    /// <summary>
    /// 正式配置数据，从第6行开始配置（行号从0开始）
    /// </summary>
    public static int DataFieldDataStartRowIndex = 5;//5

    #endregion Data数据sheet表格式要求

    #region config配置sheet表格式要求

    /// <summary>
    /// Excel文件中存放该表格配置的工作簿Sheet名
    /// </summary>
    public const string ExcelConfigSheetName = "config$";

    /// <summary>
    /// 每张配置表中的一列为一个配置参数的声明，其中第1行声明参数名，其余行声明具体参数（行号从0开始）
    /// </summary>
    public const int ConfigFieldDefingRowIndex = 0;

    /// <summary>
    /// 从第2行开始，每个参数配置1行（行号从0开始）
    /// </summary>
    public const int ConfigFieldParamStartRowIndex = 1;

    #endregion config配置sheet表格式要求
}