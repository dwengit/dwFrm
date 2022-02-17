using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dw.Framework.ApplicationCore.Repositorys
{
    public class AppAdminMallDbContext : BaseDbContext
    {
        public AppAdminMallDbContext(DbContextOptions options) : base(options)
        {

        }
        #region 系统管理
        /// <summary>
        /// 角色表
        /// </summary>
        public virtual DbSet<SysRole> SysRole { get; set; }
        /// <summary>
        /// 用户表
        /// </summary>
        public virtual DbSet<SysUser> SysUser { get; set; }
        /// <summary>
        /// 用户表角色关联表
        /// </summary>
        public virtual DbSet<SysUserRole> SysUserRole { get; set; }
        /// <summary>
        /// 资源表
        /// </summary>
        public virtual DbSet<SysModuleResource> SysModuleResource { get; set; }
        /// <summary>
        /// 资源拥有特权表
        /// </summary>
        public virtual DbSet<SysPrivilege> SysPrivilege { get; set; }
        /// <summary>
        /// 资源拥有特权表
        /// </summary>
        public virtual DbSet<SysDept> SysDept { get; set; }
        /// <summary>
        /// 操作日志
        /// </summary>
        public virtual DbSet<SysOperLog> SysOperLog { get; set; }
        /// <summary>
        /// 登录日志
        /// </summary>
        public virtual DbSet<SysLoginLog> SysLoginLog { get; set; }
        /// <summary>
        /// 业务文件存储信息
        /// </summary>
        public virtual DbSet<SysFileBusines> SysFileBusines { get; set; }
        #endregion

        #region 博客管理
        /// <summary>
        /// 博文管理
        /// </summary>
        public virtual DbSet<BlogArticle> BlogArticle { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public virtual DbSet<BlogCategory> BlogCategory { get; set; }
        /// <summary>
        /// 博文评论
        /// </summary>
        public virtual DbSet<BlogComment> BlogComment { get; set; }
        /// <summary>
        /// 友情链接
        /// </summary>
        public virtual DbSet<BlogLink> BlogLinks { get; set; }
        /// <summary>
        /// 博文标签
        /// </summary>
        public virtual DbSet<BlogTag> BlogTag { get; set; }
        /// <summary>
        /// 站点留言
        /// </summary>
        public virtual DbSet<BlogWebSiteMsg> BlogWebSiteMsg { get; set; }
        /// <summary>
        /// 博客站点管理
        /// </summary>
        public virtual DbSet<BlogWebSiteManage> BlogWebSiteManage { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //加载实体与表结构的映射关系
            modelBuilder.AddEntityConfigurationsFromAssembly(Assembly.Load("Dw.Framework.ApplicationCore"));
            //初始化种子数据
            modelBuilder.BuildSeed();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        public static readonly LoggerFactory LoggerFactory =
               new LoggerFactory(new[] { new DebugLoggerProvider() });
    }
}
