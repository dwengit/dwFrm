using AutoMapper;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.AutoMapperProfile
{
    public class SystemMap : Profile
    {
        public SystemMap()
        {
            CreateMap<SysModuleResource, MenuFunctionalPageTree>();
            CreateMap<SysModuleResource, MenuFunctionalDto>();
            CreateMap<SaveMenuFunctionalInput, SysModuleResource>();
            CreateMap<SysModuleResource, MenuFunctionalTreeOptions>().ForMember(dest => dest.Label, opt => opt.MapFrom(s => s.ResourceName)); 

            CreateMap<SysDept, DeptPageTree>();
            CreateMap<SysDept, DeptDto>();
            CreateMap<SaveDeptInput, SysDept>();
            CreateMap<SysDept, DeptTreeOptions>().ForMember(dest => dest.Label, opt => opt.MapFrom(s => s.DeptName));

            CreateMap<SysRole, RolePage>();
            CreateMap<Page<SysRole>, Page<RolePage>>();
            CreateMap<SaveRoleInput, SysRole>();
            CreateMap<SysRole, RoleDto>();

            CreateMap<SaveUserInput, SysUser>();
            CreateMap<SysUser, UserDto>();
            CreateMap<Page<SysUser>, Page<UserPage>>();

            CreateMap<SysLoginLog, SysLoginLogPage>();
            CreateMap<Page<SysLoginLog>, Page<SysLoginLogPage>>();

            CreateMap<SysOperLog, SysOperLogPage>();
            CreateMap<Page<SysOperLog>, Page<SysOperLogPage>>();
        }
    }
}
