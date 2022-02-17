using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.EntityConfigMaps
{
    public class SysFileBusinesConfig : EntityTypeConfiguration<SysFileBusines>
    {
        public override void Map(EntityTypeBuilder<SysFileBusines> builder)
        {
            builder.ToTable("Sys_File_busines");
            builder.Property(x => x.BucketName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Extension).HasMaxLength(30).IsRequired(false);
            builder.Property(x => x.ObjectName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Size).IsRequired();
            builder.Property(x => x.AccountCode).HasMaxLength(20).IsRequired();
            builder.Property(x => x.FileStatus).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.BusinessCode).HasMaxLength(20).IsRequired();
            builder.Property(x => x.BusinessId).IsRequired();
        }
    }
}
