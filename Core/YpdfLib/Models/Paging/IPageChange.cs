using iText.Kernel.Colors;
using iText.Kernel.Geom;

namespace YpdfLib.Models.Paging
{
    public interface IPageChange : IDeepCloneable<IPageChange>
    {
        int? NewWidth { get; set; }
        int? NewHeight { get; set; }
        IPageIncrease? Increase { get; set; }

        Color FillColor { get; set; }
        PageSize? PageSize { get; set; }
    }
}