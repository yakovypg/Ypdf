using System.Collections.Generic;
using iText.Kernel.Pdf;
using Ypdf.Core.Informing;

namespace Ypdf.Core.Tools;

public class RemoveWatermarkAnnotationTool : ITool
{
    public RemoveWatermarkAnnotationTool(IEnumerable<int>? pages = null)
    {
        Pages = pages;
    }

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

        foreach (int pageNumber in pages)
        {
            PdfPage currPage = pdfDocument.GetPage(pageNumber);
            PdfDictionary currPageDictionary = currPage.GetPdfObject();
            PdfArray? annotators = currPageDictionary.GetAsArray(PdfName.Annots);

            if (annotators is null)
                continue;

            for (int i = 0; i < annotators.Size(); ++i)
            {
                PdfDictionary annotation = annotators.GetAsDictionary(i);
                PdfName annotationName = annotation.GetAsName(PdfName.Subtype);

                if (PdfName.Watermark.Equals(annotationName))
                    annotation.Clear();
            }
        }
    }
}
