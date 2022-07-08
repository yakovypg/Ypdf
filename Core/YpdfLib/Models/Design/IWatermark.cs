using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Extgstate;

namespace YpdfLib.Models.Design
{
    public interface IWatermark
    {
        string Text { get; set; }

        float FontSize { get; set; }
        string FontFamily { get; set; }

        Color Color { get; set; }

        float FormXObjWidth { get; set; }
        float FormXObjHeight { get; set; }
        float FormXObjXOffset { get; set; }
        float FormXObjYOffset { get; set; }

        float XTranslation { get; set; }
        float YTranslation { get; set; }

        float TrimmingRectangleWidth { get; set; }
        float TrimmingRectangleHeight { get; set; }

        float ExtGStateOpacity { get; set; }
        double RotationAngle { get; set; }

        PdfFont Font { get; }
        PdfExtGState ExtGState { get; }
        Rectangle FormXObjRectangle { get; }

        AffineTransform Transform { get; }

        Rectangle GetTrimmingRectangle(float bottomLeftX, float bottomLeftY);
    }
}
