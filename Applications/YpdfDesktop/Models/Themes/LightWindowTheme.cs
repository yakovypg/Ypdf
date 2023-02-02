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
            ExplorerTextForeground = SolidColorBrush.Parse("#000000");
            ExplorerSplitterBackground = SolidColorBrush.Parse("#D3D3D3");
            ExplorerMenuBackground = SolidColorBrush.Parse("#D3D3D3");
            ExplorerMenuTextForeground = SolidColorBrush.Parse("#000000");
            ExplorerMenuIconForeground = SolidColorBrush.Parse("#A52A2A");
            ExplorerMenuTileBackground = SolidColorBrush.Parse("#D3D3D3");
            ExplorerMenuTileBackgroundPointerOver = SolidColorBrush.Parse("#AEAEAE");
            ExplorerMenuTileBackgroundPressed = SolidColorBrush.Parse("#898989");

            ButtonBackground = SolidColorBrush.Parse("#CCCCCC");
            ButtonBackgroundPointerOver = SolidColorBrush.Parse("#E6E6E6");
            ButtonBackgroundPressed = SolidColorBrush.Parse("#999999");
            ButtonBackgroundDisabled = SolidColorBrush.Parse("#CCCCCC");
            ButtonForeground = SolidColorBrush.Parse("#000000");
            ButtonForegroundPointerOver = SolidColorBrush.Parse("#000000");
            ButtonForegroundPressed = SolidColorBrush.Parse("#000000");
            ButtonForegroundDisabled = SolidColorBrush.Parse("#66000000");
            ButtonIconForeground = SolidColorBrush.Parse("#800000");

            CheckBoxForeground = SolidColorBrush.Parse("#000000");
            CheckBoxForegroundPointerOver = SolidColorBrush.Parse("#000000");
            CheckBoxForegroundPressed = SolidColorBrush.Parse("#000000");

            LinkIconForeground = SolidColorBrush.Parse("#292177");
            FavoriteStarIconForeground = SolidColorBrush.Parse("#FF8C00");
            NotFavoriteStarIconForeground = SolidColorBrush.Parse("#808080");

            PanelSelectorBackground = SolidColorBrush.Parse("#EE9BA2E4");
            PanelSelectorIconForeground = SolidColorBrush.Parse("#292177");
            PanelSelectorTextForeground = SolidColorBrush.Parse("#000000");
            PanelSelectorTileBackground = SolidColorBrush.Parse("#EE9BA2E4");
            PanelSelectorTileBackgroundPointerOver = SolidColorBrush.Parse("#EEB3ADF0");
            PanelSelectorTileBackgroundPressed = SolidColorBrush.Parse("#EE877EE7");

            ToolTileBorderBrush = SolidColorBrush.Parse("#C6C6C6");
            ToolTileIconForeground = SolidColorBrush.Parse("#EE17254B");
            ToolTileTextForeground = SolidColorBrush.Parse("#000000");
            ToolTileBackground = SolidColorBrush.Parse("#EECFE0DF");
            ToolTileBackgroundPointerOver = SolidColorBrush.Parse("#EEB3ADF0");
            ToolTileBackgroundPressed = SolidColorBrush.Parse("#EE877EE7");

            InputFilesTileBackground = SolidColorBrush.Parse("#F2F2F2");
            InputFilesTileTextForeground = SolidColorBrush.Parse("#000000");
            RunningStatusBrush = SolidColorBrush.Parse("#EECFCFE0");
            CompletedStatusBrush = SolidColorBrush.Parse("#EECFE0DF");
            FaultedStatusBrush = SolidColorBrush.Parse("#EEE0CFCF");
        }
    }
}
