using System.Collections.Generic;
using iText.Kernel.Pdf;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Tools;

public class MovePageTool : ITool
{
    public MovePageTool(PageOrder pageOrder)
        : this(pageOrder.Pages) { }

    public MovePageTool(int pageToMove, int newPosition)
    {
        DefaultExceptions.ThrowIfLessThan(pageToMove, 1, nameof(pageToMove));
        DefaultExceptions.ThrowIfLessThan(newPosition, 1, nameof(newPosition));

        MoveInfo = (pageToMove, newPosition);
    }

    protected MovePageTool(IEnumerable<int> pageOrder)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(pageOrder, nameof(pageOrder));
        DefaultExceptions.ThrowIfContainsNotAllowedItem(pageOrder, t => t >= 1, nameof(pageOrder));
        DefaultExceptions.ThrowIfContainsNotUniqueItems(pageOrder, nameof(pageOrder));

        PageOrder = pageOrder;
    }

    protected (int PageToMove, int NewPosition)? MoveInfo { get; }
    protected IEnumerable<int>? PageOrder { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var sourceDocument = new PdfDocument(reader);
        using var outputDocument = new PdfDocument(writer);

        if (MoveInfo is not null)
            MovePage(sourceDocument, outputDocument);
        else if (PageOrder is not null)
            ChangePageOrder(sourceDocument, outputDocument);
        else
            throw new InternalException($"{nameof(MoveInfo)} and {nameof(PageOrder)} are null.");
    }

    private void MovePage(PdfDocument sourceDocument, PdfDocument outputDocument)
    {
        ExtendedArgumentNullException.ThrowIfNull(sourceDocument, nameof(sourceDocument));
        ExtendedArgumentNullException.ThrowIfNull(outputDocument, nameof(outputDocument));

        if (MoveInfo is null)
            throw new InternalException($"{nameof(MoveInfo)} is null.");

        sourceDocument.MovePage(MoveInfo.Value.PageToMove, MoveInfo.Value.NewPosition);
        sourceDocument.CopyTo(outputDocument);
    }

    private void ChangePageOrder(PdfDocument sourceDocument, PdfDocument outputDocument)
    {
        ExtendedArgumentNullException.ThrowIfNull(sourceDocument, nameof(sourceDocument));
        ExtendedArgumentNullException.ThrowIfNull(outputDocument, nameof(outputDocument));

        if (PageOrder is null)
            throw new InternalException($"{nameof(PageOrder)} is null.");

        int numOfPages = sourceDocument.GetNumberOfPages();
        var pageOrder = new List<int>(PageOrder);

        if (pageOrder.Count != numOfPages)
            throw new PageNumbersNotMatchException(null, numOfPages, pageOrder.Count);

        PageMovement[] movements = new PageMovement[numOfPages];

        for (int i = 0; i < numOfPages; ++i)
        {
            int pagePosition = i + 1;
            int pageIndex = pageOrder[i];
            PdfPage page = sourceDocument.GetPage(pageIndex);

            movements[i] = new PageMovement(page, pagePosition);
        }

        foreach (PageMovement movement in movements)
        {
            sourceDocument.MovePage(movement.Page, movement.Position);
        }

        sourceDocument.CopyPagesTo(1, numOfPages, outputDocument);
    }
}
