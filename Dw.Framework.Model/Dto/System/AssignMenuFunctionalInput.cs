using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class AssignMenuFunctionalInput
    {
        /// <summary>
        /// 用户/角色
        /// </summary>
        public EnumOwnerIdentityType EnumOwnerIdentityType { get; set; }
        /// <summary>
        /// 拥有者id
        /// </summary>
        public Guid OwnerId { get; set; }
        /// <summary>
        /// 选中的模块/功能ID
        /// </summary>
        public List<Guid> MenuFunctionalIds { get; set; }
    }
}
