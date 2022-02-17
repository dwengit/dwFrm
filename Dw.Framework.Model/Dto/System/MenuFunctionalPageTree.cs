using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    /// <summary>
    /// 菜单功能权限模块-列表
    /// </summary>
    public class MenuFunctionalPageTree : MenuFunctionalDto
    {
        /// <summary>
        /// 菜单功能子集合
        /// </summary>
        public List<MenuFunctionalPageTree> Children { get; set; }
    }
}
