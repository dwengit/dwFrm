using AutoMapper;
using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.Web.Infrastructure.Common.Filters;
using Dw.Framework.Web.Infrastructure.CustomMiddleWare;
using Dw.Framework.Web.Infrastructure.ServiceCollectionExtensions;
using Dw.Framework.ApplicationCore;
using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.ApplicationCore.Repositorys;
using Dw.Framework.Infrastructure.Caches.RedisHelper.Service;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.MiddlewareExtensions;
using Dw.Framework.Infrastructure.ObjectStorage;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dw.Framework.Admin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<RedisZSetService>();
            services.AddScoped<RedisStringService>();
            services.AddScoped<RedisHashService>();
            services.AddScoped<RedisListService>(); 
            services.AddScoped<RedisSetService>();

            services.AddSingleton(new Appsettings(Configuration));
            services.AddSingleton<MinioAPI>();
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.Filters.Add<ValidateParameterFilterAttribute>();
            })
            .AddXmlDataContractSerializerFormatters()
            .AddJsonOptions(options =>
            {
                //时间格式格式化
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
            });
            services.AddAutoMapper(Assembly.Load("Dw.Framework.ApplicationCore"));
            services.AddRepository<AppAdminMallDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Amdin_Mall"));
                //options.UseSqlServer(Configuration.GetConnectionString("Amdin_Mall"));
            });

            services.AddCustomAuthentication();
            services.AddCustomSwagger(Assembly.GetExecutingAssembly().GetName().Name);
            services.AddCustomAppService();

            //禁用默认ModelState行为
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.Use(next => new RequestDelegate(
                async context =>
                {
                    context.Request.EnableBuffering();
                    await next(context);
                }
            ));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),//wwwroot相当于真实目录
                RequestPath = new PathString("/src") //src相当于别名，为了安全
            });
            app.UseInfrastructureCore();
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Enable Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
                {
                    s.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"Grapefruit.VuCore API {description.GroupName.ToUpperInvariant()}");
                }
            });

            #endregion


        }
    }
}
