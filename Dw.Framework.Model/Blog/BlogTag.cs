using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Blog
{
    /// <summary>
    /// 博文标签
    /// </summary>
    public class BlogTag : Entity<Guid>
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagTitle { get; set; }
        /// <summary>
        /// 快速导航
        /// </summary>
        public bool IsQuickNav { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int SortNO { get; set; }
    }
}
