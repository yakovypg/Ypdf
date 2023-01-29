using Avalonia.Media;

namespace YpdfDesktop.Models.Themes
{
    public class LightWindowTheme : WindowTheme
    {
        public LightWindowTheme() : base("light_unix1674225488", "Light")
        {
        }

        protected override void SetBrushes()
        {
            ExplorerBackground = SolidColorBrush.Parse("#F0F8FF");
            ExplorerSplitterBackground = SolidColorBrush.Parse("#D3D3D3");
            ExplorerTextForeground = SolidColorBrush.Parse("#000000");

            ButtonBackground = SolidColorBrush.Parse("#D3D3D3");
            ButtonForeground = SolidColorBrush.Parse("#000000");
            ButtonIconForeground = SolidColorBrush.Parse("#800000");

            LinkIconForeground = PanelSelectorIconForeground = SolidColorBrush.Parse("#292177");
            FavoriteStarIconForeground = SolidColorBrush.Parse("#FF8C00");
            NotFavoriteStarIconForeground = SolidColorBrush.Parse("#808080");

            PanelSelectorBackground = SolidColorBrush.Parse("#EE9BA2E4");
            PanelSelectorIconForeground = SolidColorBrush.Parse("#292177");
            PanelSelectorTextForeground = SolidColorBrush.Parse("#000000");
            PanelSelectorTileDefaultBackground = SolidColorBrush.Parse("#EE9BA2E4");
            PanelSelectorTileEnteredBackground = SolidColorBrush.Parse("#EEB3ADF0");
            PanelSelectorTilePressedBackground = SolidColorBrush.Parse("#EE877EE7");

            ToolTileBorderBrush = SolidColorBrush.Parse("#C6C6C6");
            ToolTileIconForeground = SolidColorBrush.Parse("#EE17254B");
            ToolTileTextForeground = SolidColorBrush.Parse("#000000");
            ToolTileDefaultBackground = SolidColorBrush.Parse("#EECFE0DF");
            ToolTileEnteredBackground = SolidColorBrush.Parse("#EEB3ADF0");
            ToolTilePressedBackground = SolidColorBrush.Parse("#EE877EE7");
        }
    }
}
