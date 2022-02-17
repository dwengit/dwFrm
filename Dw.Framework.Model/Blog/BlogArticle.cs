using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Blog
{
    /// <summary>
    /// 博文管理
    /// </summary>
    public class BlogArticle : Entity<Guid>
    {
        /// <summary>
        /// 发表用户ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string ArticleTitle { get; set; }
        /// <summary>
        /// 摘要/简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string ArticleContent { get;set; }
        /// <summary>
        /// 博文状态
        /// </summary>
        public BlogArticleStatus ArticleStatus { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid CategoryId { get;set;}
        /// <summary>
        /// 置顶（0：默认不置定，1：置顶）
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewNum { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeNum { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentNum { get; set; }
        /// <summary>
        /// 权限（0：默认公开，1：Vip）
        /// </summary>
        public int Auth { get; set; }
        /// <summary>
        /// 是否禁用评论
        /// </summary>
        public bool IsDisableComment { get; set; }
    }
}
