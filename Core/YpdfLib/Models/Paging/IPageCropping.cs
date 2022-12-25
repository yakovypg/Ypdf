using iText.Kernel.Geom;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Models.Paging
{
    public interface IPageCropping : IDeepCloneable<IPageCropping>
    {
        int PageNumber { get; }
        FloatPoint LowerLeft { get; }
        FloatPoint UpperRight { get; }
        Rectangle Box { get; }
    }
}
