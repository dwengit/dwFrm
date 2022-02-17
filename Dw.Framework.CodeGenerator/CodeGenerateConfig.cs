using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dw.Framework.Infrastructure;
using Dw.Framework.CodeGenerator.Model;

namespace Dw.Framework.CodeGenerator
{
    public class CodeGenerateConfig
    {
        /// <summary>
        /// 项目命名空间
        /// </summary>
        public string BaseNamespace { get; set; }
        /// <summary>
        /// 数据实体命名空间
        /// </summary>
        public string ModelsNamespace { get; set; }
        /// <summary>
        /// 输入输出数据实体名称空间
        /// </summary>
        public string DtosNamespace { get; set; }
        /// <summary>
        /// 仓储接口命名空间
        /// </summary>
        public string IRepositoriesNamespace { get; set; }
        /// <summary>
        /// 仓储实现名称空间
        /// </summary>
        public string RepositoriesNamespace { get; set; }
        /// <summary>
        /// 服务接口命名空间
        /// </summary>
        public string IServicsNamespace { get; set; }
        /// <summary>
        /// 服务接口实现命名空间
        /// </summary>
        public string ServicsNamespace { get; set; }
        /// <summary>
        /// Api控制器命名空间
        /// </summary>
        public string ApiControllerNamespace { get; set; }
        /// <summary>
        /// 表名去下划线
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// Csharp表名
        /// </summary>
        public string AbbrCodeName { get; set; }
        /// <summary>
        /// 业务类型名称
        /// </summary>
        public string BusTypeName { get; set; }
        /// <summary>
        /// 表列信息
        /// </summary>
        public IList<ColumnDto> Columns { get; set; }
        /// <summary>
        /// 代码生成时间
        /// </summary>
        public string AddTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 主键字段
        /// </summary>
        public string PKName { get; set; }
        /// <summary>
        /// 主键类型
        /// </summary>
        public string PKType { get; set; }
        /// <summary>
        /// 构建默认的代码生成器
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static CodeGenerateConfig BuildDefault(string namespaceName, string tableName, IList<DbColumnInfo> tableColumns)
        {
            CodeGenerateConfig config = new CodeGenerateConfig();
            config.BusTypeName = tableName.Substring(0, tableName.IndexOf('_')).FirstUpperCase();
            config.AbbrCodeName = ConvertCodeName(tableName).FirstUpperCase();
            config.CodeName = tableName.UnderScoreToCamelCase().FirstUpperCase();
            config.BaseNamespace = namespaceName;
            config.ModelsNamespace = namespaceName + "Model";
            config.DtosNamespace = config.ModelsNamespace + ".Dto";
            config.IRepositoriesNamespace = namespaceName + "ApplicationCore.Repositorys." + config.BusTypeName;
            config.RepositoriesNamespace = config.IRepositoriesNamespace;
            config.IServicsNamespace = namespaceName + "ApplicationCore.IAppServices." + config.BusTypeName;
            config.ServicsNamespace = namespaceName + "ApplicationCore.AppServices." + config.BusTypeName;
            config.ApiControllerNamespace = namespaceName + "Admin.Api";
            config.Columns = ConvertColumnDto(tableColumns.Where(w => !GenConstants.BASE_ENTITY.Contains(w.DbColumnName)).ToList());
            var pmk = tableColumns.FirstOrDefault(f => f.IsPrimarykey);
            if(pmk != null)
            {
                config.PKName = pmk.DbColumnName;
                string typeName = GetCSharpDatatype(pmk.DataType);
                config.PKType = typeName == "string" ? "Guid" : typeName;
            }
            return config;
        }
        /// <summary>
        /// 构建默认的代码生成器
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IList<CodeGenerateConfig> BuildDefault(string namespaceName, Dictionary<string, IList<DbColumnInfo>> tables)
        {
            IList<CodeGenerateConfig> configs = new List<CodeGenerateConfig>();
            foreach (var table in tables)
            {
                configs.Add(BuildDefault(namespaceName, table.Key, table.Value));
            }
            return configs;
        }
        /// <summary>
        /// 表名转代码名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string ConvertCodeName(string tableName)
        {
            var rmIndex = tableName.IndexOf('_');
            if (rmIndex >= 0)
            {
                tableName = tableName.Substring(rmIndex + 1);
            }
            return tableName.UnderScoreToCamelCase();
        }
        /// <summary>
        /// 数据库列转Csharp属性
        /// </summary>
        /// <param name="dbColumns"></param>
        /// <returns></returns>
        public static IList<ColumnDto> ConvertColumnDto(IList<DbColumnInfo> dbColumns)
        {
            IList<ColumnDto> columns = new List<ColumnDto>();
            foreach (var dbColumn in dbColumns)
            {
                ColumnDto column = new ColumnDto();
                column.ColumnComment = dbColumn.ColumnDescription;
                column.IsPk = dbColumn.IsPrimarykey;
                column.IsRequired = !dbColumn.IsNullable;
                column.CsharpField = dbColumn.DbColumnName.FirstUpperCase();
                column.CsharpType = GetCSharpDatatype(dbColumn.DataType);
                if(column.CsharpType == "string" && dbColumn.Length == 36)
                {
                    column.CsharpType = "Guid";
                }
                columns.Add(column);
            }
            return columns;
        }
        /// <summary>
        /// 获取C# 类型
        /// </summary>
        /// <param name="sDatatype"></param>
        /// <returns></returns>
        public static string GetCSharpDatatype(string sDatatype)
        {
            sDatatype = sDatatype.ToLower();
            string sTempDatatype = sDatatype switch
            {
                "int" or "number" or "integer" or "smallint" => "int",
                "bigint" => "long",
                "tinyint" => "byte",
                "numeric" or "real" or "float" => "float",
                "decimal" or "numer(8,2)" or "numeric" => "decimal",
                "bit" => "bool",
                "date" or "datetime" or "datetime2" or "smalldatetime" or "timestamp" => "DateTime",
                "money" or "smallmoney" => "decimal",
                _ => "string",
            };
            return sTempDatatype;
        }
        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="tableDataType"></param>
        /// <returns></returns>
        public static bool IsNumber(string tableDataType)
        {
            string[] arr = new string[] { "int", "long" };
            return arr.Any(f => f.Contains(GetCSharpDatatype(tableDataType)));
        }
    }
}
