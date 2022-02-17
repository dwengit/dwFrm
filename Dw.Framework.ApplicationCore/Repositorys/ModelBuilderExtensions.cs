using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.Repositorys
{
    public static class ModelBuilderExtensions
    {
        public static void BuildSeed(this ModelBuilder modelBuilder)
        {
            Guid userId = Guid.Parse("35fe9b5d-57cc-11ec-ad18-0242ac110004");
            Guid deptId = Guid.Parse("a89f00d7-57bf-11ec-ad18-0242ac110004");
            modelBuilder.Entity<SysUser>().HasData(new SysUser()
            {
                Id = userId,
                AccountCode = "admin",
                UserName = "超级管理员",
                Password = Encrypt.MD5Encrypt("123456"),
                Gender = EnumGender.MALE,
                UserEmail = "dwen@outlook.com",
                PhoneNum = "",
                LoginDate = null,
                LoginIP = null,
                DeptId = deptId,
                NickName = "超级管理员",
                Avatar = "https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fup.enterdesk.com%2Fedpic%2F0e%2F17%2Fa6%2F0e17a6c3b98c9128bfe420450ac4df56.jpg&refer=http%3A%2F%2Fup.enterdesk.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1641955220&t=a69ff4e2830ba1e41a4541136091f473",
                CreateTime = DateTime.Now,
            });
            Guid roleId = Guid.Parse("c81edfd1-4bb2-11ec-9aa7-0242ac110003");
            modelBuilder.Entity<SysRole>().HasData(new SysRole()
            {
                Id = roleId,
                RoleName = "系统管理员",
                RoleCode = "admin",
                Status = EnumSysRoleStatus.ENABLE,
                Description = "系统管理员，拥有该系统整个权限",
                OrderSort = 1,
                CreateTime = DateTime.Now,
            });
            modelBuilder.Entity<SysUserRole>().HasData(new SysUserRole()
            {
                Id = Guid.Parse("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                UserId = userId,
                RoleId = roleId,
                CreateTime = DateTime.Now,
            });
            modelBuilder.Entity<SysDept>().HasData(new SysDept()
            {
                Id = deptId,
                ParentId = Guid.Empty,
                CurrentLevelCode = 1,
                AncestorsCode = "0.1",
                AncestorsFullName = "",
                DeptName = "总公司",
                SortNO = 1,
                CreateTime = DateTime.Now,
            });
            
        }
    }
}
