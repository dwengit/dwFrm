﻿using Dw.Framework.Infrastructure.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dw.Framework.CodeGenerator
{
    public class DbProvider
    {
        protected static SqlSugarClient CodeDb;

        /// <summary>
        /// 获取动态连接字符串
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        public SqlSugarClient GetSugarDbContext(string dbName = "", string conn = "", int dbType = -1)
        {
            string connStr = !string.IsNullOrEmpty(conn) ? conn : Appsettings.App(GenConstants.Gen_conn);
            dbType = dbType == -1 ? int.Parse(Appsettings.App(GenConstants.Gen_conn_dbType)) : dbType;

            if (!string.IsNullOrEmpty(dbName))
            {
                string replaceStr = GetValue(connStr, "Database=", ";");
                connStr = connStr.Replace(replaceStr, dbName);
            }
            var db = new SqlSugarClient(new List<ConnectionConfig>()
            {
                new ConnectionConfig(){
                    ConnectionString = connStr,
                    DbType = (DbType)dbType,
                    IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样
                    InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                },
            });

            CodeDb = db;
            return db;
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
    }
}
