using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Blog
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class BlogLink : Entity<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string LinkTitle { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int SortNO { get; set; }
    }
}
