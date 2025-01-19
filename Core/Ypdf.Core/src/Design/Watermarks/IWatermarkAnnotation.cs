using iText.Kernel.Geom;
using iText.Kernel.Pdf.Extgstate;
using Ypdf.Core.Geometry;

namespace Ypdf.Core.Design.Watermarks;

public interface IWatermarkAnnotation : IWatermark
{
    float TrimmingRectangleWidth { get; }
    float TrimmingRectangleHeight { get; }

    float FormXObjWidth { get; }
    float FormXObjHeight { get; }
    float FormXObjXOffset { get; }
    float FormXObjYOffset { get; }

    float XTranslation { get; }
    float YTranslation { get; }

    PdfExtGState ExtGState { get; }
    Rectangle FormXObjRectangle { get; }
    AffineTransform Transform { get; }

    void SetWidth(float witdh);
    void SetHeight(float height);

    FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize);
    Rectangle GetTrimmingRectangle(float bottomLeftX, float bottomLeftY);

    IIndelibleWatermark ToIndelibleWatermark();
}
