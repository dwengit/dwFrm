using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.System
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class SysRole : Entity<Guid>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色权限Code
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色状态（0正常 1停用）
        /// </summary>
        public EnumSysRoleStatus Status { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///排序
        /// </summary>
        public int OrderSort { get; set; }
    }
}
