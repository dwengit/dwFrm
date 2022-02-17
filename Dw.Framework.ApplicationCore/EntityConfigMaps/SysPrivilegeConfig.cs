using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.EntityConfigMaps
{
    public class SysPrivilegeConfig : EntityTypeConfiguration<SysPrivilege>
    {
        public override void Map(EntityTypeBuilder<SysPrivilege> builder)
        {
            builder.ToTable("sys_privilege");
        }
    }
}
