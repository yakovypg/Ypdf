using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using YpdfLib.Informing;
using YpdfLib.Models.Design;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Paging;

namespace YpdfLib.Designers
{
    public class PageResizer
    {
        private readonly PdfPage _page;

        public PageResizer(PdfPage page)
        {
            _page = page;
        }

        public void Resize(float width, float height)
        {
            _ = new MediaBox(_page)
            {
                Width = width,
                Height = height
            };
        }

        public void IncreaseLeft(float value)
        {
            IncreaseLeft(value, ColorConstants.WHITE);
        }

        public void IncreaseLeft(float value, Color fillColor, float heightIncrease = 0)
        {
            var mediaBox = new MediaBox(_page);

            mediaBox.StartX -= value;

            mediaBox.DrawRectangle(
                x: mediaBox.InitialStartX - value,
                y: mediaBox.InitialStartY,
                width: value,
                height: mediaBox.InitialHeight + heightIncrease,
                fillColor: fillColor
            );
        }

        public void IncreaseTop(float value)
        {
            IncreaseTop(value, ColorConstants.WHITE);
        }

        public void IncreaseTop(float value, Color fillColor, float widthIncrease = 0)
        {
            var mediaBox = new MediaBox(_page);

            mediaBox.Height += value;

            mediaBox.DrawRectangle(
                x: mediaBox.InitialStartX,
                y: mediaBox.InitialHeight,
                width: mediaBox.InitialWidth + widthIncrease,
                height: value,
                fillColor: fillColor
            );
        }

        public void IncreaseRight(float value)
        {
            IncreaseRight(value, ColorConstants.WHITE);
        }

        public void IncreaseRight(float value, Color fillColor, float heightIncrease = 0)
        {
            var mediaBox = new MediaBox(_page);

            mediaBox.Width += value;

            mediaBox.DrawRectangle(
                x: mediaBox.InitialWidth,
                y: mediaBox.InitialStartY,
                width: value,
                height: mediaBox.InitialHeight + heightIncrease,
                fillColor: fillColor
            );
        }

        public void IncreaseBottom(float value)
        {
            IncreaseBottom(value, ColorConstants.WHITE);
        }

        public void IncreaseBottom(float value, Color fillColor, float widtIncrease = 0)
        {
            var mediaBox = new MediaBox(_page);

            mediaBox.StartY -= value;

            mediaBox.DrawRectangle(
                x: mediaBox.InitialStartX,
                y: mediaBox.InitialStartY - value,
                width: mediaBox.InitialWidth + widtIncrease,
                height: value,
                fillColor: fillColor
            );
        }

        public void Increase(float left, float top, float right, float bottom)
        {
            Increase(left, top, right, bottom, ColorConstants.WHITE);
        }

        public void Increase(float left, float top, float right, float bottom, Color fillColor)
        {
            Increase(left, top, right, bottom, fillColor, fillColor, fillColor, fillColor);
        }

        public void Increase(float left, float top, float right, float bottom, Color leftColor, Color topColor, Color rightColor, Color bottomColor)
        {
            IncreaseLeft(left, leftColor);
            IncreaseRight(right, rightColor);
            IncreaseTop(top, topColor, left);
            IncreaseBottom(bottom, bottomColor, left);
        }

        public static void IncreaseLeft(string inputFile, string destPath, float value, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseLeft(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseLeft(string inputFile, string destPath, float value, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);
            IncreaseLeft(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseLeft(string inputFile, string destPath, float value, Color fillColor, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseLeft(inputFile, destPath, value, fillColor, pages);
        }

        public static void IncreaseLeft(string inputFile, string destPath, float value, Color fillColor, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);

            var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
            {
                new PageResizer(page).IncreaseLeft(value, color);
            });

            Increase(inputFile, destPath, value, fillColor, increaseAction, pages);
        }

        public static void IncreaseTop(string inputFile, string destPath, float value, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseTop(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseTop(string inputFile, string destPath, float value, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);
            IncreaseTop(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseTop(string inputFile, string destPath, float value, Color fillColor, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseTop(inputFile, destPath, value, fillColor, pages);
        }

        public static void IncreaseTop(string inputFile, string destPath, float value, Color fillColor, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);

            var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
            {
                new PageResizer(page).IncreaseTop(value, color);
            });

            Increase(inputFile, destPath, value, fillColor, increaseAction, pages);
        }

        public static void IncreaseRight(string inputFile, string destPath, float value, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseRight(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseRight(string inputFile, string destPath, float value, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);
            IncreaseRight(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseRight(string inputFile, string destPath, float value, Color fillColor, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseRight(inputFile, destPath, value, fillColor, pages);
        }

        public static void IncreaseRight(string inputFile, string destPath, float value, Color fillColor, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);

            var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
            {
                new PageResizer(page).IncreaseRight(value, color);
            });

            Increase(inputFile, destPath, value, fillColor, increaseAction, pages);
        }

        public static void IncreaseBottom(string inputFile, string destPath, float value, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseBottom(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseBottom(string inputFile, string destPath, float value, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);
            IncreaseBottom(inputFile, destPath, value, ColorConstants.WHITE, pages);
        }

        public static void IncreaseBottom(string inputFile, string destPath, float value, Color fillColor, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            IncreaseBottom(inputFile, destPath, value, fillColor, pages);
        }

        public static void IncreaseBottom(string inputFile, string destPath, float value, Color fillColor, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);

            var increaseAction = new Action<PdfPage, float, Color>((page, value, color) =>
            {
                new PageResizer(page).IncreaseBottom(value, color);
            });

            Increase(inputFile, destPath, value, fillColor, increaseAction, pages);
        }

        public static void Increase(string inputFile, string destPath, IPageIncrease pageIncrease, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            Increase(inputFile, destPath, pageIncrease, ColorConstants.WHITE, pages);
        }

        public static void Increase(string inputFile, string destPath, IPageIncrease pageIncrease, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);
            Increase(inputFile, destPath, pageIncrease, ColorConstants.WHITE, pages);
        }

        public static void Increase(string inputFile, string destPath, IPageIncrease pageIncrease, Color fillColor, IPageRange[] pageRanges)
        {
            int[] pages = PageRange.GetAllItems(pageRanges);
            Increase(inputFile, destPath, pageIncrease, fillColor, pages);
        }

        public static void Increase(string inputFile, string destPath, IPageIncrease pageIncrease, Color fillColor, int[]? pages = null)
        {
            pages ??= PdfInfo.GetAllPageNumbers(inputFile);

            var increaseAction = new Action<PdfPage, float, float, float, float, Color>((page, left, top, right, bottom, color) =>
            {
                new PageResizer(page).Increase(left, top, right, bottom, color);
            });

            Increase(inputFile, destPath, pageIncrease, fillColor, increaseAction, pages);
        }

        private static void Increase(string inputFile, string destPath, float value, Color fillColor, Action<PdfPage, float, Color> increaseAction, int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile), new PdfWriter(destPath)))
            {
                foreach (int pageNum in pages)
                {
                    PdfPage page = pdfDoc.GetPage(pageNum);
                    increaseAction.Invoke(page, value, fillColor);
                }
            }
        }

        private static void Increase(string inputFile, string destPath, IPageIncrease pageIncrease, Color fillColor, Action<PdfPage, float, float, float, float, Color> increaseAction, int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile), new PdfWriter(destPath)))
            {
                foreach (int pageNum in pages)
                {
                    PdfPage page = pdfDoc.GetPage(pageNum);

                    increaseAction.Invoke(page, pageIncrease.Left, pageIncrease.Top,
                        pageIncrease.Right, pageIncrease.Bottom, fillColor);
                }
            }
        }
    }
}
