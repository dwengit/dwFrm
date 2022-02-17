using Dw.Framework.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.Infrastructure.MiddlewareExtensions.CustomMiddleWare
{
    internal class InfrastructureCoreMiddleware
    {
        private readonly RequestDelegate next;

        public InfrastructureCoreMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            ThreadDbContextFactory.SetDbContext(serviceProvider.GetService<DbContext>());
            await next(context);
        }
    }
}
