using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using YpdfLib.Models.Design;

namespace YpdfLib.Designers
{
    public static class WatermarkDesigner
    {
        public static void AddWatermark(string sourceFile, string destPath, IWatermark watermark)
        {
            int numOfPages;

            using (var pdfDoc = new PdfDocument(new PdfReader(sourceFile)))
                numOfPages = pdfDoc.GetNumberOfPages();

            int[] pages = Enumerable.Range(1, numOfPages).ToArray();

            AddWatermark(sourceFile, destPath, watermark, pages);
        }

        public static void AddWatermark(string sourceFile, string destPath, IWatermark watermark, params int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            using (var pdfDoc = new PdfDocument(new PdfReader(sourceFile), new PdfWriter(destPath)))
            {
                foreach (int pageNum in pages)
                {
                    PdfPage curPage = pdfDoc.GetPage(pageNum);
                    Rectangle curPageSize = curPage.GetPageSize();
                    PdfWatermarkAnnotation annotation = CreateWatermarkAnnotation(pdfDoc, curPageSize, watermark);

                    curPage.AddAnnotation(annotation);
                }
            }
        }

        public static void RemoveWatermark(string sourceFile, string destPath)
        {
            int numOfPages;

            using (var pdfDoc = new PdfDocument(new PdfReader(sourceFile)))
                numOfPages = pdfDoc.GetNumberOfPages();

            int[] pages = Enumerable.Range(1, numOfPages).ToArray();

            RemoveWatermark(sourceFile, destPath, pages);
        }

        public static void RemoveWatermark(string sourceFile, string destPath, params int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            using (var pdfDoc = new PdfDocument(new PdfReader(sourceFile), new PdfWriter(destPath)))
            {
                foreach (int pageNum in pages)
                {
                    PdfPage curPage = pdfDoc.GetPage(pageNum);
                    PdfDictionary curPageDict = curPage.GetPdfObject();
                    PdfArray annotators = curPageDict.GetAsArray(PdfName.Annots);

                    for (int i = 0; i < annotators.Size(); ++i)
                    {
                        PdfDictionary annotation = annotators.GetAsDictionary(i);
                        PdfName annotationName = annotation.GetAsName(PdfName.Subtype);

                        if (PdfName.Watermark.Equals(annotationName))
                            annotation.Clear();
                    }
                }
            }
        }

        private static PdfWatermarkAnnotation CreateWatermarkAnnotation(PdfDocument pdfDoc, Rectangle pageSize, IWatermark watermark)
        {
            float pageWidth = pageSize.GetWidth();
            float pageHeight = pageSize.GetHeight();

            float bottomLeftX = pageWidth / 2 - watermark.TrimmingRectangleWidth / 2;
            float bottomLeftY = pageHeight / 2 - watermark.TrimmingRectangleHeight / 2;

            var trimmingRectangle = watermark.GetTrimmingRectangle(bottomLeftX, bottomLeftY);
            var annotation = new PdfWatermarkAnnotation(trimmingRectangle);

            var fixedPrint = new PdfFixedPrint();
            annotation.SetFixedPrint(fixedPrint);

            var formObj = new PdfFormXObject(watermark.FormXObjRectangle);
            var canvas = new PdfCanvas(formObj, pdfDoc);

            float[] transformValues = new float[6];

            var transform = watermark.Transform;
            transform.GetMatrix(transformValues);

            canvas.SaveState()
                .BeginText()
                .SetColor(watermark.Color, true)
                .SetExtGState(watermark.ExtGState)
                .SetTextMatrix(transformValues[0], transformValues[1], transformValues[2], transformValues[3], transformValues[4], transformValues[5])
                .SetFontAndSize(watermark.Font, watermark.FontSize)
                .ShowText(watermark.Text)
                .EndText()
                .RestoreState()
                .Release();

            var annotationAppearance = new PdfAnnotationAppearance(formObj.GetPdfObject());

            annotation.SetAppearance(PdfName.N, annotationAppearance);
            annotation.SetFlags(PdfAnnotation.PRINT);

            return annotation;
        }
    }
}
