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
    public class BlogCommentConfig : EntityTypeConfiguration<BlogComment>
    {
        public override void Map(EntityTypeBuilder<BlogComment> builder)
        {
            builder.ToTable("blog_comment");
            builder.Property(w => w.CommentNickName).HasMaxLength(10).IsRequired();
            builder.Property(w => w.ArticleId).IsRequired();
            builder.Property(w => w.CommentContent).HasMaxLength(500).IsRequired();
            builder.Property(w => w.CommentLikeNum).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.ParentCommentId).HasDefaultValue(Guid.Empty).IsRequired().IsConcurrencyToken();
            builder.Property(w => w.CommentRootId).HasDefaultValue(Guid.Empty).IsRequired();
            builder.Property(w => w.CommentType).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.CommenterType).HasDefaultValue(0).IsRequired();
            builder.Property(w => w.CommentIp).IsRequired(false);
            builder.Property(w => w.CommentLocation).HasMaxLength(50).IsRequired(false);
        }
    }
}
