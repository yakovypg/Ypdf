namespace YpdfLib.Models.Paging
{
    public class PageMovementInfo : IPageMovementInfo, IDeepCloneable<PageMovementInfo>, IEquatable<PageMovementInfo?>
    {
        public int? PageToMove { get; set; }
        public int? NewPosition { get; set; }

        public PageMovementInfo(int? pageToMove = null, int? newPosition = null)
        {
            PageToMove = pageToMove;
            NewPosition = newPosition;
        }

        public int GetPageToMoveOrDefault()
        {
            return PageToMove ?? default;
        }

        public int GetNewPositionOrDefault()
        {
            return NewPosition ?? default;
        }

        public PageMovementInfo Copy()
        {
            return new PageMovementInfo(PageToMove, NewPosition);
        }

        IPageMovementInfo IDeepCloneable<IPageMovementInfo>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageMovementInfo? other)
        {
            return other is not null &&
                   PageToMove == other.PageToMove &&
                   NewPosition == other.NewPosition;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageMovementInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageToMove, NewPosition);
        }
    }
}
