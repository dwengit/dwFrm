using Dw.Framework.ApplicationCore.AppServices.Account;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dw.Framework.Web.Infrastructure.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly IAccountService _userAppService;

        public PermissionAuthorizationHandler(IAccountService userAppService)
        {
            _userAppService = userAppService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var isDemoMode = Appsettings.App("IsDemoMode").ParseToBool();
                if (isDemoMode && !requirement.Name.Contains("Read"))
                {
                    throw new CustomException(ResultCodeEnums.ISDEMOMODE, "预览模式，不能操作");
                }
                if (context.User.IsInRole("admin") || context.User.IsInRole("sys"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var accountCodeClaim = context.User.FindFirst(_ => _.Type == ClaimTypes.PrimarySid);
                    if (accountCodeClaim != null)
                    {
                        if (_userAppService.CheckPermission(accountCodeClaim.Value, requirement.Name).GetAwaiter().GetResult())
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
