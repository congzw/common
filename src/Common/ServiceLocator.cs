using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    /// <summary>
    /// 用于某些无法实现注入的特殊场合，尽量避免直接使用！
    /// ServiceLocator is an anti-pattern, avoid using it as possible as you can!
    /// only for static inject or legacy code hacking!
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// 获取当前的IServiceProvider
        /// </summary>
        /// <returns></returns>
        IServiceProvider GetServiceProvider();
    }
    
    /// <summary>
    /// 用于某些无法实现注入的特殊场合，尽量避免直接使用！默认的单例模式实现
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _rootProvider;

        public ServiceLocator(IServiceProvider rootProvider)
        {
            _rootProvider = rootProvider ?? throw new ArgumentNullException(nameof(rootProvider));
        }

        public IServiceProvider GetServiceProvider()
        {
            var contextAccessor = _rootProvider.GetService<IHttpContextAccessor>();
            var httpContext = contextAccessor?.HttpContext;
            if (httpContext != null)
            {
                return contextAccessor.HttpContext.RequestServices;
            }
            return _rootProvider;
        }

        #region for di extensions

        /// <summary>
        /// 获取当前的IServiceProvider
        /// </summary>
        public static IServiceProvider Current => Resolve().GetServiceProvider();

        internal static Func<IServiceLocator> Resolve { get; set; }

        #endregion

        /// <summary>
        /// 初始化ServiceLocator
        /// </summary>
        /// <param name="rootProvider"></param>
        public static void SetRootProvider(IServiceProvider rootProvider)
        {
            Resolve = rootProvider.GetService<IServiceLocator>;
        }
    }
}