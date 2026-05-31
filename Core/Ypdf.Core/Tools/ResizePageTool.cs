using System.Collections.Generic;
using iText.Kernel.Pdf;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Tools;

public class ResizePageTool : ITool
{
    public ResizePageTool(IEnumerable<PageResizing> pageResizings)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(pageResizings, nameof(pageResizings));
        PageResizings = pageResizings;
    }

    protected IEnumerable<PageResizing> PageResizings { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var sourceDocument = new PdfDocument(reader);
        using var outputDocument = new PdfDocument(writer);

        sourceDocument.CopyTo(outputDocument);

        foreach (PageResizing pageResizing in PageResizings)
        {
            PdfPage page = outputDocument.GetPage(pageResizing.PageNumber);
            PageResizer pageResizer = new(page);

            pageResizer.ResizePage(pageResizing.Width, pageResizing.Height);
        }
    }
}
