namespace YpdfLib.Models.Paging
{
    public class PageIncrease : IPageIncrease, IDeepCloneable<PageIncrease>, IEquatable<PageIncrease?>
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        public PageIncrease(float left = 0, float top = 0, float right = 0, float bottom = 0)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public PageIncrease Copy()
        {
            return new PageIncrease(Left, Top, Right, Bottom);
        }

        IPageIncrease IDeepCloneable<IPageIncrease>.Copy()
        {
            return Copy();
        }

        public static PageIncrease Parse(string data)
        {
            string[] parts = data.Split(',');

            if (parts.Length != 4)
                throw new ArgumentException("Incorrect format. Format: L,T,R,B", nameof(data));

            float left = float.Parse(parts[0]);
            float top = float.Parse(parts[1]);
            float right = float.Parse(parts[2]);
            float bottom = float.Parse(parts[3]);

            return new PageIncrease(left, top, right, bottom);
        }

        public bool Equals(PageIncrease? other)
        {
            return other is not null &&
                   Left == other.Left &&
                   Top == other.Top &&
                   Right == other.Right &&
                   Bottom == other.Bottom;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageIncrease);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Top, Right, Bottom);
        }
    }
}
