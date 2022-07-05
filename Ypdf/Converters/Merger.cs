using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace Ypdf.Converters
{
    public static class Merger
    {
        public static void Merge(string destPath, params string[] inputPaths)
        {
            if (inputPaths is null)
                throw new ArgumentNullException(nameof(inputPaths));

            var destDoc = new PdfDocument(new PdfWriter(destPath));
            var merger = new PdfMerger(destDoc);

            foreach (string path in inputPaths)
            {
                var currDoc = new PdfDocument(new PdfReader(path));
                merger.Merge(currDoc, 1, currDoc.GetNumberOfPages());

                currDoc.Close();
            }

            merger.Close();
            destDoc.Close();
        }
    }
}
