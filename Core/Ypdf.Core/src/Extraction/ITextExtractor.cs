using iText.Kernel.Pdf;

namespace Ypdf.Core.Extraction;

public interface ITextExtractor
{
    string ExtractText(PdfPage page);
}
