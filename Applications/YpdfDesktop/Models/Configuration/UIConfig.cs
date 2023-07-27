using System.Collections.Generic;

namespace YpdfDesktop.Models.Configuration
{
    public class UIConfig : IUIConfig
    {
        public string? SelectedLocaleId { get; set; }
        public string? SelectedThemeId { get; set; }
        public bool? ResetAfterExecution { get; set; }
        public List<ToolType>? FavoriteTools { get; set; }

        public UIConfig()
        {
        }
    }
}
