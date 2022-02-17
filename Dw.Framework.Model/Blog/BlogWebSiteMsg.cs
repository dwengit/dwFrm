using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Blog
{
    /// <summary>
    /// 站点留言
    /// </summary>
    public class BlogWebSiteMsg : Entity<Guid>
    {
        /// <summary>
        /// 留言的用户昵称
        /// </summary>
        public string MsgNickName { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string MsgContent { get; set; }
        /// <summary>
        /// 留言父Id
        /// </summary>
        public Guid ParentMsgId { get; set; }
    }
}
