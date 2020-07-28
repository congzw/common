using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Common.Web.MyContexts
{
    public class MyContext
    {
        public MyContext()
        {
            Groups = new List<MyContextGroup>();
        }
        
        public IList<MyContextGroup> Groups { get; set; }

        public static MyContext GetCurrent(HttpContext httpContext)
        {
            return MyContextService.Resolve().GetMyContext(httpContext);
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
    
    public static class MyContextExtensions
    {
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

        internal static MyContextGroup GetOrCreate(this MyContext context, string group, bool autoCreate = true)
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
