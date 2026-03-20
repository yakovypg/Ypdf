using System.Collections.Generic;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Ypdf.Core.Design.Watermarks;
using Ypdf.Core.Geometry;
using Ypdf.Core.Informing;

namespace Ypdf.Core.Tools;

public class AddIndelibleWatermarkTool : ITool
{
    public AddIndelibleWatermarkTool(
        IIndelibleWatermark watermark,
        IEnumerable<int>? pages = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(watermark, nameof(watermark));

        Watermark = watermark;
        Pages = pages;
    }

    protected IIndelibleWatermark Watermark { get; }
    protected IEnumerable<int>? Pages { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        IEnumerable<int> pages = Pages ?? PdfInfo.GetAllPageNumbers(inputPath);

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(reader, writer);
        using var outputDocument = new Document(pdfDocument);

        outputDocument.SetMargins(0, 0, 0, 0);

        foreach (int pageNumber in pages)
        {
            PdfPage currPage = pdfDocument.GetPage(pageNumber);
            Rectangle currPageSize = currPage.GetPageSize();

            Paragraph indelibleWatermark = CreateIndelibleWatermark(
                pageNumber, currPageSize, Watermark);

            outputDocument.Add(indelibleWatermark);
        }
    }

    private static Paragraph CreateIndelibleWatermark(
        int pageNumber,
        Rectangle pageSize,
        IIndelibleWatermark watermark)
    {
        DefaultExceptions.ThrowIfLessThan(pageNumber, 1, nameof(pageNumber));
        ExtendedArgumentNullException.ThrowIfNull(pageSize, nameof(pageSize));
        ExtendedArgumentNullException.ThrowIfNull(watermark, nameof(watermark));

        FloatPoint lowerLeftPoint = watermark.LowerLeftPoint
            ?? watermark.GetCenterredLowerLeftPoint(pageSize);

        var text = new Text(watermark.Text)
            .SetTextAlignment(watermark.TextAllocator.TextAlignment)
            .SetHorizontalAlignment(watermark.TextAllocator.TextHorizontalAlignment)
            .SetOpacity(watermark.FontInfo.Opacity)
            .SetFontColor(watermark.FontInfo.Color)
            .SetFont(watermark.FontInfo.CreatePdfFont())
            .SetFontSize(watermark.FontInfo.Size);

        var paragraph = new Paragraph(text)
            .SetVerticalAlignment(watermark.TextAllocator.TextContainerVerticalAlignment)
            .SetWidth(watermark.Width)
            .SetHeight(watermark.Height)
            .SetRotationAngle(watermark.RotationAngleRadians)
            .SetFixedPosition(pageNumber, lowerLeftPoint.X, lowerLeftPoint.Y, watermark.Width);

        if (watermark.Border is not null)
            paragraph.SetBorder(watermark.Border.Value.Create());

        return paragraph;
    }
}
