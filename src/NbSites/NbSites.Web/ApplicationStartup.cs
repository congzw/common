using System;
using Common;
using Common.Modules;
using Common.Modules.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NbSites.Web
{
    public class ApplicationStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyServiceLocator();

            var moduleStartupHelper = ModuleStartupHelper.Instance;
            moduleStartupHelper.GetAssemblies = () => moduleStartupHelper.GetAllModuleAssemblies(AppDomain.CurrentDomain.BaseDirectory, "NbSites");
            services.AddMyModules();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMyServiceLocator();

            app.UseMyModules();
        }
    }
}