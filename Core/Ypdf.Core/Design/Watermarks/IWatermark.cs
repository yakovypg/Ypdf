using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Geometry;

namespace Ypdf.Core.Design.Watermarks;

public interface IWatermark
{
    string Text { get; }
    double RotationAngleRadians { get; }
    TextFontInfo FontInfo { get; }
    FloatPoint? LowerLeftPoint { get; }

    double GetRotationAngleInDegrees();
    void SetRotationAngleInDegrees(double degrees);
}
