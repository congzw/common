using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public interface IServiceLocator
    {
        /// <summary>
        /// 获取IServiceProvider
        /// </summary>
        /// <returns></returns>
        IServiceProvider GetServiceProvider();
    }

    ////ServiceLocator is an anti-pattern, avoid using it as possible as you can!
    ////only for static inject or legacy code hacking!
    public class ServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _serviceProvider;
        public ServiceLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }

        #region for extensions and easy use

        public static IServiceProvider Current => Resolve().GetServiceProvider();

        public static Func<IServiceLocator> Resolve = GetLocator;
        private static IServiceLocator GetLocator()
        {
            if (_rootProvider == null)
            {
                throw new InvalidOperationException("没有通过SetRootProvider初始化！");
            }
            var contextAccessor = _rootProvider.GetService<IHttpContextAccessor>();
            var httpContext = contextAccessor?.HttpContext;
            if (httpContext != null)
            {
                return contextAccessor.HttpContext.RequestServices.GetService<IServiceLocator>();
            }

            return _rootProvider.GetService<IServiceLocator>();
        }

        private static IServiceProvider _rootProvider;
        public static void SetRootProvider(IServiceProvider rootProvider)
        {
            _rootProvider = rootProvider;
        }

        #endregion
    }
}
