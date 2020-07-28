using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Common.Web.MyContexts
{
    public class MyPageInfo
    {
        public string LayoutPath { get; set; }
        public string PagePath { get; set; }
    }

    public static class MyPageInfoExtensions
    {
        private static string Group_PageInfos = "PageInfos";
        public static string GetGroupName_PageInfos(this MyContext context)
        {
            return Group_PageInfos;
        }
        
        public static MyContext AppendPageInfos(this MyContext context, ViewContext viewContext)
        {
            var theGroup = context.GetOrCreate(context.GetGroupName_PageInfos());
            var myPageInfo = viewContext.CreateMyPageInfo();
            theGroup.Items[viewContext.View.Path] = myPageInfo.ToJson();
            return context;
        }
        
        public static IList<MyPageInfo> GetPageInfos(this MyContext context)
        {
            var myPageInfos = new List<MyPageInfo>();
            var theGroup = context.GetOrCreate(context.GetGroupName_PageInfos(), false);
            if (theGroup == null)
            {
                return myPageInfos;
            }

            var jsonList = theGroup.Items.Values.ToList();
            return jsonList.Select(x => x.FromJson<MyPageInfo>(null)).Where(x => x != null).ToList();
        }

        public static MyPageInfo CreateMyPageInfo(this ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            var myPageInfo = new MyPageInfo();
            var razorPage = ((RazorView)viewContext.View).RazorPage;
            myPageInfo.LayoutPath = razorPage.Layout;
            myPageInfo.PagePath = razorPage.Path;
            return myPageInfo;
        }
    }
}
