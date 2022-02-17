using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.System.IService
{
    public interface ISysRoleAppService
    {
        /// <summary>
        /// 获取-角色分页
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Page<RolePage>> GetRolePage(string roleName, EnumSysRoleStatus? status, int pageIndex, int pageSize);
        /// <summary>
        /// 获取-角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RoleDto> GetRoleInfo(Guid id);
        /// <summary>
        /// 获取-角色下拉选项
        /// </summary>
        /// <returns></returns>
        List<RoleOption> GetRoleOptions();
        /// <summary>
        /// 保存-角色
        /// </summary>
        /// <param name="saveRoleInput"></param>
        /// <returns></returns>
        Task<bool> SaveRole(SaveRoleInput saveRoleInput);
        /// <summary>
        /// 删除-角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> DeleteRole(Guid id);
    }
}
