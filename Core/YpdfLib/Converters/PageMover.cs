using iText.Kernel.Pdf;
using YpdfLib.Extensions;

namespace YpdfLib.Converters
{
    public static class PageMover
    {
        public static void MovePage(string inputFile, string destPath, int pageToMove, int newPosition)
        {
            using (var inputDoc = new PdfDocument(new PdfReader(inputFile)))
            using (var destDoc = new PdfDocument(new PdfWriter(destPath)))
            {
                inputDoc.MovePage(pageToMove, newPosition);
                inputDoc.CopyTo(destDoc);
            }
        }

        public static void ReorderPages(string inputPath, string destPath, int[] newOrder)
        {
            if (newOrder is null)
                throw new ArgumentNullException(nameof(newOrder));

            var inputDoc = new PdfDocument(new PdfReader(inputPath));
            var destDoc = new PdfDocument(new PdfWriter(destPath));

            int pageNum = inputDoc.GetNumberOfPages();

            if (newOrder.Length != pageNum)
                throw new ArgumentException("The number of pages in the new order does not match the number of pages in the document.", nameof(newOrder));

            var movements = new Tuple<PdfPage, int>[pageNum];

            for (int i = 0; i < pageNum; ++i)
            {
                int pagePosition = i + 1;
                int pageIndex = newOrder[i];
                PdfPage page = inputDoc.GetPage(pageIndex);

                movements[i] = Tuple.Create(page, pagePosition);
            }

            foreach (var movement in movements)
                inputDoc.MovePage(movement.Item1, movement.Item2);

            inputDoc.CopyPagesTo(1, pageNum, destDoc);

            inputDoc.Close();
            destDoc.Close();
        }
    }
}
