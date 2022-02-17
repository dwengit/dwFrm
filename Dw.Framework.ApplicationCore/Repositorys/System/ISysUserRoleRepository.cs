using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public interface ISysUserRoleRepository : IRepository<SysUserRole, Guid>
    {
        /// <summary>
        /// 获取用户关联的角色Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Guid> GetUserRoleIds(Guid userId);
    }
}
