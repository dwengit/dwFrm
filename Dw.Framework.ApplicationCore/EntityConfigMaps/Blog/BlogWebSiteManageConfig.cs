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
    public class BlogWebSiteManageConfig : EntityTypeConfiguration<BlogWebSiteManage>
    {
        public override void Map(EntityTypeBuilder<BlogWebSiteManage> builder)
        {
            builder.ToTable("blog_website_manage");
            builder.Property(w => w.BackImage).HasMaxLength(50).IsRequired();
            builder.Property(w => w.WebSitName).HasMaxLength(20).IsRequired();
        }
    }
}
