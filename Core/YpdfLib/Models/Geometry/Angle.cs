namespace YpdfLib.Models.Geometry
{
    public readonly struct Angle : IAngle, IEquatable<Angle>
    {
        public double RadiansValue { get; }
        public double DegreesValue => RadiansToDegrees(RadiansValue);

        public Angle(double radValue = 0)
        {
            RadiansValue = radValue;
        }

        public static Angle FromRadians(double radians)
        {
            return new Angle(radians);
        }

        public static Angle FromDegrees(double degrees)
        {
            return new Angle(DegreesToRadians(degrees));
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        public static double DegreesToRadians(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        public bool Equals(Angle other)
        {
            return RadiansValue == other.RadiansValue;
        }

        public override bool Equals(object? obj)
        {
            return obj is Angle angle && Equals(angle);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RadiansValue);
        }

        public static bool operator ==(Angle left, Angle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Angle left, Angle right)
        {
            return !(left == right);
        }

        public static bool operator >(Angle left, Angle right)
        {
            return left.RadiansValue > right.RadiansValue;
        }

        public static bool operator <(Angle left, Angle right)
        {
            return left.RadiansValue < right.RadiansValue;
        }

        public static bool operator >=(Angle left, Angle right)
        {
            return !(left < right);
        }

        public static bool operator <=(Angle left, Angle right)
        {
            return !(left > right);
        }
    }
}
