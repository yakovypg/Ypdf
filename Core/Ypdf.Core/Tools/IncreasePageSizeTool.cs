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
            _defaultFillColor)
    { }

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
        IncreasePageMode increasePageMode = IncreasePageMode.Bottom)
        : this(generalPageSizeAdjustment, _defaultFillColor, increasePageMode) { }

    public IncreasePageSizeTool(
        float generalPageSizeAdjustment,
        Color fillColor,
        IncreasePageMode increasePageMode = IncreasePageMode.Bottom)
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

        sourceDocument.CopyTo(outputDocument);

        if (PageSizeAdjustments.Any())
            IncreasePageSizes(outputDocument, PageSizeAdjustments, FillColor);
        else
            IncreasePageSizes(outputDocument, GeneralPageSizeAdjustment, IncreasePageMode, FillColor);
    }

    private static void IncreasePageSizes(
        PdfDocument pdfDocument,
        IEnumerable<PageSizeAdjustment> pageSizeAdjustments,
        Color fillColor)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfDocument, nameof(pdfDocument));
        ExtendedArgumentException.ThrowIfNullOrEmpty(pageSizeAdjustments, nameof(pageSizeAdjustments));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        foreach (PageSizeAdjustment pageSizeAdjustment in pageSizeAdjustments)
        {
            PdfPage page = pdfDocument.GetPage(pageSizeAdjustment.PageNumber);
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
        PdfDocument pdfDocument,
        float generalPageSizeAdjustment,
        IncreasePageMode increasePageMode,
        Color fillColor)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfDocument, nameof(pdfDocument));
        DefaultExceptions.ThrowIfNegativeOrZero(generalPageSizeAdjustment, nameof(generalPageSizeAdjustment));
        ExtendedArgumentNullException.ThrowIfNull(fillColor, nameof(fillColor));

        int numOfPages = pdfDocument.GetNumberOfPages();

        for (int i = 1; i <= numOfPages; ++i)
        {
            PdfPage page = pdfDocument.GetPage(i);
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

        if (increasePageMode == IncreasePageMode.Left)
            pageResizer.IncreasePageSizeLeft(adjustment, fillColor);
        else if (increasePageMode == IncreasePageMode.Top)
            pageResizer.IncreasePageSizeTop(adjustment, fillColor);
        else if (increasePageMode == IncreasePageMode.Right)
            pageResizer.IncreasePageSizeRight(adjustment, fillColor);
        else if (increasePageMode == IncreasePageMode.Bottom)
            pageResizer.IncreasePageSizeBottom(adjustment, fillColor);
    }
}
