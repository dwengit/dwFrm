using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dw.Framework.Infrastructure.Helper;
using Newtonsoft.Json;
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Auth.Policys;

namespace Dw.Framework.Web.Infrastructure.ServiceCollectionExtensions
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            //读取配置文件
            var audienceConfig = configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.Role,//基于角色的授权
                audienceConfig["Issuer"],//发行人
                audienceConfig["Audience"],//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 20)//接口的过期时间
            );

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(60 * 20),
                RequireExpirationTime = true,
            };

            ////注册全局鉴权策略
            //services.AddControllers(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder(new string[] { JwtBearerDefaults.AuthenticationScheme, "Cookie" })
            //            .RequireAuthenticatedUser()
            //            .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});

            // 开启Bearer认证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // 添加JwtBearer服务
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
                o.ForwardChallenge = nameof(ApiResponseHandler);
                o.ForwardForbid = nameof(ApiResponseHandler);
                o.ClaimsIssuer = tokenValidationParameters.ValidIssuer;
                o.Audience = tokenValidationParameters.ValidAudience;
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var token = context.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);
                            if (jwtToken.Issuer != tokenValidationParameters.ValidIssuer)
                            {
                                context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                            }
                            if (jwtToken.Audiences.FirstOrDefault() != tokenValidationParameters.ValidAudience)
                            {
                                context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                            }
                            // 如果过期，则把<是否过期>添加到，返回头信息中
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            })
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
             {
                 o.Cookie.Name = "tokenCookie";//设置存储用户用户Token的Cookie名称
                 o.Cookie.HttpOnly = true;//无法通过客户端浏览器脚本(如JavaScript等)访问
                 o.ExpireTimeSpan = tokenValidationParameters.ClockSkew;// 过期时间
                 o.SlidingExpiration = true;// 是否在过期时间过半的时候，自动延期
                 o.LoginPath = "/Account/Login";
                 o.LogoutPath = "/Account/LoginOut";
             })
             .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });

            services.AddSingleton(permissionRequirement);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissionPolicy", policy =>
                    policy.Requirements.Add(new PermissionAuthorizationRequirement()));
            });
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
           
            return services;
        }

    }
}
