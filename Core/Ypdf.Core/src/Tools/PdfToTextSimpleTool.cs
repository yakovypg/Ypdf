using System.IO;
using System.Text;
using iText.Kernel.Pdf;
using Ypdf.Core.Extraction;

namespace Ypdf.Core.Tools;

public class PdfToTextSimpleTool : ITool
{
    public PdfToTextSimpleTool(ITextExtractor textExtractor)
    {
        ExtendedArgumentNullException.ThrowIfNull(textExtractor, nameof(textExtractor));
        TextExtractor = textExtractor;
    }

    protected ITextExtractor TextExtractor { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        var textBuilder = new StringBuilder();

        using var reader = new PdfReader(inputPath);
        using var pdfDocument = new PdfDocument(reader);

        int numOfPages = pdfDocument.GetNumberOfPages();

        for (int i = 1; i < numOfPages; ++i)
        {
            PdfPage currPage = pdfDocument.GetPage(i);
            string currPagetext = TextExtractor.ExtractText(currPage);
            textBuilder.AppendLine(currPagetext);
        }

        PdfPage lastPage = pdfDocument.GetPage(numOfPages);

        string lastPageText = TextExtractor.ExtractText(lastPage);
        textBuilder.Append(lastPageText);

        File.WriteAllText(outputPath, textBuilder.ToString());
    }
}
