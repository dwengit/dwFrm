using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.EntityConfigMaps.Blog
{
    internal class BlogArticleTagConfig : EntityTypeConfiguration<BlogArticleTag>
    {
        public override void Map(EntityTypeBuilder<BlogArticleTag> builder)
        {
            builder.ToTable("blog_article_tag");
            builder.Property(w => w.ArticleId).IsRequired();
            builder.Property(w => w.TagId).IsRequired();
        }
    }
}
