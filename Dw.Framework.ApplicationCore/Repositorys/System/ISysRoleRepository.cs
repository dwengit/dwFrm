using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public interface ISysRoleRepository : IRepository<SysRole, Guid>
    {
        /// <summary>
        /// 加载所有角色数据到缓存
        /// </summary>
        /// <returns></returns>
        Task<List<SysRole>> LoadAllRolesToCache();
        /// <summary>
        /// 插入角色数据到缓存，如果存在会删除重新添加到缓存
        /// </summary>
        /// <param name="role"></param>
        void InsertRoleToCache(SysRole role);
        /// <summary>
        /// 从缓存中获取所有角色数据
        /// </summary>
        /// <returns></returns>
        Task<List<SysRole>> GetAllRoleFromCache();
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="id"></param>
        void RemoveRoleCache(Guid id);
        /// <summary>
        /// 根据角色ids返回过滤好的角色信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<SysRole>> FilterRolesByRoles(IEnumerable<Guid> ids);
        /// 根据角色ids返回过滤好的角色id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Guid>> FilterRolesByRoleIds(IEnumerable<Guid> ids);
    }
}
