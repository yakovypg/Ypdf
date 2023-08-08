namespace YpdfLib.Models.Geometry.Transformation
{
    public class RotationRectangleTransform : ITransform<FloatRectangle>
    {
        public Angle Angle { get; }
        public FloatPoint RotationCenter { get; }

        public RotationRectangleTransform(Angle angle, FloatPoint rotationCenter = default)
        {
            Angle = angle;
            RotationCenter = rotationCenter;
        }

        public FloatRectangle Transform(FloatRectangle rect)
        {
            var coordinateTransform = new RotationCoordinateTransform(Angle, RotationCenter);

            FloatPoint topLeft = coordinateTransform.Transform(rect.TopLeft);
            FloatPoint topRight = coordinateTransform.Transform(rect.TopRight);
            FloatPoint bottomRight = coordinateTransform.Transform(rect.BottomRight);
            FloatPoint bottomLeft = coordinateTransform.Transform(rect.BottomLeft);

            return new FloatRectangle(topLeft, topRight, bottomRight, bottomLeft);
        }
    }
}
