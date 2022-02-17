using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public interface ISysPrivilegeRepository : IRepository<SysPrivilege, Guid> 
    {
        IEnumerable<Guid> GetUserAllPermissionIds(Guid userId, IEnumerable<Guid> roleIds);
        IEnumerable<Guid> GetUserAllPermissionIdsCatch(SysUser user, IEnumerable<Guid> roleIds);
        /// <summary>
        /// 刷新用户所有权限Code缓存
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        IEnumerable<Guid> RefreshUserAllPermissionIdsCatch(SysUser user, IEnumerable<Guid> roleIds);
    }
}
