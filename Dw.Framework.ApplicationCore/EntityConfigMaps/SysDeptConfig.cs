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
    public class SysDeptConfig : EntityTypeConfiguration<SysDept>
    {
        public override void Map(EntityTypeBuilder<SysDept> builder)
        {
            builder.ToTable("sys_dept");
            builder.Property(w => w.ParentId).IsRequired();
            builder.Property(w => w.CurrentLevelCode).HasDefaultValue(100).IsRequired();
            builder.Property(w => w.AncestorsCode).HasMaxLength(400).IsRequired();
            builder.Property(w => w.AncestorsFullName).HasMaxLength(400).IsRequired();
            builder.Property(w => w.DeptName).HasMaxLength(50).IsRequired();
            builder.Property(w => w.SortNO).IsRequired();
            builder.Property(w => w.Leader).HasMaxLength(20).IsRequired(false);
            builder.Property(w => w.Phone).HasMaxLength(50).IsRequired(false);
            builder.Property(w => w.Email).HasMaxLength(100).IsRequired(false);
            builder.Property(w => w.Status).HasDefaultValue(EnumSysDept.ENABLE).IsRequired();
            builder.Property(w => w.Level).IsRequired();
        }
    }
}
