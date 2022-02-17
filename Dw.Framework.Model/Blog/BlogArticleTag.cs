using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Blog
{
    public class BlogArticleTag : Entity<Guid>
    {
        public Guid ArticleId { get; set; }
        public Guid TagId { get; set; }
    }
}
