using System;
using System.Collections.Generic;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design;

public class MediaBox : IMediaBox, IEquatable<MediaBox?>
{
    private readonly PdfPage _page;
    private readonly PdfArray _parameters;

    public MediaBox(PdfPage page)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));

        _page = page;

        _parameters = page
            .GetPdfObject()
            .GetAsArray(PdfName.MediaBox);

        InitialStartX = StartX;
        InitialStartY = StartY;
        InitialWidth = Width;
        InitialHeight = Height;
    }

    public float InitialStartX { get; }
    public float InitialStartY { get; }
    public float InitialWidth { get; }
    public float InitialHeight { get; }

    public float StartX
    {
        get => _parameters.GetAsNumber(0).FloatValue();
        set => _parameters.Set(0, new PdfNumber(value));
    }

    public float StartY
    {
        get => _parameters.GetAsNumber(1).FloatValue();
        set => _parameters.Set(1, new PdfNumber(value));
    }

    public float Width
    {
        get => _parameters.GetAsNumber(2).FloatValue();
        set
        {
            DefaultExceptions.ThrowIfNegativeOrZero(value, nameof(value));
            _parameters.Set(2, new PdfNumber(value));
        }
    }

    public float Height
    {
        get => _parameters.GetAsNumber(3).FloatValue();
        set
        {
            DefaultExceptions.ThrowIfNegativeOrZero(value, nameof(value));
            _parameters.Set(3, new PdfNumber(value));
        }
    }

    public void DrawRectangle(
        float x,
        float y,
        float width,
        float height)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(width, nameof(width));
        DefaultExceptions.ThrowIfNegativeOrZero(height, nameof(height));

        DrawRectangle(x, y, width, height, ColorConstants.WHITE);
    }

    public void DrawRectangle(
        float x,
        float y,
        float width,
        float height,
        Color fillColor)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(width, nameof(width));
        DefaultExceptions.ThrowIfNegativeOrZero(height, nameof(height));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        _ = new PdfCanvas(_page)
            .SaveState()
            .SetFillColor(fillColor)
            .Rectangle(x, y, width, height)
            .Fill()
            .RestoreState();
    }

    public bool Equals(MediaBox? other)
    {
        return other is not null
            && EqualityComparer<PdfPage>.Default.Equals(_page, other._page)
            && EqualityComparer<PdfArray>.Default.Equals(_parameters, other._parameters)
            && InitialStartX == other.InitialStartX
            && InitialStartY == other.InitialStartY
            && InitialWidth == other.InitialWidth
            && InitialHeight == other.InitialHeight;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as MediaBox);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(
            _page,
            _parameters,
            InitialStartX,
            InitialStartY,
            InitialWidth,
            InitialHeight);
    }
}
