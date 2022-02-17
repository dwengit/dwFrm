using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog
{
    public class SubCommentInput
    {
        /// <summary>
        /// 评论根Id/一级评论，（如果CommentType==1，默认为空）
        /// </summary>
        public Guid CommentRootId { get; set; }
        /// <summary>
        /// 回复人父级Id，（如果CommentType==1，默认为空）
        /// </summary>
        public Guid ParentCommentId { get; set; }
        /// <summary>
        /// 类型，0：评论，1：回复
        /// </summary>
        public int CommentType { get; set; }
        /// <summary>
        /// 博文Id
        /// </summary>
        public Guid ArticleId { get; set; }
        /// <summary>
        /// 评论/回复，内容
        /// </summary>
        public string CommentContent { get; set; }
        /// <summary>
        /// 评论人/回复人，昵称
        /// </summary>
        public string CommentNickName { get; set; }    
    }
}
