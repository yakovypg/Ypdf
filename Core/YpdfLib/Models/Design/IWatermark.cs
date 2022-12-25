using YpdfLib.Models.Design.Fonts;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Models.Design
{
    public interface IWatermark
    {
        string Text { get; set; }
        double RotationAngle { get; set; }

        IFontInfo FontInfo { get; set; }
        FloatPoint? LowerLeftPoint { get; set; }

        double GetRotationAngleInDegrees();
        void SetRotationAngleInDegrees(double degrees);
    }
}
