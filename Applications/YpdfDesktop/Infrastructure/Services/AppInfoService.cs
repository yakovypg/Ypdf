using System.Reflection;
using YpdfDesktop.Models.Informing;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class AppInfoService
    {
        public static AppInfo GetAppInfo()
        {
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            string? name = assemblyName.Name?.ToString();
            string? version = assemblyName.Version?.ToString();

            return new AppInfo(name, version);
        }
    }
}
