using Common.Modules;
using Common.Modules.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NbSites.Web.Boots
{
    public class MainStartup : IModuleStartup
    {
        private readonly ILogger<MainStartup> _logger;
        private readonly IHostingEnvironment _env;

        public MainStartup(ILogger<MainStartup> logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public int Order { get; } = -100;

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<TestAppService>();

            var mvcBuilder = services.AddMvc();
            mvcBuilder.AddMyModulePart();
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMyStaticFiles(_env, _logger);

            //do not forget to add[Area("Foo")] in controllers of area
            app.UseMvc(routes =>
            {
                //:exists => applies KnownRouteValueConstraint on the route.
                //It makes the route only matches if an action is found with a corresponding route value.
                //Meaning it will only match the route if it targets an existing area in the case of { area: exists}.
                //for more => https://github.com/aspnet/Mvc/blob/rel/2.0.0/src/Microsoft.AspNetCore.Mvc.Core/Routing/KnownRouteValueConstraint.cs

                routes.MapRoute(
                    name: "route_space",
                    template: "Space/{user}/{controller}/{action}/{id?}",
                    defaults: new { site = (string)null },
                    constraints: null,
                    dataTokens: new
                    {
                        route = "route_space",
                        area = "space"
                    });


                routes.MapRoute(
                    name: "route_site_area",
                    template: "{site}/{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { site = "default", user = (string)null },
                    constraints: null,
                    dataTokens: new
                    {
                        route = "route_site_area"
                    });

                routes.MapRoute(
                    name: "route_area",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { site = "default", user = (string)null },
                    constraints: null,
                    dataTokens: new
                    {
                        route = "route_area"
                    });

                //todo redirect default "/" by config from HomeController
                routes.MapRoute(
                    name: "route_root",
                    template: "{controller=Home}/{action=Index}/{id?}",
                    defaults: new { site = "default", user = (string)null },
                    constraints: null,
                    dataTokens: new
                    {
                        route = "route_root",
                        area = (string)null
                    });
            });
        }
    }
}
