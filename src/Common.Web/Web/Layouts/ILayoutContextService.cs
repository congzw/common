using System;

namespace Common.Web.Layouts
{
    public interface ILayoutContextService
    {
        LayoutContext GetLayoutContext();
    }

    public class LayoutContextService : ILayoutContextService
    {
        public LayoutContext GetLayoutContext()
        {
            return new LayoutContext { Layout = "_Layout" };
        }

        private static readonly Lazy<LayoutContextService> _lazy = new Lazy<LayoutContextService>(() => new LayoutContextService());
        public static Func<ILayoutContextService> Resolve { get; set; } = () => ServiceLocator.Current.GetService<ILayoutContextService>() ?? _lazy.Value;
    }
}