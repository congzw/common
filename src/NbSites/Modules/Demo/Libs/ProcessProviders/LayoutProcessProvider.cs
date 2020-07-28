using NbSites.Common.Layouts;
using NbSites.Common.ProcessProviders;

namespace NbSites.Areas.Web.Demo.Libs.ProcessProviders
{
    public class LayoutProcessProvider : IMyProcessProvider
    {
        public float ProcessOrder { get; set; }
        public bool ShouldProcess(object context)
        {
            return context is LayoutContext;
        }
        public void Process(object context)
        {
            var layoutContext = context as LayoutContext;
            if (layoutContext == null)
            {
                return;
            }

            if (layoutContext.Config == null)
            {
                layoutContext.Config = new LayoutConfig();
            }

            layoutContext.Config.Layout = "_Demo/_DemoLayout";
        }
    }
}
