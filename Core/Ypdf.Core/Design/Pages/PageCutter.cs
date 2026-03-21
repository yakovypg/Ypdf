using System;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace Ypdf.Core.Design.Pages;

public static class PageCutter
{
    public static void CropRelativeToDivision(
        PdfPage firstHalf,
        PdfPage secondHalf,
        PageDivision division)
    {
        ExtendedArgumentNullException.ThrowIfNull(firstHalf, nameof(firstHalf));
        ExtendedArgumentNullException.ThrowIfNull(secondHalf, nameof(secondHalf));

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
                throw new NotSupportedException(
                    $"Page division orientation {division.Orientation} isn't supported.");
        }
    }

    public static void CropPart(PdfPage page, PagePart pagePart, float centerOffset = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));

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
                newBoxWidth = (sourceBoxWidth / 2) + centerOffset;
                newBoxHeight = sourceBoxHeight;
                break;

            case PagePart.Right:
                newBoxX = ((sourceBoxX + sourceBoxWidth) / 2) + centerOffset;
                newBoxY = sourceBoxY;
                newBoxWidth = sourceBoxWidth;
                newBoxHeight = sourceBoxHeight;
                break;

            case PagePart.Top:
                newBoxX = sourceBoxX;
                newBoxY = ((sourceBoxY + sourceBoxHeight) / 2) + centerOffset;
                newBoxWidth = sourceBoxWidth;
                newBoxHeight = sourceBoxHeight;
                break;

            case PagePart.Bottom:
                newBoxX = sourceBoxX;
                newBoxY = sourceBoxY;
                newBoxWidth = sourceBoxWidth;
                newBoxHeight = (sourceBoxHeight / 2) + centerOffset;
                break;

            default:
                throw new NotSupportedException($"Page part {pagePart} isn't supported.");
        }

        Rectangle newBox = new(newBoxX, newBoxY, newBoxWidth, newBoxHeight);
        CropBox(page, newBox);
    }

    public static void CropBox(PdfPage page, Rectangle box)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));
        ExtendedArgumentNullException.ThrowIfNull(box, nameof(box));

        var boxArray = new PdfArray(new float[]
        {
            box.GetX(),
            box.GetY(),
            box.GetWidth(),
            box.GetHeight()
        });

        PdfDictionary firstPageObj = page.GetPdfObject();
        _ = firstPageObj.Put(PdfName.CropBox, boxArray);
        _ = firstPageObj.Put(PdfName.MediaBox, boxArray);
    }
}
