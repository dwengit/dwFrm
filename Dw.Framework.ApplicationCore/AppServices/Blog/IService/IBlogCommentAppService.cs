using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog.IService
{
    public interface IBlogCommentAppService
    {
        Task<IList<CommentList>> GetArticleCommentList(Guid articleId, string ip);
        Task SubComment(SubCommentInput input, string ip, string accountCode);
        Task Like(Guid commentId, string ip);
        Task Delete(Guid commentId);
    }
}
