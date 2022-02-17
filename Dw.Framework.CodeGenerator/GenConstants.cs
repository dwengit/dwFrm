using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.CodeGenerator
{
    public class GenConstants
    {
        public static string Gen_conn = "gen:conn";
        public static string Gen_conn_dbType = "gen:dbType";
        /// <summary>
        /// Entity基类字段
        /// </summary>
        public static string[] BASE_ENTITY = { "Id", "CreateTime", "UpdateTime", "IsDelete", "DeleteTime" };
        /// <summary>
        /// 过滤表
        /// </summary>
        public static string[] FILTER_DB = { "mysql", "information_schema", "performance_schema", "sys" };
    }
}
