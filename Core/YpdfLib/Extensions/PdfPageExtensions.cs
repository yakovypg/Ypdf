using iText.Kernel.Pdf;

namespace YpdfLib.Extensions
{
    internal static class PdfPageExtensions
    {
        public static PdfName[] GetImageReferences(this PdfPage page)
        {
            return GetImageReferences(page, out _);
        }

        public static PdfName[] GetImageReferences(this PdfPage page, out PdfDictionary? xObjects)
        {
            PdfDictionary? dictionary = page.GetPdfObject();
            PdfDictionary? resources = dictionary?.GetAsDictionary(PdfName.Resources);

            xObjects = resources?.GetAsDictionary(PdfName.XObject);

            return xObjects?.KeySet()?.ToArray() ?? Array.Empty<PdfName>();
        }
    }
}
