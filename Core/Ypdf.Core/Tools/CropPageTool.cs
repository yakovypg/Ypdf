using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Pdf;
using Ypdf.Core.Design.Pages;

namespace Ypdf.Core.Tools;

public class CropPageTool : ITool
{
    public CropPageTool(IEnumerable<PageCropping> croppings)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(croppings, nameof(croppings));
        Croppings = croppings;
    }

    protected IEnumerable<PageCropping> Croppings { get; }

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
            PageCropping cropping = Croppings.FirstOrDefault(t => t.PageNumber == i);

            if (cropping == default)
            {
                outputDocument.AddPage(currPage.CopyTo(outputDocument));
                continue;
            }

            outputDocument.AddPage(currPage.CopyTo(outputDocument));
            PdfPage pageToCrop = outputDocument.GetLastPage();

            PageCutter.CropBox(pageToCrop, cropping.Box);
        }
    }
}
