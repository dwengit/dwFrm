using Dw.Framework.ApplicationCore.AppServices.Accounts.Dto;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Dto.Account;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Account
{
    public interface IAccountService
    {
        Task<UserBasicInfoDto> GetUserInfo(string accountCode);
        Task<SysUser> GetUser(string accountCode);
        Task<bool> ValidateLoginUser(string accountCode, string pwd);
        Task<bool> CheckPermission(string accountCode, string permissionCode);
        Task<List<UserRouteDto>> UserRoutes(string accountCode);
        /// <summary>
        /// 刷新用户所有权限缓存
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        Task RefreshUserAllPermissionIdsCatch(string accountCode);
        /// <summary>
        /// 获取用户的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<SysRole>> GetUserAllRole(Guid userId);
        /// <summary>
        /// 记录用户登陆信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        SysLoginLog BuildLogInfo(HttpContext context, EnumSysLoginLogStatus status, string message, string accountCode);
        /// <summary>
        /// 检查登录，登录成功返回用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Respbase<SysUser>> CheckLoginAsync(LoginInput req);
    }
}
