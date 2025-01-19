using System;
using System.Collections.Generic;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Fonts;

public readonly struct TextFontInfo : IEquatable<TextFontInfo>
{
    private readonly Color _color;

    public TextFontInfo()
        : this(StandardFonts.TIMES_ROMAN) { }

    public TextFontInfo(string family)
        : this(ColorConstants.DARK_GRAY)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(family, nameof(family));
        Family = family;
    }

    public TextFontInfo(string path, string encoding)
        : this(ColorConstants.DARK_GRAY)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(encoding, nameof(encoding));

        Path = path;
        Encoding = encoding;
    }

    private TextFontInfo(Color color, float size = 12f, float opacity = 1f)
    {
        ExtendedArgumentNullException.ThrowIfNull(color, nameof(color));
        DefaultExceptions.ThrowIfNegativeOrZero(size, nameof(size));
        DefaultExceptions.ThrowIfNotBetween(opacity, 0, 1, nameof(opacity));

        _color = color;

        Size = size;
        Opacity = opacity;
    }

    public readonly string? Path { get; }
    public readonly string? Encoding { get; }

    public readonly string? Family { get; }

    public readonly float Size { get; init; }
    public readonly float Opacity { get; init; }

    public readonly Color Color
    {
        get => _color;
        init => _color = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static bool operator ==(TextFontInfo left, TextFontInfo right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TextFontInfo left, TextFontInfo right)
    {
        return !(left == right);
    }

    public PdfFont CreatePdfFont()
    {
        return Path is not null && Encoding is not null
            ? TextFont.Create(Path, Encoding).PdfFont
            : PdfFontFactory.CreateFont(Family);
    }

    public readonly bool Equals(TextFontInfo other)
    {
        return Path == other.Path
            && Encoding == other.Encoding
            && Family == other.Family
            && Size == other.Size
            && Opacity == other.Opacity
            && EqualityComparer<Color>.Default.Equals(Color, other.Color);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is TextFontInfo other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(
            Path,
            Encoding,
            Family,
            Size,
            Opacity,
            Color);
    }
}
