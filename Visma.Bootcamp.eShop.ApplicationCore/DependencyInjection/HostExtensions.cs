using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Visma.Bootcamp.eShop.ApplicationCore.DependencyInjection
{
    public static class HostExtensions
    {
        public static async Task<IHost> MigrateDbContextAsync<TContext>(this IHost host)
            where TContext : DbContext
        {
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider provider = scope.ServiceProvider;
            ILogger<TContext> logger = provider.GetRequiredService<ILogger<TContext>>();
            TContext context = provider.GetRequiredService<TContext>();

            try
            {
                logger.LogDebug("Migrating database associated with context {context}", typeof(TContext).Name);

                await context.Database.MigrateAsync();

                logger.LogDebug("Successfully migrated database associated with context {context}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred while migrating database associated with context {context}: {err}",
                    typeof(TContext).Name, ex.Message);
            }

            return host;
        }
    }
}
