using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace Common.Modules
{
    public class ModuleStartupHelper
    {
        public ModuleStartupHelper()
        {
            LoadContext = AssemblyLoadContext.Default;
            GetAssemblies = () => GetModuleAssemblies(AppDomain.CurrentDomain.BaseDirectory);
        }


        public AssemblyLoadContext LoadContext { get; set; }
        
        public Func<IList<Assembly>> GetAssemblies { get; set; }

        public IDictionary<string, string> AddApplicationPart(IMvcBuilder mvcBuilder)
        {
            var loadResult = new Dictionary<string, string>();
            var assemblies = GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    //for test loadResult
                    var asm = LoadContext.LoadFromAssemblyName(assembly.GetName());
                    mvcBuilder.AddApplicationPart(asm);
                    loadResult[assembly.FullName] = "OK";
                }
                catch (Exception ex)
                {
                    loadResult[assembly.FullName] = "KO: " + ex.Message;
                }
            }

            return loadResult;
        }

        public static ModuleStartupHelper Instance = new ModuleStartupHelper();
        private IList<Assembly> GetModuleAssemblies(string root, string modulePrefix = null)
        {
            if (string.IsNullOrWhiteSpace(modulePrefix))
            {
                modulePrefix = TryGetPrefix();
            }

            if (string.IsNullOrWhiteSpace(modulePrefix))
            {
                throw new ArgumentNullException(nameof(modulePrefix));
            }

            var allLib = DependencyContext.Default.CompileLibraries;
            var libs = allLib.Where(x => x.Name.StartsWith(modulePrefix, StringComparison.OrdinalIgnoreCase));
            var assemblies = libs.Select(lib =>
                {
                    try
                    {
                        return LoadContext.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            ).Where(x => x != null).ToList();

            var moduleFiles = Directory.GetFiles(root, modulePrefix + "*.dll");
            foreach (var moduleFile in moduleFiles)
            {
                var fileInfo = new FileInfo(moduleFile);
                var theOne = assemblies.SingleOrDefault(x => fileInfo.Name.Equals(x.GetName().Name + ".dll", StringComparison.OrdinalIgnoreCase));
                if (theOne == null)
                {
                    var assembly = LoadContext.LoadFromAssemblyPath(fileInfo.FullName);
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }
        private static string TryGetPrefix()
        {
            var ns = typeof(DefaultModuleServiceContext).Namespace;
            if (ns != null)
            {
                var modulePrefix = ns.Split(".").FirstOrDefault();
                return modulePrefix;
            }
            //todo: read from config?
            return "";
        }
    }
}