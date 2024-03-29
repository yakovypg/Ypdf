﻿using System.Collections.Generic;

namespace YpdfDesktop.Models.Configuration
{
    public interface IUIConfig
    {
        string? SelectedLocaleId { get; set; }
        string? SelectedThemeId { get; set; }
        bool? ResetAfterExecution { get; set; }
        List<ToolType>? FavoriteTools { get; set; }
    }
}