using System;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public class TextPageParameters : PageParameters, ITextPageParameters, IEquatable<TextPageParameters?>
{
    private const TextAlignment _defaultTextAlignment = TextAlignment.LEFT;

    public TextPageParameters(
        PageSize? pageSize = null,
        TextAlignment textAlignment = _defaultTextAlignment)
        : this(new TextFontInfo(), pageSize, textAlignment) { }

    public TextPageParameters(
        TextFontInfo fontInfo,
        PageSize? pageSize = null,
        TextAlignment textAlignment = _defaultTextAlignment)
        : this(fontInfo, new Margin(75.876f), pageSize, textAlignment) { }

    public TextPageParameters(
        Margin margin,
        PageSize? pageSize = null,
        TextAlignment textAlignment = _defaultTextAlignment)
        : this(new TextFontInfo(), margin, pageSize, textAlignment) { }

    public TextPageParameters(
        TextFontInfo fontInfo,
        Margin margin,
        PageSize? pageSize = null,
        TextAlignment textAlignment = _defaultTextAlignment)
    {
        FontInfo = fontInfo;
        HorizontalAlignment = textAlignment;
        Margin = margin;
        Size = pageSize;
    }

    public TextFontInfo FontInfo { get; init; }
    public TextAlignment HorizontalAlignment { get; init; }

    public bool Equals(TextPageParameters? other)
    {
        return other is not null
            && base.Equals(other)
            && FontInfo == other.FontInfo
            && HorizontalAlignment == other.HorizontalAlignment;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TextPageParameters);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(
            base.GetHashCode(),
            FontInfo,
            HorizontalAlignment);
    }
}
