using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Ypdf.Converters.Configuration;

namespace Ypdf.Converters
{
    public static class PageDesigner
    {
        public static void AddPageNumbers(string sourceFile, string destPath, PageNumberStyle style)
        {
            var reader = new PdfReader(sourceFile);
            var writer = new PdfWriter(destPath);

            var pdfDoc = new PdfDocument(reader, writer);
            var doc = new Document(pdfDoc);

            ApplyPageNumStyleMargins(doc, style);

            var docMargin = new Margin(
                doc.GetTopMargin(),
                doc.GetRightMargin(),
                doc.GetBottomMargin(),
                doc.GetLeftMargin()
            );

            int numOfPages = pdfDoc.GetNumberOfPages();

            for (int i = 1; i <= numOfPages; i++)
            {
                PdfPage curPage = pdfDoc.GetPage(i);
                Rectangle curPageSize = curPage.GetPageSize();

                string presenterText = style.TextPresenter.GetText(i, numOfPages);
                Text numText = style.GetStylizedText(presenterText);

                var paragraph = CreatePageNumParagraph(curPageSize, docMargin, numText, style);
                doc.Add(paragraph);
            }

            doc.Close();
            pdfDoc.Close();
        }

        private static Paragraph CreatePageNumParagraph(Rectangle pageSize, Margin docMargin, Text numText, PageNumberStyle numStyle)
        {
            float pageWidth = pageSize.GetWidth();
            float pageHeight = pageSize.GetHeight();

            float width = pageWidth - docMargin.Left - docMargin.Right;
            float height = pageHeight - docMargin.Top - docMargin.Bottom;

            float tabPosition = numStyle.HorizontalAlignment switch
            {
                TabAlignment.LEFT => 0,
                TabAlignment.RIGHT => width,
                _ => width / 2,
            };

            var tabStop = new TabStop(tabPosition, numStyle.HorizontalAlignment);

            var output = new Paragraph()
                .AddTabStops(tabStop)
                .Add(new Tab())
                .Add(numText)
                .SetHeight(height)
                .SetMarginLeft(numStyle.Margin.Left)
                .SetMarginTop(numStyle.Margin.Top)
                .SetMarginRight(numStyle.Margin.Right)
                .SetMarginBottom(numStyle.Margin.Bottom)
                .SetVerticalAlignment(numStyle.VerticalAlignment);

            return output;
        }

        private static void ApplyPageNumStyleMargins(Document document, PageNumberStyle style)
        {
            if (!style.ConsiderLeftPageMargin)
                document.SetLeftMargin(0);

            if (!style.ConsiderTopPageMargin)
                document.SetTopMargin(0);

            if (!style.ConsiderRightPageMargin)
                document.SetRightMargin(0);

            if (!style.ConsiderBottomPageMargin)
                document.SetBottomMargin(0);
        }
    }
}
