using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.EntityConfigMaps
{
    public class SysUserRoleConfig : EntityTypeConfiguration<SysUserRole>
    {
        public override void Map(EntityTypeBuilder<SysUserRole> builder)
        {
            builder.ToTable("sys_user_role");
        }
    }
}
