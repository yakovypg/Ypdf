using iText.Kernel.Geom;
using Ypdf.Core.Design.Borders;
using Ypdf.Core.Geometry;

namespace Ypdf.Core.Design.Watermarks;

public interface IIndelibleWatermark : IWatermark
{
    float Width { get; }
    float Height { get; }
    WatermarkTextAllocator TextAllocator { get; }
    LazyBorder? Border { get; }

    FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize);
}
