using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Common.Web.MyContexts
{
    public class MyRequestContext
    {
        public MyRequestContext()
        {
            Groups = new List<MyContextGroup>();
        }
        
        public IList<MyContextGroup> Groups { get; set; }

        public static MyRequestContext Create()
        {
            var context = new MyRequestContext();
            return context;
        }
    }

    public class MyContextGroup
    {
        public MyContextGroup()
        {
            Items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string GroupName { get; set; }
        public IDictionary<string, string> Items { get; set; }
    }
    
    public static class MyRequestContextExtensions
    {
        public static string MyRequestContextCacheKey = "MyCache." + typeof(MyRequestContext).FullName;

        public static MyRequestContext GetMyRequestContext(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Items[MyRequestContextCacheKey] != null)
            {
                return (MyRequestContext)context.Items[MyRequestContextCacheKey];
            }

            var requestContext = MyRequestContext.Create()
                .SetRouteInfo(context.GetRouteData())
                .SetQueryInfo(context.Request)
                .SetUserInfo(context.Request);
            context.Items[MyRequestContextCacheKey] = requestContext;

            return requestContext;
        }

        internal static void SetProperties(this IDictionary<string, string> items, object instance)
        {
            if (instance == null)
            {
                return;
            }

            if (items == null)
            {
                return;

            }

            var theType = instance.GetType();
            var propertyInfos = theType.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                foreach (var key in items.Keys)
                {
                    if (propertyInfo.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        if (items.TryGetValue(key, out var itemValue))
                        {
                            propertyInfo.SetValue(instance, itemValue, null);
                            break;
                        }
                    }
                }
            }
        }

        internal static MyContextGroup GetOrCreate(this MyRequestContext context, string group, bool autoCreate = true)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }

            var theGroup = context.Groups.SingleOrDefault(x => x.GroupName == group);
            if (theGroup == null)
            {
                if (autoCreate)
                {
                    theGroup = new MyContextGroup { GroupName = @group };
                    context.Groups.Add(theGroup);
                }
            }
            return theGroup;
        }
    }
}
