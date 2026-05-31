using System;
using System.Collections.Generic;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
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

    public static void IncreasePageSizeLeft(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeLeft(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeLeft(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreasePageSizeLeft(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeLeft(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeLeft(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreasePageSizeLeft(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreasePageSizeLeft(value, color);
        });

        IncreasePageSize(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void IncreasePageSizeTop(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeTop(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeTop(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreasePageSizeTop(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeTop(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeTop(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreasePageSizeTop(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreasePageSizeTop(value, color);
        });

        IncreasePageSize(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void IncreasePageSizeRight(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeRight(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeRight(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreasePageSizeRight(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeRight(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeRight(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreasePageSizeRight(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreasePageSizeRight(value, color);
        });

        IncreasePageSize(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void IncreasePageSizeBottom(
        string inputPath,
        string outputPath,
        float value,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeBottom(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeBottom(
        string inputPath,
        string outputPath,
        float value,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreasePageSizeBottom(inputPath, outputPath, value, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSizeBottom(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSizeBottom(inputPath, outputPath, value, fillColor, pages);
    }

    public static void IncreasePageSizeBottom(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
        {
            new PageResizer(page).IncreasePageSizeBottom(value, color);
        });

        IncreasePageSize(inputPath, outputPath, value, fillColor, increaseAction, pages);
    }

    public static void IncreasePageSize(
        string inputPath,
        string outputPath,
        PageSizeAdjustment pageSizeAdjustment,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        if (!pageSizeAdjustment.IsPositiveOrZero)
            throw new ArgumentException("All adjustment values must be non-negative.", nameof(pageSizeAdjustment));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSize(inputPath, outputPath, pageSizeAdjustment, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSize(
        string inputPath,
        string outputPath,
        PageSizeAdjustment pageSizeAdjustment,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        if (!pageSizeAdjustment.IsPositiveOrZero)
            throw new ArgumentException("All adjustment values must be non-negative.", nameof(pageSizeAdjustment));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);
        IncreasePageSize(inputPath, outputPath, pageSizeAdjustment, ColorConstants.WHITE, pages);
    }

    public static void IncreasePageSize(
        string inputPath,
        string outputPath,
        PageSizeAdjustment pageSizeAdjustment,
        Color fillColor,
        PageRange[] pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        ExtendedArgumentNullException.ThrowIfNull(pageRanges, nameof(pageRanges));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        if (!pageSizeAdjustment.IsPositiveOrZero)
            throw new ArgumentException("All adjustment values must be non-negative.", nameof(pageSizeAdjustment));

        IList<int> pages = PageRange.GetAllItems(pageRanges);
        IncreasePageSize(inputPath, outputPath, pageSizeAdjustment, fillColor, pages);
    }

    public static void IncreasePageSize(
        string inputPath,
        string outputPath,
        PageSizeAdjustment pageSizeAdjustment,
        Color fillColor,
        IList<int>? pages = null)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        if (!pageSizeAdjustment.IsPositiveOrZero)
            throw new ArgumentException("All adjustment values must be non-negative.", nameof(pageSizeAdjustment));

        pages ??= PdfInfo.GetAllPageNumbers(inputPath);

        var increaseAction = new Action<PdfPage, float, float, float, float, Color>(
            (page, left, top, right, bottom, color) =>
        {
            new PageResizer(page).IncreasePageSize(left, top, right, bottom, color);
        });

        IncreasePageSize(inputPath, outputPath, pageSizeAdjustment, fillColor, increaseAction, pages);
    }

    public void ResizePage(float width, float height)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(width, nameof(width));
        DefaultExceptions.ThrowIfNegativeOrZero(height, nameof(height));

        var rectangle = new Rectangle(0, 0, width, height);

        _page.SetMediaBox(rectangle);
        _page.SetCropBox(rectangle);
    }

    public void IncreasePageSizeLeft(float value)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        IncreasePageSizeLeft(value, ColorConstants.WHITE);
    }

    public void IncreasePageSizeLeft(float value, Color fillColor, float heightDelta = 0)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.StartX -= value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialStartX - value,
            y: mediaBox.InitialStartY,
            width: value,
            height: mediaBox.InitialHeight + heightDelta,
            fillColor: fillColor);
    }

    public void IncreasePageSizeTop(float value)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        IncreasePageSizeTop(value, ColorConstants.WHITE);
    }

    public void IncreasePageSizeTop(float value, Color fillColor, float widthDelta = 0)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.Height += value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialStartX,
            y: mediaBox.InitialHeight,
            width: mediaBox.InitialWidth + widthDelta,
            height: value,
            fillColor: fillColor);
    }

    public void IncreasePageSizeRight(float value)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        IncreasePageSizeRight(value, ColorConstants.WHITE);
    }

    public void IncreasePageSizeRight(float value, Color fillColor, float heightDelta = 0)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.Width += value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialWidth,
            y: mediaBox.InitialStartY,
            width: value,
            height: mediaBox.InitialHeight + heightDelta,
            fillColor: fillColor);
    }

    public void IncreasePageSizeBottom(float value)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        IncreasePageSizeBottom(value, ColorConstants.WHITE);
    }

    public void IncreasePageSizeBottom(float value, Color fillColor, float widthDelta = 0)
    {
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        var mediaBox = new MediaBox(_page);

        mediaBox.StartY -= value;

        mediaBox.DrawRectangle(
            x: mediaBox.InitialStartX,
            y: mediaBox.InitialStartY - value,
            width: mediaBox.InitialWidth + widthDelta,
            height: value,
            fillColor: fillColor);
    }

    public void IncreasePageSize(float left, float top, float right, float bottom)
    {
        DefaultExceptions.ThrowIfNegative(left, nameof(left));
        DefaultExceptions.ThrowIfNegative(top, nameof(top));
        DefaultExceptions.ThrowIfNegative(right, nameof(right));
        DefaultExceptions.ThrowIfNegative(bottom, nameof(bottom));

        IncreasePageSize(left, top, right, bottom, ColorConstants.WHITE);
    }

    public void IncreasePageSize(
        float left,
        float top,
        float right,
        float bottom,
        Color fillColor)
    {
        DefaultExceptions.ThrowIfNegative(left, nameof(left));
        DefaultExceptions.ThrowIfNegative(top, nameof(top));
        DefaultExceptions.ThrowIfNegative(right, nameof(right));
        DefaultExceptions.ThrowIfNegative(bottom, nameof(bottom));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        IncreasePageSize(left, top, right, bottom, fillColor, fillColor, fillColor, fillColor);
    }

    public void IncreasePageSize(
        float left,
        float top,
        float right,
        float bottom,
        Color leftFillColor,
        Color topFillColor,
        Color rightFillColor,
        Color bottomFillColor)
    {
        DefaultExceptions.ThrowIfNegative(left, nameof(left));
        DefaultExceptions.ThrowIfNegative(top, nameof(top));
        DefaultExceptions.ThrowIfNegative(right, nameof(right));
        DefaultExceptions.ThrowIfNegative(bottom, nameof(bottom));

        ExtendedArgumentNullException.ThrowIfNull(leftFillColor, nameof(leftFillColor));
        ExtendedArgumentNullException.ThrowIfNull(topFillColor, nameof(topFillColor));
        ExtendedArgumentNullException.ThrowIfNull(rightFillColor, nameof(rightFillColor));
        ExtendedArgumentNullException.ThrowIfNull(bottomFillColor, nameof(bottomFillColor));

        IncreasePageSizeLeft(left, leftFillColor);
        IncreasePageSizeRight(right, rightFillColor);
        IncreasePageSizeTop(top, topFillColor, left);
        IncreasePageSizeBottom(bottom, bottomFillColor, left);
    }

    private static void IncreasePageSize(
        string inputPath,
        string outputPath,
        float value,
        Color fillColor,
        Action<PdfPage, float, Color> increaseAction,
        IList<int> pages)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfNegative(value, nameof(value));
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

    private static void IncreasePageSize(
        string inputPath,
        string outputPath,
        PageSizeAdjustment pageSizeAdjustment,
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

        if (!pageSizeAdjustment.IsPositiveOrZero)
            throw new ArgumentException("All adjustment values must be non-negative.", nameof(pageSizeAdjustment));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(reader, writer);

        foreach (int pageNum in pages)
        {
            PdfPage page = pdfDocument.GetPage(pageNum);

            increaseAction.Invoke(
                page,
                pageSizeAdjustment.Left,
                pageSizeAdjustment.Top,
                pageSizeAdjustment.Right,
                pageSizeAdjustment.Bottom,
                fillColor);
        }
    }
}
