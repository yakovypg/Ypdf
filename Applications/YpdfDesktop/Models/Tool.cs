using Avalonia.Media;
using System;
using System.Collections.Generic;
using YpdfDesktop.Models.Base;

namespace YpdfDesktop.Models
{
    public class Tool : ReactiveModel, ITool, IEquatable<Tool?>
    {
        public string Name { get; }
        public string IconName { get; }

        private bool _isFavorite = false;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (!RaiseAndSetIfChanged(ref _isFavorite, value))
                    return;

                StarBrush = value
                    ? Brushes.DarkOrange
                    : Brushes.Gray;
            }
        }

        private ISolidColorBrush _starBrush = Brushes.Gray;
        public ISolidColorBrush StarBrush
        {
            get => _starBrush;
            private set => RaiseAndSetIfChanged(ref _starBrush, value);
        }

        public Tool(string name, string iconName, bool isFavorite = false)
        {
            Name = name;
            IconName = iconName;
            IsFavorite = isFavorite;
        }

        public bool Equals(Tool? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   IconName == other.IconName &&
                   _isFavorite == other._isFavorite &&
                   EqualityComparer<ISolidColorBrush>.Default.Equals(_starBrush, other._starBrush);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Tool);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IconName, _isFavorite, _starBrush);
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
