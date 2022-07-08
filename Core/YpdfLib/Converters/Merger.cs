using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace YpdfLib.Converters
{
    public static class Merger
    {
        public static void Merge(string destPath, params string[] inputFiles)
        {
            if (inputFiles is null)
                throw new ArgumentNullException(nameof(inputFiles));

            if (inputFiles.Length < 2)
                throw new ArgumentException("At least two files are needed to merge.", nameof(inputFiles));

            var destDoc = new PdfDocument(new PdfWriter(destPath));
            var merger = new PdfMerger(destDoc);

            foreach (string path in inputFiles)
            {
                using (var currDoc = new PdfDocument(new PdfReader(path)))
                    merger.Merge(currDoc, 1, currDoc.GetNumberOfPages());
            }

            merger.Close();
            destDoc.Close();
        }
    }
}
