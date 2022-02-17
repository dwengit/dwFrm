using Dw.Framework.Infrastructure.Caches.RedisHelper.Service;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Helper;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public class SysPrivilegeRepository : Repository<SysPrivilege, Guid>, ISysPrivilegeRepository
    {
        private readonly RedisStringService _redisStringService;
        public SysPrivilegeRepository(RedisStringService redisStringService)
        {
            this._redisStringService = redisStringService;
        }
        public const string PRIVILEGE_CACHE = "user_all_permission:";
        public static string GetCatchName(string accountCode)
        {
            return PRIVILEGE_CACHE + accountCode;
        }
        public IEnumerable<Guid> GetUserAllPermissionIds(Guid userId, IEnumerable<Guid> roleIds)
        {
            return Query().Where(w => !w.IsDelete).Where(w => roleIds.Contains(w.OwnerId) || w.OwnerId == userId).Select(w => w.ResourceId).ToList();
        }
        /// <summary>
        /// 获取用户全部已缓存的用户权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetUserAllPermissionIdsCatch(SysUser user, IEnumerable<Guid> roleIds)
        {
            var strJson = _redisStringService.Get(GetCatchName(user.AccountCode));
            if (string.IsNullOrEmpty(strJson))
            {
                var pids = GetUserAllPermissionIds(user.Id, roleIds).Distinct();
                _redisStringService.Set(GetCatchName(user.AccountCode), pids, TimeSpan.FromMinutes(40));
                return pids;
            }
            return strJson.StrJsonToObj<List<Guid>>();
        }
        /// <summary>
        /// 刷新用户所有权限Code缓存
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public IEnumerable<Guid> RefreshUserAllPermissionIdsCatch(SysUser user, IEnumerable<Guid> roleIds)
        {
            _redisStringService.Remove(GetCatchName(user.AccountCode));
            var pids = GetUserAllPermissionIds(user.Id, roleIds).Distinct();
            _redisStringService.Set(GetCatchName(user.AccountCode), pids, TimeSpan.FromMinutes(40));
            return pids;
        }
    }
}
