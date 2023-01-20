using Avalonia.Media;
using System.Collections.ObjectModel;
using YpdfDesktop.Models;
using YpdfDesktop.Models.Localization;
using YpdfDesktop.Models.Themes;

namespace YpdfDesktop.Infrastructure.Default
{
    internal static class DefaultTools
    {
        public static ObservableCollection<Tool> Get()
        {
            return Get(DefaultLocales.English, DefaultThemes.Light);
        }

        public static ObservableCollection<Tool> Get(ILocale locale, IWindowTheme theme)
        {
            ISolidColorBrush favoriteStarBrush = theme.FavoriteStarIconForeground ?? Brushes.Black;
            ISolidColorBrush notFavoriteStarBrush = theme.NotFavoriteStarIconForeground ?? Brushes.Black;

            return new ObservableCollection<Tool>()
            {
                new Tool(locale.Split, "fa-scissors", ToolType.Split, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.Merge, "fa-copy", ToolType.Merge, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.Compress, "fa-compress", ToolType.Compress, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.HandlePages, "fa-table", ToolType.HandlePages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.CropPages, "fa-crop", ToolType.CropPages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.DividePages, "fa-columns", ToolType.DividePages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.AddPageNumbers, "fa-sort-numeric-down", ToolType.AddPageNumbers, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.AddWatermark, "fa-paint-brush", ToolType.AddWatermark, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.RemoveWatermark, "fa-eraser", ToolType.RemoveWatermark, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.ImageToPdf, "fa-file-pdf", ToolType.ImageToPdf, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.TextToPdf, "fa-file-pdf", ToolType.TextToPdf, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.ExtractImages, "fa-file-image", ToolType.ExtractImages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.ExtractText, "fa-file-alt", ToolType.ExtractText, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.SetPassword, "fa-lock", ToolType.SetPassword, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.RemovePassword, "fa-unlock", ToolType.RemovePassword, favoriteStarBrush, notFavoriteStarBrush)
            };
        }
    }
}
