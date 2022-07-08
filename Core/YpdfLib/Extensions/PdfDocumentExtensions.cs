using iText.Kernel.Pdf;

namespace YpdfLib.Extensions
{
    internal static class PdfDocumentExtensions
    {
        public static void CopyTo(this PdfDocument source, PdfDocument dest)
        {
            int numOfPages = source.GetNumberOfPages();
            source.CopyPagesTo(1, numOfPages, dest);
        }
    }
}
