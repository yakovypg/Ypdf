namespace YpdfLib.Models.Geometry
{
    public readonly struct FloatPoint : IFloatPoint, IEquatable<FloatPoint>
    {
        public float X { get; }
        public float Y { get; }

        public FloatPoint() : this(0, 0)
        {
        }

        public FloatPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(FloatPoint other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is FloatPoint point && Equals(point);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(FloatPoint left, FloatPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FloatPoint left, FloatPoint right)
        {
            return !(left == right);
        }

        public static FloatPoint Parse(string data)
        {
            data = data.Replace(" ", null);

            if (data.StartsWith('('))
                data = data[1..];

            if (data.EndsWith(')'))
                data = data.Remove(data.Length - 1);

            string[] parts = data.Split(';');

            if (parts.Length != 2)
                throw new ArgumentException("Incorrect format. Format: (x;y) or x;y.", nameof(data));

            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);

            return new FloatPoint(x, y);
        }

        public static FloatPoint FromDoubleCoordinates(double x, double y)
        {
            return new FloatPoint(Convert.ToSingle(x), Convert.ToSingle(y));
        }
    }
}
