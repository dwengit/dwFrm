using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.ApplicationCore.AppServices.Account;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model;
using Dw.Framework.Model.Dto.Account;
using Dw.Framework.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dw.Framework.Admin.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class UserController : ApiControllersBase
    {
        readonly IAccountService _accountAppService;
        public UserController(IAccountService accountAppService)
        {
            _accountAppService = accountAppService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Respbase<UserBasicInfoDto>> UserInfo()
        {
            string accountCode = HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.PrimarySid).Value;
            var res = await _accountAppService.GetUserInfo(accountCode);
            return RespOk(res);
        }
    }
}
