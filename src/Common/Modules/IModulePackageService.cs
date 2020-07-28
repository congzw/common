using System.Collections.Generic;

namespace Common.Modules
{
    public interface IModulePackageService
    {
        void Install(ModulePackage package);
        void Uninstall(ModulePackage package);
        IList<ModulePackage> GetModulePackages();
        void ExportModulePackage(ModulePackage package);
    }

    public class ModulePackage
    {
        public string FilePath { get; set; }
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }
        public string Version { get; set; }
    }
}
