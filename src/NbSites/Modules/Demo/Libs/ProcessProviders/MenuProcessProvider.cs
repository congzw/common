using NbSites.Common.Menus;
using NbSites.Common.ProcessProviders;

namespace NbSites.Areas.Web.Demo.Libs.ProcessProviders
{
    public class MenuProcessProvider : IMyProcessProvider
    {
        public float ProcessOrder { get; set; }

        public bool ShouldProcess(object context)
        {
            return context is MenuContext;
        }

        public void Process(object context)
        {
            var menuContext = context as MenuContext;
            if (menuContext == null)
            {
                return;
            }

            AddDemoMenu(menuContext, "~/");
            AddDemoMenu(menuContext, "~/default/Demo/Home/Index");
            AddDemoMenu(menuContext, "~/foo/Demo/Home/Index");
            AddDemoMenu(menuContext, "~/foo/Demo/Home/Index?site=bar");
            AddDemoMenu(menuContext, "~/Demo/Home/Index");
            AddDemoMenu(menuContext, "~/Demo/Home/DiTest");
            AddDemoMenu(menuContext, "~/Space/Admin/Home/Index?site=abc&user=allen&foo=whatever");
            AddDemoMenu(menuContext, "~/index.html");
        }

        private void AddDemoMenu(MenuContext menuContext, string href)
        {
            menuContext.Menus.Add(new Menu() { FromArea = "Demo", Href = href, Key = href, ParentKey = "/", Text = href });
        }
    }
}
