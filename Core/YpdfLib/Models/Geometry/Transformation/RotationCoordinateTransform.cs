namespace YpdfLib.Models.Geometry.Transformation
{
    public class RotationCoordinateTransform : ITransform<FloatPoint>
    {
        public Angle Angle { get; }
        public FloatPoint RotationCenter { get; }

        public RotationCoordinateTransform(Angle angle, FloatPoint rotationCenter = default)
        {
            Angle = angle;
            RotationCenter = rotationCenter;
        }

        public FloatPoint Transform(FloatPoint point)
        {
            double angle = Angle.RadiansValue;
        
            float x = point.X;
            float y = point.Y;

            float x0 = RotationCenter.X;
            float y0 = RotationCenter.Y;

            double x2 = x0 + (x - x0) * Math.Cos(angle) - (y - y0) * Math.Sin(angle);
            double y2 = y0 + (x - x0) * Math.Sin(angle) + (y - y0) * Math.Cos(angle);

            float resX = Convert.ToSingle(x2);
            float resY = Convert.ToSingle(y2);

            return new FloatPoint(resX, resY);
        }
    }
}
