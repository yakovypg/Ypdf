using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using YpdfLib.Extensions;
using YpdfLib.Models.Design;
using YpdfLib.Models.Paging;

namespace YpdfLib.Designers
{
    public static class PageDesigner
    {
        public static void AddPageNumbers(string inputFile, string destPath, IPageNumberStyle style, IPageContentShift[]? numberShifts = null)
        {
            var reader = new PdfReader(inputFile);
            var writer = new PdfWriter(destPath);

            var pdfDoc = new PdfDocument(reader, writer);
            var doc = new Document(pdfDoc);

            ApplyPageNumStyleMargins(doc, style);

            IMargin docMargin = doc.GetMargin();
            int numOfPages = pdfDoc.GetNumberOfPages();

            for (int i = 1; i <= numOfPages; ++i)
            {
                PdfPage curPage = pdfDoc.GetPage(i);
                Rectangle curPageSize = curPage.GetPageSize();

                string presenterText = style.TextPresenter.GetText(i, numOfPages);
                Text numText = style.GetStylizedText(presenterText);

                Paragraph paragraph = CreatePageNumParagraph(curPageSize, docMargin, numText, style);
                IPageContentShift? numberShift = numberShifts?.FirstOrDefault(t => t.PageNumber == i);

                if (numberShift is not null)
                {
                    float curMarginLeft = paragraph.GetMarginLeft().GetValue();
                    float curMarginTop = paragraph.GetMarginTop().GetValue();

                    paragraph.SetMarginLeft(curMarginLeft + numberShift.Horizontal);
                    paragraph.SetMarginTop(curMarginTop + numberShift.Vertical);
                }

                doc.Add(paragraph);
            }

            doc.Close();
        }

        private static Paragraph CreatePageNumParagraph(Rectangle pageSize, IMargin docMargin, Text numText, IPageNumberStyle numStyle)
        {
            float pageWidth = pageSize.GetWidth();
            float pageHeight = pageSize.GetHeight();

            float width = pageWidth - docMargin.Left - docMargin.Right -
                numStyle.Margin.Left - numStyle.Margin.Right;

            float height = pageHeight - docMargin.Top - docMargin.Bottom -
                numStyle.Margin.Top - numStyle.Margin.Bottom;

            float tabPosition = numStyle.HorizontalAlignment switch
            {
                TabAlignment.LEFT => 0,
                TabAlignment.RIGHT => width,
                TabAlignment.CENTER => width / 2,
                _ => 0
            };

            const int DECREASE_RATIO = 1;
            const int HEIGHT_REDUCTION = DECREASE_RATIO * 2;

            var tabStop = new TabStop(tabPosition, numStyle.HorizontalAlignment);

            var output = new Paragraph()
                .AddTabStops(tabStop)
                .Add(new Tab())
                .Add(numText)
                .SetHeight(height - HEIGHT_REDUCTION)
                .SetMarginLeft(numStyle.Margin.Left)
                .SetMarginTop(numStyle.Margin.Top)
                .SetMarginRight(numStyle.Margin.Right)
                .SetMarginBottom(numStyle.Margin.Bottom)
                .SetVerticalAlignment(numStyle.VerticalAlignment);

            return output;
        }

        private static void ApplyPageNumStyleMargins(Document document, IPageNumberStyle style)
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
