using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.Blog
{
    public class BlogArticleRepository : Repository<BlogArticle, Guid>, IBlogArticleRepository
    {
        public async Task ArticleCommentIncr(Guid articleId)        {
            var data = await FirstOrDefaultAsync(w => w.Id == articleId);
            if(data != null)
            {
                data.CommentNum += 1;
                Update(data);
            }
        }
        public async Task ArticleCommentReduce(Guid articleId)
        {
            var data = await FirstOrDefaultAsync(w => w.Id == articleId);
            if (data != null)
            {
                data.CommentNum -= 1;
                Update(data);
            }
        }
    }
}
