﻿using Avalonia.Media;

namespace YpdfDesktop.Models
{
    public interface ITool
    {
        string Name { get; }
        string IconName { get; }
        bool IsFavorite { get; set; }

        ISolidColorBrush StarBrush { get; }
        ISolidColorBrush FavoriteStarBrush { get; set; }
        ISolidColorBrush NotFavoriteStarBrush { get; set; }
    }
}