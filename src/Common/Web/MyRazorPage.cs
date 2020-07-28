using Common.Web.MyContexts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Common.Web
{
    public abstract class MyRazorPage<TModel> : RazorPage<TModel>
    {
        public override ViewContext ViewContext
        {
            get => base.ViewContext;
            set
            {
                base.ViewContext = value;
                MyContext.GetCurrent(Context).AppendPageInfos(base.ViewContext);
            }
        }
    }

    public abstract class MyRazorPage : MyRazorPage<dynamic>
    {
    }
}
