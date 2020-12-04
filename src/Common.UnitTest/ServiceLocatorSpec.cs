using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common
{
    [TestClass]
    public class ServiceLocatorSpec
    {
        [TestMethod]
        public void ServiceLocator_Should_Singleton()
        {
            var services = new ServiceCollection();
            services.AddMyServiceLocator();
            var rootProvider = services.BuildServiceProvider();
            ServiceLocator.SetRootProvider(rootProvider);

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
            }
            scopedLocator1.ShouldEqual(rootLocator);
            scopedLocator1.ShouldEqual(scopedLocator2);
        }

        [TestMethod]
        public void ServiceLocator_NotAddMyServiceLocator_Should_Singleton()
        {
            var services = new ServiceCollection();
            services.AddMyServiceLocator();
            var rootProvider = services.BuildServiceProvider();
            ServiceLocator.SetRootProvider(rootProvider);

            var rootLocator = ServiceLocator.Current.GetRequiredService<IServiceLocator>();
            var rootLocator2 = ServiceLocator.Current.GetRequiredService<IServiceLocator>();
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
            }
            scopedLocator1.ShouldEqual(rootLocator);
            scopedLocator1.ShouldEqual(scopedLocator2);
        }
    }
}
