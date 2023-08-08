using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using YpdfLib.Informing;
using YpdfLib.Models.Design;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Designers
{
    public static class WatermarkDesigner
    {
        public static void AddIndelibleWatermark(string inputFile, string destPath, IIndelibleWatermark watermark)
        {
            int[] pages = PdfInfo.GetAllPageNumbers(inputFile);
            AddIndelibleWatermark(inputFile, destPath, watermark, pages);
        }

        public static void AddIndelibleWatermark(string inputFile, string destPath, IIndelibleWatermark watermark, int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile), new PdfWriter(destPath)))
            using (var doc = new Document(pdfDoc))
            {
                doc.SetMargins(0, 0, 0, 0);

                foreach (int pageNum in pages)
                {
                    PdfPage curPage = pdfDoc.GetPage(pageNum);
                    Rectangle curPageSize = curPage.GetPageSize();
                    Paragraph indelibleWatermark = CreateIndelibleWatermark(pageNum, curPageSize, watermark);

                    doc.Add(indelibleWatermark);
                }
            }
        }

        public static void AddWatermarkAnnotation(string inputFile, string destPath, IWatermarkAnnotation watermark)
        {
            int[] pages = PdfInfo.GetAllPageNumbers(inputFile);
            AddWatermarkAnnotation(inputFile, destPath, watermark, pages);
        }

        public static void AddWatermarkAnnotation(string inputFile, string destPath, IWatermarkAnnotation watermark, int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile), new PdfWriter(destPath)))
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

        public static void RemoveWatermarkAnnotation(string inputFile, string destPath)
        {
            int[] pages = PdfInfo.GetAllPageNumbers(inputFile);
            RemoveWatermarkAnnotation(inputFile, destPath, pages);
        }

        public static void RemoveWatermarkAnnotation(string inputFile, string destPath, int[] pages)
        {
            if (pages is null)
                throw new ArgumentNullException(nameof(pages));

            using (var pdfDoc = new PdfDocument(new PdfReader(inputFile), new PdfWriter(destPath)))
            {
                foreach (int pageNum in pages)
                {
                    PdfPage curPage = pdfDoc.GetPage(pageNum);
                    PdfDictionary curPageDict = curPage.GetPdfObject();
                    PdfArray? annotators = curPageDict.GetAsArray(PdfName.Annots);

                    for (int i = 0; i < annotators?.Size(); ++i)
                    {
                        PdfDictionary annotation = annotators.GetAsDictionary(i);
                        PdfName annotationName = annotation.GetAsName(PdfName.Subtype);

                        if (PdfName.Watermark.Equals(annotationName))
                            annotation.Clear();
                    }
                }
            }
        }

        private static Paragraph CreateIndelibleWatermark(int pageNumber, Rectangle pageSize, IIndelibleWatermark watermark)
        {
            FloatPoint lowerLeftPoint = watermark.LowerLeftPoint ?? watermark.GetCenterredLowerLeftPoint(pageSize);

            var text = new Text(watermark.Text);
            text.SetTextAlignment(watermark.TextAllocator.TextAlignment);
            text.SetHorizontalAlignment(watermark.TextAllocator.TextHorizontalAlignment);
            text.SetOpacity(watermark.FontInfo.Opacity);
            text.SetFontColor(watermark.FontInfo.Color);
            text.SetFont(watermark.FontInfo.GetPdfFont());
            text.SetFontSize(watermark.FontInfo.Size);

            var paragraph = new Paragraph(text);
            paragraph.SetVerticalAlignment(watermark.TextAllocator.TextContainerVerticalAlignment);
            paragraph.SetWidth(watermark.Width);
            paragraph.SetHeight(watermark.Height);
            paragraph.SetRotationAngle(watermark.RotationAngle);
            paragraph.SetFixedPosition(pageNumber, lowerLeftPoint.X, lowerLeftPoint.Y, watermark.Width);

            if (watermark.Border is not null)
                paragraph.SetBorder(watermark.Border.Create());

            return paragraph;
        }

        private static PdfWatermarkAnnotation CreateWatermarkAnnotation(PdfDocument pdfDoc, Rectangle pageSize, IWatermarkAnnotation watermark)
        {
            FloatPoint lowerLeftPoint = watermark.LowerLeftPoint ?? watermark.GetCenterredLowerLeftPoint(pageSize);

            Rectangle trimmingRectangle = watermark.GetTrimmingRectangle(lowerLeftPoint.X, lowerLeftPoint.Y);
            PdfWatermarkAnnotation annotation = new(trimmingRectangle);

            PdfFixedPrint fixedPrint = new();
            annotation.SetFixedPrint(fixedPrint);

            PdfFormXObject formObj = new(watermark.FormXObjRectangle);
            PdfCanvas canvas = new(formObj, pdfDoc);

            float[] transformValues = new float[6];

            AffineTransform transform = watermark.Transform;
            transform.GetMatrix(transformValues);

            canvas.SaveState()
                .BeginText()
                .SetColor(watermark.FontInfo.Color, true)
                .SetExtGState(watermark.ExtGState)
                .SetTextMatrix(transformValues[0], transformValues[1], transformValues[2], transformValues[3], transformValues[4], transformValues[5])
                .SetFontAndSize(watermark.FontInfo.GetPdfFont(), watermark.FontInfo.Size)
                .ShowText(watermark.Text)
                .EndText()
                .RestoreState()
                .Release();

            PdfAnnotationAppearance annotationAppearance = new(formObj.GetPdfObject());

            annotation.SetAppearance(PdfName.N, annotationAppearance);
            annotation.SetFlag(PdfAnnotation.READ_ONLY);
            annotation.SetFlags(PdfAnnotation.PRINT);

            return annotation;
        }
    }
}
