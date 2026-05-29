using System;
using System.Globalization;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design;

public readonly struct Margin : IEquatable<Margin>
{
    public Margin()
        : this(0) { }

    public Margin(float value)
        : this(value, value, value, value) { }

    public Margin(float horizontal, float vertical)
        : this(horizontal, vertical, horizontal, vertical) { }

    public Margin(float left, float top, float right, float bottom)
    {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }

    public readonly float Left { get; }
    public readonly float Top { get; }
    public readonly float Right { get; }
    public readonly float Bottom { get; }

    public readonly float HorizontalSum => Left + Right;
    public readonly float VerticalSum => Top + Bottom;

    public readonly bool IsZero => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;

    public static bool operator ==(Margin left, Margin right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Margin left, Margin right)
    {
        return !(left == right);
    }

    public static Margin Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(',');
        const string expectedStringFormat = "M or H,V or L,T,R,B";

        switch (parts.Length)
        {
            case 1:
                float value = float.Parse(parts[0], CultureInfo.InvariantCulture);
                return new Margin(value);

            case 2:
                float horizontal = float.Parse(parts[0], CultureInfo.InvariantCulture);
                float vertical = float.Parse(parts[1], CultureInfo.InvariantCulture);
                return new Margin(horizontal, vertical);

            case 4:
                float left = float.Parse(parts[0], CultureInfo.InvariantCulture);
                float top = float.Parse(parts[1], CultureInfo.InvariantCulture);
                float right = float.Parse(parts[2], CultureInfo.InvariantCulture);
                float bottom = float.Parse(parts[3], CultureInfo.InvariantCulture);
                return new Margin(left, top, right, bottom);

            default:
                throw new IncorrectDataFormatException(null, data, expectedStringFormat);
        }
    }

    public readonly bool Equals(Margin other)
    {
        return Top == other.Top &&
                Right == other.Right &&
                Bottom == other.Bottom &&
                Left == other.Left;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Margin other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(
            Top,
            Right,
            Bottom,
            Left);
    }
}
