using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace YpdfLib.Converters
{
    public class Splitter : PdfSplitter
    {
        private const int DEFAULT_SPLITTING_PART_SIZE = 3 * 1024 * 1024;

        public string DestinationDirectory { get; set; }

        public Splitter(string inputFile, string destDir) : this(new PdfDocument(new PdfReader(inputFile)), destDir)
        {
        }

        public Splitter(PdfDocument pdfDocument, string destDir) : base(pdfDocument)
        {
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            DestinationDirectory = destDir;
        }

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            IList<int> pages = documentPageRange.GetQualifyingPageNums(int.MaxValue);

            string name = pages.Count > 0
                ? pages[0].ToString()
                : "0";

            if (pages.Count > 1)
            {
                int lastPage = pages[^1];
                name += $"-{lastPage}";
            }

            string pathWithoutExtension = Path.Combine(DestinationDirectory, $"splitted_{name}");
            string uniquePath = pathWithoutExtension + ".pdf";

            int num = 0;

            while (File.Exists(uniquePath))
                uniquePath = $"{pathWithoutExtension} ({++num}).pdf";

            return new PdfWriter(uniquePath);
        }

        public void Split(params string[] ranges)
        {
            if (ranges is null)
                throw new ArgumentNullException(nameof(ranges));

            if (ranges.Length == 0)
                return;

            PageRange[] pageRanges = ranges.Select(t => new PageRange(t)).ToArray();
            IList<PdfDocument> docs = ExtractPageRanges(pageRanges);

            foreach (var doc in docs)
                doc.Close();
        }

        public void Split()
        {
            IList<PdfDocument> docs = SplitBySize(DEFAULT_SPLITTING_PART_SIZE);

            foreach (var doc in docs)
                doc.Close();
        }
    }
}
