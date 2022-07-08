using iText.Kernel.Geom;
using iText.Layout.Properties;
using YpdfLib.Models.Design;


namespace YpdfLib.Models.Paging
{
    public interface IImagePageParameters
    {
        int CompressionLevel { get; set; }
        bool AutoIncreaseSize { get; set; }

        IMargin? Margin { get; set; }
        HorizontalAlignment HorizontalAlignment { get; set; }

        PageSize? Size { get; set; }
    }
}
