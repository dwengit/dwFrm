using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Blog
{
    /// <summary>
    /// 博客站点管理
    /// </summary>
    public class BlogWebSiteManage : Entity<Guid>
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string WebSitName { get; set; }
        /// <summary>
        /// 站点背景图
        /// </summary>
        public string BackImage { get; set; }
    }
}
