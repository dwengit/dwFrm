using Dw.Framework.Infrastructure;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dw.Framework.CodeGenerator
{
    public class CodeGeneraterService : DbProvider
    {
        /// <summary>
        /// 获取所有数据库名
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllDataBases(string conn = "", int dbType = 0)
        {
            var db = GetSugarDbContext("", conn, dbType);
            try
            {
                return db.DbMaintenance.GetDataBaseList(db).Where(w => !GenConstants.FILTER_DB.Contains(w)).ToList();
            }
            catch (Exception)
            {
                throw new CustomException(ResultCodeEnums.PARAM_ERROR, "链接数据库失败");
            }
        }
        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public IList<DbTableInfo> GetAllTables(string dbName, string tableName, string conn, int dbType)
        {
            var tableList = GetSugarDbContext(dbName, conn, dbType).DbMaintenance.GetTableInfoList(true);
            if (!string.IsNullOrEmpty(tableName))
            {
                tableList = tableList.Where(f => f.Name.ToLower().Contains(tableName.ToLower())).ToList();
            }
           return tableList;
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IList<DbColumnInfo> GetColumnInfo(string dbName, string tableName)
        {
            return GetSugarDbContext(dbName).DbMaintenance.GetColumnInfosByTableName(tableName, true);
        }
    }
}
