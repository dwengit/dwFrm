using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.System
{
    /// <summary>
    /// 模块资源表
    /// </summary>
    public class SysModuleResource : Entity<Guid>
    {
        /// <summary>
        /// 资源Code
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 资源图标
        /// </summary>
        public string ResourceIcon { get; set; }
        /// <summary>
        /// 资源路径/组件地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 资源名称/路由地址
        /// </summary>
        public string PathName { get; set; }
        /// <summary>
        /// 是否缓存页面
        /// </summary>
        public bool NoCache { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public EnumSysModuleResourceType ResourceType { get; set; }
        /// <summary>
        /// 资源的父id
        /// </summary>
        public Guid ParentResourceID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNO { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 当设置 false 的时候该路由不会在侧边栏出现
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public EnumSysModuleResourceState State { get; set; }
        /// <summary>
        /// 是否是外部链接
        /// </summary>
        public bool IsExternalLink { get; set; }
        /// <summary>
        /// 子集合
        /// </summary>
        public List<SysModuleResource> Children = new List<SysModuleResource>();
    }
}
