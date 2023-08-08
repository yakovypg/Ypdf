namespace YpdfLib.Models.Geometry
{
    public static class FloatMath
    {
        public static float Abs(float value)
        {
            return value < 0 ? -value : value;
        }

        public static float Pow(float value, float power)
        {
            double res = Math.Pow(value, power);
            return Convert.ToSingle(res);
        }

        public static float Sqrt(float value)
        {
            double res = Math.Sqrt(value);
            return Convert.ToSingle(res);
        }
    }
}
