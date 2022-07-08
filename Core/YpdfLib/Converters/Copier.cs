using iText.Kernel.Pdf;
using YpdfLib.Extensions;

namespace YpdfLib.Converters
{
    public static class Copier
    {
        public static void Copy(string inputFile, string destPath)
        {
            using (var inputDoc = new PdfDocument(new PdfReader(inputFile)))
            using (var destDoc = new PdfDocument(new PdfWriter(destPath)))
            {
                inputDoc.CopyTo(destDoc);
            }
        }
    }
}
