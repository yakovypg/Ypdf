using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace YpdfLib.Converters
{
    public class Splitter : PdfSplitter
    {
        public const long DEFAULT_SPLITTING_PART_SIZE = 3 * 1024 * 1024;

        private readonly string? _inputFilePath;
        private readonly string? _inputFileName;

        public string DestinationDirectory { get; }

        public Splitter(string inputFile, string destDir) : this(new PdfDocument(new PdfReader(inputFile)), destDir)
        {
            _inputFilePath = inputFile;
            _inputFileName = GetInputFileName(_inputFilePath);
        }

        public Splitter(PdfDocument pdfDocument, string destDir) : base(pdfDocument)
        {
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

            string pathWithoutExtension = Path.Combine(DestinationDirectory, $"{_inputFileName}_{name}");
            string uniquePath = pathWithoutExtension + ".pdf";

            int num = 0;

            while (File.Exists(uniquePath))
                uniquePath = $"{pathWithoutExtension} ({++num}).pdf";

            return new PdfWriter(uniquePath);
        }

        public void Split(string[] ranges)
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

        public void Split(long splittingPartSize = DEFAULT_SPLITTING_PART_SIZE)
        {
            IList<PdfDocument> docs = SplitBySize(splittingPartSize);

            foreach (var doc in docs)
                doc.Close();
        }

        private static string GetInputFileName(string inputFilePath)
        {
            try
            {
                return inputFilePath.Contains('.')
                    ? Path.GetFileNameWithoutExtension(inputFilePath)
                    : Path.GetFileName(inputFilePath);
            }
            catch
            {
                return "splitted";
            }
        }
    }
}
