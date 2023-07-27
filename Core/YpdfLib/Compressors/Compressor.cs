using FileSystemLib.Naming;
using FileSystemLib.Paths;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using YpdfLib.Extensions;
using YpdfLib.Extractors;
using YpdfLib.Models.Imaging;

namespace YpdfLib.Compressors
{
    public static class Compressor
    {
        public const int EXTRACTED_IMAGES_PART_LENGTH = 5;
        public const int EXTRACTED_IMAGES_FOR_CHECKING_VALIDITY = 2;

        public static void Compress(string inputFile, string destPath, IImageCompression compression,
            bool checkCompressionValidity = true, TextWriter? outputWriter = null)
        {
            Compress(inputFile, destPath, null, compression, checkCompressionValidity, outputWriter);
        }

        public static void Compress(string inputFile, string destPath, string? pythonAlias, IImageCompression imageCompression,
            bool checkCompressionValidity = true, TextWriter? outputWriter = null)
        {
            if (imageCompression.Extension is null)
            {
                imageCompression = imageCompression.Copy();
                imageCompression.Extension = "jpg";
            }

            string tempDirPath = SharedConfig.Directories.Temp;

            IUniqueDirectory uniqueDirGenerator = new UniqueDirectory(tempDirPath);
            DirectoryInfo uniqueDir = uniqueDirGenerator.Create();

            if (checkCompressionValidity)
            {
                CheckCompressionValidity(inputFile, pythonAlias, imageCompression,
                    EXTRACTED_IMAGES_FOR_CHECKING_VALIDITY, outputWriter);
            }

            ImageExtractor.Extract(uniqueDir.FullName, pythonAlias, new string[] { inputFile }, -1, outputWriter);

            var extractedImagePaths = uniqueDir.GetFiles().Select(t => t.FullName);
            string[] sortedExtractedImagePaths = OrderPathsByImageNumber(extractedImagePaths);

            var sortedExtractedImagesParts = PathsSplitter.Split(sortedExtractedImagePaths, EXTRACTED_IMAGES_PART_LENGTH);

            foreach (var part in sortedExtractedImagesParts)
            {
                ImageCompressor.Compress(part, uniqueDir.FullName, imageCompression, pythonAlias, outputWriter);
            }

            var compressedImagePaths = uniqueDir.GetFiles($"*_compressed.*").Select(t => t.FullName);
            string[] sortedCompressedImagePaths = OrderPathsByImageNumber(compressedImagePaths);

            bool success = true;

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile), new PdfWriter(destPath)))
            {
                int imageIndex = 0;
                int numOfPages = pdfDoc.GetNumberOfPages();

                for (int i = 1; i <= numOfPages; ++i)
                {
                    PdfPage curPage = pdfDoc.GetPage(i);
                    PdfName[] imageRefs = curPage.GetImageReferences(out PdfDictionary? curPageXObjects);

                    foreach (var imgRef in imageRefs)
                    {
                        if (imageIndex >= sortedCompressedImagePaths.Length)
                        {
                            success = false;
                            break;
                        }

                        string compressedImgPath = sortedCompressedImagePaths[imageIndex++];
                        ImageData compressedImgData = ImageDataFactory.Create(compressedImgPath);

                        Image compressedImg = new(compressedImgData);
                        PdfStream compressedImgStream = compressedImg.GetXObject().GetPdfObject();

                        curPageXObjects?.Put(imgRef, compressedImgStream);
                    }

                    if (!success)
                        break;
                }
            }

            Directory.Delete(uniqueDir.FullName, true);

            if (!success)
                throw new CompressionException("Could not extract and compress images.");

            if (outputWriter is not null)
            {
                outputWriter.WriteLine();
                PrintCompressionSummary(inputFile, destPath, outputWriter);
            }
        }

        public static bool IsCompressionValid(string inputFile, string? pythonAlias, IImageCompression imageCompression,
            int extractedImagesCount = EXTRACTED_IMAGES_FOR_CHECKING_VALIDITY)
        {
            string tempDirPath = SharedConfig.Directories.Temp;

            IUniqueDirectory uniqueDirGenerator = new UniqueDirectory(tempDirPath);
            DirectoryInfo uniqueDir = uniqueDirGenerator.Create();

            ImageExtractor.Extract(uniqueDir.FullName, pythonAlias, new string[] { inputFile }, extractedImagesCount, null);

            var extractedImages = uniqueDir.GetFiles().OrderBy(t => t.Name).ToArray();
            var extractedImagePaths = extractedImages.Select(t => t.FullName).ToArray();

            ImageCompressor.Compress(extractedImagePaths, uniqueDir.FullName, imageCompression, pythonAlias, null);

            var compressedImages = uniqueDir
                .GetFiles($"*_compressed.*")
                .OrderBy(t => t.Name)
                .ToArray();

            if (compressedImages.Length != extractedImages.Length)
                throw new CompressionException("Could not check if compression is valid.");

            bool result = true;

            for (int i = 0; i < extractedImages.Length; ++i)
            {
                if (extractedImages[i].Length < compressedImages[i].Length)
                {
                    result = false;
                    break;
                }
            }

            Directory.Delete(uniqueDir.FullName, true);

            return result;
        }

        private static void CheckCompressionValidity(string inputFile, string? pythonAlias, IImageCompression imageCompression,
            int extractedImagesCount = EXTRACTED_IMAGES_FOR_CHECKING_VALIDITY, TextWriter? outputWriter = null)
        {
            outputWriter?.WriteLine("Compression validity is being checked...");

            if (!IsCompressionValid(inputFile, pythonAlias, imageCompression, extractedImagesCount))
            {
                string errorMessage = "Validation of compression failed. The source document " +
                    "contains images that cannot be more compressed.";

                throw new CompressionException(errorMessage);
            }

            outputWriter?.WriteLine("Compression validation is complete.");
        }

        private static string[] OrderPathsByImageNumber(IEnumerable<string> paths)
        {
            static int GetImageNumber(string path)
            {
                const string numberMark = "_n";

                int numberMarkIndex = path.LastIndexOf(numberMark);
                int tailStartIndex = numberMarkIndex + numberMark.Length;
                string tail = path[tailStartIndex..];

                char delimiter = tail.First(t => !char.IsDigit(t));
                int delimiterIndex = tail.IndexOf(delimiter);
                string refStr = tail.Remove(delimiterIndex);

                return int.TryParse(refStr, out int num)
                    ? num
                    : throw new FormatException("Failed to order paths by image numbers.");
            }

            return paths.OrderBy(t => GetImageNumber(t)).ToArray();
        }

        private static void PrintCompressionSummary(string inputFile, string outputFile, TextWriter outputWriter)
        {
            long inputFileSize = new FileInfo(inputFile).Length;
            long outputFileSize = new FileInfo(outputFile).Length;

            double inputFileSizeKb = inputFileSize / 1024.0;
            double outputFileSizeKb = outputFileSize / 1024.0;
            double compression = (1 - outputFileSize / (double)inputFileSize) * 100;

            double inputFileSizeRounded = Math.Round(inputFileSizeKb, 5);
            double outputFileSizeRounded = Math.Round(outputFileSizeKb, 5);
            double compressionRounded = Math.Round(compression, 5);

            outputWriter.WriteLine("Compression summary");
            outputWriter.WriteLine($"Input document size: {inputFileSizeRounded} KB");
            outputWriter.WriteLine($"Output document size: {outputFileSizeRounded} KB");
            outputWriter.WriteLine($"Compression: {compressionRounded}%");
        }
    }
}
