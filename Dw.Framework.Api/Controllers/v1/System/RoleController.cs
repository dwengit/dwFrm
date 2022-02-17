using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Dw.Framework.ApplicationCore.AppServices.System.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Web.Infrastructure.Auth;

namespace Dw.Framework.Admin.Api.Controllers.v1.System
{
    [Route("api/v{version:apiVersion}/System/[controller]/[action]")]
    [ApiController]
    public class RoleController : ApiControllersBase
    {
        private readonly ISysRoleAppService _roleAppService;

        public RoleController(ISysRoleAppService roleAppService)
        {
            this._roleAppService = roleAppService;
        }
        /// <summary>
        /// 获取-角色分页
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.RoleRead)]
        public async Task<Respbase<Page<RolePage>>> GetRolePage(string roleName, int? status, int pageIndex, int pageSize)
        {
            EnumSysRoleStatus? stateEnum = null;
            if (Enum.IsDefined(typeof(EnumSysRoleStatus), status))
            {
                stateEnum = (EnumSysRoleStatus)status;
            }
            var res = await _roleAppService.GetRolePage(roleName, stateEnum, pageIndex, pageSize);
            return RespOk(res);
        }
        /// <summary>
        /// 获取-角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.RoleRead)]
        public async Task<Respbase<RoleDto>> GetRoleInfo(Guid id)
        {
            var res = await _roleAppService.GetRoleInfo(id);
            return RespOk(res);
        }
        /// <summary>
        /// 获取-角色下拉选项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Respbase<List<RoleOption>> GetRoleOptions()
        {
            var res =  _roleAppService.GetRoleOptions();
            return RespOk(res);
        }
        /// <summary>
        /// 新增-角色
        /// </summary>
        /// <param name="saveRoleInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.RoleAdd)]
        [Log(Title = "角色管理", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> AddRole(SaveRoleInput saveRoleInput)
        {
            await _roleAppService.SaveRole(saveRoleInput);
            return RespOk();
        }
        /// <summary>
        /// 修改-角色
        /// </summary>
        /// <param name="saveRoleInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.RoleEdit)]
        [Log(Title = "角色管理", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> UpdateRole(SaveRoleInput saveRoleInput)
        {
            await _roleAppService.SaveRole(saveRoleInput);
            return RespOk();
        }
        /// <summary>
        /// 删除-角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.RoleDelete)]
        [Log(Title = "角色管理", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> DeleteRole(Guid id)
        {
            await _roleAppService.DeleteRole(id);
            return RespOk();
        }
    }
}
