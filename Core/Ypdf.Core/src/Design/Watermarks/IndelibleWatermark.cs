using System;
using System.Collections.Generic;
using iText.Kernel.Geom;
using Ypdf.Core.Design.Borders;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Geometry;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Watermarks;

public class IndelibleWatermark : Watermark, IIndelibleWatermark, IEquatable<IndelibleWatermark?>
{
    public IndelibleWatermark(
        float width,
        float height,
        string text = DefaultText,
        double rotationAngleInRadians = DefaultRotationAngleInRadians)
        : this(width, height, text, DefaultFontInfo, rotationAngleInRadians) { }

    public IndelibleWatermark(
        float width,
        float height,
        string text = DefaultText,
        TextFontInfo fontInfo = default,
        double rotationAngleInRadians = DefaultRotationAngleInRadians,
        FloatPoint? lowerLeftPoint = null,
        WatermarkTextAllocator textAllocator = default,
        LazyBorder? border = null)
        : base(text, fontInfo, rotationAngleInRadians, lowerLeftPoint)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(width, nameof(width));
        DefaultExceptions.ThrowIfNegativeOrZero(height, nameof(height));
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));

        Width = width;
        Height = height;
        TextAllocator = textAllocator;
        Border = border;
    }

    public float Width { get; }
    public float Height { get; }
    public WatermarkTextAllocator TextAllocator { get; }
    public LazyBorder? Border { get; }

    public FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageSize, nameof(pageSize));

        float pageWidth = pageSize.GetWidth();
        float pageHeight = pageSize.GetHeight();

        float bottomLeftX = (pageWidth / 2) - (Width / 2);
        float bottomLeftY = (pageHeight / 2) - (Height / 2);

        return new FloatPoint(bottomLeftX, bottomLeftY);
    }

    public bool Equals(IndelibleWatermark? other)
    {
        return other is not null
            && base.Equals(other)
            && Width == other.Width
            && Height == other.Height
            && TextAllocator == other.TextAllocator
            && EqualityComparer<LazyBorder?>.Default.Equals(Border, other.Border);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as IndelibleWatermark);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(
            Width,
            Height,
            TextAllocator,
            Border);
    }
}
