using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class SaveRoleInput
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string RoleName { get; set; }
        /// <summary>
        /// 角色权限Code
        /// </summary>
        [Required]
        public string RoleCode { get; set; }
        /// <summary>
        /// 帐号状态（0正常 1停用）
        /// </summary>
        [Required]
        public EnumSysRoleStatus Status { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///排序
        /// </summary>
        [Required]
        public int OrderSort { get; set; }
    }
}
