namespace YpdfLib.Models.Geometry
{
    public static class Distance
    {
        public static float Calc(FloatPoint a, FloatPoint b)
        {
            return FloatMath.Sqrt(
                FloatMath.Pow(b.X - a.X, 2) +
                FloatMath.Pow(b.Y - a.Y, 2)
            );
        }

        public static float Calc(float x, float y)
        {
            return FloatMath.Abs(y - x);
        }
    }
}
