using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Dw.Framework.Infrastructure.Helper;
using Dw.Framework.Infrastructure;

namespace Dw.Framework.Web.Infrastructure.Auth.Policys
{
    public class ApiResponseHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ApiResponseHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 未登录处理
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var tokenExpired = Response.Headers["Token-Expired"].FirstOrDefault();
            string msg = !string.IsNullOrEmpty(tokenExpired) && bool.Parse(tokenExpired) ? "登录状态已过期，请重新登录" : null;
            var res = new ApiResponse(ResultCodeEnums.CODE401, msg);
            await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(res.ObjToJsonCamel()));
        }
        /// <summary>
        /// 未授权处理
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            var res = new ApiResponse(ResultCodeEnums.CODE403);
            await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(res.ObjToJsonCamel()));
        }

    }
}
