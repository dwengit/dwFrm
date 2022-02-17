using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog
{
    public class CommentList
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 评论根Id/一级评论，（如果CommentType==1，默认为空）
        /// </summary>
        public Guid CommentRootId { get; set; }
        /// <summary>
        /// 回复人父级Id，（如果CommentType==0，默认为空）
        /// </summary>
        public Guid ParentCommentId { get; set; }
        /// <summary>
        /// 评论人IP地点
        /// </summary>
        public string CommentLocation { get; set; }
        /// <summary>
        /// 类型，0：评论，1：回复
        /// </summary>
        public int CommentType { get; set; }
        /// <summary>
        /// 评论人类型，0：普通用户，1：作者
        /// </summary>
        public int CommenterType { get; set; }
        /// <summary>
        /// 博文Id
        /// </summary>
        public Guid ArticleId { get; set; }
        private string commentContent;
        /// <summary>
        /// 评论/回复，内容
        /// </summary>
        public string CommentContent
        {
            set
            {
                commentContent = value;
            }
            get
            {
                return this.IsDelete ? "评论已删除" : commentContent;
            }
        }
        /// <summary>
        /// 评论/回复，点赞数
        /// </summary>
        public int CommentLikeNum { get; set; }
        /// <summary>
        /// 评论人/回复人，昵称
        /// </summary>
        public string CommentNickName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 是否已点赞
        /// </summary>
        public bool IsLike { get; set; }
    }
}
