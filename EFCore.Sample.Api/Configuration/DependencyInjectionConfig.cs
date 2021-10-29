using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Models.Safe2Pay;
using EFCore.Sample.Business.Notifications;
using EFCore.Sample.ExternalApi.Repository;
using EFCore.Sample.ExternalApi.Services;
using EFCore.Sample.ORM.Context;
using EFCore.Sample.ORM.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EFCore.Sample.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<Sped_SafewebContext>();
            services.AddScoped<Sped_SafewebContextMemory>();
            services.AddScoped<IContasReceberRepository, ContasReceberRepository>();
            services.AddScoped<ISafePayRepository, SafePayRepository>();
            services.AddScoped<ISafePayService, SafePayService>();
            services.AddScoped<INotificator, Notificator>();
            services.AddSingleton<ISafe2PayConfigManager, Safe2PayConfigManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}