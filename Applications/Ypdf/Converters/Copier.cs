using iText.Kernel.Pdf;

namespace Ypdf.Converters
{
    public static class Copier
    {
        public static void Copy(string destPath, string inputPath)
        {
            var inputDoc = new PdfDocument(new PdfReader(inputPath));
            var destDoc = new PdfDocument(new PdfWriter(destPath));

            int pageNum = inputDoc.GetNumberOfPages();
            inputDoc.CopyPagesTo(1, pageNum, destDoc);

            inputDoc.Close();
            destDoc.Close();
        }
    }
}
