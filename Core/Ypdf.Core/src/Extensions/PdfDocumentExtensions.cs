using iText.Kernel.Pdf;

namespace Ypdf.Core.Extensions;

internal static class PdfDocumentExtensions
{
    public static void CopyTo(this PdfDocument source, PdfDocument destination)
    {
        int numOfPages = source.GetNumberOfPages();
        source.CopyPagesTo(1, numOfPages, destination);
    }
}
