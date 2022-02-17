using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Accounts.Dto;
using Dw.Framework.Model.Dto.Account;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.AutoMapperProfile
{
    public class AccountMap : Profile
    {
        public AccountMap()
        {
            CreateMap<SysUser, UserBasicInfoDto>();
            CreateMap<SysModuleResource, PermissionDto>()
                .ForMember(dest => dest.PermissionCode, opt => opt.MapFrom(src => src.ResourceCode))
                .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.ResourceName))
                .ForMember(dest => dest.PermissionIcon, opt => opt.MapFrom(src => src.ResourceIcon))
                .ForMember(dest => dest.Children, opt => opt.Ignore());

        }
    }
}
