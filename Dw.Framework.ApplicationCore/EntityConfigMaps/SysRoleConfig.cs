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
    public class SysRoleConfMap : EntityTypeConfiguration<SysRole>
    {
        public override void Map(EntityTypeBuilder<SysRole> builder)
        {
            builder.ToTable("sys_role");
            builder.Property(w => w.RoleName).HasMaxLength(20).IsRequired();
            builder.Property(w => w.RoleCode).HasMaxLength(50).IsRequired();
            builder.Property(w => w.Status).HasDefaultValue(EnumSysRoleStatus.ENABLE);
            builder.Property(w => w.Description).HasMaxLength(500).IsRequired();
        }
    }
}
