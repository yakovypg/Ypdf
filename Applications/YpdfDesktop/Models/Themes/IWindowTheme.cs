using Avalonia.Media;

namespace YpdfDesktop.Models.Themes
{
    public interface IWindowTheme
    {
        string Id { get; }
        string Name { get; set; }

        ISolidColorBrush? ExplorerBackground { get; set; }
        ISolidColorBrush? ExplorerEmptyPlaceBackground { get; set; }
        ISolidColorBrush? ExplorerTextForeground { get; set; }
        ISolidColorBrush? ExplorerSplitterBackground { get; set; }
        ISolidColorBrush? ExplorerMenuBackground { get; set; }
        ISolidColorBrush? ExplorerMenuTextForeground { get; set; }
        ISolidColorBrush? ExplorerMenuIconForeground { get; set; }
        ISolidColorBrush? ExplorerMenuTileBackground { get; set; }
        ISolidColorBrush? ExplorerMenuTileBackgroundPointerOver { get; set; }
        ISolidColorBrush? ExplorerMenuTileBackgroundPressed { get; set; }

        ISolidColorBrush? ButtonBackground { get; set; }
        ISolidColorBrush? ButtonBackgroundPointerOver { get; set; }
        ISolidColorBrush? ButtonBackgroundPressed { get; set; }
        ISolidColorBrush? ButtonBackgroundDisabled { get; set; }
        ISolidColorBrush? ButtonForeground { get; set; }
        ISolidColorBrush? ButtonForegroundPointerOver { get; set; }
        ISolidColorBrush? ButtonForegroundPressed { get; set; }
        ISolidColorBrush? ButtonForegroundDisabled { get; set; }
        ISolidColorBrush? ButtonIconForeground { get; set; }

        ISolidColorBrush? CheckBoxForeground { get; set; }
        ISolidColorBrush? CheckBoxForegroundPointerOver { get; set; }
        ISolidColorBrush? CheckBoxForegroundPressed { get; set; }

        ISolidColorBrush? LinkIconForeground { get; set; }
        ISolidColorBrush? FavoriteStarIconForeground { get; set; }
        ISolidColorBrush? NotFavoriteStarIconForeground { get; set; }

        ISolidColorBrush? PanelSelectorBackground { get; set; }
        ISolidColorBrush? PanelSelectorIconForeground { get; set; }
        ISolidColorBrush? PanelSelectorTextForeground { get; set; }
        ISolidColorBrush? PanelSelectorTileBackground { get; set; }
        ISolidColorBrush? PanelSelectorTileBackgroundPointerOver { get; set; }
        ISolidColorBrush? PanelSelectorTileBackgroundPressed { get; set; }

        ISolidColorBrush? ToolTileBorderBrush { get; set; }
        ISolidColorBrush? ToolTileIconForeground { get; set; }
        ISolidColorBrush? ToolTileTextForeground { get; set; }
        ISolidColorBrush? ToolTileBackground { get; set; }
        ISolidColorBrush? ToolTileBackgroundPointerOver { get; set; }
        ISolidColorBrush? ToolTileBackgroundPressed { get; set; }

        ISolidColorBrush? InputFilesTileBackground { get; set; }
        ISolidColorBrush? InputFilesTileTextForeground { get; set; }
        ISolidColorBrush? RunningStatusBrush { get; set; }
        ISolidColorBrush? CompletedStatusBrush { get; set; }
        ISolidColorBrush? FaultedStatusBrush { get; set; }

        ISolidColorBrush? ContrastBorderBrush { get; set; }
    }
}