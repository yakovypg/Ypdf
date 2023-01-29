using Avalonia.Media;

namespace YpdfDesktop.Models.Themes
{
    public interface IWindowTheme
    {
        string Id { get; }
        string Name { get; set; }

        ISolidColorBrush? ExplorerBackground { get; set; }
        ISolidColorBrush? ExplorerSplitterBackground { get; set; }
        ISolidColorBrush? ExplorerTextForeground { get; set; }

        ISolidColorBrush? ButtonBackground { get; set; }
        ISolidColorBrush? ButtonForeground { get; set; }
        ISolidColorBrush? ButtonIconForeground { get; set; }

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
    }
}