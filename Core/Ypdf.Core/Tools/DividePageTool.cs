using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Pdf;
using Ypdf.Core.Design.Pages;

namespace Ypdf.Core.Tools;

public class DividePageTool : ITool
{
    public DividePageTool(IEnumerable<PageDivision> divisions)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(divisions, nameof(divisions));
        Divisions = divisions;
    }

    protected IEnumerable<PageDivision> Divisions { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var sourceDocument = new PdfDocument(reader);
        using var outputDocument = new PdfDocument(writer);

        int numOfPages = sourceDocument.GetNumberOfPages();

        for (int i = 1; i <= numOfPages; ++i)
        {
            PdfPage currPage = sourceDocument.GetPage(i);
            PageDivision division = Divisions.FirstOrDefault(t => t.PageNumber == i);

            if (division != default)
            {
                outputDocument.AddPage(currPage.CopyTo(outputDocument));
                continue;
            }

            outputDocument.AddPage(currPage.CopyTo(outputDocument));
            PdfPage firstHalf = outputDocument.GetLastPage();

            outputDocument.AddPage(currPage.CopyTo(outputDocument));
            PdfPage secondHalf = outputDocument.GetLastPage();

            PageCutter.CropRelativeToDivision(firstHalf, secondHalf, division);
        }
    }
}
