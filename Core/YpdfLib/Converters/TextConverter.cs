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

            var doc = new Document(pdfDoc)
                .SetTextAlignment(pageParams.TextAlignment)
                .SetFontSize(pageParams.FontSize);

            doc.SetMargins(
                pageParams.Margin.Top,
                pageParams.Margin.Right,
                pageParams.Margin.Bottom,
                pageParams.Margin.Left
            );

            var paragraph = new Paragraph().SetFont(pageParams.Font.PdfFont);

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
