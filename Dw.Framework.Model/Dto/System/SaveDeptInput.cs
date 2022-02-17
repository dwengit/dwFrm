using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class SaveDeptInput
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 父部门ID
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        public string DeptName { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        [Required]
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
        /// 邮箱，多个逗号分割
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 部门状态:0正常,1停用
        /// </summary>
        [Required]
        public EnumSysDept Status { get; set; }
    }
}
