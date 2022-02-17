using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Dw.Framework.Web.Infrastructure.ServiceCollectionExtensions
{
    public static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string apiAssemblyName)
        {
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;//return versions in a response header
                o.DefaultApiVersion = new ApiVersion(1, 0);//default version select 
                o.AssumeDefaultVersionWhenUnspecified = true;//if not specifying an api version,show the default version
            }).AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVVV";//api group name
                option.AssumeDefaultVersionWhenUnspecified = true;//whether provide a service API version
            });

            
            services.AddSwaggerGen(s =>
            {
                var apiVersionDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                //生成api描述文档
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    s.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Contact = new OpenApiContact
                        {
                            Name = "Devin",
                            Email = "Devin@163.com",
                        },
                        Description = "A front-background project build by ASP.NET Core 3.1",
                        Title = "Dw.Framework",
                        Version = description.ApiVersion.ToString(),
                    });
                }

                //以url地址显示api版本
                s.DocInclusionPredicate((version, apiDescription) =>
                {
                    if (!version.Equals(apiDescription.GroupName))
                        return false;

                    var values = apiDescription.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", apiDescription.GroupName));

                    apiDescription.RelativePath = string.Join("/", values);
                    return true;
                });

                //Remove version parameter
                s.OperationFilter<RemoveVersionFromParameter>();

                //添加注释描述
                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);//get application located directory
                var apiPath = apiAssemblyName + ".xml";
                s.IncludeXmlComments(apiPath, true);

                Assembly asmModel = Assembly.Load("Dw.Framework.Model"); // 读取嵌入式资源
                Stream sms = asmModel.GetManifestResourceStream("Dw.Framework.Model.ModelDoc.xml");
                var model = new XPathDocument(sms);
                s.IncludeXmlComments(() => model, true);

                Assembly asmInfrastructure = Assembly.Load("Dw.Framework.Infrastructure"); // 读取嵌入式资源
                Stream smsInfrastructure = asmInfrastructure.GetManifestResourceStream("Dw.Framework.Infrastructure.Infrastructure.xml");
                var infrastructure = new XPathDocument(smsInfrastructure);
                s.IncludeXmlComments(() => infrastructure, true);

                //添加认证类型
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });
            return services;
        }
    }
}
