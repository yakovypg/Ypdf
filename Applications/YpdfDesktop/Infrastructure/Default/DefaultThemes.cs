using System.Collections.ObjectModel;
using YpdfDesktop.Models.Themes;

namespace YpdfDesktop.Infrastructure.Default
{
    internal static class DefaultThemes
    {
        public static WindowTheme Light => new LightWindowTheme();

        public static ObservableCollection<WindowTheme> Get()
        {
            return new ObservableCollection<WindowTheme>()
            {
                Light
            };
        }
    }
}
