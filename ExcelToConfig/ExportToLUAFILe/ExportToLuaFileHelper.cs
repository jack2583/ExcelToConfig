﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportToLuaFileHelper
{
    public static void ExportToLuaFile(TableInfo tableInfo)
    {
       string errorString = null;

        TableExportToLuaFileHelper.ExportTableToLuaFile(tableInfo, out errorString);
        if (errorString != null)
            AppLog.LogErrorAndExit(errorString);
        else
            AppLog.Log("按默认方式导出LuaFile成功");
        
    }
}
