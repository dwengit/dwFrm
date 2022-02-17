using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dw.Framework.Web.Infrastructure.ServiceCollectionExtensions
{
    public static class AppCustomServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAppService(this IServiceCollection services)
        {
            var assembly = Assembly.Load("Dw.Framework.ApplicationCore");
            //获取程序集下所有类
            Type[] types = assembly.GetTypes().Where(w => w.FullName.EndsWith("AppService")).ToArray();

            types.ToList().ForEach(t =>
            {
                //非接口，非抽象类
                if (!t.IsInterface && !t.IsAbstract)
                {
                    //获取已实现或继承的所有接口
                    Type[] interfaces = t.GetInterfaces();
                    //注入
                    interfaces.ToList().ForEach(r =>
                    {
                        services.AddScoped(r, t);
                    });
                }
            });

            return services;
        }
    }
}
