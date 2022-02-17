using Dw.Framework.ApplicationCore.AppServices.Account;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Dto.Account;
using Dw.Framework.Web.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class PermissionController : ApiControllersBase
    {
        readonly IAccountService _accountAppService;
        public PermissionController(IAccountService accountAppService)
        {
            _accountAppService = accountAppService;
        }
        [HttpGet]
        public async Task<Respbase<List<UserRouteDto>>> UserRoutes()
        {
            string accountCode = HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.PrimarySid).Value;
            return RespOk(await _accountAppService.UserRoutes(accountCode));
        }
    }
}
