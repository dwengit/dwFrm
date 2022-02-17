using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Dw.Framework.ApplicationCore.AppServices.System.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.Shared;
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
    public class UserController : ApiControllersBase
    {
        private readonly ISysUserAppService _userAppService;

        public UserController(ISysUserAppService userAppService)
        {
            this._userAppService = userAppService;
        }
        /// <summary>
        /// 获取-用户分页
        /// </summary>
        /// <param name="searchContent">用户名/昵称/电话号码</param>
        /// <param name="status"></param>
        /// <param name="deptId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.UserRead)]
        public async Task<Respbase<Page<UserPage>>> GetUserPage(string searchContent, int? status, Guid? deptId, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            EnumSysUserStatus? stateEnum = null;
            if (Enum.IsDefined(typeof(EnumSysUserStatus), status))
            {
                stateEnum = (EnumSysUserStatus)status;
            }
            var res = await _userAppService.GetUserPage(searchContent, stateEnum, deptId, beginTime, endTime, pageIndex, pageSize);
            return RespOk(res);
        }
        /// <summary>
        /// 获取-用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.UserRead)]
        public async Task<Respbase<UserDto>> GetUserInfo(Guid id)
        {
            var res = await _userAppService.GetUserInfo(id);
            return RespOk(res);
        }
        /// <summary>
        /// 新增-用户
        /// </summary>
        /// <param name="saveUserInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.UserAdd)]
        [Log(Title = "用户管理", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> AddUser(SaveUserInput saveUserInput)
        {
            await _userAppService.SaveUser(saveUserInput);
            return RespOk();
        }
        /// <summary>
        /// 修改-用户
        /// </summary>
        /// <param name="saveUserInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.UserEdit)]
        [Log(Title = "用户管理", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> UpdateUser(SaveUserInput saveUserInput)
        {
            await _userAppService.SaveUser(saveUserInput);
            return RespOk();
        }
        /// <summary>
        /// 更改-用户状态
        /// </summary>
        /// <param name="changeUser"></param>
        /// <returns></returns>
        [HttpPut]
        [PermissionFilter(_System.UserEdit)]
        [Log(Title = "用户管理_修改状态", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> ChangeUserStatus(ChangeUserValue changeUser)
        {
            await _userAppService.ChangeUser(changeUser.Id,  "Status:" + changeUser.Value);
            return RespOk();
        }
        /// <summary>
        /// 重置-用户密码
        /// </summary>
        /// <param name="changeUser"></param>
        /// <returns></returns>
        [HttpPut]
        [PermissionFilter(_System.UserResetPwd)]
        [Log(Title = "用户管理_重置密码", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> ChangeUserPwd(ChangeUserValue changeUser)
        {
            await _userAppService.ChangeUser(changeUser.Id, "Pwd:" + changeUser.Value);
            return RespOk();
        }
        /// <summary>
        /// 删除-用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.UserDelete)]
        [Log(Title = "用户管理", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> DeleteUser(Guid id)
        {
            await _userAppService.DeleteUser(id);
            return RespOk();
        }
    }
}
