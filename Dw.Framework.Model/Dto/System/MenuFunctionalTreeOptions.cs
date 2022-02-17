using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class MenuFunctionalTreeOptions
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public Guid ParentResourceID { get; set; }
        /// <summary>
        /// 模块/功能名称
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public EnumSysModuleResourceType ResourceType { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<MenuFunctionalTreeOptions> Children { get; set; }
    }
}
