using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EFCore.Sample.Api.Configuration;
using EFCore.Sample.ORM.Context;
using System;

namespace EFCore.Sample.Api
{
    public class Startup
    {
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<Sped_SafewebContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Connection_DEV"),
                    p => p.EnableRetryOnFailure(
                            maxRetryCount: 2,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null).MigrationsHistoryTable("SafeWebContainmentORM")
                    );
            });

            services.AddDbContext<Sped_SafewebContextMemory>(options => options.UseInMemoryDatabase(databaseName: "SafeWebInMemory"));

            services.AddAutoMapper(typeof(Startup));

            services.AddApiConfig();

            services.AddSwaggerConfig();

            services.ResolveDependencies();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseApiConfig(env);

            app.UseSwaggerConfig(provider);
        }
    }
}
