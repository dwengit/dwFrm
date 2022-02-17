using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Controllers.v1
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("成功");
        }

        [AllowAnonymous]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(string userName, string pwd, string returnUrl = "")
        {
            bool succee = (userName == "admin") && (pwd == "123");

            if (succee)
            {
                //创建用户身份标识
                var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaims(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid, userName),
                        new Claim(ClaimTypes.Name, pwd),
                        new Claim(ClaimTypes.Role, "admin"),
                    });

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Redirect("/swagger/index.html");
            }
            else
            {
                return Content("帐号或者密码错误");
            }
        }
    }
}
