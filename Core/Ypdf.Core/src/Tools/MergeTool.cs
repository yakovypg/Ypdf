using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace Ypdf.Core.Tools;

public class MergeTool : IMultipleInputTool
{
    public MergeTool() { }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        var copyTool = new CopyTool();
        copyTool.Execute(inputPath, outputPath);
    }

    public void Execute(IEnumerable<string> inputPaths, string outputPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfContainsNotExistingFile(inputPaths, nameof(inputPaths));

        using var writer = new PdfWriter(outputPath);
        using var outputDocument = new PdfDocument(writer);

        var merger = new PdfMerger(outputDocument);

        foreach (string path in inputPaths)
        {
            using var reader = new PdfReader(path);
            using var currDoc = new PdfDocument(reader);

            int numOfPages = currDoc.GetNumberOfPages();
            merger.Merge(currDoc, 1, numOfPages);
        }
    }
}
