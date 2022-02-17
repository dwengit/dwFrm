using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dw.Framework.ApplicationCore.Repositorys.Accounts
{
    public class SysUserRoleRepository : Repository<SysUserRole, Guid>, ISysUserRoleRepository
    {
        /// <summary>
        /// 获取用户关联的角色Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetUserRoleIds(Guid userId)
        {
           return Query(w => w.UserId == userId && !w.IsDelete).Select(s => s.RoleId).AsEnumerable();
        }
    }
}
