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
    public class SysOperLogConfig : EntityTypeConfiguration<SysOperLog>
    {
        public override void Map(EntityTypeBuilder<SysOperLog> builder)
        {
            builder.ToTable("sys_oper_log");
            builder.Property(w => w.Title).HasDefaultValue(100).IsRequired();
            builder.Property(w => w.Method).HasDefaultValue(100).IsRequired();
            builder.Property(w => w.RequestMethod).HasDefaultValue(10).IsRequired();
            builder.Property(w => w.OperatorType).IsRequired();
            builder.Property(w => w.OperName).HasDefaultValue(20).IsRequired();
            builder.Property(w => w.DeptName).HasDefaultValue(50).IsRequired(false);
            builder.Property(w => w.OperUrl).HasDefaultValue(200).IsRequired();
            builder.Property(w => w.OperIp).HasDefaultValue(50).IsRequired();
            builder.Property(w => w.OperLocation).HasDefaultValue(200).IsRequired(false);
            builder.Property(w => w.OperParam).IsRequired(false);
            builder.Property(w => w.JsonResult).IsRequired(false);
            builder.Property(w => w.Status).IsRequired();
            builder.Property(w => w.ErrorMsg).IsRequired(false);
            builder.Property(w => w.Elapsed).HasDefaultValue(0).IsRequired();
            builder.Ignore(w => w.IsSaveData);
        }
    }
}
