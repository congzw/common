using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Common.Web.MyContexts
{
    public class MyQueryInfo
    {
        public string Site { get; set; }
        public string User { get; set; }
    }

    public static class MyQueryInfoExtensions
    {
        private static string Group_QueryInfos = "QueryInfos";
        public static string GetGroupName_QueryInfos(this MyRequestContext context)
        {
            return Group_QueryInfos;
        }

        public static MyRequestContext SetQueryInfo(this MyRequestContext context, HttpRequest httpRequest)
        {
            var theGroup = context.GetOrCreate(context.GetGroupName_QueryInfos());
            var queryCollection = httpRequest.Query;
            var keys = queryCollection.Keys;
            foreach (var key in keys)
            {
                StringValues values;
                if (!queryCollection.TryGetValue(key, out values))
                {
                    theGroup.Items[key] = null;
                }
                else
                {
                    values = values.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    foreach (var value in values)
                    {
                        //set by last one
                        theGroup.Items[key] = value;
                    }
                }
            }
            return context;
        }

        public static MyQueryInfo GetQueryInfo(this MyRequestContext context)
        {
            var info = new MyQueryInfo();
            var theGroup = context.GetOrCreate(context.GetGroupName_QueryInfos());
            theGroup.Items.SetProperties(info);
            return info;
        }
    }
}