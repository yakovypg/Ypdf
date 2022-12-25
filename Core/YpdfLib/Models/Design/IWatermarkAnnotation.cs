using iText.Kernel.Geom;
using iText.Kernel.Pdf.Extgstate;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Models.Design
{
    public interface IWatermarkAnnotation : IWatermark, IDeepCloneable<IWatermarkAnnotation>
    {
        float TrimmingRectangleWidth { get; set; }
        float TrimmingRectangleHeight { get; set; }

        float FormXObjWidth { get; set; }
        float FormXObjHeight { get; set; }
        float FormXObjXOffset { get; set; }
        float FormXObjYOffset { get; set; }

        float XTranslation { get; set; }
        float YTranslation { get; set; }

        PdfExtGState ExtGState { get; }
        Rectangle FormXObjRectangle { get; }
        AffineTransform Transform { get; }

        void SetWidth(float witdh);
        void SetHeight(float height);

        FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize);
        Rectangle GetTrimmingRectangle(float bottomLeftX, float bottomLeftY);

        IIndelibleWatermark ToIndelibleWatermark();
    }
}
