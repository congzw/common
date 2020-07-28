using Common;
using Common.AppContexts;
using Common.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Areas.Web.Demo.Libs.AppServices;

namespace NbSites.Areas.Web.Demo.Libs.Boots
{
    public class DemoStartup : IModuleStartup
    {
        public int Order { get; } = 0;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFooSingleton, FooService>();
            services.AddScoped<IFooScoped, FooService>();
            services.AddTransient<IFooTransient, FooService>();
        }

        public void Configure(IApplicationBuilder builder)
        {
            var myAppContext = MyAppContext.Current;
            myAppContext.SetBagValue("EntryUri", "~/Demo/Home/Index");
        }
    }
}
