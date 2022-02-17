using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Dw.Framework.ApplicationCore.AppServices.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
using Dw.Framework.Model.Dto.Shared;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class MenuFunctionalController : ApiControllersBase
    {
        private readonly ISysMenuFunctionalAppService _menuFunctionalAppService;
        private readonly IAuthorizationService _authorizationService;
        public MenuFunctionalController(ISysMenuFunctionalAppService menuFunctionalAppService, IAuthorizationService authorizationService)
        {
            this._menuFunctionalAppService = menuFunctionalAppService;
            this._authorizationService = authorizationService;
        }
        /// <summary>
        /// 获取-分配功能模块权限树
        /// </summary>
        /// <param name="enumOwnerIdentityType"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Respbase<AssignMenuFunctionalTree>> GetAssignMenuFunctionalTree(EnumOwnerIdentityType enumOwnerIdentityType, Guid ownerId) 
        {
            var res = await _menuFunctionalAppService.GetAssignMenuFunctionalTree(enumOwnerIdentityType, ownerId);
            return RespOk(res);
        }
        /// <summary>
        /// 获取-菜单功能树列表
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.MenuFunctionalRead)]
        public async Task<Respbase<List<MenuFunctionalPageTree>>> GetMenuFunctionalList(string resourceName, int state)
        {
            EnumSysModuleResourceState? stateEnum = null;
            if(Enum.IsDefined(typeof(EnumSysModuleResourceState), state))
            {
                stateEnum = (EnumSysModuleResourceState)state;
            }
            var res = await _menuFunctionalAppService.GetMenuFunctionalList(resourceName, stateEnum);
            return RespOk(res);
        }
        /// <summary>
        /// 获取详情-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.MenuFunctionalRead)]
        public async Task<Respbase<MenuFunctionalDto>> GetMenuFunctional(Guid id)
        {
            var res = await _menuFunctionalAppService.GetMenuFunctional(id);
            return RespOk(res);
        }
        /// <summary>
        /// 获取全部-权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Respbase<List<MenuFunctionalTreeOptions>>> AllMenuFunctionalTreeOptions()
        {
            var res = await _menuFunctionalAppService.AllMenuFunctionalTreeOptions();
            return RespOk(res);
        }
        /// <summary>
        /// 新增-功能模块
        /// </summary>
        /// <param name="saveInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.MenuFunctionalAdd)]
        [Log(Title = "模块管理", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> AddMenuFunctional(SaveMenuFunctionalInput saveInput)
        {
            await _menuFunctionalAppService.SaveMenuFunctional(saveInput);
            return RespOk();
        }
        /// <summary>
        /// 修改-功能模块
        /// </summary>
        /// <param name="saveInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.MenuFunctionalEdit)]
        [Log(Title = "模块管理", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> UpdateMenuFunctional(SaveMenuFunctionalInput saveInput)
        {
            await _menuFunctionalAppService.SaveMenuFunctional(saveInput);
            return RespOk();
        }
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Log(Title = "模块管理", BusinessType = BusinessType.GRANT)]
        public async Task<Respbase> AssignMenuFunctional(AssignMenuFunctionalInput input)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(base.User, null, input.EnumOwnerIdentityType == EnumOwnerIdentityType.ROLE ? Operations.RoleAssignPermission : Operations.UserAssignPermission);
            if (!authorizationResult.Succeeded)
            {
                throw new CustomException(ResultCodeEnums.CODE401);
            }
            await _menuFunctionalAppService.AssignMenuFunctional(input, HttpContext.GetAccountCode());
            return RespOk();
        }
        /// <summary>
        /// 删除-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.MenuFunctionalDelete)]
        [Log(Title = "模块管理", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> DeleteMenuFunctional(Guid id)
        {
            await _menuFunctionalAppService.DeleteMenuFunctional(id);
            return RespOk();
        }
        /// <summary>
        /// 更改状态-模块/菜单/按钮
        /// </summary>
        /// <param name="setMenu"></param>
        /// <returns></returns>
        [HttpPut]
        [PermissionFilter(_System.MenuFunctionalEdit)]
        [Log(Title = "模块管理", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> SetMenuFunctional(ChangeFieldValue setMenu)
        {
            await _menuFunctionalAppService.SetMenuFunctional(setMenu.Id, setMenu.FieldVal);
            return RespOk();
        }
    }
}
