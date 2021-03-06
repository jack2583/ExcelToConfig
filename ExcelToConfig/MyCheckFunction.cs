﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;//先导入这个使用正则表达式

/// <summary>
/// 该类定义那些复杂非通用的检查条件所需要手工编写的检查函数
/// 注意：自定义字段检查函数必须形如public static bool funcName(FieldInfo fieldInfo, out string errorString),建议函数名为CheckXxxField
/// 注意：自定义整表检查函数必须形如public static bool funcName(TableInfo tableInfo, out string errorString),建议函数名为CheckXxxTable
/// </summary>
public static class MyCheckFunction
{
    public static bool CheckItemID5(TableInfo tableInfo, out string errorString)
    {
        CheckItemIDBate( tableInfo, 5, out errorString);
        if (errorString == null)
            return true;
        else
        {
            return false;
        }

    }
    public static bool CheckItemID(TableInfo tableInfo, out string errorString)
    {
        CheckItemIDBate(tableInfo, 9, out errorString);
        if (errorString == null)
            return true;
        else
        {
            return false;
        }
    }
    public static bool CheckItemIDBate(TableInfo tableInfo,int num, out string errorString)
    {
        // 获取检查涉及的字段数据
        const string itemIdName = "id";
        const string itemMainTypeName = "main_type";
        const string itemSubTypeName = "sub_type";
        const string itemColorName = "color";
        const string itemLvlName = "item_lvl";
        const string itemTypeName = "type";

        List<object> itemIdValueList = null;
        List<object> itemMainTypeValueList = null;
        List<object> itemSubTypeValueList = null;
        List<object> itemColorValueList = null;
        List<object> itemLvlValueList = null;
        List<object> itemTypeValueList = null;

        //获取字段内容
        if (tableInfo.GetFieldInfoByFieldName(itemIdName) != null)
            itemIdValueList = tableInfo.GetFieldInfoByFieldName(itemIdName).Data;
        else
        {
            errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfo.TableName, itemIdName);
            return false;
        }
        if (tableInfo.GetFieldInfoByFieldName(itemMainTypeName) != null)
            itemMainTypeValueList = tableInfo.GetFieldInfoByFieldName(itemMainTypeName).Data;
        else
        {
            errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfo.TableName, itemMainTypeName);
            return false;
        }
        if (tableInfo.GetFieldInfoByFieldName(itemSubTypeName) != null)
            itemSubTypeValueList = tableInfo.GetFieldInfoByFieldName(itemSubTypeName).Data;
        else
        {
            errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfo.TableName, itemSubTypeName);
            return false;
        }
        if (tableInfo.GetFieldInfoByFieldName(itemColorName) != null)
            itemColorValueList = tableInfo.GetFieldInfoByFieldName(itemColorName).Data;
        else
        {
            errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfo.TableName, itemColorName);
            return false;
        }
        if (tableInfo.GetFieldInfoByFieldName(itemLvlName) != null)
            itemLvlValueList = tableInfo.GetFieldInfoByFieldName(itemLvlName).Data;
        else
        {
            errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfo.TableName, itemLvlName);
            return false;
        }
        if (tableInfo.GetFieldInfoByFieldName(itemTypeName) != null)
            itemTypeValueList = tableInfo.GetFieldInfoByFieldName(itemTypeName).Data;
        else
        {
            errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfo.TableName, itemTypeName);
            return false;
        }

        //先对以上字段非空检查
        if (ExcelFolder.IsAllowedNullNumber == true)
        {
            FieldCheckRule numberNotEmptyCheckRule = new FieldCheckRule();
            numberNotEmptyCheckRule.CheckType = TableCheckType.NotEmpty;
            numberNotEmptyCheckRule.CheckRuleString = "notEmpty";

            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(itemMainTypeName), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("{0}表中{1}字段中{2}\n", tableInfo.TableName, itemMainTypeName, errorString);
                return false;
            }
            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(itemSubTypeName), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("{0}表中{1}字段中{2}\n", tableInfo.TableName, itemSubTypeName, errorString);
                return false;
            }
            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(itemColorName), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("{0}表中{1}字段中{2}\n", tableInfo.TableName, itemColorName, errorString);
                return false;
            }
            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(itemLvlName), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("{0}表中{1}字段中{2}\n", tableInfo.TableName, itemLvlName, errorString);
                return false;
            }
            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(itemTypeName), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("{0}表中{1}字段中{2}\n", tableInfo.TableName, itemTypeName, errorString);
                return false;
            }
        }



        //resources-资源表
        const string resourcesTableName = "resources";
        const string resourcesIDName = "id";
        TableInfo tableInfoCheckResources = null;
        List<object> ResourcesIdValueList = null;
        if (AppValues.TableInfo.ContainsKey(resourcesTableName))
        {
            tableInfoCheckResources = AppValues.TableInfo[resourcesTableName];
            if (tableInfoCheckResources.GetFieldInfoByFieldName(resourcesIDName) != null)
                ResourcesIdValueList = tableInfoCheckResources.GetFieldInfoByFieldName(resourcesIDName).Data;
            else
            {
                errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfoCheckResources.TableName, resourcesIDName);
                return false;
            }
        }
        //resources-背包表
        const string packageTableName = "package";
        const string packageIDName = "package";
        TableInfo tableInfoCheckPackage = null;
        List<object> packageIdValueList = null;
        if (AppValues.TableInfo.ContainsKey(packageTableName))
        {
            tableInfoCheckPackage = AppValues.TableInfo[packageTableName];
            if (tableInfoCheckPackage.GetFieldInfoByFieldName(packageIDName) != null)
                packageIdValueList = tableInfoCheckPackage.GetFieldInfoByFieldName(packageIDName).Data;
            else
            {
                errorString = string.Format("\"{0}\"表中找不到名为\"{1}\"的字段，无法进行整表检查，请修正后重试\n", tableInfoCheckPackage.TableName, packageIDName);
                return false;
            }
        }

        int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        StringBuilder errorStringBuilder = new StringBuilder();
        for (int i = 0; i < dataCount; ++i)
        {
            int itemIdValue = (int)itemIdValueList[i];
            int itemMainTypeValue = (int)itemMainTypeValueList[i];
            int itemSubTypeValue = (int)itemSubTypeValueList[i];
            int itemColorValue = (int)itemColorValueList[i];
            int itemLvlValue = (int)itemLvlValueList[i];
            int itemTypeValue = (int)itemTypeValueList[i];

            if (itemMainTypeValue < 100 || itemMainTypeValue > 999)
            {
                errorStringBuilder.AppendFormat("第{0}行字段{1}值{2}不符合要求，值范围应在[100,999]之间\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, itemMainTypeName, itemMainTypeValue);
            }

            if (itemSubTypeValue < 0 || itemSubTypeValue > 99)
            {
                errorStringBuilder.AppendFormat("第{0}行字段{1}值{2}不符合要求，值范围应在[0,99]之间\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, itemSubTypeName, itemSubTypeValue);
            }

            if (itemColorValue < 0 || itemColorValue > 9)
            {
                errorStringBuilder.AppendFormat("第{0}行字段{1}值{2}不符合要求，值范围应在[0,9]之间\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, itemColorName, itemColorValue);
            }

            if (itemLvlValue < 0 || itemLvlValue > 999)
            {
                errorStringBuilder.AppendFormat("第{0}行字段{1}值{2}不符合要求，值范围应在[0,999]之间\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, itemLvlName, itemLvlValue);
            }
            int readid= itemMainTypeValue * 1000000 + itemSubTypeValue * 10000 + itemColorValue * 1000 + itemLvlValue;
            if (itemIdValue.ToString().Substring(0,num)  != readid.ToString().Substring(0, num))
            {
                errorStringBuilder.AppendFormat("第{0}行字段{1}值{2}不符合要求，应为{3}\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, itemIdName, itemIdValue, readid);
            }
            if (itemMainTypeValue == 100)
            {
                if (ResourcesIdValueList != null)
                {
                    if (!ResourcesIdValueList.Contains(itemIdValue))
                    {
                        errorStringBuilder.AppendFormat("第{0}行字段{1}值{2}不符合要求，因为main_type=100的必须要在{3}有对应id\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, itemIdName, itemIdValue, resourcesTableName);
                    }
                }
            }
            if (itemMainTypeValue == 100 && itemTypeValue == 1)
                continue;

            if (packageIdValueList != null)
            {
                if (!packageIdValueList.Contains(itemTypeValue))
                {
                    errorStringBuilder.AppendFormat("第{0}行字段{1}值{2}不符合要求，因为type字段的值必须要在{3}有对应id\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, itemTypeName, itemTypeValue, packageTableName);
                }
            }
        }

        string lackDataErrorString = errorStringBuilder.ToString();
        if (string.IsNullOrEmpty(lackDataErrorString))
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = string.Format("{0}表中存在以下配置缺失:\n{1}\n", tableInfo.TableName, lackDataErrorString);
            return false;
        }

    }

  public static bool CheckIsChinese(FieldInfo fieldInfo, out string errorString)
    {
        errorString = null;
        if (fieldInfo.DataType == DataType.String)
        {
            // 存储检查出的空值对应的行号
            List<int> ChineseDataLines = new List<int>();
            Regex reg = new Regex(@"[\uFE30-\uFFA0]");//(@"[\u4e00-\u9fa5]");//正则表达式
            for (int i = 0; i < fieldInfo.Data.Count; ++i)
            {
                if (reg.IsMatch(fieldInfo.Data[i].ToString()))
                {
                    ChineseDataLines.Add(i);
                }
            }
            if (ChineseDataLines.Count > 0)
            {
                StringBuilder errorStringBuild = new StringBuilder();
                errorStringBuild.Append("存在以下中文标点数据，行号分别为：");
                string separator = ", ";
                foreach (int lineNum in ChineseDataLines)
                    errorStringBuild.AppendFormat("{0}{1}", lineNum + ExcelTableSetting.DataFieldDataStartRowIndex + 1, separator);

                // 去掉末尾多余的", "
                errorStringBuild.Remove(errorStringBuild.Length - separator.Length, separator.Length);

                errorStringBuild.Append("\n");
                errorString = errorStringBuild.ToString();

                return false;
            }
        }
        else
        {
            errorString = string.Format("自定义函数CheckIsChinese只能检查string类型，{0}中{1}列{2}不是string类型", fieldInfo.TableName, fieldInfo.ColumnSeq, fieldInfo.ChildField);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 检查奖励列表字段是否配置正确，要求字段的数据结构必须为array[dict[3]:n]，定义一种奖励类型的三个int型字段分别叫type、id、count，每个奖励项的类型必须存在，除道具类型之外不允许奖励同一种类型，奖励数量必须大于0，如果是道具类型则道具id在道具表中要存在
    /// </summary>
    public static bool CheckRewardListField(FieldInfo fieldInfo, out string errorString)
    {
        // 道具类型对应的type
        int PROP_TYPE;
        const string PROP_TYPE_CONFIG_KEY = "propType";
        if (AppValues.ConfigData.ContainsKey(PROP_TYPE_CONFIG_KEY))
        {
            string configValue = AppValues.ConfigData[PROP_TYPE_CONFIG_KEY];
            if (int.TryParse(configValue, out PROP_TYPE) == false)
            {
                errorString = string.Format("config配置中用于表示道具类型对应数字（名为\"{0}\"）的配置项所填值不是合法的数字（你填的为\"{1}\"），无法进行奖励列表字段的检查，请修正配置后再重试\n", PROP_TYPE_CONFIG_KEY, configValue);
                return false;
            }
        }
        else
        {
            errorString = string.Format("config配置文件找不到名为\"{0}\"的表示道具类型对应数字的配置，无法进行奖励列表字段的检查，请填写配置后再重试\n", PROP_TYPE_CONFIG_KEY);
            return false;
        }
        // 合法奖励类型检查规则
        List<FieldCheckRule> CHECK_TYPE_RULES;
        const string TYPE_CONFIG_KEY = "$type";
        if (AppValues.ConfigData.ContainsKey(TYPE_CONFIG_KEY))
        {
            CHECK_TYPE_RULES = TableCheckHelper.GetCheckRules(AppValues.ConfigData[TYPE_CONFIG_KEY], out errorString);
            if (errorString != null)
            {
                errorString = string.Format("config文件中用于检查奖励类型是否合法的规则\"{0}\"有误，{1}\n", TYPE_CONFIG_KEY, errorString);
                return false;
            }
            if (CHECK_TYPE_RULES == null)
            {
                errorString = string.Format("config文件中用于检查奖励类型是否合法的规则\"{0}\"为空，无法进行奖励列表字段的检查，请填写配置后再重试\n", TYPE_CONFIG_KEY, errorString);
                return false;
            }
        }
        else
        {
            errorString = string.Format("config配置文件找不到名为\"{0}\"的表示合法奖励类型对应数字数组的配置，无法进行奖励列表字段的检查，请填写配置后再重试\n", TYPE_CONFIG_KEY);
            return false;
        }
        // 读取Prop表的主键id字段，用于填写道具id的值引用检查
        List<object> PROP_KEYS = null;
        const string PROP_TABLE_NAME = "Prop";
        if (AppValues.TableInfo.ContainsKey(PROP_TABLE_NAME))
            PROP_KEYS = AppValues.TableInfo[PROP_TABLE_NAME].GetKeyColumnFieldInfo().Data;
        else
        {
            errorString = string.Format("找不到名为\"{0}\"用于配置道具属性的表格，无法进行奖励列表字段的检查\n", PROP_TABLE_NAME);
            return false;
        }

        // 要求字段的数据结构必须为array[dict[3]:n]
        if (!(fieldInfo.DataType == DataType.Array && fieldInfo.ArrayChildDataType == DataType.Dict))
        {
            errorString = "奖励列表字段的数据结构必须为array[dict[3]:n]";
            return false;
        }

        // 标识组成一个奖励项的三个字段的名称（type、id、count）
        List<string> FIELD_NAMES = new List<string>();
        FIELD_NAMES.Add("type");
        FIELD_NAMES.Add("id");
        FIELD_NAMES.Add("count");
        // 要求定义一种奖励类型的三个int型字段分别叫type、id、count
        foreach (FieldInfo childDictField in fieldInfo.ChildField)
        {
            if (childDictField.ChildField.Count != 3)
            {
                errorString = string.Format("一个奖励项的dict必须由type、id、count三个int型字段组成，而你填写的奖励项由{0}个字段组成，出错的dict列号为{1}", childDictField.ChildField.Count, ExcelMethods.GetExcelColumnName(childDictField.ColumnSeq + 1));
                return false;
            }
            foreach (FieldInfo childBaseField in childDictField.ChildField)
            {
                if (!FIELD_NAMES.Contains(childBaseField.FieldName.ToLower()))
                {
                    errorString = string.Format("一个奖励项的dict必须由type、id、count三个int型字段组成，而你填写的奖励项中含有名为\"{0}\"的字段，出错的dict列号为{1}", childBaseField.FieldName, ExcelMethods.GetExcelColumnName(childDictField.ColumnSeq + 1));
                    return false;
                }
                else if (childBaseField.DataType != DataType.Int)
                {
                    errorString = string.Format("一个奖励项的dict必须由type、id、count三个int型字段组成，而你填写的奖励项中的{0}字段的数据类型为{1}，出错的dict列号为{2}", childBaseField.DataType, childBaseField.FieldName, ExcelMethods.GetExcelColumnName(childDictField.ColumnSeq + 1));
                    return false;
                }
            }
        }
        // 读取并检查每一行数据是否符合奖励列表的要求
        StringBuilder errorStringBuilder = new StringBuilder();
        for (int i = 0; i < fieldInfo.Data.Count; ++i)
        {
            // 如果本行定义的奖励列表声明为无效，直接跳过
            if ((bool)fieldInfo.Data[i] == false)
                continue;

            // 记录该行已配置的奖励类型（key：type， value：恒为true），但注意道具类型不计入因为允许奖励多种道具类型，只要id不同即可
            Dictionary<int, bool> rewardType = new Dictionary<int, bool>();
            // 记录该行已配置的奖励道具的id（key：道具id， value：恒为true）
            Dictionary<int, bool> rewardPropId = new Dictionary<int, bool>();

            //bool isCorrectForTheLineData = true;
            foreach (FieldInfo childDictField in fieldInfo.ChildField)
            {
                // 一旦读到用-1标识的无效数据，则不再读取array中后面的dict字段，即如果为array[dict[3]:4]并且第二个dict用-1标为无效数据，则认为此行仅配置了1个奖励项，不再读取判断第三四个奖励项是否配置
                if ((bool)childDictField.Data[i] == false)
                    break;

                int type = -1;
                int id = -1;
                int count = -1;
                foreach (FieldInfo field in childDictField.ChildField)
                {
                    if (field.FieldName.Equals("type", System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (field.Data[i] == null)
                        {
                            errorString = string.Format("第{0}行第{1}列\"type\"字段填写值为空", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ExcelMethods.GetExcelColumnName(field.ColumnSeq + 1));
                            return false;
                        }
                        else
                            type = (int)field.Data[i];
                    }
                    else if (field.FieldName.Equals("id", System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (field.Data[i] == null)
                        {
                            errorString = string.Format("第{0}行第{1}列\"id\"字段填写值为空", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ExcelMethods.GetExcelColumnName(field.ColumnSeq + 1));
                            return false;
                        }
                        else
                            id = (int)field.Data[i];
                    }
                    else if (field.FieldName.Equals("count", System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (field.Data[i] == null)
                        {
                            errorString = string.Format("第{0}行第{1}列\"count\"字段填写值为空", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ExcelMethods.GetExcelColumnName(field.ColumnSeq + 1));
                            return false;
                        }
                        else
                            count = (int)field.Data[i];
                    }
                }

                // 对于道具类型，需检查奖励的道具id不能重复且Prop表中要存在该道具id
                if (type == PROP_TYPE)
                {
                    if (rewardPropId.ContainsKey(id))
                        errorStringBuilder.AppendFormat("第{0}行的奖励列表中含有同种道具（id={1}）类型\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, id);
                    else
                        rewardPropId.Add(id, true);

                    if (!PROP_KEYS.Contains(id))
                        errorStringBuilder.AppendFormat("第{0}行第{1}列的奖励项中所填的奖励道具（id={2}）在道具表中不存在\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ExcelMethods.GetExcelColumnName(childDictField.ColumnSeq + 1), id);
                }
                // 对于非道具类型，需检查奖励类型type不能重复
                else
                {
                    if (rewardType.ContainsKey(type))
                        errorStringBuilder.AppendFormat("第{0}行的奖励列表中含有同种奖励类型（{1}）\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, type);
                    else
                        rewardType.Add(type, true);
                }
                // 均要检查奖励count不能低于1
                if (count < 1)
                    errorStringBuilder.AppendFormat("第{0}行第{1}列的奖励项中所填的奖励数量低于1\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, ExcelMethods.GetExcelColumnName(childDictField.ColumnSeq + 1), type);
            }
        }

        // 按字段检查所有type列所填的类型是否合法
        foreach (FieldInfo childDictField in fieldInfo.ChildField)
        {
            foreach (FieldInfo field in childDictField.ChildField)
            {
                if (field.FieldName.Equals("type", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    TableCheckHelper.CheckByRules(CHECK_TYPE_RULES, field, out errorString);
                    if (errorString != null)
                    {
                        errorStringBuilder.AppendFormat("检查type列发现以下无效type类型：\n{0}", errorString);
                        errorString = null;
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(errorStringBuilder.ToString()))
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = errorStringBuilder.ToString();
            return false;
        }
    }

    /// <summary>
    /// 检查HeroEquipment表，凡是填写的英雄都必须填写在所有品阶下四个槽位可穿戴装备信息
    /// </summary>
    public static bool CheckHeroEquipmentTable(TableInfo tableInfo, out string errorString)
    {
        // 从配置文件中读取英雄的所有品阶（key：品阶， value：恒为true）
        Dictionary<int, bool> HERO_QUALITY = new Dictionary<int, bool>();
        // 每品阶英雄所需穿戴的装备数量
        const int HERO_EQUIPMENT_COUNT = 4;

        const string HERO_QUALITY_CONFIG_KEY = "$heroQuality";
        if (AppValues.ConfigData.ContainsKey(HERO_QUALITY_CONFIG_KEY))
        {
            string configString = AppValues.ConfigData[HERO_QUALITY_CONFIG_KEY];
            // 去除首尾花括号后，通过英文逗号分隔每个有效值即可
            if (!(configString.StartsWith("{") && configString.EndsWith("}")))
            {
                errorString = string.Format("表示英雄所有品阶的配置{0}错误，必须在首尾用一对花括号包裹整个定义内容，请修正后重试\n", HERO_QUALITY_CONFIG_KEY);
                return false;
            }
            string temp = configString.Substring(1, configString.Length - 2).Trim();
            string[] values = temp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length < 1)
            {
                errorString = string.Format("表示英雄所有品阶的配置{0}错误，不允许为空值，请修正后重试\n", HERO_QUALITY_CONFIG_KEY);
                return false;
            }

            for (int i = 0; i < values.Length; ++i)
            {
                string oneValueString = values[i].Trim();
                int oneValue;
                if (int.TryParse(oneValueString, out oneValue) == true)
                {
                    if (HERO_QUALITY.ContainsKey(oneValue))
                        AppLog.LogWarning(string.Format("警告：表示英雄所有品阶的配置{0}错误，出现了相同的品阶\"{1}\"，本工具忽略此问题继续进行检查，需要你之后修正规则定义错误\n", HERO_QUALITY_CONFIG_KEY, oneValue));
                    else
                        HERO_QUALITY.Add(oneValue, true);
                }
                else
                {
                    errorString = string.Format("表示英雄所有品阶的配置{0}错误，出现了非int型有效值的规则定义，其为\"{1}\"，请修正后重试\n", HERO_QUALITY_CONFIG_KEY, oneValueString);
                    return false;
                }
            }
        }
        else
        {
            errorString = string.Format("config配置文件找不到名为\"{0}\"的表示英雄所有品阶的配置，无法进行HeroEquipment整表的检查，请填写配置后再重试\n", HERO_QUALITY_CONFIG_KEY);
            return false;
        }

        // 获取检查涉及的字段数据
        const string HERO_ID_FIELD_NAME = "heroId";
        const string HERO_QUALITY_FIELD_NAME = "heroQuality";
        const string SEQ_FIELD_NAME = "seq";
        List<object> heroIdList = null;
        List<object> heroQualityList = null;
        List<object> equipmentSeqList = null;
        if (tableInfo.GetFieldInfoByFieldName(HERO_ID_FIELD_NAME) != null)
            heroIdList = tableInfo.GetFieldInfoByFieldName(HERO_ID_FIELD_NAME).Data;
        else
        {
            errorString = string.Format("HeroEquipment表中找不到名为{0}的字段，无法进行整表检查，请修正后重试\n", HERO_ID_FIELD_NAME);
            return false;
        }
        if (tableInfo.GetFieldInfoByFieldName(HERO_QUALITY_FIELD_NAME) != null)
            heroQualityList = tableInfo.GetFieldInfoByFieldName(HERO_QUALITY_FIELD_NAME).Data;
        else
        {
            errorString = string.Format("HeroEquipment表中找不到名为{0}的字段，无法进行整表检查，请修正后重试\n", HERO_QUALITY_FIELD_NAME);
            return false;
        }
        if (tableInfo.GetFieldInfoByFieldName(SEQ_FIELD_NAME) != null)
            equipmentSeqList = tableInfo.GetFieldInfoByFieldName(SEQ_FIELD_NAME).Data;
        else
        {
            errorString = string.Format("HeroEquipment表中找不到名为{0}的字段，无法进行整表检查，请修正后重试\n", SEQ_FIELD_NAME);
            return false;
        }

        // 先对heroId、quality、seq三个字段进行非空检查，避免填写空值
        if (ExcelFolder.IsAllowedNullNumber == true)
        {
            FieldCheckRule numberNotEmptyCheckRule = new FieldCheckRule();
            numberNotEmptyCheckRule.CheckType = TableCheckType.NotEmpty;
            numberNotEmptyCheckRule.CheckRuleString = "notEmpty";

            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(HERO_ID_FIELD_NAME), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("HeroEquipment表中{0}字段中{1}\n", HERO_ID_FIELD_NAME, errorString);
                return false;
            }
            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(HERO_QUALITY_FIELD_NAME), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("HeroEquipment表中{0}字段中{1}\n", HERO_QUALITY_FIELD_NAME, errorString);
                return false;
            }
            TableCheckHelper.CheckNotEmpty(tableInfo.GetFieldInfoByFieldName(SEQ_FIELD_NAME), numberNotEmptyCheckRule, out errorString);
            if (errorString != null)
            {
                errorString = string.Format("HeroEquipment表中{0}字段中{1}\n", SEQ_FIELD_NAME, errorString);
                return false;
            }
        }

        // 记录实际填写的信息（key：从外层到内层依次表示heroId、quality、seq，最内层value为从0开始计的数据行序号）
        Dictionary<int, Dictionary<int, Dictionary<int, int>>> inputData = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();

        int dataCount = tableInfo.GetKeyColumnFieldInfo().Data.Count;
        StringBuilder errorStringBuilder = new StringBuilder();
        for (int i = 0; i < dataCount; ++i)
        {
            int heroId = (int)heroIdList[i];
            int heroQuality = (int)heroQualityList[i];
            int seq = (int)equipmentSeqList[i];
            if (!inputData.ContainsKey(heroId))
                inputData.Add(heroId, new Dictionary<int, Dictionary<int, int>>());

            Dictionary<int, Dictionary<int, int>> qualityInfo = inputData[heroId];
            if (!qualityInfo.ContainsKey(heroQuality))
                qualityInfo.Add(heroQuality, new Dictionary<int, int>());

            Dictionary<int, int> seqInfo = qualityInfo[heroQuality];
            if (seqInfo.ContainsKey(seq))
                errorStringBuilder.AppendFormat("第{0}行与第{1}行完全重复\n", i + ExcelTableSetting.DataFieldDataStartRowIndex + 1, seqInfo[seq] + ExcelTableSetting.DataFieldDataStartRowIndex + 1);
            else
                seqInfo.Add(seq, i);
        }

        string repeatedLineErrorString = errorStringBuilder.ToString();
        if (!string.IsNullOrEmpty(repeatedLineErrorString))
        {
            errorString = string.Format("HeroEquipment表中以下行中的heroId、heroQuality、seq字段与其他行存在完全重复的错误:\n{0}\n", repeatedLineErrorString);
            return false;
        }

        // 检查配置的每个英雄是否都含有所有品阶下四个槽位的可穿戴装备信息
        foreach (var heroInfo in inputData)
        {
            var qualityInfo = heroInfo.Value;
            List<int> qualityList = new List<int>(qualityInfo.Keys);
            List<int> LEGAL_QUALITY_LIST = new List<int>(HERO_QUALITY.Keys);
            foreach (int quality in LEGAL_QUALITY_LIST)
            {
                // 检查是否含有所有品阶
                if (!qualityList.Contains(quality))
                    errorStringBuilder.AppendFormat("英雄（heroId={0}）缺少品质为{1}的装备配置\n", heroInfo.Key, quality);
            }
            // 检查每个品阶下是否配全了四个槽位的装备信息
            foreach (var oneQualityInfo in qualityInfo)
            {
                var seqInfo = oneQualityInfo.Value;
                List<int> seqList = new List<int>(seqInfo.Keys);
                for (int seq = 1; seq <= HERO_EQUIPMENT_COUNT; ++seq)
                {
                    if (!seqList.Contains(seq) && LEGAL_QUALITY_LIST.Contains(oneQualityInfo.Key))
                        errorStringBuilder.AppendFormat("英雄（heroId={0}）在品质为{1}下缺少第{2}个槽位的装备配置\n", heroInfo.Key, oneQualityInfo.Key, seq);
                }
            }
        }

        string lackDataErrorString = errorStringBuilder.ToString();
        if (string.IsNullOrEmpty(lackDataErrorString))
        {
            errorString = null;
            return true;
        }
        else
        {
            errorString = string.Format("HeroEquipment表中存在以下配置缺失:\n{0}\n", lackDataErrorString);
            return false;
        }
    }
}