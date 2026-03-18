using System;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Geometry;

public readonly struct Angle : IEquatable<Angle>, IComparable<Angle>
{
    public Angle(double radiansValue = 0)
    {
        RadiansValue = radiansValue;
    }

    public readonly double RadiansValue { get; }
    public readonly double DegreesValue => RadiansToDegrees(RadiansValue);

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

    public int CompareTo(Angle other)
    {
        return RadiansValue.CompareTo(other.RadiansValue);
    }

    public bool Equals(Angle other)
    {
        return RadiansValue == other.RadiansValue;
    }

    public override bool Equals(object? obj)
    {
        return obj is Angle other
            && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(RadiansValue);
    }
}
