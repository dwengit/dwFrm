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
    public class BlogLinkConfig : EntityTypeConfiguration<BlogLink>
    {
        public override void Map(EntityTypeBuilder<BlogLink> builder)
        {
            builder.ToTable("blog_link");
            builder.Property(w => w.LinkTitle).HasMaxLength(50).IsRequired();
            builder.Property(w => w.LinkUrl).HasMaxLength(500).IsRequired();
        }
    }
}
