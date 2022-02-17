using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Dw.Framework.Model.Dto.Account
{
    public class PermissionDto
    {
        public PermissionDto()
        {
            Children = new List<PermissionDto>();
        }
        [JsonIgnore]
        public Guid Id { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionIcon { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public int ResourceType { get; set; }
        /// <summary>
        /// 资源的父id
        /// </summary>
        [JsonIgnore]
        public Guid ParentResourceID { get; set; }
        public List<PermissionDto> Children { get; set; }
    }
    public class UserRouteDto
    {
        public UserRouteDto()
        {
            Children = new List<UserRouteDto>();
        }
        /// <summary>
        /// 路由请求路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当设置 true 的时候该路由不会在侧边栏出现 如401，login等页面，或者如一些编辑页面/edit/1
        /// </summary>
        public bool Hidden { get; set; }
        /// <summary>
        /// 调整地址
        /// </summary>
        public string Redirect { get; set; }
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 是否一直显示根路由
        /// </summary>
        public bool AlwaysShow { get; set; } 
        /// <summary>
        /// 扩展描述信息
        /// </summary>
        public MetaDto Meta { get; set; }
        /// <summary>
        /// 菜单子模块/子目录
        /// </summary>
        public List<UserRouteDto> Children { get; set; }
    }
    /// <summary>
    /// 路由扩展描述信息
    /// </summary>
    public class MetaDto
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 是否缓存页面
        /// </summary>
        public bool NoCache { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
}
