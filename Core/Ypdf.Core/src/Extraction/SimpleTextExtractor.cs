using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Ypdf.Core.Extraction;

public class SimpleTextExtractor : ITextExtractor
{
    public SimpleTextExtractor() { }

    public string ExtractText(PdfPage page)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));

        var extractionStrategy = new SimpleTextExtractionStrategy();
        return PdfTextExtractor.GetTextFromPage(page, extractionStrategy);
    }
}
