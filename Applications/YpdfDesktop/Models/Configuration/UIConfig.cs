using System.Collections.Generic;

namespace YpdfDesktop.Models.Configuration
{
    public class UIConfig
    {
        public string? SelectedLocaleId { get; set; }
        public string? SelectedThemeId { get; set; }
        public List<ToolType>? FavoriteTools { get; set; }

        public UIConfig()
        {
        }
    }
}
