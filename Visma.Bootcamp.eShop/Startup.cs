using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Visma.Bootcamp.eShop.ApplicationCore.DependencyInjection;

namespace Visma.Bootcamp.eShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:4200", "http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Visma.Bootcamp.eShop",
                    Description = "This is a simple eShop API used for education purposes only.",
                    License = new OpenApiLicense
                    {
                        Name = "Visma Labs s.r.o.",
                        Url = new Uri("https://visma.sk")
                    },
                    Contact = new OpenApiContact
                    {
                        Email = "tomas.blanarik@visma.com",
                        Name = "Tomas Blanarik - Github",
                        Url = new Uri("https://github.com/tomas-blanarik/Visma.Bootcamp.eShop")
                    },
                    Version = "1.0.0"
                });
                c.EnableAnnotations();
                c.AddServer(new OpenApiServer
                {
                    Description = "Development localhost server - Kestrel",
                    Url = "https://localhost:5001"
                });
                var securitySchema = new OpenApiSecurityScheme()
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                var securityRequirement = new OpenApiSecurityRequirement()
{
    {
        securitySchema,
        new[] {"Bearer"}
    }
};
                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(securityRequirement);
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMemoryCache();
            services.AddApplicationServices(Configuration, Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Visma.Bootcamp.eShop v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            //app.UseAuthentication(); // podozrive
            app.UseAuthorization();

            // use custom middlewares here
            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
