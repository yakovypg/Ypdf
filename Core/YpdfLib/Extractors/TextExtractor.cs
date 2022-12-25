using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using RuntimeLib.Python;
using System.Text;

namespace YpdfLib.Extractors
{
    public static class TextExtractor
    {
        public static void Extract(string inputFile, string destPath)
        {
            var textBuilder = new StringBuilder();

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile)))
            {
                int numOfPages = pdfDoc.GetNumberOfPages();

                for (int i = 1; i < numOfPages; ++i)
                {
                    PdfPage curPage = pdfDoc.GetPage(i);
                    string text = ExtractTextFromPage(curPage);

                    textBuilder.AppendLine(text);
                }

                PdfPage lastPage = pdfDoc.GetPage(numOfPages);
                textBuilder.Append(ExtractTextFromPage(lastPage));
            }

            File.WriteAllText(destPath, textBuilder.ToString());
        }

        public static void ExtractByPython(string inputFile, string destPath, string? pythonAlias = null)
        {
            string pythonTextExtractorPath = SharedConfig.Scripts.PythonTextExtractor;

            var executor = new PythonExecutor(true, true, Console.Out)
            {
                RequirePython3 = true
            };

            if (!string.IsNullOrEmpty(pythonAlias))
                executor.PythonAlias = pythonAlias;

            executor.Execute($"{pythonTextExtractorPath} -i {inputFile} -o {destPath}");
        }

        private static string ExtractTextFromPage(PdfPage page)
        {
            var strategy = new SimpleTextExtractionStrategy();
            string text = PdfTextExtractor.GetTextFromPage(page, strategy);

            return text;
        }
    }
}
