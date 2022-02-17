using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.EntityConfigMaps
{
    public class SysModuleResourceConfig : EntityTypeConfiguration<SysModuleResource>
    {
        public override void Map(EntityTypeBuilder<SysModuleResource> builder)
        {
            builder.ToTable("sys_module_resource");
            builder.Property(w => w.ResourceCode).HasMaxLength(2000).IsRequired();
            builder.Property(w => w.ResourceName).HasMaxLength(200).IsRequired();
            builder.Property(w => w.ResourceIcon).HasMaxLength(200).IsRequired();
            builder.Property(w => w.ParentResourceID).HasMaxLength(36).IsRequired();
            builder.Property(w => w.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(w => w.PathName).HasMaxLength(50).IsRequired(false);
            builder.Property(w => w.Path).HasMaxLength(200).IsRequired(false);
            builder.Property(w => w.NoCache).HasDefaultValue(false);
            builder.Property(w => w.IsShow).HasDefaultValue(false);
            builder.Property(w => w.IsExternalLink).HasDefaultValue(false);
            builder.Property(w => w.State).HasDefaultValue(EnumSysModuleResourceState.ENABLE).IsRequired();
        }
    }
}
