using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.System
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SysLoginLog : Entity<Guid>
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 登录状态 0成功 1失败
        /// </summary>
        public EnumSysLoginLogStatus Status { get; set; }

        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string Ipaddr { get; set; }

        /// <summary>
        /// 登录地点
        /// </summary>
        public string LoginLocation { get; set; }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string Browser { get; set; }

        /** 操作系统 */
        //@Excel(name = "操作系统")
        public string Os { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Msg { get; set; }
    }
}
