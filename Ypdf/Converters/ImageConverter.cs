using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Ypdf.Converters.Config;

namespace Ypdf.Converters
{
    public static class ImageConverter
    {
        public static void ToPdf(string destPath, PageParameters pageParameters, params string[] inputPaths)
        {
            if (inputPaths is null)
                throw new ArgumentNullException(nameof(inputPaths));

            Image[] images = inputPaths
                .Select(t => new Image(ImageDataFactory.Create(t)))
                .ToArray();

            var destDoc = new PdfDocument(new PdfWriter(destPath));
            var doc = new Document(destDoc);

            if (pageParameters.Margin is not null)
            {
                var margin = pageParameters.Margin;
                doc.SetMargins(margin.Top, margin.Right, margin.Bottom, margin.Left);
            }

            if (pageParameters.Size is not null)
            {
                destDoc.SetDefaultPageSize(pageParameters.Size);
            }
            else
            {
                foreach (var image in images)
                {
                    float horizontalIncrease = pageParameters.AutoIncreaseSize
                        ? pageParameters.Margin?.HorizontalSum ?? 0
                        : 0;

                    float verticalIncrease = pageParameters.AutoIncreaseSize
                        ? pageParameters.Margin?.VerticalSum ?? 0
                        : 0;

                    float imgWidth = image.GetImageWidth() + horizontalIncrease;
                    float imgHeight = image.GetImageHeight() + verticalIncrease;

                    var pageSize = new PageSize(imgWidth, imgHeight);
                    destDoc.AddNewPage(pageSize);
                }
            }

            for (int i = 0; i < images.Length; ++i)
            {
                Image image = images[i];
                image.SetHorizontalAlignment(pageParameters.HorizontalAlignment);
                image.SetAutoScale(true);

                doc.Add(image);

                if (i < inputPaths.Length - 1)
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            }

            doc.Close();
            destDoc.Close();
        }
    }
}
