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
    public class BlogWebSiteMsgConfig : EntityTypeConfiguration<BlogWebSiteMsg>
    {
        public override void Map(EntityTypeBuilder<BlogWebSiteMsg> builder)
        {
            builder.ToTable("blog_website_msg");
            builder.Property(w => w.MsgNickName).HasMaxLength(10).IsRequired();
            builder.Property(w => w.MsgContent).HasMaxLength(500).IsRequired();
            builder.Property(w => w.ParentMsgId).HasDefaultValue(Guid.Empty).IsRequired();
        }
    }
}
