using System;
using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using Ypdf.Core.Design;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Tools;

public class IncreasePageSizeTool : ITool
{
    private static readonly Color _defaultFillColor = ColorConstants.WHITE;

    public IncreasePageSizeTool(IEnumerable<PageSizeAdjustment> pageSizeAdjustments)
        : this(
            pageSizeAdjustments ?? throw new ArgumentNullException(nameof(pageSizeAdjustments)),
            _defaultFillColor) { }

    public IncreasePageSizeTool(IEnumerable<PageSizeAdjustment> pageSizeAdjustments, Color fillColor)
        : this(0, fillColor ?? throw new ArgumentNullException(nameof(fillColor)))
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(pageSizeAdjustments, nameof(pageSizeAdjustments));

        DefaultExceptions.ThrowIfContainsNotAllowedItem(
            pageSizeAdjustments, t => t.IsPositiveOrZero, nameof(pageSizeAdjustments));

        PageSizeAdjustments = pageSizeAdjustments;
    }

    public IncreasePageSizeTool(
        float generalPageSizeAdjustment,
        IncreasePageMode increasePageMode = IncreasePageMode.IncreaseBottom)
        : this(generalPageSizeAdjustment, _defaultFillColor, increasePageMode) { }

    public IncreasePageSizeTool(
        float generalPageSizeAdjustment,
        Color fillColor,
        IncreasePageMode increasePageMode = IncreasePageMode.IncreaseBottom)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(generalPageSizeAdjustment, nameof(generalPageSizeAdjustment));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));
        DefaultExceptions.ThrowIfEqual(increasePageMode, IncreasePageMode.WithoutIncrease, nameof(increasePageMode));

        PageSizeAdjustments = [];
        GeneralPageSizeAdjustment = generalPageSizeAdjustment;
        IncreasePageMode = increasePageMode;
        FillColor = fillColor;
    }

    protected IEnumerable<PageSizeAdjustment> PageSizeAdjustments { get; }
    protected float GeneralPageSizeAdjustment { get; }
    protected IncreasePageMode IncreasePageMode { get; }
    protected Color FillColor { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var sourceDocument = new PdfDocument(reader);
        using var outputDocument = new PdfDocument(writer);

        if (PageSizeAdjustments.Any())
            IncreasePageSizes(sourceDocument, PageSizeAdjustments, FillColor);
        else
            IncreasePageSizes(sourceDocument, GeneralPageSizeAdjustment, IncreasePageMode, FillColor);

        sourceDocument.CopyTo(outputDocument);
    }

    private static void IncreasePageSizes(
        PdfDocument sourceDocument,
        IEnumerable<PageSizeAdjustment> pageSizeAdjustments,
        Color fillColor)
    {
        ExtendedArgumentNullException.ThrowIfNull(sourceDocument, nameof(sourceDocument));
        ExtendedArgumentException.ThrowIfNullOrEmpty(pageSizeAdjustments, nameof(pageSizeAdjustments));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        foreach (PageSizeAdjustment pageSizeAdjustment in pageSizeAdjustments)
        {
            PdfPage page = sourceDocument.GetPage(pageSizeAdjustment.PageNumber);
            PageResizer pageResizer = new(page);

            pageResizer.IncreasePageSize(
                pageSizeAdjustment.Left,
                pageSizeAdjustment.Top,
                pageSizeAdjustment.Right,
                pageSizeAdjustment.Bottom,
                fillColor);
        }
    }

    private static void IncreasePageSizes(
        PdfDocument sourceDocument,
        float generalPageSizeAdjustment,
        IncreasePageMode increasePageMode,
        Color fillColor)
    {
        ExtendedArgumentNullException.ThrowIfNull(sourceDocument, nameof(sourceDocument));
        DefaultExceptions.ThrowIfNegativeOrZero(generalPageSizeAdjustment, nameof(generalPageSizeAdjustment));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        int numOfPages = sourceDocument.GetNumberOfPages();

        for (int i = 1; i <= numOfPages; ++i)
        {
            PdfPage page = sourceDocument.GetPage(i);
            PageResizer pageResizer = new(page);

            IncreasePageSize(pageResizer, generalPageSizeAdjustment, increasePageMode, fillColor);
        }
    }

    private static void IncreasePageSize(
        PageResizer pageResizer,
        float adjustment,
        IncreasePageMode increasePageMode,
        Color fillColor)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageResizer, nameof(pageResizer));
        DefaultExceptions.ThrowIfNegativeOrZero(adjustment, nameof(adjustment));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        if (increasePageMode == IncreasePageMode.IncreaseLeft)
            pageResizer.IncreasePageSizeLeft(adjustment, fillColor);
        else if (increasePageMode == IncreasePageMode.IncreaseTop)
            pageResizer.IncreasePageSizeTop(adjustment, fillColor);
        else if (increasePageMode == IncreasePageMode.IncreaseRight)
            pageResizer.IncreasePageSizeRight(adjustment, fillColor);
        else if (increasePageMode == IncreasePageMode.IncreaseBottom)
            pageResizer.IncreasePageSizeBottom(adjustment, fillColor);
    }
}
