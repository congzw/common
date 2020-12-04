using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public static class ServiceLocatorExtensions
    {
        public static IServiceCollection AddMyServiceLocator(this IServiceCollection services)
        {
            if (services.All(x => x.ServiceType != typeof(IHttpContextAccessor)))
            {
                //only add if the service doesn't exist
                services.AddHttpContextAccessor();
            }

            services.AddScoped<IServiceLocator, ServiceLocator>();
            
            return services;
        }

        public static void UseMyServiceLocator(this IApplicationBuilder app)
        {
            ServiceLocator.SetRootProvider(app.ApplicationServices);
        }

        public static TService GetService<TService>(this IServiceProvider serviceProvider, Func<TService> defaultImpl)
        {
            var service = serviceProvider.GetService<TService>();
            return service  != null ? service : defaultImpl();
        }
    }
}