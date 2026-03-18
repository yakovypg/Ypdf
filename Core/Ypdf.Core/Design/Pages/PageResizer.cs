using System;
using System.Collections.Generic;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Informing;

namespace Ypdf.Core.Design.Pages;

public class PageResizer
{
    private readonly PdfPage _page;

    public PageResizer(PdfPage page)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));
        _page = page;
    }

    public static void IncreaseLeft(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseLeft(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseLeft(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreaseLeft(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseLeft(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseLeft(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreaseLeft(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreaseLeft(value, color);
        });

        Increase(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void IncreaseTop(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseTop(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseTop(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreaseTop(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseTop(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseTop(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreaseTop(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreaseTop(value, color);
        });

        Increase(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void IncreaseRight(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseRight(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseRight(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreaseRight(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseRight(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseRight(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreaseRight(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreaseRight(value, color);
        });

        Increase(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void IncreaseBottom(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseBottom(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseBottom(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreaseBottom(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreaseBottom(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreaseBottom(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreaseBottom(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreaseBottom(value, color);
        });

        Increase(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void Increase(
        string inputPath,
        string outputPath,
        PageIncrease pageIncrease,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        Increase(inputPath, outputPath, pageIncrease, ColorConstants.WHITE, pages);
    }

    public static void Increase(
        string inputPath,
        string outputPath,
        PageIncrease pageIncrease,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        Increase(inputPath, outputPath, pageIncrease, ColorConstants.WHITE, pages);
    }

    public static void Increase(
        string inputPath,
        string outputPath,
        PageIncrease pageIncrease,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        Increase(inputPath, outputPath, pageIncrease, fillColor, pages);
    }

    public static void Increase(
        string inputPath,
        string outputPath,
        PageIncrease pageIncrease,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, float, float, float, Color>(
            (page, left, top, right, bottom, color) =>
        {
            new PageResizer(page).Increase(left, top, right, bottom, color);
        });

        Increase(inputPath, outputPath, pageIncrease, fillColor, increaseAction, pages);
    }

    public void Resize(float width, float height)
    {
        _ = new MediaBox(_page)
        {
            Width = width,
            Height = height
        };
    }

    public void IncreaseLeft(float value)
    {
        IncreaseLeft(value, ColorConstants.WHITE);
    }

    public void IncreaseLeft(float value, Color fillColor, float heightIncrease = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.StartX -= value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialStartX - value,
            y: mediaBox.InitialStartY,
            width: value,
            height: mediaBox.InitialHeight + heightIncrease,
            fillColor: fillColor);
    }

    public void IncreaseTop(float value)
    {
        IncreaseTop(value, ColorConstants.WHITE);
    }

    public void IncreaseTop(float value, Color fillColor, float widthIncrease = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.Height += value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialStartX,
            y: mediaBox.InitialHeight,
            width: mediaBox.InitialWidth + widthIncrease,
            height: value,
            fillColor: fillColor);
    }

    public void IncreaseRight(float value)
    {
        IncreaseRight(value, ColorConstants.WHITE);
    }

    public void IncreaseRight(float value, Color fillColor, float heightIncrease = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.Width += value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialWidth,
            y: mediaBox.InitialStartY,
            width: value,
            height: mediaBox.InitialHeight + heightIncrease,
            fillColor: fillColor);
    }

    public void IncreaseBottom(float value)
    {
        IncreaseBottom(value, ColorConstants.WHITE);
    }

    public void IncreaseBottom(float value, Color fillColor, float widtIncrease = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.StartY -= value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialStartX,
            y: mediaBox.InitialStartY - value,
            width: mediaBox.InitialWidth + widtIncrease,
            height: value,
            fillColor: fillColor);
    }

    public void Increase(float left, float top, float right, float bottom)
    {
        Increase(left, top, right, bottom, ColorConstants.WHITE);
    }

    public void Increase(
        float left,
        float top,
        float right,
        float bottom,
        Color fillColor)
    {
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        Increase(left, top, right, bottom, fillColor, fillColor, fillColor, fillColor);
    }

    public void Increase(
        float left,
        float top,
        float right,
        float bottom,
        Color leftColor,
        Color topColor,
        Color rightColor,
        Color bottomColor)
    {
        ExtendedArgumentNullException.ThrowIfNull(leftColor, nameof(leftColor));
        ExtendedArgumentNullException.ThrowIfNull(topColor, nameof(topColor));
        ExtendedArgumentNullException.ThrowIfNull(rightColor, nameof(rightColor));
        ExtendedArgumentNullException.ThrowIfNull(bottomColor, nameof(bottomColor));

        IncreaseLeft(left, leftColor);
        IncreaseRight(right, rightColor);
        IncreaseTop(top, topColor, left);
        IncreaseBottom(bottom, bottomColor, left);
    }

    private static void Increase(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        Action<PdfPage, float, Color> increaseAction,
        IList<int> pages)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(increaseAction, nameof(increaseAction));
        ExtendedArgumentNullException.ThrowIfNull(pages, nameof(pages));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(reader, writer);

        foreach (int pageNum in pages)
        {
            PdfPage page = pdfDocument.GetPage(pageNum);
            increaseAction.Invoke(page, value, fillColor);
        }
    }

    private static void Increase(
        string inputPath,
        string outputPath,
        PageIncrease pageIncrease,
        Color fillColor,
        Action<PdfPage, float, float, float, float, Color> increaseAction,
        IList<int> pages)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(increaseAction, nameof(increaseAction));
        ExtendedArgumentNullException.ThrowIfNull(pages, nameof(pages));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(reader, writer);

        foreach (int pageNum in pages)
        {
            PdfPage page = pdfDocument.GetPage(pageNum);

            increaseAction.Invoke(
                page,
                pageIncrease.Left,
                pageIncrease.Top,
                pageIncrease.Right,
                pageIncrease.Bottom,
                fillColor);
        }
    }
}
