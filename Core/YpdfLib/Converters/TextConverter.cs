using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using YpdfLib.Models.Paging;

namespace YpdfLib.Converters
{
    public static class TextConverter
    {
        public static void ConvertToPdf(string inputFile, string destPath)
        {
            ConvertToPdf(inputFile, destPath, new TextPageParameters());
        }

        public static void ConvertToPdf(string inputFile, string destPath, ITextPageParameters pageParams)
        {
            var pdfDoc = new PdfDocument(new PdfWriter(destPath));
            var doc = new Document(pdfDoc);

            if (pageParams.PageSize is not null)
                pdfDoc.SetDefaultPageSize(pageParams.PageSize);

            doc.SetMargins(
                pageParams.Margin.Top,
                pageParams.Margin.Right,
                pageParams.Margin.Bottom,
                pageParams.Margin.Left
            );

            var paragraph = new Paragraph();
            paragraph.SetTextAlignment(pageParams.TextAlignment);
            paragraph.SetFontSize(pageParams.FontInfo.Size);
            paragraph.SetFontColor(pageParams.FontInfo.Color);
            paragraph.SetOpacity(pageParams.FontInfo.Opacity);
            paragraph.SetFont(pageParams.FontInfo.GetPdfFont());

            using (var reader = new StreamReader(inputFile))
            {
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine() ?? string.Empty;
                    line = line.Replace("\u0020", "\u00A0");

                    paragraph.Add(line + '\n');
                }
            }

            doc.Add(paragraph);
            doc.Close();
        }
    }
}
