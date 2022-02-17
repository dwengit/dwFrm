using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class DeptPageTree
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
        /// 当前代数Code
        /// </summary>
        public int CurrentLevelCode { get; set; }
        /// <summary>
        /// 祖级列表Code
        /// </summary>
        public string AncestorsCode { get; set; }
        /// <summary>
        /// 祖级列表全路径名称
        /// </summary>
        public string AncestorsFullName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int SortNO { get; set; }
        /// <summary>
        /// 负责人，多个逗号分割
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 联系电话，多个逗号分割
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 部门状态:0正常,1停用
        /// </summary>
        public EnumSysDept Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; private set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<DeptPageTree> Children { get; set; }
    }
}
