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
    public class SysResourceRepository : Repository<SysModuleResource, Guid>, ISysResourceRepository
    {
        private readonly RedisHashService _redisHashService;
        public SysResourceRepository(RedisHashService redisHashService)
        {
            this._redisHashService = redisHashService;
        }
        public const string RESOURCE_CACHE = "module_resource";
        public async Task<List<SysModuleResource>> LoadAllResourceToCache()
        {
            var resource = await QueryNoTracking(w => !w.IsDelete).ToListAsync();
            _redisHashService.Remove(RESOURCE_CACHE);
            foreach (var role in resource)
            {
                var res = _redisHashService.SetEntryInHash(RESOURCE_CACHE, role.Id.ToString(), role.ObjToJson());
                if (!res)
                {
                    throw new CustomException(ResultCodeEnums.CODE500);
                }
            }
            return resource;
        }

        public void InsertResourceToCache(SysModuleResource resource)
        {
            string keyId = resource.Id.ToString();
            _redisHashService.SetEntryInHash(RESOURCE_CACHE, keyId, resource.ObjToJson());
        }

        public async Task<List<SysModuleResource>> GetAllResourceFromCache()
        {
            var strResourceCache = _redisHashService.GetHashValues(RESOURCE_CACHE);
            if (strResourceCache.Count == 0)
            {
                return (await LoadAllResourceToCache()).OrderBy(w => w.SortNO).ToList();
            }
            return strResourceCache.ListStrJsonToObj<SysModuleResource>().OrderBy(w => w.SortNO).ToList();
        }
        public void RemoveResourceToCache(Guid id)
        {
            _redisHashService.RemoveEntryFromHash(RESOURCE_CACHE, id.ToString());
        }
    }
}
