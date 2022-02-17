using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dw.Framework.Model.Enums
{
    /// <summary>
    /// 模块资源表-状态
    /// </summary>
    public enum EnumSysModuleResourceState
    {
        /// <summary>
        /// 停用
        /// </summary>
        DISABLE = 0,
        /// <summary>
        /// 启用
        /// </summary>
        ENABLE = 1,
    }
    /// <summary>
    /// 模块资资源-类型
    /// </summary>
    public enum EnumSysModuleResourceType
    {
        /// <summary>
        /// 模块-类型
        /// </summary>
        [Description("模块")]
        MODULE = 1,
        /// <summary>
        /// 菜单-类型
        /// </summary>
        [Description("菜单")]
        MENU = 2,
        /// <summary>
        /// 按钮/Action权限-类型
        /// </summary>
        [Description("按钮")]
        BUTTON = 3
    }
}
