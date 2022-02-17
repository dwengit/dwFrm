using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.System
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public class SysUserRole : Entity<Guid>
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 角色表
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
