using Avalonia.Media;
using Newtonsoft.Json;
using System;
using YpdfDesktop.Infrastructure.Serialization;

namespace YpdfDesktop.Models.Themes
{
    public class WindowTheme : IWindowTheme
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerTextForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerSplitterBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerMenuBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerMenuTextForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerMenuIconForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerMenuTileDefaultBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerMenuTileEnteredBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ExplorerMenuTilePressedBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ButtonBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ButtonForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ButtonIconForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? LinkIconForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? FavoriteStarIconForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? NotFavoriteStarIconForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? PanelSelectorBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? PanelSelectorIconForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? PanelSelectorTextForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? PanelSelectorTileDefaultBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? PanelSelectorTileEnteredBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? PanelSelectorTilePressedBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ToolTileBorderBrush { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ToolTileIconForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ToolTileTextForeground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ToolTileDefaultBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ToolTileEnteredBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? ToolTilePressedBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? InputFilesTileBackground { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? RunningStatusBrush { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? CompletedStatusBrush { get; set; }

        [JsonConverter(typeof(ISolidColorBrushConverter))]
        public ISolidColorBrush? FaultedStatusBrush { get; set; }

        public WindowTheme() : this(Guid.NewGuid().ToString())
        {
        }

        public WindowTheme(string id) : this(id, string.Empty)
        {
        }

        public WindowTheme(string id, string name)
        {
            Id = id;
            Name = name;

            SetBrushes();
        }

        protected virtual void SetBrushes()
        {
        }
    }
}
