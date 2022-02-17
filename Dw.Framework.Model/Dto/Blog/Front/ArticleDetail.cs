using Dw.Framework.Model.Dto.Shared;
using Dw.Framework.Model.Dto.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog.Front
{
    public class ArticleDetail
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
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string ArticleContent { get; set; }
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
        /// 标签名称
        /// </summary>
        public IList<DefaultSelectOption> Tags { get; set; }
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
        /// <summary>
        /// 上一章
        /// </summary>
        public ArticlePrevNext Prev { get; set; }
        /// <summary>
        /// 下一章
        /// </summary>
        public ArticlePrevNext Next { get; set; }
    }
}
