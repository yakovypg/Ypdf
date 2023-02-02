using Avalonia.Media;

namespace YpdfDesktop.Models.Themes
{
    public interface IWindowTheme
    {
        string Id { get; }
        string Name { get; set; }

        ISolidColorBrush? ExplorerBackground { get; set; }
        ISolidColorBrush? ExplorerTextForeground { get; set; }
        ISolidColorBrush? ExplorerSplitterBackground { get; set; }
        ISolidColorBrush? ExplorerMenuBackground { get; set; }
        ISolidColorBrush? ExplorerMenuTextForeground { get; set; }
        ISolidColorBrush? ExplorerMenuIconForeground { get; set; }
        ISolidColorBrush? ExplorerMenuTileDefaultBackground { get; set; }
        ISolidColorBrush? ExplorerMenuTileEnteredBackground { get; set; }
        ISolidColorBrush? ExplorerMenuTilePressedBackground { get; set; }

        ISolidColorBrush? ButtonBackground { get; set; }
        ISolidColorBrush? ButtonBackgroundPointerOver { get; set; }
        ISolidColorBrush? ButtonBackgroundPressed { get; set; }
        ISolidColorBrush? ButtonBackgroundDisabled { get; set; }
        ISolidColorBrush? ButtonForeground { get; set; }
        ISolidColorBrush? ButtonForegroundPointerOver { get; set; }
        ISolidColorBrush? ButtonForegroundPressed { get; set; }
        ISolidColorBrush? ButtonForegroundDisabled { get; set; }
        ISolidColorBrush? ButtonIconForeground { get; set; }

        ISolidColorBrush? CheckBoxDefaultForeground { get; set; }
        ISolidColorBrush? CheckBoxEnteredForeground { get; set; }
        ISolidColorBrush? CheckBoxPressedForeground { get; set; }

        ISolidColorBrush? LinkIconForeground { get; set; }
        ISolidColorBrush? FavoriteStarIconForeground { get; set; }
        ISolidColorBrush? NotFavoriteStarIconForeground { get; set; }

        ISolidColorBrush? PanelSelectorBackground { get; set; }
        ISolidColorBrush? PanelSelectorIconForeground { get; set; }
        ISolidColorBrush? PanelSelectorTextForeground { get; set; }
        ISolidColorBrush? PanelSelectorTileDefaultBackground { get; set; }
        ISolidColorBrush? PanelSelectorTileEnteredBackground { get; set; }
        ISolidColorBrush? PanelSelectorTilePressedBackground { get; set; }

        ISolidColorBrush? ToolTileBorderBrush { get; set; }
        ISolidColorBrush? ToolTileIconForeground { get; set; }
        ISolidColorBrush? ToolTileTextForeground { get; set; }
        ISolidColorBrush? ToolTileDefaultBackground { get; set; }
        ISolidColorBrush? ToolTileEnteredBackground { get; set; }
        ISolidColorBrush? ToolTilePressedBackground { get; set; }

        ISolidColorBrush? InputFilesTileBackground { get; set; }
        ISolidColorBrush? RunningStatusBrush { get; set; }
        ISolidColorBrush? CompletedStatusBrush { get; set; }
        ISolidColorBrush? FaultedStatusBrush { get; set; }
    }
}