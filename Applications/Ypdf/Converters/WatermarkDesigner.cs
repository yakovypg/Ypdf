using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using Ypdf.Converters.Configuration;

namespace Ypdf.Converters
{
    public static class WatermarkDesigner
    {
        public static void AddWatermark(string sourceFile, string destPath, Watermark watermark, int[]? pages = null)
        {
            var reader = new PdfReader(sourceFile);
            var writer = new PdfWriter(destPath);
            var pdfDoc = new PdfDocument(reader, writer);

            int numOfPages = pdfDoc.GetNumberOfPages();

            if (numOfPages == 0)
            {
                pdfDoc.Close();
                return;
            }

            if (pages is null)
                pages = Enumerable.Range(1, numOfPages).ToArray();

            foreach (int pageNum in pages)
            {
                PdfPage curPage = pdfDoc.GetPage(pageNum);

                Rectangle curPageSize = curPage.GetPageSize();
                float curPageWidth = curPageSize.GetWidth();
                float curPageHeight = curPageSize.GetHeight();

                float bottomLeftX = curPageWidth / 2 - watermark.TrimmingRectangleWidth / 2;
                float bottomLeftY = curPageHeight / 2 - watermark.TrimmingRectangleHeight / 2;

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

                curPage.AddAnnotation(annotation);
            }

            pdfDoc.Close();
        }

        public static void RemoveWatermark(string sourceFile, string destPath, int[]? pages = null)
        {
            var reader = new PdfReader(sourceFile);
            var writer = new PdfWriter(destPath);
            var pdfDoc = new PdfDocument(reader, writer);

            var numOfPages = pdfDoc.GetNumberOfPages();

            if (numOfPages == 0)
            {
                pdfDoc.Close();
                return;
            }

            if (pages is null)
                pages = Enumerable.Range(1, numOfPages).ToArray();

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

            pdfDoc.Close();
        }
    }
}
