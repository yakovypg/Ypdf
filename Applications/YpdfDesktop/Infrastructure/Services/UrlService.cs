using System.Diagnostics;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class UrlService
    {
        public static bool TryOpenUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            
            try
            {
                var startInfo = new ProcessStartInfo(url) { UseShellExecute = true };
                return Process.Start(startInfo) is not null;
            }
            catch
            {
                return false;
            }
        }
    }
}
