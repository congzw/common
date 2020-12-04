using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common
{
    [TestClass]
    public class ServiceLocatorSpec
    {
        [TestMethod]
        public void AddMyServiceLocator_Should_Ok()
        {
            var services = new ServiceCollection();
            services.AddMyServiceLocator();
            var rootProvider = services.BuildServiceProvider();

            var rootLocator = rootProvider.GetRequiredService<IServiceLocator>();
            var rootLocator2 = rootProvider.GetRequiredService<IServiceLocator>();
            rootLocator2.ShouldEqual(rootLocator);

            IServiceLocator scopedLocator1;
            IServiceLocator scopedLocator2;
            using (var scope = rootLocator.GetServiceProvider().CreateScope())
            {
                scopedLocator1 = scope.ServiceProvider.GetRequiredService<IServiceLocator>();
                scopedLocator1.ShouldNotNull().GetType().ShouldEqual(typeof(ServiceLocator));
            }

            using (var scope = rootLocator.GetServiceProvider().CreateScope())
            {
                scopedLocator2 = scope.ServiceProvider.GetRequiredService<IServiceLocator>();
                scopedLocator2.ShouldNotNull().GetType().ShouldEqual(typeof(ServiceLocator));

                var scopedLocator3 = scope.ServiceProvider.GetRequiredService<IServiceLocator>();
                scopedLocator3.ShouldEqual(scopedLocator2);
            }

            scopedLocator1.ShouldNotEqual(rootLocator);
            scopedLocator1.ShouldNotEqual(scopedLocator2);
        }

        [TestMethod]
        public void ServiceProvider_Resolve_Should_Ok()
        {
            var services = new ServiceCollection();
            services.AddMyServiceLocator();
            var rootProvider = services.BuildServiceProvider();
            ServiceLocator.SetRootProvider(rootProvider);
            
            IServiceLocator rootLocator = null;
            IServiceLocator rootLocator2 = null;
            IServiceLocator scopedLocator1 = null;
            IServiceLocator scopedLocator2 = null;

            rootLocator = ServiceLocator.Resolve();
            rootLocator2 = ServiceLocator.Resolve();
            rootLocator2.ShouldEqual(rootLocator);

            using (var scope = rootProvider.CreateScope())
            {
                scopedLocator1 = scope.ServiceProvider.GetRequiredService<IServiceLocator>();
                scopedLocator1.ShouldNotNull().GetType().ShouldEqual(typeof(ServiceLocator));
            }

            using (var scope = rootProvider.CreateScope())
            {
                scopedLocator2 = scope.ServiceProvider.GetRequiredService<IServiceLocator>();
                scopedLocator2.ShouldNotNull().GetType().ShouldEqual(typeof(ServiceLocator));

                var scopedLocator3 = scope.ServiceProvider.GetRequiredService<IServiceLocator>();
                scopedLocator3.ShouldEqual(scopedLocator2);
            }

            scopedLocator1.ShouldNotEqual(rootLocator);
            scopedLocator1.ShouldNotEqual(scopedLocator2);
        }
    }
}
