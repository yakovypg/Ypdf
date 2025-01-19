using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Ypdf.Core.Extraction;

public class LocationBasedTextExtractor : ITextExtractor
{
    public LocationBasedTextExtractor() { }

    public string ExtractText(PdfPage page)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));

        var extractionStrategy = new LocationTextExtractionStrategy();
        var parser = new PdfCanvasProcessor(extractionStrategy);

        parser.ProcessPageContent(page);
        return extractionStrategy.GetResultantText();
    }
}
