using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class MenuFunctionalDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 菜单功能Code
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 菜单功能名称
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 菜单功能图标
        /// </summary>
        public string ResourceIcon { get; set; }
        /// <summary>
        /// 菜单视图路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 路由名称
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
        /// 菜单功能的父id
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
        /// 显示/隐藏
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 是否是外部链接
        /// </summary>
        public bool IsExternalLink { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public EnumSysModuleResourceState State { get; set; }
    }
}
