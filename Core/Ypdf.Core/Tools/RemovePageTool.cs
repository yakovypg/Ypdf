using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Pdf;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Tools;

public class RemovePageTool : ITool
{
    public RemovePageTool(IEnumerable<PageRange> pageRanges)
        : this(PageRange.GetAllItems(pageRanges)) { }

    public RemovePageTool(IEnumerable<int> pages)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(pages, nameof(pages));
        Pages = pages;
    }

    protected IEnumerable<int> Pages { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var sourceDocument = new PdfDocument(reader);
        using var outputDocument = new PdfDocument(writer);

        IEnumerable<PdfPage> pdfPages = Pages
            .OrderByDescending(t => t)
            .Select(t => sourceDocument.GetPage(t));

        foreach (PdfPage page in pdfPages)
        {
            sourceDocument.RemovePage(page);
        }

        sourceDocument.CopyTo(outputDocument);
    }
}
