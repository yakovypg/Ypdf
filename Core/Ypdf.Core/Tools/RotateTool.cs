using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Pdf;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Tools;

public class RotateTool : ITool
{
    public RotateTool(int angleDegrees)
    {
        AngleDegrees = angleDegrees;
        Rotations = [];
    }

    public RotateTool(IEnumerable<PageRotation> rotations)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(rotations, nameof(rotations));
        Rotations = rotations;
    }

    protected int AngleDegrees { get; }
    protected IEnumerable<PageRotation> Rotations { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var sourceDocument = new PdfDocument(reader);
        using var outputDocument = new PdfDocument(writer);

        IEnumerable<PageRotation> rotations = Rotations;

        if (AngleDegrees != 0)
        {
            int numOfPages = sourceDocument.GetNumberOfPages();

            rotations = Enumerable.Range(1, numOfPages)
                .Select(t => new PageRotation(t, AngleDegrees));
        }

        foreach (PageRotation rotation in rotations)
        {
            _ = sourceDocument
                .GetPage(rotation.PageNumber)
                .SetRotation(-rotation.AngleDegrees);
        }

        sourceDocument.CopyTo(outputDocument);
    }
}
