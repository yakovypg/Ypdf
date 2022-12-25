using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using YpdfLib.Models.Design;
using YpdfLib.Models.Paging;

namespace YpdfLib.Imaging
{
    public static class ImageInserter
    {
        public static void ConvertToPdf(string destPath, IImagePageParameters pageParams, string[] inputFiles)
        {
            if (inputFiles is null)
                throw new ArgumentNullException(nameof(inputFiles));

            if (inputFiles.Length == 0)
                throw new ArgumentException("There are no files to convert.", nameof(inputFiles));

            Image[] images = inputFiles
                .Select(t => new Image(ImageDataFactory.Create(t)))
                .ToArray();

            var pdfWriter = new PdfWriter(destPath);
            var destDoc = new PdfDocument(pdfWriter);
            var doc = new Document(destDoc);

            if (pageParams.Margin is not null)
            {
                IMargin margin = pageParams.Margin;
                doc.SetMargins(margin.Top, margin.Right, margin.Bottom, margin.Left);
            }

            if (pageParams.Size is not null)
                destDoc.SetDefaultPageSize(pageParams.Size);
            else
                AddImageDependentPagesToDoc(destDoc, pageParams, images);

            for (int i = 0; i < images.Length; ++i)
            {
                Image image = images[i];
                pageParams.ApplyStyleToImage(image);

                doc.Add(image);

                if (i < inputFiles.Length - 1)
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            }

            doc.Close();
        }

        private static void AddImageDependentPagesToDoc(PdfDocument doc, IImagePageParameters pageParams, IEnumerable<Image> images)
        {
            foreach (var image in images)
            {
                float horizontalIncrease = pageParams.AutoIncreaseSize
                    ? pageParams.Margin?.HorizontalSum ?? 0
                    : 0;

                float verticalIncrease = pageParams.AutoIncreaseSize
                    ? pageParams.Margin?.VerticalSum ?? 0
                    : 0;

                float imgWidth = image.GetImageWidth() + horizontalIncrease;
                float imgHeight = image.GetImageHeight() + verticalIncrease;

                var pageSize = new PageSize(imgWidth, imgHeight);
                doc.AddNewPage(pageSize);
            }
        }
    }
}
