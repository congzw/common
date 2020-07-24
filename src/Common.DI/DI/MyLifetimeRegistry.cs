using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DI
{
    public class MyLifetimeRegistry
    {
        public IList<Type> IgnoreServiceInterfaces { get; set; } = new List<Type>();

        public MyLifetimeRegistry()
        {
            IgnoreServiceInterfaces.Add(typeof(IDisposable));
        }

        public AutoRegisterServiceCache Cache { get; set; } = new AutoRegisterServiceCache();

        public void AutoRegister(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            var lifetimeType = typeof(IMyLifetime);
            var lifetimeSingletonType = typeof(IMySingleton);
            var lifetimeScopedType = typeof(IMyScoped);
            var lifetimeTransientType = typeof(IMyTransient);
            var lifetimeIgnoreType = typeof(IMyLifetimeIgnore);

            var autoBindTypes = assemblies.SelectMany(x =>
                    x.ExportedTypes.Where(t =>
                        lifetimeType.IsAssignableFrom(t)
                        && !lifetimeIgnoreType.IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface)).ToList();

            foreach (var autoBindType in autoBindTypes)
            {
                var bindTypeImplInterfaces = autoBindType.GetInterfaces();
                var serviceInterfaces = bindTypeImplInterfaces.Where(t =>
                    t != lifetimeType
                    && t != lifetimeSingletonType
                    && t != lifetimeScopedType
                    && t != lifetimeTransientType).ToList();

                if (lifetimeSingletonType.IsAssignableFrom(autoBindType))
                {
                    //bind self
                    services.AddSingleton(autoBindType);
                    AddIfNotExist(autoBindType, autoBindType, ServiceLifetime.Singleton);

                    foreach (var serviceInterface in serviceInterfaces)
                    {
                        if (!IgnoreServiceInterfaces.Contains(serviceInterface))
                        {
                            services.AddSingleton(serviceInterface, sp => sp.GetService(autoBindType));
                            AddIfNotExist(autoBindType, serviceInterface, ServiceLifetime.Singleton);
                        }
                    }
                    continue;
                }

                if (lifetimeScopedType.IsAssignableFrom(autoBindType))
                {
                    //bind self
                    services.AddScoped(autoBindType);
                    AddIfNotExist(autoBindType, autoBindType, ServiceLifetime.Scoped);

                    foreach (var serviceInterface in serviceInterfaces)
                    {
                        if (!IgnoreServiceInterfaces.Contains(serviceInterface))
                        {
                            services.AddScoped(serviceInterface, sp => sp.GetService(autoBindType));
                            AddIfNotExist(autoBindType, serviceInterface, ServiceLifetime.Scoped);
                        }
                    }
                    continue;
                }

                //"IMyTransient" and "IMyLifetime" will use IMyTransient
                //bind self
                services.AddTransient(autoBindType);
                AddIfNotExist(autoBindType, autoBindType, ServiceLifetime.Transient);

                foreach (var serviceInterface in serviceInterfaces)
                {
                    if (!IgnoreServiceInterfaces.Contains(serviceInterface))
                    {
                        services.AddTransient(serviceInterface, autoBindType);
                        AddIfNotExist(autoBindType, serviceInterface, ServiceLifetime.Transient);
                    }
                }
            }
        }

        private void AddIfNotExist(Type autoBindType, Type serviceInterface, ServiceLifetime lifetime)
        {
            Cache.AddIfNotExist(autoBindType, serviceInterface, lifetime);
        }

        public static MyLifetimeRegistry Instance = new MyLifetimeRegistry();
    }

    public class AutoRegisterServiceCache
    {
        public IDictionary<Type, List<ServiceDescriptor>> Items { get; set; } = new Dictionary<Type, List<ServiceDescriptor>>();
        public void AddIfNotExist(Type classType, Type serviceType, ServiceLifetime lifetime)
        {
            var lifetimeRegisterInfo = new ServiceDescriptor(serviceType, classType, lifetime);

            if (!Items.ContainsKey(classType))
            {
                Items.Add(classType, new List<ServiceDescriptor> { lifetimeRegisterInfo });
            }
            else
            {
                var serviceTypes = Items[classType];
                var theOne = serviceTypes.FirstOrDefault(x => x.ServiceType == serviceType);
                if (theOne == null)
                {
                    serviceTypes.Add(lifetimeRegisterInfo);
                }
            }
        }
        public IList<ClassTypeInfo> ToClassTypeInfos()
        {
            var classTypeInfos = new List<ClassTypeInfo>();
            foreach (var item in this.Items)
            {
                var classType = item.Key;

                var classTypeInfo = new ClassTypeInfo();
                classTypeInfo.Namespace = classType.Namespace;
                classTypeInfo.Name = classType.Name;

                var infos = item.Value.Where(x => x.ServiceType != classType).ToList();
                foreach (var info in infos)
                {
                    classTypeInfo.Services.Add(new ServiceTypeInfo() { Name = info.ServiceType.Name, Namespace = info.ServiceType.Namespace, Lifetime = info.Lifetime.ToString() });
                }

                classTypeInfos.Add(classTypeInfo);
            }

            return classTypeInfos;
        }
    }
    
    #region vo

    public interface ITypeInfo
    {
        string Name { get; set; }
        string Namespace { get; set; }
    }
    public class ServiceTypeInfo : ITypeInfo
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Lifetime { get; set; }
    }
    public class ClassTypeInfo : ITypeInfo
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public List<ServiceTypeInfo> Services { get; set; } = new List<ServiceTypeInfo>();
    }

    #endregion
}
