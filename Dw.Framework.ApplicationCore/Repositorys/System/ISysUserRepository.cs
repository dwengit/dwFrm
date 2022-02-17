using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public interface ISysUserRepository : IRepository<SysUser, Guid>
    {
        Task<SysUser> InserUserToCache(string accountCode);
        void InserUserToCache(SysUser user);
        Task<SysUser> GetUserFromCache(string accountCode);
        Task<SysUser> GetUserFromCacheNotNull(string accountCode);
    }
}
