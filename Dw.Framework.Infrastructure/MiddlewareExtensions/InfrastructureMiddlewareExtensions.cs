using Dw.Framework.Infrastructure.MiddlewareExtensions.CustomMiddleWare;
using Microsoft.AspNetCore.Builder;

namespace Dw.Framework.Infrastructure.MiddlewareExtensions
{
    public static class InfrastructureMiddlewareExtensions
    {
        public static IApplicationBuilder UseInfrastructureCore(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<InfrastructureCoreMiddleware>();
        }
    }
}
