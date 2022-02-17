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
    public class BlogTagConfig : EntityTypeConfiguration<BlogTag>
    {
        public override void Map(EntityTypeBuilder<BlogTag> builder)
        {
            builder.ToTable("blog_tag");
            builder.Property(w => w.TagTitle).HasMaxLength(50).IsRequired();
            builder.Property(w => w.IsQuickNav).HasDefaultValue(false).IsRequired();
        }
    }
}
