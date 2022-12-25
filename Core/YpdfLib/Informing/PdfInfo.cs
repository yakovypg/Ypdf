using FileSystemLib.Naming;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using YpdfLib.Designers;
using YpdfLib.Models.Enumeration;

namespace YpdfLib.Informing
{
    public static class PdfInfo
    {
        public static int GetPageCount(string inputFile)
        {
            int numOfPages;

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile)))
                numOfPages = pdfDoc.GetNumberOfPages();

            return numOfPages;
        }

        public static int[] GetAllPageNumbers(string inputFile)
        {
            int pageCount = GetPageCount(inputFile);
            return Enumerable.Range(1, pageCount).ToArray();
        }

        public static bool ContainsAssociatedPages(string inputFile)
        {
            return GetAssociatedPages(inputFile).Count > 0;
        }

        public static List<int[]> GetAssociatedPages(string inputFile)
        {
            List<int[]> result = new();

            string tempFilePath = new UniqueFile("pdf", SharedConfig.Directories.TEMP).GetNext();

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile), new PdfWriter(tempFilePath)))
            {
                int[] consideredPages = new PageRange(1, pdfDoc.GetNumberOfPages()).Items;

                while (true)
                {
                    int[] associatedPages = GetFirstAssociatedPages(pdfDoc, consideredPages);

                    if (associatedPages.Length == 0)
                        break;

                    result.Add(associatedPages);
                    consideredPages = consideredPages.Except(associatedPages).ToArray();
                }
            }

            File.Delete(tempFilePath);
            return result;
        }

        private static int[] GetFirstAssociatedPages(PdfDocument pdfDoc, int[] consideredPages)
        {
            if (consideredPages.Length < 2)
                return Array.Empty<int>();

            Rectangle[] pageSizes = new Rectangle[consideredPages.Length];

            for (int i = 0; i < consideredPages.Length; ++i)
                pageSizes[i] = pdfDoc.GetPage(consideredPages[i]).GetPageSize();

            for (int i = 0; i < consideredPages.Length; ++i)
            {
                int curPageNum = consideredPages[i];
                PdfPage curPage = pdfDoc.GetPage(curPageNum);

                var resizer = new PageResizer(curPage);
                resizer.IncreaseBottom(50);

                pageSizes[i] = curPage.GetPageSize();

                var associatedPages = new List<int>() { curPageNum };

                for (int j = 0; j < consideredPages.Length; ++j)
                {
                    int num = consideredPages[j];
                    Rectangle size = pdfDoc.GetPage(num).GetPageSize();

                    if (!pageSizes[j].EqualsWithEpsilon(size))
                        associatedPages.Add(num);
                }

                if (associatedPages.Count > 1)
                    return associatedPages.ToArray();
            }

            return Array.Empty<int>();
        }
    }
}
