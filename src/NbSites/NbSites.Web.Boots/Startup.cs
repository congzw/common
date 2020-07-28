using System;
using Common.Modules;
using Common.Modules.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NbSites.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var moduleStartupHelper = ModuleStartupHelper.Instance;
            moduleStartupHelper.GetAssemblies = () => moduleStartupHelper.GetAllModuleAssemblies(AppDomain.CurrentDomain.BaseDirectory, "NbSites");
            services.AddMyModules();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMyModules();
        }
    }
}