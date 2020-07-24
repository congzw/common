using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.DI
{
    [TestClass]
    public class MyLifetimeRegistrySpec
    {
        private IServiceCollection services;
        private IServiceProvider provider;
        private MyLifetimeRegistry myLifetimeRegistry;

        [TestInitialize]
        public void TestInitialize()
        {
            services = new ServiceCollection();
            myLifetimeRegistry = new MyLifetimeRegistry();
            myLifetimeRegistry.AutoRegister(services, new[] {
                typeof(MyLifetimeRegistrySpec).Assembly
            });
            provider = services.BuildServiceProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var lifetimeRegisterInfoCache = myLifetimeRegistry.Cache;
            var classTypeInfos = lifetimeRegisterInfoCache.ToClassTypeInfos().Where(x => x.Name.StartsWith("Foo")).ToList();
            foreach (var classTypeInfo in classTypeInfos)
            {
                classTypeInfo.ToJson(true).Log();
            }
            myLifetimeRegistry = null;
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AssembliesNull_Should_Throws()
        {
            myLifetimeRegistry.AutoRegister(services, null);
        }


        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ServicesNull_Should_Throws()
        {
            myLifetimeRegistry.AutoRegister(null, new List<Assembly>());
        }

        [TestMethod]
        public void Singleton_Should_ReturnOK()
        {
            var theOneWithInterface = provider.GetService<IFooSingleton>();
            using (var scope = provider.CreateScope())
            {
                var theOne = scope.ServiceProvider.GetService<IFooSingleton>();
                var theOne2 = scope.ServiceProvider.GetService<FooSingleton>();
                Assert.IsNotNull(theOne);
                Assert.IsNotNull(theOne2);
                Assert.AreSame(theOne, theOne2);
                Assert.AreSame(theOne, theOneWithInterface);
            }
        }

        [TestMethod]
        public void MyScope_Should_OK()
        {
            var disposed = false;
            using (var scope = provider.CreateScope())
            {
                var theOne = scope.ServiceProvider.GetService<FooScope>();
                theOne.DisposeInvoked = () => disposed = true;
                var theOne2 = scope.ServiceProvider.GetService<FooScope>();
                Assert.IsNotNull(theOne);
                Assert.IsNotNull(theOne2);
                Assert.AreSame(theOne, theOne2);
            }
            Assert.IsTrue(disposed);
        }
        
        [TestMethod]
        public void Transient_Should_OK()
        {
            using (var scope = provider.CreateScope())
            {
                var theOne = scope.ServiceProvider.GetService<FooTransient>();
                var theOne2 = scope.ServiceProvider.GetService<FooTransient>();
                Assert.IsNotNull(theOne);
                Assert.IsNotNull(theOne2);
                Assert.AreNotSame(theOne, theOne2);
            }
        }
        
        [TestMethod]
        public void Default_Should_OK()
        {
            using (var scope = provider.CreateScope())
            {
                var theOne = scope.ServiceProvider.GetService<FooDefault>();
                var theOne2 = scope.ServiceProvider.GetService<FooDefault>();
                Assert.IsNotNull(theOne);
                Assert.IsNotNull(theOne2);
                Assert.AreNotSame(theOne, theOne2);
            }
        }
        
        [TestMethod]
        public void NotAutoBind_Should_Ok()
        {
            using (var scope = provider.CreateScope())
            {
                var theFoo = scope.ServiceProvider.GetService<Foo>();
                Assert.IsNull(theFoo);
            }
        }

        [TestMethod]
        public void Ignore_Should_Ok()
        {
            using (var scope = provider.CreateScope())
            {
                var theFoo = scope.ServiceProvider.GetService<FooIgnore>();
                Assert.IsNull(theFoo);
            }
        }

        [TestMethod]
        public void RegisterLater_Should_Replace()
        {
            myLifetimeRegistry.AutoRegister(services, new[] {
                typeof(IMyLifetime).Assembly,
                typeof(MyLifetimeRegistrySpec).Assembly
            });

            services.AddSingleton<FooTransient>();
            var newProvider = services.BuildServiceProvider();

            using (var scope = newProvider.CreateScope())
            {
                var theFoo = scope.ServiceProvider.GetService<FooTransient>();
                var theFoo2 = scope.ServiceProvider.GetService<FooTransient>();
                Assert.AreSame(theFoo, theFoo2);
            }
        }
    }
}
