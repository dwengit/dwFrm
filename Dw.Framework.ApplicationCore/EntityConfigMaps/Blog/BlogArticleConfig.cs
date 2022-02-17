using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.EntityConfigMaps.Blog
{
    public class BlogArticleConfig : EntityTypeConfiguration<BlogArticle>
    {
        public override void Map(EntityTypeBuilder<BlogArticle> builder)
        {
            builder.ToTable("blog_article");
            builder.Property(w => w.ArticleTitle).HasMaxLength(500).IsRequired();
            builder.Property(w => w.Introduction).HasMaxLength(256).IsRequired(false);
            builder.Property(w => w.ArticleContent).IsRequired();
            builder.Property(w => w.UserId).IsRequired();
            builder.Property(w => w.ArticleStatus).HasDefaultValue(BlogArticleStatus.DRAFT).IsRequired();
            builder.Property(w => w.CategoryId).IsRequired();
            builder.Property(w => w.Top).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.ViewNum).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.LikeNum).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.CommentNum).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.Auth).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.IsDisableComment).HasDefaultValue(false).IsRequired();
        }
    }
}
