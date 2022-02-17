using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public interface ISysResourceRepository : IRepository<SysModuleResource, Guid>
    {
        /// <summary>
        /// 加载所有模块功能数据到缓存
        /// </summary>
        /// <returns></returns>
        Task<List<SysModuleResource>> LoadAllResourceToCache();
        /// <summary>
        /// 插入模块功能数据到缓存，如果存在会删除重新添加到缓存
        /// </summary>
        /// <param name="role"></param>
        void InsertResourceToCache(SysModuleResource resource);
        /// <summary>
        /// 从缓存中获取所有模块功能数据
        /// </summary>
        /// <returns></returns>
        Task<List<SysModuleResource>> GetAllResourceFromCache();
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="id"></param>
        void RemoveResourceToCache(Guid id);
    }
}
