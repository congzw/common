using System.Linq;
using Common.Modules.Extensions;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Modules
{
    [TestClass]
    public class ModuleServiceContextSpec
    {
        [TestMethod]
        public void ModuleServiceContext_Startup_Should_Ok()
        {
            var services = new ServiceCollection();

            //replace hostingEnvironment:
            //var hostingEnvironment = new HostingEnvironment();
            //hostingEnvironment.ApplicationName = GetType().Assembly.GetName().Name;
            //hostingEnvironment.ContentRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules");
            //services.AddSingleton(typeof(IHostingEnvironment), hostingEnvironment);

            services.AddMyModules();

            var provider = services.BuildServiceProvider();
            var builder = new ApplicationBuilder(provider);
            builder.UseMyModules();
            builder.Build();

            var context = provider.GetService<IModuleServiceContext>();
            var appServices = context.Services;
            
            appServices.ShouldNotNull();
            appServices.ShouldEqual(services);

            var modules = builder.ApplicationServices.GetServices<IModuleStartup>().ToList();
            modules.Log();
            modules.Count.ShouldEqual(2);
        }
    }
}
