using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dw.Framework.Infrastructure.Database
{
    public class BaseDbContext : DbContext
    {

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetCurrentPathAssembly().Where(x => !(x.GetName().Name.Equals("Dw.Framework.Infrastructure")));
            foreach (var assembly in assemblies)
            {
                var entityTypes = assembly.GetTypes()
                    .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                    .Where(type => type.IsClass)
                    .Where(type => type.Name != nameof(Entity))
                    .Where(type => type.BaseType != null)
                    .Where(type => typeof(ITrack).IsAssignableFrom(type));

                foreach (var entityType in entityTypes)
                {
                    // 防止重复附加模型，否则会在生成指令中报错
                    if (modelBuilder.Model.FindEntityType(entityType) != null) continue;
                    // AddEntityType把继承Entity添加到DbSet<XXX>
                    modelBuilder.Model.AddEntityType(entityType);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetTrackInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTrackInfo();
            return base.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// SaveChanges时，设置已实现的实体值
        /// </summary>
        private void SetTrackInfo()
        {
            //扫描所跟踪的实体实例，以检测对实例所做的任何更改
            ChangeTracker.DetectChanges();

            //获取跟踪实体已实现ITrack并且是（新增/更新）的实体
            var entries = this.ChangeTracker.Entries()
                .Where(x => x.Entity is ITrack)
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            //遍历实体，根据状态设置对应的值
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var entityBase = entity as ITrack;
                //if(entry.State == EntityState.Added && entity is ICreateTimeTrack)
                //{
                //    (entity as ICreateTimeTrack).CreateTime = DateTime.Now;
                //}
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entityBase.UpdateTime = DateTime.Now;
                        break;
                    case EntityState.Added:
                        entityBase.CreateTime = DateTime.Now;
                        break;
                }
            }
        }
    }
}
