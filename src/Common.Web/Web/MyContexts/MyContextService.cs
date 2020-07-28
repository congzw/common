using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Common.Web.MyContexts
{
    public interface IMyContextService
    {
        MyContext GetMyContext(HttpContext context);
    }

    public class MyContextService : IMyContextService
    {
        public static string MyContextCacheKey = "MyCache." + typeof(MyContext).FullName;

        public MyContext GetMyContext(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Items[MyContextCacheKey] != null)
            {
                return (MyContext)context.Items[MyContextCacheKey];
            }

            var requestContext = new MyContext()
                .SetRouteInfo(context.GetRouteData())
                .SetQueryInfo(context.Request)
                .SetUserInfo(context.Request);
            context.Items[MyContextCacheKey] = requestContext;

            return requestContext;
        }

        private static readonly Lazy<MyContextService> _lazy = new Lazy<MyContextService>(() => new MyContextService());
        public static Func<IMyContextService> Resolve { get; set; } = () => ServiceLocator.Current.GetService<IMyContextService>() ?? _lazy.Value;
    }
}
