using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.System
{
    public class SysPrivilege : Entity<Guid>
    {
        /// <summary>
        /// 资源拥有者（角色/用户）
        /// </summary>
        public Guid OwnerId { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public EnumOwnerIdentityType OwnerType { get; set; }
        /// <summary>
        /// 资源Id
        /// </summary>
        public Guid ResourceId { get; set; }
    }
}
