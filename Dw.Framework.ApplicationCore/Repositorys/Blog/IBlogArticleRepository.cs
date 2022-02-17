using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.Blog
{
    public interface IBlogArticleRepository : IRepository<BlogArticle, Guid>
    {
        Task ArticleCommentIncr(Guid articleId);
        Task ArticleCommentReduce(Guid articleId);
    }
}
