using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Models.Design
{
    public interface IIndelibleWatermark : IWatermark, IDeepCloneable<IIndelibleWatermark>
    {
        float Width { get; set; }
        float Height { get; set; }

        IWatermarkTextAllocator TextAllocator { get; set; }
        ILazyBorder? Border { get; set; }

        FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize);
    }
}
