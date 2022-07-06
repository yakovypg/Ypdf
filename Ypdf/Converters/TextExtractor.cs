using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;

namespace Ypdf.Converters
{
    public static class TextExtractor
    {
        public static void Extract(string destPath, string inputPath)
        {
            var textBuilder = new StringBuilder();
            var pdfDoc = new PdfDocument(new PdfReader(inputPath));

            int docPages = pdfDoc.GetNumberOfPages();

            for (int i = 1; i <= docPages; ++i)
            {
                var page = pdfDoc.GetPage(i);
                var strategy = new SimpleTextExtractionStrategy();

                string text = PdfTextExtractor.GetTextFromPage(page, strategy);

                if (i < docPages)
                    textBuilder.AppendLine(text);
                else
                    textBuilder.Append(text);
            }

            pdfDoc.Close();
            File.WriteAllText(destPath, textBuilder.ToString());
        }
    }
}
