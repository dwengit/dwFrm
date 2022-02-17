using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog.Front
{
    /// <summary>
    /// 博文时间轴
    /// </summary>
    public class BlogArchives
    {
        /// <summary>
        /// 时间轴年度
        /// </summary>
        public string YearTime { get; set; }
        /// <summary>
        /// 博文时间轴阶段信息
        /// </summary>
        public IList<BlogArchiveStage> ArchiveStages { get; set; }
    }
    /// <summary>
    /// 博文时间轴阶段信息
    /// </summary>
    public class BlogArchiveStage
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid ArticleId { get; set; }
        /// <summary>
        /// 文章时间轴时间
        /// </summary>
        public string TimeLine { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string ArticleTitle { get; set; }
    }
}
