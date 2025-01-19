using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Ypdf.Core.Design;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Tools;

public class AddPageNumbersTool : ITool
{
    public AddPageNumbersTool(
        PageNumberStyle pageNumberStyle,
        IEnumerable<PageContentShift>? pageNumberShifts = null)
    {
        PageNumberStyle = pageNumberStyle;
        PageNumberShifts = pageNumberShifts;
    }

    protected PageNumberStyle PageNumberStyle { get; }
    protected IEnumerable<PageContentShift>? PageNumberShifts { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(reader, writer);
        using var outputDocument = new Document(pdfDocument);

        ApplyPageNumberStyleMargins(outputDocument, PageNumberStyle);

        Margin documentMargin = outputDocument.GetMargin();
        int numOfPages = pdfDocument.GetNumberOfPages();

        for (int i = 1; i <= numOfPages; ++i)
        {
            PdfPage currPage = pdfDocument.GetPage(i);
            Rectangle currPageSize = currPage.GetPageSize();

            string presenterText = PageNumberStyle.TextPresenter.GetText(i, numOfPages);
            Text pageNumberText = PageNumberStyle.GetStylizedText(presenterText);

            Paragraph pageNumberParagraph = CreatePageNumberParagraph(
                currPageSize,
                documentMargin,
                pageNumberText,
                PageNumberStyle);

            PageContentShift? pageNumberShift = PageNumberShifts?
                .FirstOrDefault(t => t.PageNumber == i);

            if (pageNumberShift is not null)
                ApplyPageNumberShift(pageNumberParagraph, pageNumberShift.Value);

            outputDocument.Add(pageNumberParagraph);
        }
    }

    private static Paragraph CreatePageNumberParagraph(
        Rectangle pageSize,
        Margin documentMargin,
        Text pageNumberText,
        PageNumberStyle pageNumberStyle)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageSize, nameof(pageSize));
        ExtendedArgumentNullException.ThrowIfNull(pageNumberText, nameof(pageNumberText));
        ExtendedArgumentNullException.ThrowIfNull(pageNumberStyle, nameof(pageNumberStyle));

        float pageWidth = pageSize.GetWidth();
        float pageHeight = pageSize.GetHeight();

        float width = pageWidth - documentMargin.Left - documentMargin.Right -
            pageNumberStyle.Margin.Left - pageNumberStyle.Margin.Right;

        float height = pageHeight - documentMargin.Top - documentMargin.Bottom -
            pageNumberStyle.Margin.Top - pageNumberStyle.Margin.Bottom;

        float tabPosition = pageNumberStyle.HorizontalAlignment switch
        {
            TabAlignment.RIGHT => width,
            TabAlignment.CENTER => width / 2,
            _ => 0
        };

        const int decreaseRatio = 1;
        const int heightReduction = decreaseRatio * 2;

        var tab = new Tab();
        var tabStop = new TabStop(tabPosition, pageNumberStyle.HorizontalAlignment);

        return new Paragraph()
            .AddTabStops(tabStop)
            .Add(tab)
            .Add(pageNumberText)
            .SetHeight(height - heightReduction)
            .SetMarginLeft(pageNumberStyle.Margin.Left)
            .SetMarginTop(pageNumberStyle.Margin.Top)
            .SetMarginRight(pageNumberStyle.Margin.Right)
            .SetMarginBottom(pageNumberStyle.Margin.Bottom)
            .SetVerticalAlignment(pageNumberStyle.VerticalAlignment);
    }

    private static void ApplyPageNumberStyleMargins(Document document, PageNumberStyle style)
    {
        ExtendedArgumentNullException.ThrowIfNull(document, nameof(document));

        if (!style.ConsiderLeftPageMargin)
            document.SetLeftMargin(0);

        if (!style.ConsiderTopPageMargin)
            document.SetTopMargin(0);

        if (!style.ConsiderRightPageMargin)
            document.SetRightMargin(0);

        if (!style.ConsiderBottomPageMargin)
            document.SetBottomMargin(0);
    }

    private static void ApplyPageNumberShift(
        Paragraph pageNumberParagraph,
        PageContentShift pageNumberShift)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageNumberParagraph, nameof(pageNumberParagraph));

        float currMarginLeft = pageNumberParagraph
            .GetMarginLeft()
            .GetValue();

        float currMarginTop = pageNumberParagraph
            .GetMarginTop()
            .GetValue();

        float leftMargin = currMarginLeft + pageNumberShift.Horizontal;
        float topMargin = currMarginTop + pageNumberShift.Vertical;

        pageNumberParagraph.SetMarginLeft(leftMargin);
        pageNumberParagraph.SetMarginTop(topMargin);
    }
}
