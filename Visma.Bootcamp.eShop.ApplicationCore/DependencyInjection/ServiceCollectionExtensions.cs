using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Database;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Services;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            AddDatabase(services, configuration);
            AddLogging(services, environment);
            AddServices(services);
            AddCache(services, configuration);
            //podozrive
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Token:Key").Value)),
                        ValidIssuer = configuration.GetSection("Token:Issuer").Value,
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });
            //podozrive koniec
            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IBasketService, BasketService>();
            services.AddTransient<IAuthService, AuthService>();
        }

        #region Infrastructure
        private static void AddLogging(IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddSerilog(new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(new EnvironmentLoggingLevelSwitch(environment))
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Conditional(l => environment.IsDevelopment(), c => c.Console())
                    .CreateLogger());
            });
        }

        private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                //options.UseInMemoryDatabase("Visma.Bootcamp.eShop-db");
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    opts =>
                    {
                        opts.MigrationsAssembly("Visma.Bootcamp.eShop.ApplicationCore");
                    });
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });
        }

        private static void AddCache(IServiceCollection services, IConfiguration configuration)
        {
            if (bool.TryParse(configuration["UseRedisCache"], out bool useRedisCache) && useRedisCache)
            {
                string connectionString = configuration.GetConnectionString("RedisConnection");
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = connectionString;
                    options.InstanceName = "Redis Cache";
                });

                services.AddSingleton<ICacheManager, DistributedCacheManager>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICacheManager, CacheManager>();
            }
        }
        #endregion
    }

    internal class EnvironmentLoggingLevelSwitch : LoggingLevelSwitch
    {
        public EnvironmentLoggingLevelSwitch(IWebHostEnvironment environment)
        {
            MinimumLevel = environment.IsDevelopment()
                ? LogEventLevel.Debug
                : LogEventLevel.Information;
        }
    }
}
