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
    public class BlogCategoryConfig : EntityTypeConfiguration<BlogCategory>
    {
        public override void Map(EntityTypeBuilder<BlogCategory> builder)
        {
            builder.ToTable("blog_category");
            builder.Property(w => w.CategoryTitle).HasMaxLength(20).IsRequired();
            builder.Property(w => w.ParentId).HasDefaultValue(Guid.Empty).IsRequired();
        }
    }
}
