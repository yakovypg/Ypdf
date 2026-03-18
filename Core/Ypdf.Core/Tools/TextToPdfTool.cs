using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Ypdf.Core.Design.Pages;
using Ypdf.Extensions;

namespace Ypdf.Core.Tools;

public class TextToPdfTool : ITool
{
    public TextToPdfTool()
        : this(new TextPageParameters()) { }

    public TextToPdfTool(ITextPageParameters pageParameters)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageParameters, nameof(pageParameters));
        PageParameters = pageParameters;
    }

    protected ITextPageParameters PageParameters { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new StreamReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(writer);
        using var outputDocument = new Document(pdfDocument);

        ApplyStyleToDocument(outputDocument);

        Paragraph paragraph = CreateParagraph();

        while (!reader.EndOfStream)
        {
            string? currLine = reader.ReadLine();
            string? correctedLine = CorrectTextLine(currLine);

            paragraph.Add($"{correctedLine}{Environment.NewLine}");
        }

        outputDocument.Add(paragraph);
    }

    private static string CorrectTextLine(string? line)
    {
        return line?.Replace("\u0020", "\u00A0", StringComparison.CurrentCulture)
            ?? string.Empty;
    }

    private void ApplyStyleToDocument(Document document)
    {
        ExtendedArgumentNullException.ThrowIfNull(document, nameof(document));

        if (PageParameters.Size is not null)
        {
            PdfDocument pdfDocument = document.GetPdfDocument();
            pdfDocument.SetDefaultPageSize(PageParameters.Size);
        }

        if (PageParameters.Margin is not null)
        {
            document.SetMargins(
                PageParameters.Margin.Value.Top,
                PageParameters.Margin.Value.Right,
                PageParameters.Margin.Value.Bottom,
                PageParameters.Margin.Value.Left);
        }
    }

    private Paragraph CreateParagraph()
    {
        return new Paragraph()
            .SetTextAlignment(PageParameters.HorizontalAlignment)
            .SetFontSize(PageParameters.FontInfo.Size)
            .SetFontColor(PageParameters.FontInfo.Color)
            .SetOpacity(PageParameters.FontInfo.Opacity)
            .SetFont(PageParameters.FontInfo.CreatePdfFont());
    }
}
