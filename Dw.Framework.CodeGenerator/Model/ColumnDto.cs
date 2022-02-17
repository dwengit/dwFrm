using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dw.Framework.CodeGenerator.Model
{
    public class ColumnDto
    {
        /// <summary>
        /// 列说明
        /// </summary>
        public string ColumnComment { get; set; } = string.Empty;
        /// <summary>
        /// C#类型
        /// </summary>
        public string CsharpType { get; set; }
        /// <summary>
        /// C# 字段名 首字母大写
        /// </summary>
        public string CsharpField { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPk { get; set; }
        /// <summary>
        /// 是否必填（1是）
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// 是否为可空类型
        /// </summary>
        public string RequiredStr
        {
            get
            {
                string[] arr = new string[] { "int", "long" };
                return (!IsRequired && (arr.Any(f => f.Contains(CsharpType))) || typeof(DateTime).Name == CsharpType) ? "?" : "";
            }
        }
    }
}
