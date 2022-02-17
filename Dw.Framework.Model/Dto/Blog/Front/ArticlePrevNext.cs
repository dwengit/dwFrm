using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog.Front
{
    public class ArticlePrevNext
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 行号索引
        /// </summary>
        public int RowIndex { get; set; }
    }
}
