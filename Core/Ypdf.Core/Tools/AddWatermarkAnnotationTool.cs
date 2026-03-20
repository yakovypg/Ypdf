using System;
using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using Ypdf.Core.Design.Watermarks;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;
using Ypdf.Core.Informing;

namespace Ypdf.Core.Tools;

public class AddWatermarkAnnotationTool : ITool
{
    public AddWatermarkAnnotationTool(
        IWatermarkAnnotation watermark,
        IEnumerable<PageRange>? pages)
        : this(
            watermark ?? throw new ArgumentNullException(nameof(watermark)),
            pages?.SelectMany(t => t.Items)) { }

    public AddWatermarkAnnotationTool(
        IWatermarkAnnotation watermark,
        IEnumerable<int>? pages = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(watermark, nameof(watermark));

        Watermark = watermark;
        Pages = pages;
    }

    protected IWatermarkAnnotation Watermark { get; }
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

        foreach (int pageNum in pages)
        {
            PdfPage currPage = pdfDocument.GetPage(pageNum);
            Rectangle curPageSize = currPage.GetPageSize();

            PdfWatermarkAnnotation annotation = CreateWatermarkAnnotation(
                pdfDocument, curPageSize, Watermark);

            currPage.AddAnnotation(annotation);
        }
    }

    private static PdfWatermarkAnnotation CreateWatermarkAnnotation(
        PdfDocument pdfDocument,
        Rectangle pageSize,
        IWatermarkAnnotation watermark)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfDocument, nameof(pdfDocument));
        ExtendedArgumentNullException.ThrowIfNull(pageSize, nameof(pageSize));
        ExtendedArgumentNullException.ThrowIfNull(watermark, nameof(watermark));

        FloatPoint lowerLeftPoint = watermark.LowerLeftPoint
            ?? watermark.GetCenterredLowerLeftPoint(pageSize);

        Rectangle trimmingRectangle = watermark.GetTrimmingRectangle(
            lowerLeftPoint.X, lowerLeftPoint.Y);

        PdfFixedPrint fixedPrint = new();

        PdfWatermarkAnnotation annotation = new(trimmingRectangle);
        annotation.SetFixedPrint(fixedPrint);

        PdfFormXObject formObj = new(watermark.FormXObjRectangle);
        PdfCanvas canvas = new(formObj, pdfDocument);

        float[] matrix = new float[6];

        AffineTransform transform = watermark.Transform;
        transform.GetMatrix(matrix);

        canvas.SaveState()
            .BeginText()
            .SetColor(watermark.FontInfo.Color, true)
            .SetExtGState(watermark.ExtGState)
            .SetTextMatrix(matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5])
            .SetFontAndSize(watermark.FontInfo.CreatePdfFont(), watermark.FontInfo.Size)
            .ShowText(watermark.Text)
            .EndText()
            .RestoreState()
            .Release();

        PdfAnnotationAppearance annotationAppearance = new(formObj.GetPdfObject());

        annotation.SetAppearance(PdfName.N, annotationAppearance)
            .SetFlag(PdfAnnotation.READ_ONLY)
            .SetFlags(PdfAnnotation.PRINT);

        return annotation;
    }
}
