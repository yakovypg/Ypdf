using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using YpdfLib.Models.Design;

namespace YpdfLib.Models.Paging
{
    public class PageParameters : IPageParameters
    {
        public int CompressionLevel { get; }
        public bool AutoIncreaseSize { get; }

        public IMargin? Margin { get; }
        public HorizontalAlignment HorizontalAlignment { get; }

        private readonly PageSize? _size = null;
        public PageSize? Size
        {
            get
            {
                if (_size is null || !AutoIncreaseSize)
                    return _size;

                float horizontalIncrease = Margin?.HorizontalSum ?? 0;
                float verticalIncrease = Margin?.VerticalSum ?? 0;

                if (horizontalIncrease == 0 && verticalIncrease == 0)
                    return _size;

                float width = _size.GetWidth() + horizontalIncrease;
                float height = _size.GetHeight() + verticalIncrease;

                return new PageSize(width, height);
            }
        }

        public PageParameters(PageSize? size = null, HorizontalAlignment horizontalAlignment = HorizontalAlignment.CENTER, IMargin? margin = null,
            bool autoIncreaseSize = true, int compressionLevel = CompressionConstants.UNDEFINED_COMPRESSION)
        {
            _size = size;

            HorizontalAlignment = horizontalAlignment;
            Margin = margin;
            AutoIncreaseSize = autoIncreaseSize;
            CompressionLevel = compressionLevel;
        }
    }
}
