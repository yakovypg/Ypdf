using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using YpdfLib.Extensions;
using YpdfLib.Models.Design;

namespace YpdfLib.Models.Paging
{
    public class ImagePageParameters : IImagePageParameters, IDeepCloneable<ImagePageParameters>, IEquatable<ImagePageParameters?>
    {
        public double RotationAngle { get; set; }
        public bool AutoIncreaseSize { get; set; }

        public IMargin? Margin { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }

        private PageSize? _size;
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
            set
            {
                _size = value;
            }
        }

        public ImagePageParameters(PageSize? size = null, HorizontalAlignment horizontalAlignment = HorizontalAlignment.CENTER, IMargin? margin = null, bool autoIncreaseSize = true, double rotationAngle = 0)
        {
            Size = size;
            HorizontalAlignment = horizontalAlignment;
            Margin = margin;
            AutoIncreaseSize = autoIncreaseSize;
            RotationAngle = rotationAngle;
        }

        public void ApplyStyleToImage(Image image)
        {
            image.SetAutoScale(true);
            image.SetRotationAngle(RotationAngle);
            image.SetHorizontalAlignment(HorizontalAlignment);
        }

        public ImagePageParameters Copy()
        {
            return new ImagePageParameters()
            {
                RotationAngle = RotationAngle,
                AutoIncreaseSize = AutoIncreaseSize,
                Margin = Margin?.Copy(),
                HorizontalAlignment = HorizontalAlignment,
                Size = _size?.DeepClone()
            };
        }

        IImagePageParameters IDeepCloneable<IImagePageParameters>.Copy()
        {
            return Copy();
        }

        public bool Equals(ImagePageParameters? other)
        {
            return other is not null &&
                   RotationAngle == other.RotationAngle &&
                   AutoIncreaseSize == other.AutoIncreaseSize &&
                   EqualityComparer<IMargin?>.Default.Equals(Margin, other.Margin) &&
                   HorizontalAlignment == other.HorizontalAlignment &&
                   EqualityComparer<PageSize?>.Default.Equals(_size, other._size);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ImagePageParameters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RotationAngle, AutoIncreaseSize, Margin, HorizontalAlignment, _size);
        }
    }
}
