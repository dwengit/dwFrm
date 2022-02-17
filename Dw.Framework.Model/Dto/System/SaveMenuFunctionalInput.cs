using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class SaveMenuFunctionalInput
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 菜单功能Code
        /// </summary>
        [Required]
        public string ResourceCode { get; set; }
        /// <summary>
        /// 菜单功能名称
        /// </summary>
        [Required]
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
        [Required]
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

        public void ValidateData(out string msg)
        {
            msg = "";
            if (this.ResourceType != EnumSysModuleResourceType.BUTTON)
            {
                if (string.IsNullOrWhiteSpace(this.PathName))
                {
                    throw new ArgumentException("路由地址不能为空");
                }
                else if (this.ResourceType == EnumSysModuleResourceType.MENU)
                {
                    if (string.IsNullOrWhiteSpace(this.Path))
                    {
                        throw new ArgumentException("组件地址不能为空");
                    }
                }
            }
        }
        public void FilterData()
        {
            if (this.ResourceType == EnumSysModuleResourceType.MODULE)
            {
                this.Path = null;
            } 
            else if (this.ResourceType == EnumSysModuleResourceType.BUTTON)
            {
                this.Path = null;
                this.PathName = null;
            }
        }
    }
}
