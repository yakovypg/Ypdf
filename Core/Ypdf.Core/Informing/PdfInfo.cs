using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Enumeration;
using Ypdf.Core.FileSystem.Naming;
using Ypdf.Paths;

namespace Ypdf.Core.Informing;

public static class PdfInfo
{
    public static int GetNumberOfPages(string inputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var pdfDocument = new PdfDocument(reader);

        return pdfDocument.GetNumberOfPages();
    }

    public static IList<int> GetAllPageNumbers(string inputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        int numOfPages = GetNumberOfPages(inputPath);

        return Enumerable.Range(1, numOfPages)
            .ToList();
    }

    public static Rectangle[] GetPageSizes(string inputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var pdfDocument = new PdfDocument(reader);

        int numOfPages = pdfDocument.GetNumberOfPages();
        Rectangle[] pageSizes = new Rectangle[numOfPages];

        for (int i = 1; i <= numOfPages; ++i)
        {
            PdfPage curPage = pdfDocument.GetPage(i);
            pageSizes[i - 1] = curPage.GetPageSize();
        }

        return pageSizes;
    }

    public static bool ContainsAssociatedPages(string inputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        return GetAssociatedPages(inputPath).Count > 0;
    }

    public static IList<IList<int>> GetAssociatedPages(string inputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        var result = new List<IList<int>>();

        var uniqueFile = new UniqueFile("pdf", Directories.Temp);
        string tempFilePath = uniqueFile.GetNext();

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(tempFilePath);
        using var pdfDocument = new PdfDocument(reader, writer);

        int numOfPages = pdfDocument.GetNumberOfPages();

        IReadOnlyCollection<int> pagesInRange = new PageRange(1, numOfPages).Items;
        var consideredPages = new List<int>(pagesInRange);

        while (true)
        {
            IList<int> associatedPages = GetFirstAssociatedPages(pdfDocument, consideredPages);

            if (associatedPages.Count == 0)
                break;

            result.Add(associatedPages);

            consideredPages = consideredPages
                .Except(associatedPages)
                .ToList();
        }

        File.Delete(tempFilePath);
        return result;
    }

    private static IList<int> GetFirstAssociatedPages(
        PdfDocument pdfDocument,
        List<int> consideredPages)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfDocument, nameof(pdfDocument));
        ExtendedArgumentNullException.ThrowIfNull(consideredPages, nameof(consideredPages));

        if (consideredPages.Count < 2)
            return new List<int>();

        Rectangle[] pageSizes = new Rectangle[consideredPages.Count];

        for (int i = 0; i < consideredPages.Count; ++i)
        {
            pageSizes[i] = pdfDocument
                .GetPage(consideredPages[i])
                .GetPageSize();
        }

        for (int i = 0; i < consideredPages.Count; ++i)
        {
            int currPageNum = consideredPages[i];
            PdfPage currPage = pdfDocument.GetPage(currPageNum);

            var resizer = new PageResizer(currPage);
            resizer.IncreaseBottom(50);

            pageSizes[i] = currPage.GetPageSize();

            var associatedPages = new List<int>() { currPageNum };

            for (int j = 0; j < consideredPages.Count; ++j)
            {
                int num = consideredPages[j];

                Rectangle size = pdfDocument
                    .GetPage(num)
                    .GetPageSize();

                if (!pageSizes[j].EqualsWithEpsilon(size))
                    associatedPages.Add(num);
            }

            if (associatedPages.Count > 1)
                return associatedPages.ToArray();
        }

        return new List<int>();
    }
}
