using Dw.Framework.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Web.Infrastructure
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiControllersBase : ControllerBase
    {
        protected Respbase RespOk( string msg = "ok", int code = 1000)
        {
            return RespbaseFac.RespOk(msg, code);
        }
        protected Respbase<T> RespOk<T>(T t = default, string msg = "ok", int code = 1000)
        {
            return RespbaseFac.RespOk<T>(t, msg, code);
        }
        protected Respbase<T> RespFail<T>(string msg = "fail", T t = default, int code = -1)
        {
            return RespbaseFac.RespFail<T>(msg, t, code);
        }
        protected Respbase RespCustom(string msg, int code)
        {
            return RespbaseFac.RespCustom(msg, code);
        }
        protected Respbase<T> RespCustom<T>(string msg, int code, T t = default)
        {
            return RespbaseFac.RespCustom<T>(msg, code, t);
        }
        protected string CurrentIP
        {
            get
            {
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                return ip == "::1" ? "127.0.0.1" : ip;
            }
        }
    }
}
