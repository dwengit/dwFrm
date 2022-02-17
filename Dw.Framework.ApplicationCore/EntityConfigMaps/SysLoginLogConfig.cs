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
    public class SysLoginLogConfig : EntityTypeConfiguration<SysLoginLog>
    {
        public override void Map(EntityTypeBuilder<SysLoginLog> builder)
        {
            builder.ToTable("sys_login_log");
            builder.Property(w => w.AccountCode).HasDefaultValue(50).IsRequired();
            builder.Property(w => w.Status).HasDefaultValue(EnumSysLoginLogStatus.ERROR).IsRequired();
            builder.Property(w => w.Ipaddr).HasDefaultValue(50).IsRequired();
            builder.Property(w => w.LoginLocation).HasDefaultValue(50).IsRequired();
            builder.Property(w => w.Ipaddr).HasDefaultValue(200).IsRequired();
            builder.Property(w => w.Browser).HasDefaultValue(20).IsRequired(false);
            builder.Property(w => w.Os).HasDefaultValue(40).IsRequired(false);
            builder.Property(w => w.Msg).HasDefaultValue(50).IsRequired(false);
        }
    }
}
