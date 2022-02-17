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
    public class SysUserConfig : EntityTypeConfiguration<SysUser>
    {
        public override void Map(EntityTypeBuilder<SysUser> builder)
        {
            builder.ToTable("sys_user");
            builder.Property(w => w.UserEmail).HasMaxLength(50).IsRequired();
            builder.Property(w => w.AccountCode).HasMaxLength(20).IsRequired();
            builder.Property(w => w.UserName).HasMaxLength(20).IsRequired();
            builder.Property(w => w.NickName).HasMaxLength(20).IsRequired();
            builder.Property(w => w.Password).HasMaxLength(32).IsRequired();
            builder.Property(w => w.PhoneNum).HasMaxLength(32).IsRequired(false);
            builder.Property(w => w.Avatar).HasMaxLength(400).IsRequired(false);
            builder.Property(w => w.DeptId).IsRequired();
            builder.Property(w => w.LoginIP).HasMaxLength(20).IsRequired(false);
            builder.Property(w => w.LoginDate).IsRequired(false);
            builder.Property(w => w.Description).HasMaxLength(200).IsRequired(false);
            builder.Property(w => w.Status).HasDefaultValue(EnumSysUserStatus.ENABLE).IsRequired();
        }
    }
}
