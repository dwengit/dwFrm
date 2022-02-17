using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Caches.RedisHelper.Service;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Helper;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.Accounts
{
    public class SysRoleRepository : Repository<SysRole, Guid>, ISysRoleRepository
    {
        private readonly RedisHashService _redisHashService;
        public SysRoleRepository(RedisHashService redisHashService)
        {
            this._redisHashService = redisHashService;
        }
        public const string ROLE_CACHE = "roles";
        public async Task<List<SysRole>> LoadAllRolesToCache()
        {
            var roles = await QueryNoTracking(w => !w.IsDelete).ToListAsync();
            _redisHashService.Remove(ROLE_CACHE);
            foreach (var role in roles)
            {
                _redisHashService.SetEntryInHash(ROLE_CACHE, role.Id.ToString(), role.ObjToJson());
            }
            return roles;
        }
        public void InsertRoleToCache(SysRole role)
        {
            string keyId = role.Id.ToString();
            _redisHashService.SetEntryInHashIfNotExists(ROLE_CACHE, keyId, role.ObjToJson());
        }
        public async Task<List<SysRole>> GetAllRoleFromCache()
        {
            var strRolesCache = _redisHashService.GetHashValues(ROLE_CACHE);
            if (strRolesCache.Count == 0)
            {
                return await LoadAllRolesToCache();
            }
            return strRolesCache.ListStrJsonToObj<SysRole>();
        }
        public void RemoveRoleCache(Guid id)
        {
            _redisHashService.RemoveEntryFromHash(ROLE_CACHE, id.ToString());
        }
        /// <summary>
        /// 根据角色ids返回过滤好的角色信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysRole>> FilterRolesByRoles(IEnumerable<Guid> ids)
        {
            var roles = await GetAllRoleFromCache();
            var res = roles.Where(w => !w.IsDelete && w.Status == Model.Enums.EnumSysRoleStatus.ENABLE && ids.Contains(w.Id));
            return res;
        }
        /// 根据角色ids返回过滤好的角色id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Guid>> FilterRolesByRoleIds(IEnumerable<Guid> ids)
        {
            return (await FilterRolesByRoles(ids)).Select(s => s.Id);
        }
    }
}
