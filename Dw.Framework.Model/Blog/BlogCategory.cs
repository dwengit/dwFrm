using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Blog
{
    /// <summary>
    /// 博客分类
    /// </summary>
    public class BlogCategory : Entity<Guid>
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryTitle { get; set; }
        /// <summary>
        /// 分类父Id
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int SortNO { get; set; }
        /// <summary>
        /// 子集合
        /// </summary>
        public List<BlogCategory> Children = new List<BlogCategory>();
    }
}
