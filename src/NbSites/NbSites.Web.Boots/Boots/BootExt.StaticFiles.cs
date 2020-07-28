using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace NbSites.Web.Boots
{
    public static partial class BootExt
    {
        public static void UseMyStaticFiles(this IApplicationBuilder app, IHostingEnvironment hostingEnvironment, ILogger logger)
        {
            ////map request "~/" to "index.html"
            //app.UseDefaultFiles(new DefaultFilesOptions()
            //{
            //    DefaultFileNames = new List<string>()
            //    {
            //        "index.html"
            //    }
            //});

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = { [".vue"] = "text/html" }
                }
            });


            var rootPath = hostingEnvironment.ContentRootPath;
            var publicFiles = Directory.GetFiles(rootPath, "public_this_folder.txt", SearchOption.AllDirectories);
            foreach (var publicFile in publicFiles)
            {
                //=> "~/Areas/Common/whatever/scripts/test.js"
                //webPath => D:\any_folder\src\MyApp.Web
                //publicFolder => D:\any_folder\src\MyApp.Web\Areas\Common\whatever\scripts
                //requestPath => \Areas\Common\whatever\scripts

                var publicFolder = Path.GetDirectoryName(publicFile);
                var requestPath = publicFolder.Replace(rootPath, string.Empty, StringComparison.OrdinalIgnoreCase);
                requestPath = requestPath.Replace('\\', '/').TrimEnd('/');
                logger.LogDebug(string.Format("{0} => {1}", publicFolder, requestPath));

                var physicalFileProvider = new PhysicalFileProvider(publicFolder);
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = physicalFileProvider,
                    RequestPath = requestPath
                });

                ////make directory browser ok:
                //app.UseDirectoryBrowser(new DirectoryBrowserOptions
                //{
                //    FileProvider = physicalFileProvider,
                //    RequestPath = requestPath
                //});
            }

            ////make both rq ok:
            ////rq => MyImages/oops.png
            ////rq => images/oops.png
            //var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"));
            //var imageRequestPath = "/MyImages";
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = fileProvider,
            //    RequestPath = imageRequestPath
            //});

            ////make directory browser ok:
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = fileProvider,
            //    RequestPath = imageRequestPath
            //});
        }
    }
}
