using iText.Kernel.Pdf;

namespace Ypdf.Core.Extensions;

public static class PdfDocumentExtensions
{
    public static void CopyTo(this PdfDocument source, PdfDocument destination)
    {
        ExtendedArgumentNullException.ThrowIfNull(source, nameof(source));
        ExtendedArgumentNullException.ThrowIfNull(destination, nameof(destination));

        int numOfPages = source.GetNumberOfPages();
        source.CopyPagesTo(1, numOfPages, destination);
    }
}
