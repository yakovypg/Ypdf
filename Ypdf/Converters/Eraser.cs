using iText.Kernel.Pdf;

namespace Ypdf.Converters
{
    public static class Eraser
    {
        public static void RemovePages(string destPath, string inputPath, params int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            var inputDoc = new PdfDocument(new PdfReader(inputPath));
            var destDoc = new PdfDocument(new PdfWriter(destPath));

            PdfPage[] pdfPages = pages
                .Select(t => inputDoc.GetPage(t))
                .ToArray();

            foreach (var page in pdfPages)
                inputDoc.RemovePage(page);

            int pageNum = inputDoc.GetNumberOfPages();
            inputDoc.CopyPagesTo(1, pageNum, destDoc);

            inputDoc.Close();
            destDoc.Close();
        }

        public static void RemovePages(string destPath, string inputPath, params Configuration.Range[] ranges)
        {
            if (ranges is null)
                throw new ArgumentNullException(nameof(ranges));

            var pages = new List<int>();

            foreach (var range in ranges)
                pages.AddRange(range.Items);

            RemovePages(destPath, inputPath, pages.ToArray());
        }
    }
}
