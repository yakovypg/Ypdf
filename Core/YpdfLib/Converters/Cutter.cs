using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using YpdfLib.Models.Paging;

namespace YpdfLib.Converters
{
    public static class Cutter
    {
        public static void DividePages(string inputFile, string destPath, params IPageDivision[] divisions)
        {
            ArgumentNullException.ThrowIfNull(divisions, nameof(divisions));

            using (var inputDoc = new PdfDocument(new PdfReader(inputFile)))
            using (var destDoc = new PdfDocument(new PdfWriter(destPath)))
            {
                int numOfPages = inputDoc.GetNumberOfPages();

                for (int i = 1; i <= numOfPages; ++i)
                {
                    PdfPage currPage = inputDoc.GetPage(i);
                    IPageDivision? division = divisions.FirstOrDefault(t => t.PageNumber == i);

                    if (division is null)
                    {
                        destDoc.AddPage(currPage.CopyTo(destDoc));
                        continue;
                    }

                    destDoc.AddPage(currPage.CopyTo(destDoc));
                    PdfPage firstHalf = destDoc.GetLastPage();

                    destDoc.AddPage(currPage.CopyTo(destDoc));
                    PdfPage secondHalf = destDoc.GetLastPage();

                    switch (division.Orientation)
                    {
                        case PageDivisionOrientation.Vertical:
                            CropPart(firstHalf, PagePart.Left, division.CenterOffset);
                            CropPart(secondHalf, PagePart.Right, division.CenterOffset);
                            break;

                        case PageDivisionOrientation.Horizontal:
                            CropPart(firstHalf, PagePart.Top, division.CenterOffset);
                            CropPart(secondHalf, PagePart.Bottom, division.CenterOffset);
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        public static void CropPages(string inputFile, string destPath, params IPageCropping[] croppings)
        {
            using (var inputDoc = new PdfDocument(new PdfReader(inputFile)))
            using (var destDoc = new PdfDocument(new PdfWriter(destPath)))
            {
                int numOfPages = inputDoc.GetNumberOfPages();

                for (int i = 1; i <= numOfPages; ++i)
                {
                    PdfPage currPage = inputDoc.GetPage(i);
                    IPageCropping? cropping = croppings.FirstOrDefault(t => t.PageNumber == i);

                    if (cropping is null)
                    {
                        destDoc.AddPage(currPage.CopyTo(destDoc));
                        continue;
                    }

                    destDoc.AddPage(currPage.CopyTo(destDoc));
                    PdfPage pageToCrop = destDoc.GetLastPage();

                    CropBox(pageToCrop, cropping.Box);
                }
            }
        }

        public static void CropPart(PdfPage page, PagePart pagePart, float centerOffset = 0)
        {
            Rectangle sourcePageBox = page.GetCropBox();

            float sourceBoxX = sourcePageBox.GetX();
            float sourceBoxY = sourcePageBox.GetY();
            float sourceBoxWidth = sourcePageBox.GetWidth();
            float sourceBoxHeight = sourcePageBox.GetHeight();

            float newBoxX;
            float newBoxY;
            float newBoxWidth;
            float newBoxHeight;

            switch (pagePart)
            {
                case PagePart.Left:
                    newBoxX = sourceBoxX;
                    newBoxY = sourceBoxY;
                    newBoxWidth = sourceBoxWidth / 2 + centerOffset;
                    newBoxHeight = sourceBoxHeight;
                    break;

                case PagePart.Right:
                    newBoxX = (sourceBoxX + sourceBoxWidth) / 2 + centerOffset;
                    newBoxY = sourceBoxY;
                    newBoxWidth = sourceBoxWidth;
                    newBoxHeight = sourceBoxHeight;
                    break;

                case PagePart.Top:
                    newBoxX = sourceBoxX;
                    newBoxY = (sourceBoxY + sourceBoxHeight) / 2 + centerOffset;
                    newBoxWidth = sourceBoxWidth;
                    newBoxHeight = sourceBoxHeight;
                    break;

                case PagePart.Bottom:
                    newBoxX = sourceBoxX;
                    newBoxY = sourceBoxY;
                    newBoxWidth = sourceBoxWidth;
                    newBoxHeight = sourceBoxHeight / 2 + centerOffset;
                    break;

                default:
                    throw new NotImplementedException();
            }

            Rectangle newBox = new(newBoxX, newBoxY, newBoxWidth, newBoxHeight);
            CropBox(page, newBox);
        }

        public static void CropBox(PdfPage page, Rectangle box)
        {
            PdfArray boxArray = new PdfArray(new float[]
            {
                box.GetX(),
                box.GetY(),
                box.GetWidth(),
                box.GetHeight()
            });

            PdfDictionary firstPageObj = page.GetPdfObject();
            firstPageObj.Put(PdfName.CropBox, boxArray);
            firstPageObj.Put(PdfName.MediaBox, boxArray);
        }
    }
}
