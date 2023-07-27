using Avalonia.Media;
using System;
using System.Collections.Generic;
using YpdfDesktop.Models.Base;

namespace YpdfDesktop.Models
{
    public class Tool : ReactiveModel, ITool, IEquatable<Tool?>
    {
        public string IconName { get; }
        public ToolType Type { get; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => RaiseAndSetIfChanged(ref _name, value);
        }

        private bool _isFavorite = false;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (!RaiseAndSetIfChanged(ref _isFavorite, value))
                    return;

                StarBrush = value
                    ? FavoriteStarBrush
                    : NotFavoriteStarBrush;
            }
        }

        private ISolidColorBrush _starBrush = Brushes.Gray;
        public ISolidColorBrush StarBrush
        {
            get => _starBrush;
            private set => RaiseAndSetIfChanged(ref _starBrush, value);
        }

        private ISolidColorBrush _favoriteStarBrush = Brushes.DarkOrange;
        public ISolidColorBrush FavoriteStarBrush
        {
            get => _favoriteStarBrush;
            set
            {
                if (!RaiseAndSetIfChanged(ref _favoriteStarBrush, value))
                    return;

                if (IsFavorite)
                    StarBrush = value;
            }
        }

        private ISolidColorBrush _notFavoriteStarBrush = Brushes.Gray;
        public ISolidColorBrush NotFavoriteStarBrush
        {
            get => _notFavoriteStarBrush;
            set
            {
                if (!RaiseAndSetIfChanged(ref _notFavoriteStarBrush, value))
                    return;

                if (!IsFavorite)
                    StarBrush = value;
            }
        }

        public Tool(string? name, string? iconName, ToolType type, bool isFavorite = false)
            : this(name, iconName, type, Brushes.DarkOrange, Brushes.Gray, isFavorite)
        {
        }

        public Tool(string? name, string? iconName, ToolType type, ISolidColorBrush favoriteStarBrush,
            ISolidColorBrush notFavoriteStarBrush, bool isFavorite = false)
        {
            Name = name ?? string.Empty;
            IconName = iconName ?? string.Empty;
            Type = type;
            FavoriteStarBrush = favoriteStarBrush;
            NotFavoriteStarBrush = notFavoriteStarBrush;
            IsFavorite = isFavorite;
        }

        public bool Equals(Tool? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   IconName == other.IconName &&
                   Type == other.Type &&
                   IsFavorite == other.IsFavorite &&
                   EqualityComparer<ISolidColorBrush>.Default.Equals(StarBrush, other.StarBrush) &&
                   EqualityComparer<ISolidColorBrush>.Default.Equals(FavoriteStarBrush, other.FavoriteStarBrush) &&
                   EqualityComparer<ISolidColorBrush>.Default.Equals(NotFavoriteStarBrush, other.NotFavoriteStarBrush);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Tool);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IconName, Type, IsFavorite, StarBrush,
                FavoriteStarBrush, NotFavoriteStarBrush);
        }

        public static bool operator ==(Tool? left, Tool? right)
        {
            return EqualityComparer<Tool>.Default.Equals(left, right);
        }

        public static bool operator !=(Tool? left, Tool? right)
        {
            return !(left == right);
        }
    }
}
