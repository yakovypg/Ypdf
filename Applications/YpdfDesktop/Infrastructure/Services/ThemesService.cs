using System.Collections.ObjectModel;
using YpdfDesktop.Models.Themes;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class ThemesService
    {
        public static bool TryLoadThemes(out ObservableCollection<WindowTheme> themes)
        {
            return JsonPackService.TryLoadPacks(SharedConfig.Directories.Themes, out themes);
        }
    }
}
