using FileSystemLib.Paths;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using YpdfLib.Extensions;
using YpdfLib.Extractors;
using YpdfLib.Imaging;

namespace YpdfLib.Compressors
{
    public static class Compressor
    {
        public static void Compress(string inputFile, string destPath, float imageQualityFactor = 0.75f)
        {
            string tempDirPath = Properties.Resources.TEMP_DIRECTORY;

            IUniqueDirectory uniqueDirGenerator = new UniqueDirectory(tempDirPath);
            DirectoryInfo uniqueDir = uniqueDirGenerator.Create();

            ImageExtractor.Extract(uniqueDir.FullName, inputFile);

            string[] extractedImages = uniqueDir
                .GetFiles()
                .Select(t => t.FullName)
                .ToArray();

            ImageCompressor.Compress(extractedImages, uniqueDir.FullName, imageQualityFactor, 1.0f, "jpg");

            string[] compressedImages = uniqueDir
                .GetFiles("*_compressed.jpg")
                .Select(t => t.FullName)
                .ToArray();

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
                        if (imageIndex >= compressedImages.Length)
                        {
                            success = false;
                            break;
                        }

                        string compressedImgPath = compressedImages[imageIndex++];
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
                throw new ApplicationException("Could not extract all images.");
        }
    }
}
