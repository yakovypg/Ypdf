using System;
using System.Globalization;
using Ypdf.Core.Extensions;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Geometry;

public readonly struct FloatPoint : IEquatable<FloatPoint>
{
    public FloatPoint()
        : this(0, 0)
    {
    }

    public FloatPoint(float x, float y)
    {
        X = x;
        Y = y;
    }

    public readonly float X { get; }
    public readonly float Y { get; }

    public static bool operator ==(FloatPoint left, FloatPoint right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FloatPoint left, FloatPoint right)
    {
        return !(left == right);
    }

    public static FloatPoint FromDoubleCoordinates(double x, double y)
    {
        return new FloatPoint(
            Convert.ToSingle(x),
            Convert.ToSingle(y));
    }

    public static FloatPoint Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        data = data.Replace(" ", string.Empty, StringComparison.CurrentCulture);

        if (data.StartsWith('(', StringComparison.CurrentCulture))
            data = data.Substring(1);

        if (data.EndsWith(')', StringComparison.CurrentCulture))
            data = data.Remove(data.Length - 1);

        string[] parts = data.Split(';');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, "(x;y) or x;y");

        float x = float.Parse(parts[0], CultureInfo.CurrentCulture);
        float y = float.Parse(parts[1], CultureInfo.CurrentCulture);

        return new FloatPoint(x, y);
    }

    public readonly bool Equals(FloatPoint other)
    {
        return X == other.X
            && Y == other.Y;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is FloatPoint other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(X, Y);
    }
}
