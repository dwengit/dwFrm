using AutoMapper;
using Dw.Framework.ApplicationCore.Repositorys;
using Dw.Framework.Web.Infrastructure.CustomMiddleWare;
using Dw.Framework.Web.Infrastructure.ServiceCollectionExtensions;
using Dw.Framework.Infrastructure.Caches.RedisHelper.Service;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.MiddlewareExtensions;
using Dw.Framework.Infrastructure.ObjectStorage;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
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
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using DateTimeConverter = Dw.Framework.Infrastructure.Utility.WebApiUtility.DateTimeConverter;
using Dw.Framework.Web.Infrastructure.Common.Filters;

namespace Dw.Framework.Blog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                //ʱ���ʽ��ʽ��
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
            });
            services.AddAutoMapper(Assembly.Load("Dw.Framework.ApplicationCore"));
            services.AddRepository<AppAdminMallDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Amdin_Mall"));
            });

            services.AddCustomAuthentication();
            services.AddCustomSwagger(Assembly.GetExecutingAssembly().GetName().Name);
            services.AddCustomAppService();

            //����Ĭ��ModelState��Ϊ
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),//wwwroot�൱����ʵĿ¼
                RequestPath = new PathString("/src") //src�൱�ڱ�����Ϊ�˰�ȫ
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
