using iText.Kernel.Pdf;
using YpdfLib.Extensions;
using YpdfLib.Models.Enumeration;

namespace YpdfLib.Converters
{
    public static class Eraser
    {
        public static void RemovePages(string inputFile, string destPath, params int[] pages)
        {
            if (pages is null || pages.Length == 0)
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

        public static void RemovePages(string inputFile, string destPath, params IRange[] ranges)
        {
            var pages = new List<int>();

            foreach (var range in ranges ?? Array.Empty<IRange>())
                pages.AddRange(range.Items);

            RemovePages(inputFile, destPath, pages.ToArray());
        }
    }
}
