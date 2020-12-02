using System;
using System.Collections.Generic;
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

            services.AddSingleton(ServiceLocator.Instance);
            services.AddSingleton<IServiceLocator, HttpRequestServiceLocator>();
            
            return services;
        }

        public static void UseMyServiceLocator(this IApplicationBuilder app)
        {
            ServiceLocator.Instance.Initialize(app.ApplicationServices);
        }
    }

    //how to use:
    //1 setup => services.AddSingleton<IServiceLocator, HttpRequestServiceLocator>();
    //2 use => ServiceLocator.Initialize(app.ApplicationServices.GetService<IServiceLocator>());
    public class HttpRequestServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _rootServiceProvider;

        public HttpRequestServiceLocator(IServiceProvider rootServiceProvider)
        {
            _rootServiceProvider = rootServiceProvider;
        }

        public T GetService<T>()
        {
            var contextAccessor = _rootServiceProvider.GetService<IHttpContextAccessor>();
            var httpContext = contextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return _rootServiceProvider.GetService<T>();
            }
            return contextAccessor.HttpContext.RequestServices.GetService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            var contextAccessor = _rootServiceProvider.GetService<IHttpContextAccessor>();
            var httpContext = contextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return _rootServiceProvider.GetServices<T>();
            }
            return contextAccessor.HttpContext.RequestServices.GetServices<T>();
        }

        public object GetService(Type type)
        {
            var contextAccessor = _rootServiceProvider.GetService<IHttpContextAccessor>();
            var httpContext = contextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return _rootServiceProvider.GetService(type);
            }
            return contextAccessor.HttpContext.RequestServices.GetService(type);
        }

        public IEnumerable<object> GetServices(Type type)
        {
            var contextAccessor = _rootServiceProvider.GetService<IHttpContextAccessor>();
            var httpContext = contextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return _rootServiceProvider.GetServices(type);
            }
            return contextAccessor.HttpContext.RequestServices.GetServices(type);
        }
    }
}