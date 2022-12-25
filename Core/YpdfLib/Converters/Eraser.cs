using iText.Kernel.Pdf;
using YpdfLib.Extensions;
using YpdfLib.Models.Enumeration;

namespace YpdfLib.Converters
{
    public static class Eraser
    {
        public static void RemovePages(string inputFile, string destPath, params int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            if (pages.Length == 0)
            {
                Copier.Copy(inputFile, destPath);
                return;
            }

            using (var inputDoc = new PdfDocument(new PdfReader(inputFile)))
            using (var destDoc = new PdfDocument(new PdfWriter(destPath)))
            {
                PdfPage[] pdfPages = pages
                    .Select(t => inputDoc.GetPage(t))
                    .ToArray();

                foreach (var page in pdfPages)
                    inputDoc.RemovePage(page);

                inputDoc.CopyTo(destDoc);
            }
        }

        public static void RemovePages(string inputFile, string destPath, params IPageRange[] ranges)
        {
            int[] pages = PageRange.GetAllItems(ranges);
            RemovePages(inputFile, destPath, pages);
        }
    }
}
