using System.Collections.ObjectModel;
using YpdfDesktop.Models.Localization;

namespace YpdfDesktop.Infrastructure.Default
{
    internal static class DefaultLocales
    {
        public static Locale English => new English();

        public static ObservableCollection<Locale> Get()
        {
            return new ObservableCollection<Locale>()
            {
                English
            };
        }
    }
}
