using iText.Kernel.Pdf;
using YpdfLib.Extensions;
using YpdfLib.Models.Paging;

namespace YpdfLib.Converters
{
    public static class Rotator
    {
        public static void Rotate(string inputFile, string destPath, int angle)
        {
            using (var inputDoc = new PdfDocument(new PdfReader(inputFile)))
            using (var destDoc = new PdfDocument(new PdfWriter(destPath)))
            {
                int pages = inputDoc.GetNumberOfPages();

                for (int i = 1; i <= pages; ++i)
                    inputDoc.GetPage(i).SetRotation(-angle);

                inputDoc.CopyTo(destDoc);
            }
        }

        public static void Rotate(string inputFile, string destPath, params IPageRotation[] rotations)
        {
            if (rotations is null)
                throw new ArgumentNullException(nameof(rotations));

            using (var inputDoc = new PdfDocument(new PdfReader(inputFile)))
            using (var destDoc = new PdfDocument(new PdfWriter(destPath)))
            {
                foreach (var rotation in rotations)
                    inputDoc.GetPage(rotation.PageNumber).SetRotation(-rotation.Angle);

                inputDoc.CopyTo(destDoc);
            }
        }
    }
}
