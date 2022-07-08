using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Properties;

namespace Ypdf.Converters.Configuration
{
    public struct PageParameters
    {
        public int CompressionLevel { get; set; }
        public bool AutoIncreaseSize { get; set; }

        public Margin? Margin { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }

        private PageSize? _size = null;
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
            set => _size = value;
        }

        public PageParameters()
        {
            CompressionLevel = CompressionConstants.UNDEFINED_COMPRESSION;
            AutoIncreaseSize = true;

            Margin = null;
            HorizontalAlignment = HorizontalAlignment.CENTER;
        }
    }
}
