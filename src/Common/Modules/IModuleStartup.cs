using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Modules
{
    public interface IModuleStartup
    {
        /// <summary>
        /// 执行顺序 越小越靠前
        /// </summary>
        int Order { get; }

        void ConfigureServices(IServiceCollection services);

        void Configure(IApplicationBuilder builder);
    }

    public class ModuleStartupOrder
    {
        public int Order_Core { get; set; } = -1000;
        public int Order_App { get; set; } = 0;

        public static ModuleStartupOrder Instance = new ModuleStartupOrder();
    }

    public abstract class ModuleStartupBase : IModuleStartup
    {
        /// <inheritdoc />
        public virtual int Order { get; } = ModuleStartupOrder.Instance.Order_App;

        /// <inheritdoc />
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        /// <inheritdoc />
        public virtual void Configure(IApplicationBuilder app)
        {
        }
    }
}
