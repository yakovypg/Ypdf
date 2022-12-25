namespace YpdfLib.Models.Design
{
    public class Margin : IMargin, IDeepCloneable<Margin>, IEquatable<Margin?>
    {
        public float Left { get; }
        public float Top { get; }
        public float Right { get; }
        public float Bottom { get; }

        public float HorizontalSum => Left + Right;
        public float VerticalSum => Top + Bottom;

        public bool IsZero => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;

        public Margin() : this(0)
        {
        }

        public Margin(float value) : this(value, value, value, value)
        {
        }

        public Margin(float horizontal, float vertical) : this(horizontal, vertical, horizontal, vertical)
        {
        }

        public Margin(float left, float top, float right, float bottom)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public Margin Copy()
        {
            return new Margin(Left, Top, Right, Bottom);
        }

        IMargin IDeepCloneable<IMargin>.Copy()
        {
            return Copy();
        }

        public static Margin Parse(string data)
        {
            string[] parts = data.Split(',');

            switch (parts.Length)
            {
                case 1:
                    float value = float.Parse(parts[0]);
                    return new Margin(value);

                case 2:
                    float horizontal = float.Parse(parts[0]);
                    float vertical = float.Parse(parts[1]);
                    return new Margin(horizontal, vertical);

                case 4:
                    float left = float.Parse(parts[0]);
                    float top = float.Parse(parts[1]);
                    float right = float.Parse(parts[2]);
                    float bottom = float.Parse(parts[3]);
                    return new Margin(left, top, right, bottom);

                default:
                    throw new ArgumentException("Incorrect format. Format: M or H,V or L,T,R,B.", nameof(data));
            }
        }

        public bool Equals(Margin? other)
        {
            return other is not null &&
                   Top == other.Top &&
                   Right == other.Right &&
                   Bottom == other.Bottom &&
                   Left == other.Left;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Margin);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Top, Right, Bottom, Left, HorizontalSum, VerticalSum);
        }

        public static bool operator ==(Margin? left, Margin? right)
        {
            return EqualityComparer<Margin>.Default.Equals(left, right);
        }

        public static bool operator !=(Margin? left, Margin? right)
        {
            return !(left == right);
        }
    }
}
