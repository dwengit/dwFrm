using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dw.Framework.Web.Infrastructure.Auth.Helpers;
using Dw.Framework.Web.Infrastructure.Auth.Policys;
using Dw.Framework.ApplicationCore.AppServices.Account;
using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.ApplicationCore.AppServices.Accounts.Dto;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Caches;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using IPTools.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UAParser;
using Dw.Framework.Web.Infrastructure;

namespace Dw.Framework.Admin.Api.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ApiControllersBase
    {
        private readonly PermissionRequirement _requirement;
        private readonly IAccountService _accountAppService;
        private readonly ISysLoginLogAppService _sysLoginLogAppService;

        public LoginController(PermissionRequirement requirement, IAccountService accountAppService, ISysLoginLogAppService sysLoginLogAppService)
        {
            _requirement = requirement;
            _accountAppService = accountAppService;
            _sysLoginLogAppService = sysLoginLogAppService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<Respbase<TokenInfoViewModel>> LoginAsync(LoginInput req)
        {
            var checkRes = await _accountAppService.CheckLoginAsync(req);
            if (!checkRes.Success)
            {
                await _sysLoginLogAppService.InsertLoginLog(_accountAppService.BuildLogInfo(base.HttpContext, EnumSysLoginLogStatus.ERROR, checkRes.Msg, req.AccountCode));
                return RespFail<TokenInfoViewModel>(checkRes.Msg);
            }
            var user = checkRes.Data;
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.PrimarySid, user.AccountCode),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
            var roleClaims = (await _accountAppService.GetUserAllRole(user.Id)).Select(s => new Claim(ClaimTypes.Role, s.RoleCode));
            claims.AddRange(roleClaims);

            //用户标识
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);

            var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
            await _sysLoginLogAppService.InsertLoginLog(_accountAppService.BuildLogInfo(base.HttpContext, EnumSysLoginLogStatus.OK, "登录成功", req.AccountCode));
            return RespOk(token);
        }
        [Route("ValidateCode")]
        [HttpGet]
        public IActionResult ValidateCode(string validateKey)
        {
            string validateString = ValidateCodeHelper.CreateVaildateCode(4);
            byte[] buffer = ValidateCodeHelper.CreateValidateImgByCode(validateString);
            MemoryCacheHelper.SetCache(validateKey, validateString, 1);
            return File(buffer, @"image/png");
        }
    }
}
