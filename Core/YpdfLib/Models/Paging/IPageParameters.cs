using iText.Kernel.Geom;
using iText.Layout.Properties;
using YpdfLib.Models.Design;


namespace YpdfLib.Models.Paging
{
    public interface IPageParameters
    {
        int CompressionLevel { get; }
        bool AutoIncreaseSize { get; }

        IMargin? Margin { get; }
        HorizontalAlignment HorizontalAlignment { get; }

        PageSize? Size { get; }
    }
}
