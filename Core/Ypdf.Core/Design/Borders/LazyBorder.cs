using System;
using System.Collections.Generic;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Borders;

public readonly struct LazyBorder : IEquatable<LazyBorder>
{
    public LazyBorder(BorderType borderType)
        : this(borderType, new DeviceRgb(0, 0, 0)) { }

    public LazyBorder(
        BorderType borderType,
        DeviceRgb color,
        float thickness = 5,
        float opacity = 1)
    {
        ExtendedArgumentNullException.ThrowIfNull(color, nameof(color));
        DefaultExceptions.ThrowIfNegativeOrZero(thickness, nameof(thickness));
        DefaultExceptions.ThrowIfNotBetween(opacity, 0, 1, nameof(opacity));

        BorderType = borderType;
        Color = color;
        Thickness = thickness;
        Opacity = opacity;
    }

    public readonly BorderType BorderType { get; }
    public readonly DeviceRgb Color { get; }
    public readonly float Thickness { get; }
    public readonly float Opacity { get; }

    public static bool operator ==(LazyBorder left, LazyBorder right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(LazyBorder left, LazyBorder right)
    {
        return !(left == right);
    }

    public readonly Border Create()
    {
        return BorderType switch
        {
            BorderType.Inset => new InsetBorder(Color, Thickness, Opacity),
            BorderType.Ridge => new RidgeBorder(Color, Thickness, Opacity),
            BorderType.Solid => new SolidBorder(Color, Thickness, Opacity),
            BorderType.Dashed => new DashedBorder(Color, Thickness, Opacity),
            BorderType.Dotted => new DottedBorder(Color, Thickness, Opacity),
            BorderType.DoubleLine => new DoubleBorder(Color, Thickness, Opacity),
            BorderType.Groove => new GrooveBorder(Color, Thickness, Opacity),
            BorderType.Outset => new OutsetBorder(Color, Thickness, Opacity),
            BorderType.RoundDots => new RoundDotsBorder(Color, Thickness, Opacity),
            BorderType.FixedDashed => new FixedDashedBorder(Color, Thickness, Opacity),

            _ => throw new NotSupportedException($"Border type {BorderType} isn't supported.")
        };
    }

    public bool Equals(LazyBorder other)
    {
        return BorderType == other.BorderType
            && EqualityComparer<Color>.Default.Equals(Color, other.Color)
            && Thickness == other.Thickness
            && Opacity == other.Opacity;
    }

    public override bool Equals(object? obj)
    {
        return obj is LazyBorder other
            && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(BorderType, Color, Thickness, Opacity);
    }
}
