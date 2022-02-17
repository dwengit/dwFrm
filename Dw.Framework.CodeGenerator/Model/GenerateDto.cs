using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.CodeGenerator.Model
{
    /// <summary>
    /// 生成表传入参数
    /// </summary>
    public class GenerateInput
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        /// 命名空间名称
        /// </summary>
        public string NamespaceName { get; set; }
        /// <summary>
        /// 要生成代码的表
        /// </summary>
        public IList<string> TableNames { get; set; }
        /// <summary>
        /// 代码生成路径
        /// </summary>
        public string GenCodePath { get; set; } = "/Generatecode/";
    }
    /// <summary>
    /// 
    /// </summary>
    public class GenerateDto : GenerateInput
    {
        /// <summary>
        /// 表列信息
        /// </summary>
        public Dictionary<string, IList<DbColumnInfo>> TableColumns { get; set; } = new Dictionary<string, IList<DbColumnInfo>>();
        /// <summary>
        /// 代码模板预览存储路径存放
        /// </summary>
        public IList<GenCode> GenCodes { get; set; } = new List<GenCode>();
        /// <summary>
        /// 代码生成配置选项
        /// </summary>
        public CodeGenerateConfig GenCodeDto { get; set; }
        /// <summary>
        /// 代码生成压缩包路径
        /// </summary>
        public string ZipPath { get; set; }
    }
    public class GenCode
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }

        public GenCode(string title, string path, string content)
        {
            Title = title;
            Path = path;
            Content = content;
        }
    }
}
