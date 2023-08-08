namespace YpdfLib.Models.Geometry
{
    public readonly struct FloatRectangle : IFloatRectangle, IEquatable<FloatRectangle>
    {
        public FloatPoint TopLeft { get; }
        public FloatPoint TopRight { get; }
        public FloatPoint BottomRight { get; }
        public FloatPoint BottomLeft { get; }
        public CoordinatesMirroring CoordinatesMirroring { get; }

        public FloatPoint Center => new(
            FloatMath.Abs(TopRight.X - TopLeft.X),
            FloatMath.Abs(TopLeft.Y - BottomLeft.Y)
        );

        public float Left => GetAllX().Min();
        public float Top => GetAllY().Max();
        public float Right => GetAllX().Max();
        public float Bottom => GetAllY().Min();

        public float Width => Distance.Calc(TopLeft, TopRight);
        public float Height => Distance.Calc(TopLeft, BottomLeft);
        public FloatSize Size => new(Width, Height);

        public FloatRectangle(FloatPoint topLeft, FloatPoint bottomRight)
        {
            if (topLeft == bottomRight)
                throw new ArgumentException("Top left point is equal to bottom right point.", nameof(topLeft));
            
            TopLeft = topLeft;
            BottomRight = bottomRight;
            CoordinatesMirroring = GetCoordinatesMirroring(topLeft, bottomRight);

            float width = FloatMath.Abs(topLeft.X - bottomRight.X);

            BottomLeft = CoordinatesMirroring == CoordinatesMirroring.MirrorX || CoordinatesMirroring == CoordinatesMirroring.MirrorXY
                ? new (BottomRight.X + width, BottomRight.Y)
                : new (BottomRight.X - width, BottomRight.Y);

            TopRight = CoordinatesMirroring == CoordinatesMirroring.MirrorX || CoordinatesMirroring == CoordinatesMirroring.MirrorXY
                ? new (TopLeft.X - width, TopLeft.Y)
                : new (TopLeft.X + width, TopLeft.Y);
        }

        public FloatRectangle(FloatPoint topLeft, FloatPoint topRight, FloatPoint bottomRight, FloatPoint bottomLeft)
        {
            FloatPoint[] points = new FloatPoint[]
            {
                topLeft, topRight, bottomRight, bottomLeft
            };

            if (points.Length != points.Distinct().Count())
                throw new NotRectangleException(points);

            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
            CoordinatesMirroring = GetCoordinatesMirroring(topLeft, bottomRight);
        }
        
        public bool Equals(FloatRectangle other)
        {
            return TopLeft == other.TopLeft
                && BottomRight == other.BottomRight
                && CoordinatesMirroring == other.CoordinatesMirroring;
        }

        public override bool Equals(object? obj)
        {
            return obj is FloatRectangle rect && Equals(rect);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TopLeft, BottomRight, CoordinatesMirroring);
        }

        public static bool operator ==(FloatRectangle left, FloatRectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FloatRectangle left, FloatRectangle right)
        {
            return !(left == right);
        }

        private float[] GetAllX()
        {
            return new float[] { TopLeft.X, TopRight.X, BottomRight.X, BottomLeft.X };
        }
        private float[] GetAllY()
        {
            return new float[] { TopLeft.Y, TopRight.Y, BottomRight.Y, BottomLeft.Y };
        }

        private static CoordinatesMirroring GetCoordinatesMirroring(FloatPoint topLeft, FloatPoint bottomRight)
        {
            bool xMirrored = topLeft.X > bottomRight.X;
            bool yMirrored = topLeft.Y < bottomRight.Y;

            return !xMirrored && !yMirrored
                ? CoordinatesMirroring.Normal
                : xMirrored && yMirrored
                ? CoordinatesMirroring.MirrorXY
                : xMirrored
                ? CoordinatesMirroring.MirrorX
                : CoordinatesMirroring.MirrorY;
        }
    }
}
