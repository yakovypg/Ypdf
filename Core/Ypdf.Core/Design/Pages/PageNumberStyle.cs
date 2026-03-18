using System;
using System.Collections.Generic;
using iText.IO.Font.Constants;
using iText.Layout.Element;
using iText.Layout.Properties;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageNumberStyle : IEquatable<PageNumberStyle>
{
    private readonly IPageNumberTextPresenter _textPresenter;

    public PageNumberStyle()
    {
        _textPresenter = PageNumberTextPresenter.Default;

        FontInfo = new TextFontInfo(StandardFonts.TIMES_ROMAN);

        HorizontalAlignment = TabAlignment.CENTER;
        VerticalAlignment = VerticalAlignment.BOTTOM;

        Margin = new Margin(0);
        LocationMode = LocationMode.WithoutIncrease;

        ConsiderLeftPageMargin = true;
        ConsiderTopPageMargin = false;
        ConsiderRightPageMargin = true;
        ConsiderBottomPageMargin = false;
    }

    public readonly TextFontInfo FontInfo { get; init; }

    public readonly TabAlignment HorizontalAlignment { get; init; }
    public readonly VerticalAlignment VerticalAlignment { get; init; }

    public readonly Margin Margin { get; init; }
    public readonly LocationMode LocationMode { get; init; }

    public readonly bool ConsiderLeftPageMargin { get; init; }
    public readonly bool ConsiderTopPageMargin { get; init; }
    public readonly bool ConsiderRightPageMargin { get; init; }
    public readonly bool ConsiderBottomPageMargin { get; init; }

    public readonly IPageNumberTextPresenter TextPresenter
    {
        get => _textPresenter;
        init => _textPresenter = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static bool operator ==(PageNumberStyle left, PageNumberStyle right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PageNumberStyle left, PageNumberStyle right)
    {
        return !(left == right);
    }

    public readonly Text GetStylizedText(string text)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));

        return new Text(text)
            .SetFontSize(FontInfo.Size)
            .SetFontColor(FontInfo.Color)
            .SetOpacity(FontInfo.Opacity)
            .SetFont(FontInfo.CreatePdfFont());
    }

    public readonly bool Equals(PageNumberStyle other)
    {
        return FontInfo == other.FontInfo
            && EqualityComparer<IPageNumberTextPresenter>.Default.Equals(TextPresenter, other.TextPresenter)
            && HorizontalAlignment == other.HorizontalAlignment
            && VerticalAlignment == other.VerticalAlignment
            && Margin == other.Margin
            && LocationMode == other.LocationMode
            && ConsiderLeftPageMargin == other.ConsiderLeftPageMargin
            && ConsiderTopPageMargin == other.ConsiderTopPageMargin
            && ConsiderRightPageMargin == other.ConsiderRightPageMargin
            && ConsiderBottomPageMargin == other.ConsiderBottomPageMargin;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageNumberStyle other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(
            FontInfo,
            TextPresenter,
            HorizontalAlignment,
            VerticalAlignment,
            Margin,
            LocationMode,
            ConsiderLeftPageMargin,
            ConsiderTopPageMargin,
            ConsiderRightPageMargin,
            ConsiderBottomPageMargin);
    }
}
