using System.Collections.ObjectModel;
using YpdfDesktop.Models.Localization;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class LocalesService
    {
        public static bool TryLoadLocales(out ObservableCollection<Locale> locales)
        {
            return JsonPackService.TryLoadPacks(SharedConfig.Directories.LOCALES, out locales);
        }
    }
}
