using AutoMapper;
using Dw.Framework.ApplicationCore.Repositorys.Accounts;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Model.Dto.Account;
using Microsoft.AspNetCore.Http;
using UAParser;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
using IPTools.Core;
using Dw.Framework.Infrastructure.Caches;
using Dw.Framework.ApplicationCore.AppServices.Accounts.Dto;

namespace Dw.Framework.ApplicationCore.AppServices.Account
{
    public class AccountAppService : AppServiceBase, IAccountService
    {
        private readonly ISysUserRepository _sysUserRepository;
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly ISysUserRoleRepository _sysUserRoleRepository;
        private readonly ISysResourceRepository _sysResourceRepository;
        private readonly ISysPrivilegeRepository _sysPrivilegeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountAppService(ISysUserRepository userRepository, ISysRoleRepository sysRoleRepository, ISysUserRoleRepository sysUserRoleRepository, ISysResourceRepository sysResourceRepository, ISysPrivilegeRepository sysPrivilegeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _sysUserRepository = userRepository;
            _sysRoleRepository = sysRoleRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _sysResourceRepository = sysResourceRepository;
            _sysPrivilegeRepository = sysPrivilegeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 根据code检查用户是否拥有该权限
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="permissionCode"></param>
        /// <returns></returns>
        public async Task<bool> CheckPermission(string accountCode, string permissionCode)
        {
            var allCodes = await GetUserAllPermissionCode(accountCode);
            return allCodes.Any(w => w.Equals(permissionCode, StringComparison.Ordinal));
        }
        /// <summary>
        /// 获取用户所有模块功能资源
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysModuleResource>> GetUserAllPermissionResource(string accountCode)
        {
            var user = await _sysUserRepository.GetUserFromCacheNotNull(accountCode);
            var roles = await GetUserAllRole(user.Id);
            var resource = await _sysResourceRepository.GetAllResourceFromCache();
            if (roles.Any(w => w.RoleCode.Equals("admin", StringComparison.CurrentCulture)))
            {
                return resource;
            }
            var roleIds = roles.Select(s => s.Id);
            var ids = _sysPrivilegeRepository.GetUserAllPermissionIdsCatch(user, roleIds);
            var res = resource.Where(w => ids.Contains(w.Id));
            return res;
        }
        /// <summary>
        /// 获取用户所有模块功能资源Code
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetUserAllPermissionCode(string accountCode)
        {
            return (await GetUserAllPermissionResource(accountCode)).Select(s => s.ResourceCode);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public async Task<SysUser> GetUser(string accountCode)
        {
            return await _sysUserRepository.GetUserFromCache(accountCode);
        }
        /// <summary>
        /// 获取用户的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysRole>> GetUserAllRole(Guid userId)
        {
           return await _sysRoleRepository.FilterRolesByRoles(_sysUserRoleRepository.GetUserRoleIds(userId));
        }
        /// <summary>
        /// 获取用户详情-用户信息/角色/权限
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<UserBasicInfoDto> GetUserInfo(string accountCode)
        {
            var user = await _sysUserRepository.GetUserFromCacheNotNull(accountCode);
            var res = _mapper.Map<UserBasicInfoDto>(user);
            var roles = await GetUserAllRole(user.Id);
            //获取用户拥有的角色
            res.Roles = roles.Select(s => new UserRoleInfoDto()
            {
                RoleId = s.Id,
                RoleName = s.RoleName,
                RoleCode = s.RoleCode,
            });
            IEnumerable<SysModuleResource> userAllResource =  await GetUserAllPermissionResource(accountCode);
            res.Permissions = userAllResource.Select(w => new UserPermissionResourceDto()
            {
                PermissionCode = w.ResourceCode,
                PermissionName = w.ResourceName,
                PermissionIcon = w.ResourceIcon,
                PathName = w.PathName
            }).ToList();
            if (res.Permissions.Count <= 0)
            {
                throw new CustomException(ResultCodeEnums.PERMISSION_NOT_ASSIGNED, "当前账号权限未分配，请联系管理员");
            }
            return res;
        }
        /// <summary>
        /// 校验用户登录
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<bool> ValidateLoginUser(string accountCode, string pwd)
        {
            var res = await _sysUserRepository.FirstOrDefaultAsync(w => w.AccountCode == accountCode);
            return res.AccountCode.Equals(accountCode, StringComparison.OrdinalIgnoreCase) && Encrypt.MD5Encrypt(pwd).Equals(res.Password, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 用户路由
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public async Task<List<UserRouteDto>> UserRoutes(string accountCode)
        {
            var userAllResource = (await GetUserAllPermissionResource(accountCode)).ToList();
            var res = RecursionPermission(userAllResource, Guid.Empty);
            return res;
        }
        /// <summary>
        /// 递归-返回用户权限路由
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        private List<UserRouteDto> RecursionPermission(List<SysModuleResource> permissions, Guid parentid)
        {
            List<UserRouteDto> res = new List<UserRouteDto>();
            var parentPermissions = permissions.Where(w => w.ParentResourceID == parentid);
            if (!parentPermissions.Any())
            {
                return res;
            }
            foreach (var item in parentPermissions)
            {
                var userPermission = new UserRouteDto()
                {
                    Name = item.ResourceCode.Replace(".", String.Empty),
                    Path = item.PathName,
                    Component = item.Path,
                    Hidden = !item.IsShow,
                    Meta = new MetaDto()
                    {
                        Icon = item.ResourceIcon,
                        NoCache = item.NoCache,
                        Title = item.ResourceName
                    }
                };
                res.Add(userPermission);
                if (item.ResourceType == EnumSysModuleResourceType.MODULE)
                {
                    userPermission.Path = item.PathName;
                    userPermission.Component = item.ParentResourceID == Guid.Empty ? "Layout" : "ParentView"; //使用默认模板
                    userPermission.Redirect = "noRedirect"; //默认根目录菜单不跳转
                    userPermission.AlwaysShow = true; //当根目录有一个子路由时始终显示
                    userPermission.Children.AddRange(RecursionPermission(permissions, item.Id));
                }
            }
            return res;
        }
        /// <summary>
        /// 刷新用户所有权限缓存
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public async Task RefreshUserAllPermissionIdsCatch(string accountCode)
        {
            var user = await _sysUserRepository.GetUserFromCacheNotNull(accountCode);
            var roles = await GetUserAllRole(user.Id);
            var roleIds = roles.Select(s => s.Id);
            _sysPrivilegeRepository.RefreshUserAllPermissionIdsCatch(user, roleIds);
        }

        /// <summary>
        /// 记录用户登陆信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public SysLoginLog BuildLogInfo(HttpContext context, EnumSysLoginLogStatus status, string message, string accountCode)
        {
            ClientInfo clientInfo = context.GetClientInfo();
            SysLoginLog sysLoginLog = new SysLoginLog();
            sysLoginLog.Browser = clientInfo.Device.Family;
            sysLoginLog.Os = clientInfo.OS.ToString();
            sysLoginLog.Ipaddr = context.GetClientUserIp();
            sysLoginLog.Msg = message;
            sysLoginLog.AccountCode = accountCode;
            sysLoginLog.Status = status;
            var ip_info = IpTool.Search(sysLoginLog.Ipaddr);
            sysLoginLog.LoginLocation = ip_info.Province + "-" + ip_info.City;
            return sysLoginLog;
        }
        /// <summary>
        /// 检查登录，登录成功返回用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Respbase<SysUser>> CheckLoginAsync(LoginInput req)
        {
            //校验验证码
            string validateCodeCache = MemoryCacheHelper.GetCache(req.ValidateKey)?.ToString();
            if (string.IsNullOrWhiteSpace(validateCodeCache))
            {
                return RespbaseFac.RespFail<SysUser>("验证码已过期");
            }
            if (!req.ValidateCode.Equals(validateCodeCache, StringComparison.OrdinalIgnoreCase))
            {
                return RespbaseFac.RespFail<SysUser>("验证码不正确");
            }
            MemoryCacheHelper.RemoveCache(req.ValidateKey);

            //校验用户账号密码
            var user = await GetUser(req.AccountCode);
            if (user == null || !user.ValidateLoginUser(req.AccountCode, req.Password))
            {
                return RespbaseFac.RespFail<SysUser>("账号或密码不正确");
            }
            return RespbaseFac.RespOk<SysUser>(user);
        }
    }
}
