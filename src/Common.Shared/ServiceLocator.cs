using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public interface IServiceLocator
    {
        T GetService<T>();
        IEnumerable<T> GetServices<T>();
        object GetService(Type type);
        IEnumerable<object> GetServices(Type type);
    }

    public class NullServiceLocator : IServiceLocator
    {
        public T GetService<T>()
        {
            return default(T);
        }

        public IEnumerable<T> GetServices<T>()
        {
            return Enumerable.Empty<T>();
        }

        public object GetService(Type type)
        {
            return null;
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return Enumerable.Empty<object>();
        }
    }

    ////ServiceLocator is an anti-pattern, avoid using it as possible as you can!
    ////only for static inject or legacy code hacking!
    public class ServiceLocator
    {
        public Func<IServiceLocator> Resolve = () => NullLazy.Value;
        public void Initialize(IServiceProvider rootServiceProvider)
        {
            if (rootServiceProvider == null) throw new ArgumentNullException(nameof(rootServiceProvider));
            _returnNull = false;
            Resolve = () =>
            {
                if (_returnNull)
                {
                    return NullLazy.Value;
                }

                var serviceProvider = rootServiceProvider.GetService<IServiceProvider>();
                var serviceLocator = serviceProvider.GetService<IServiceLocator>();
                if (serviceLocator != null)
                {
                    return serviceLocator;
                }

                _returnNull = true;
                return NullLazy.Value;
            };
        }
        private static readonly Lazy<NullServiceLocator> NullLazy = new Lazy<NullServiceLocator>(() => new NullServiceLocator());
        private bool _returnNull = true;

        #region for simple use

        public static IServiceLocator Current => Instance.Resolve();
        public static ServiceLocator Instance = new ServiceLocator();

        #endregion
    }
}
