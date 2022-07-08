using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Extgstate;

namespace YpdfLib.Models.Design
{
    public class Watermark : IWatermark
    {
        public string Text { get; set; } = nameof(Watermark);

        public float FontSize { get; set; } = 50;
        public string FontFamily { get; set; } = StandardFonts.TIMES_ROMAN;

        public Color Color { get; set; } = ColorConstants.GRAY;

        public float FormXObjWidth { get; set; } = 300;
        public float FormXObjHeight { get; set; } = 300;
        public float FormXObjXOffset { get; set; } = 0;
        public float FormXObjYOffset { get; set; } = 0;

        public float XTranslation { get; set; } = 50;
        public float YTranslation { get; set; } = 25;

        public float TrimmingRectangleWidth { get; set; } = 300;
        public float TrimmingRectangleHeight { get; set; } = 300;

        public float ExtGStateOpacity { get; set; } = 0.6f;
        public double RotationAngle { get; set; } = Math.PI / 3;

        public PdfFont Font => PdfFontFactory.CreateFont(FontFamily);
        public PdfExtGState ExtGState => new PdfExtGState().SetFillOpacity(ExtGStateOpacity);
        public Rectangle FormXObjRectangle => new(FormXObjXOffset, FormXObjYOffset, FormXObjWidth, FormXObjHeight);

        public AffineTransform Transform
        {
            get
            {
                var transform = new AffineTransform();
                transform.Translate(XTranslation, YTranslation);
                transform.Rotate(RotationAngle);

                return transform;
            }
        }

        public Watermark()
        {
        }

        public Watermark(string text)
        {
            Text = text;
        }

        public Rectangle GetTrimmingRectangle(float bottomLeftX, float bottomLeftY)
        {
            return new Rectangle(bottomLeftX, bottomLeftY, TrimmingRectangleWidth, TrimmingRectangleHeight);
        }
    }
}
