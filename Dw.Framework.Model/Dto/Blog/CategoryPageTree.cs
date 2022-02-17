using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog
{
    public class CategoryPageTree
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 子集合
        /// </summary>
        public List<CategoryPageTree> Children { get; set; } = new List<CategoryPageTree>();
    }
}
