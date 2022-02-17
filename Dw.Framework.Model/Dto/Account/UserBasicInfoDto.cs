using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Account
{
    public class UserBasicInfoDto
    {
        public UserBasicInfoDto()
        {
            Permissions = new List<UserPermissionResourceDto>();
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
         /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 用户拥有的角色
        /// </summary>
        public IEnumerable<UserRoleInfoDto> Roles { get; set; }
        /// <summary>
        /// 当前用户所有权限
        /// </summary>
        public List<UserPermissionResourceDto> Permissions { get; set; }
    }
    /// <summary>
    /// 用户角色信息
    /// </summary>
    public class UserRoleInfoDto
    {
        /// <summary>
        /// 角色权限Code
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
    /// <summary>
    /// 用户所拥有的权限资源
    /// </summary>
    public class UserPermissionResourceDto
    {
        /// <summary>
        /// 资源code
        /// </summary>
        public string PermissionCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string PermissionName { get; set; }
        /// <summary>
        /// 资源图标
        /// </summary>
        public string PermissionIcon { get; set; }
        /// <summary>
        /// 资源路径名称
        /// </summary>
        public string PathName { get; set; }
    }
}
