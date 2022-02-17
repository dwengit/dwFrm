using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class DeptTreeOptions
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 父部门ID
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<DeptTreeOptions> Children { get; set; }
    }
}
