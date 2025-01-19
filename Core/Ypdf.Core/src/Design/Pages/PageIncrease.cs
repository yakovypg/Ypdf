using System;
using System.Globalization;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageIncrease : IEquatable<PageIncrease>
{
    public PageIncrease(
        float left = 0,
        float top = 0,
        float right = 0,
        float bottom = 0)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public readonly float Left { get; }
    public readonly float Top { get; }
    public readonly float Right { get; }
    public readonly float Bottom { get; }

    public static bool operator ==(PageIncrease left, PageIncrease right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageIncrease left, PageIncrease right)
    {
        return !(left == right);
    }

    public static PageIncrease Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(',');

        if (parts.Length != 4)
            throw new IncorrectDataFormatException(null, data, "L,T,R,B");

        float left = float.Parse(parts[0], CultureInfo.InvariantCulture);
        float top = float.Parse(parts[1], CultureInfo.InvariantCulture);
        float right = float.Parse(parts[2], CultureInfo.InvariantCulture);
        float bottom = float.Parse(parts[3], CultureInfo.InvariantCulture);

        return new PageIncrease(left, top, right, bottom);
    }

    public readonly bool Equals(PageIncrease other)
    {
        return Left == other.Left
            && Top == other.Top
            && Right == other.Right
            && Bottom == other.Bottom;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageIncrease other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(Left, Top, Right, Bottom);
    }
}
