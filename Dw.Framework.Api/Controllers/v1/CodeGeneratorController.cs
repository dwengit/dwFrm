using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.CodeGenerator;
using Dw.Framework.CodeGenerator.Model;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dw.Framework.Admin.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class CodeGeneratorController : ApiControllersBase
    {
        private readonly CodeGeneraterService _codeGeneraterService = new CodeGeneraterService();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CodeGeneratorController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// 获取所有的数据名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Respbase<IList<string>> GetAllDataBases(string addres, string dbName, string userName, string pwd, string port, int dbType)
        {
            string conn = BuildSql(addres, dbName, userName, pwd, port);
            dbType = conn == "" ? -1 : dbType;
            var dbList = _codeGeneraterService.GetAllDataBases(conn, dbType);
            return RespOk(dbList);
        }
        private string BuildSql(string addres, string dbName, string userName, string pwd, string port)
        {
            if (!string.IsNullOrEmpty(addres) && !string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(port))
            {
                return $"server={addres};Database={dbName};UserId={userName};Password={pwd};Port={port};";
            }
            return null;
        }
        /// <summary>
        /// 获取所有表根据数据库名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Tools.CodeRead)]
        public Respbase<IList<DbTableInfo>> GetAllTables(string dbName, string tableName, string addres,  string userName, string pwd, string port, int dbType)
        {
            string conn = BuildSql(addres, dbName, userName, pwd, port);
            dbType = conn == "" ? -1 : dbType;
            var dbList = _codeGeneraterService.GetAllTables(dbName, tableName, conn, dbType);
            return RespOk(dbList);
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Tools.CodeRead)]
        public Respbase<IList<DbColumnInfo>> GetColumnInfo(string dbName, string tableName)
        {
            var res = _codeGeneraterService.GetColumnInfo(dbName, tableName);
            return RespOk(res);
        }
        /// <summary>
        /// 生成打包代码
        /// </summary>
        /// <param name="input"></param>
        /// <returns>返回打包后的路径</returns>
        [HttpPost]
        [PermissionFilter(_Tools.CodeBuild)]
        [Log(Title = "代码生成", BusinessType = BusinessType.GENCODE)]
        public Respbase<object> CodeGenerate(GenerateInput input)
        {
            GenerateDto dto = new GenerateDto()
            {
                GenCodePath = input.GenCodePath,
                NamespaceName = input.NamespaceName,
                TableNames = input.TableNames,
                ZipPath = "GenerateCode",
            };
            foreach (var tableName in input.TableNames)
            {
                var val = _codeGeneraterService.GetColumnInfo(input.DbName, tableName);
                dto.TableColumns.Add(tableName, val);
            }

            dto.ZipPath = Path.Combine(_webHostEnvironment.WebRootPath, dto.ZipPath);
            dto.GenCodePath = Path.Combine(dto.ZipPath, DateTime.Now.ToString("yyyyMMdd"));

            //生成压缩包
            string zipReturnFileName = $"{DateTime.Now:MMddHHmmss}.zip";

            //生成代码到指定文件夹
            CodeGeneratorTool.Generate(dto);
            //下载文件
            FileHelper.ZipGenCode(dto.ZipPath, dto.GenCodePath, zipReturnFileName);

            return RespOk((object)new { path = "/GenerateCode/" + zipReturnFileName });
        }
    }
}
