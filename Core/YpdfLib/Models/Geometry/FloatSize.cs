namespace YpdfLib.Models.Geometry
{
    public readonly struct FloatSize : IFloatSize, IEquatable<FloatSize>
    {
        public float Width { get; }
        public float Height { get; }

        public float Area => Width * Height;

        public FloatSize() : this(0, 0)
        {
        }

        public FloatSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(FloatSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object? obj)
        {
            return obj is FloatSize point && Equals(point);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public static bool operator ==(FloatSize left, FloatSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FloatSize left, FloatSize right)
        {
            return !(left == right);
        }

        public static bool operator <(FloatSize left, FloatSize right)
        {
            return left.Area < right.Area;
        }

        public static bool operator >(FloatSize left, FloatSize right)
        {
            return left.Area > right.Area;
        }

        public static bool operator <=(FloatSize left, FloatSize right)
        {
            return left < right || left == right;
        }

        public static bool operator >=(FloatSize left, FloatSize right)
        {
            return left > right || left == right;
        }

        public static FloatSize FromDoubleCoordinates(double width, double height)
        {
            return new FloatSize(Convert.ToSingle(width), Convert.ToSingle(height));
        }
    }
}
