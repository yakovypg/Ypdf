using iText.Kernel.Pdf;
using Ypdf.Converters.Configuration;

namespace Ypdf.Converters
{
    public static class Rotator
    {
        public static void Rotate(string destPath, string inputPath, int angle)
        {
            var inputDoc = new PdfDocument(new PdfReader(inputPath));
            var destDoc = new PdfDocument(new PdfWriter(destPath));

            int pages = inputDoc.GetNumberOfPages();

            for (int i = 1; i <= pages; ++i)
                inputDoc.GetPage(i).SetRotation(-angle);

            inputDoc.CopyPagesTo(1, pages, destDoc);

            inputDoc.Close();
            destDoc.Close();
        }

        public static void Rotate(string destPath, string inputPath, params PageRotation[] rotations)
        {
            if (rotations is null)
                throw new ArgumentNullException(nameof(rotations));

            var inputDoc = new PdfDocument(new PdfReader(inputPath));
            var destDoc = new PdfDocument(new PdfWriter(destPath));

            foreach (var rotation in rotations)
                inputDoc.GetPage(rotation.PageNumber).SetRotation(-rotation.Angle);

            int pages = inputDoc.GetNumberOfPages();
            inputDoc.CopyPagesTo(1, pages, destDoc);

            inputDoc.Close();
            destDoc.Close();
        }
    }
}
