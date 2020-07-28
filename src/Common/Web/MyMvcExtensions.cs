using System;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web
{
    public static class MyMvcExtensions
    {
        public static string AreaContent(this IUrlHelper urlHelper, string contentPath, string area)
        {
            var areaContentPath = AreaUrlHelper.Instance.CreateAreaContent(new AreaContentArgs(){Area = area, Content = contentPath});
            return urlHelper.Content(areaContentPath);
        }
    }

    #region for extensions


    public class AreaContentArgs
    {
        public string Area { get; set; }
        public string Content { get; set; }

        public AreaContentArgs WithArea(string area)
        {
            Area = area;
            return this;
        }

        public AreaContentArgs WithContent(string content)
        {
            Content = content;
            return this;
        }

        public static AreaContentArgs Create()
        {
            return new AreaContentArgs();
        }
    }

    public class AreaUrlHelper
    {
        public Func<AreaContentArgs, string> CreateAreaContent = createAreaContent;

        private static string createAreaContent(AreaContentArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (string.IsNullOrWhiteSpace(args.Area))
            {
                throw new ArgumentNullException(nameof(args.Area));
            }

            if (string.IsNullOrWhiteSpace(args.Content))
            {
                throw new ArgumentNullException(nameof(args.Content));
            }

            //~/Content/css/foo.css" => ~/Areas/Demo/Content/css/foo.css"
            var areaContentPath = "~/Areas/" + args.Area + "/" + args.Content.TrimStart('~').TrimStart('/');
            return areaContentPath;
        }

        public static AreaUrlHelper Instance = new AreaUrlHelper();
    }

    #endregion
}
