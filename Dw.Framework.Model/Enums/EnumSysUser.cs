using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dw.Framework.Model.Enums
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum EnumGender
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        MALE = 1,

        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        FEMALE = 2,

        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        UNKNOWN = 3
    }
    /// <summary>
    /// 用户-状态
    /// </summary>
    public enum EnumSysUserStatus
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
}
