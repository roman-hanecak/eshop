using Microsoft.AspNetCore.Builder;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;

namespace Visma.Bootcamp.eShop.ApplicationCore.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }
    }
}
