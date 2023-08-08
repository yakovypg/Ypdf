namespace YpdfLib.Models.Geometry
{
    public static class BoundedRectangle
    {
        public static FloatRectangle FromRectangle(FloatRectangle rect)
        {
            return FromPoints(rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft);
        }

        public static FloatRectangle FromPoints(params FloatPoint[] points)
        {
            ArgumentNullException.ThrowIfNull(points, nameof(points));

            if (points.Length < 3)
                throw new ArgumentException("The number of points must be greater than 2.", nameof(points));
            
            var allX = points.Select(t => t.X);
            var allY = points.Select(t => t.Y);

            float minX = allX.Min();
            float maxX = allX.Max();
            float minY = allY.Min();
            float maxY = allY.Max();

            var topLeft = new FloatPoint(minX, minY);
            var bottomRight = new FloatPoint(maxX, maxY);

            return new FloatRectangle(topLeft, bottomRight);
        }
    }
}
