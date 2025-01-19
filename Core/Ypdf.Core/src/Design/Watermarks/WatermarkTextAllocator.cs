using System;
using iText.Layout.Properties;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Watermarks;

public readonly struct WatermarkTextAllocator : IEquatable<WatermarkTextAllocator>
{
    public WatermarkTextAllocator(
        TextAlignment textAlignment = TextAlignment.LEFT,
        HorizontalAlignment textHorizontalAlignment = HorizontalAlignment.LEFT,
        VerticalAlignment textContainerVerticalAlignment = VerticalAlignment.MIDDLE)
    {
        TextAlignment = textAlignment;
        TextHorizontalAlignment = textHorizontalAlignment;
        TextContainerVerticalAlignment = textContainerVerticalAlignment;
    }

    public readonly TextAlignment TextAlignment { get; init; }
    public readonly HorizontalAlignment TextHorizontalAlignment { get; init; }
    public readonly VerticalAlignment TextContainerVerticalAlignment { get; init; }

    public static bool operator ==(WatermarkTextAllocator left, WatermarkTextAllocator right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(WatermarkTextAllocator left, WatermarkTextAllocator right)
    {
        return !(left == right);
    }

    public readonly bool Equals(WatermarkTextAllocator other)
    {
        return TextAlignment == other.TextAlignment
            && TextHorizontalAlignment == other.TextHorizontalAlignment
            && TextContainerVerticalAlignment == other.TextContainerVerticalAlignment;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is WatermarkTextAllocator other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(
            TextAlignment,
            TextHorizontalAlignment,
            TextContainerVerticalAlignment);
    }
}
