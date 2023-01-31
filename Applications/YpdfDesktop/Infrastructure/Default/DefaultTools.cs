using Avalonia.Media;
using System.Collections.ObjectModel;
using YpdfDesktop.Infrastructure.Services;
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
                new Tool(locale.Split, ToolInfoService.GetIconName(ToolType.Split), ToolType.Split, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.Merge, ToolInfoService.GetIconName(ToolType.Merge), ToolType.Merge, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.Compress, ToolInfoService.GetIconName(ToolType.Compress), ToolType.Compress, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.HandlePages, ToolInfoService.GetIconName(ToolType.HandlePages), ToolType.HandlePages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.CropPages, ToolInfoService.GetIconName(ToolType.CropPages), ToolType.CropPages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.DividePages, ToolInfoService.GetIconName(ToolType.DividePages), ToolType.DividePages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.AddPageNumbers, ToolInfoService.GetIconName(ToolType.AddPageNumbers), ToolType.AddPageNumbers, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.AddWatermark, ToolInfoService.GetIconName(ToolType.AddWatermark), ToolType.AddWatermark, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.RemoveWatermark, ToolInfoService.GetIconName(ToolType.RemoveWatermark), ToolType.RemoveWatermark, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.ImageToPdf, ToolInfoService.GetIconName(ToolType.ImageToPdf), ToolType.ImageToPdf, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.TextToPdf, ToolInfoService.GetIconName(ToolType.TextToPdf), ToolType.TextToPdf, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.ExtractImages, ToolInfoService.GetIconName(ToolType.ExtractImages), ToolType.ExtractImages, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.ExtractText, ToolInfoService.GetIconName(ToolType.ExtractText), ToolType.ExtractText, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.SetPassword, ToolInfoService.GetIconName(ToolType.SetPassword), ToolType.SetPassword, favoriteStarBrush, notFavoriteStarBrush),
                new Tool(locale.RemovePassword, ToolInfoService.GetIconName(ToolType.RemovePassword), ToolType.RemovePassword, favoriteStarBrush, notFavoriteStarBrush)
            };
        }
    }
}
