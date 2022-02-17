using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog
{
    public class ArticlePage
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 发表用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string ArticleTitle { get; set; }
        /// <summary>
        /// 摘要/简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 博文状态
        /// </summary>
        public BlogArticleStatus ArticleStatus { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImage { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryTitle { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid CategoryId { get; set; }
        /// <summary>
        /// 标签名称，逗号分割
        /// </summary>
        public string TagTitles { get; set; }
        /// <summary>
        /// 标签Id，逗号分割
        /// </summary>
        public List<Guid> TagIds { get; set; }
        /// <summary>
        /// 置顶（0：默认布置定，1：置顶）
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
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
