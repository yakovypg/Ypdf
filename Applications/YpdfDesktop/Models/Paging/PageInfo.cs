using System;
using YpdfDesktop.Models.Base;

namespace YpdfDesktop.Models.Paging
{
    public class PageInfo : ReactiveModel
    {
        public int PageNumber { get; }

        private int _position;
        public int Position
        {
            get => _position;
            set => RaiseAndSetIfChanged(ref _position, value);
        }

        private int _rotationAngle;
        public int RotationAngle
        {
            get => _rotationAngle;
            set
            {
                value -= value % 90;
                value %= 360;

                if (value < 0)
                    value += 360;

                RaiseAndSetIfChanged(ref _rotationAngle, value);
            }
        }

        public PageInfo(int pageNumber, int position, int rotationAngle = 0)
        {
            PageNumber = pageNumber;
            Position = position;
            RotationAngle = rotationAngle;
        }

        public void TurnRight90()
        {
            RotationAngle -= 90;
        }

        public void TurnLeft90()
        {
            RotationAngle += 90;
        }

        public override bool Equals(object? obj)
        {
            return obj is PageInfo other
                && PageNumber == other.PageNumber
                && Position == other.Position
                && RotationAngle == other.RotationAngle;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageNumber, Position, RotationAngle);
        }

        public static bool operator ==(PageInfo? left, PageInfo? right)
        {
            return left is null && right is null
                || (left?.Equals(right) ?? false);
        }

        public static bool operator !=(PageInfo? left, PageInfo? right)
        {
            return !(left == right);
        }
    }
}
