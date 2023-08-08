using iText.Layout.Borders;
using iText.Kernel.Colors;
using YpdfLib.Infrastructure.Converters;

namespace YpdfLib.Models.Design
{
    public class LazyBorder : ILazyBorder, IDeepCloneable<LazyBorder>, IEquatable<LazyBorder?>
    {
        public BorderType BorderType { get; }
        public DeviceRgb Color { get; }

        public float Width { get; }
        public float Opacity { get; }

        public LazyBorder(BorderType borderType) : this(borderType, new DeviceRgb(0, 0, 0), 5, 1)
        {
        }
        
        public LazyBorder(BorderType borderType, DeviceRgb color, float width = 5, float opacity = 1)
        {
            BorderType = borderType;
            Color = color;
            Width = width;
            Opacity = opacity;
        }
        
        public Border Create()
        {
            return BorderType switch
            {
                BorderType.Inset => new InsetBorder(Color, Width, Opacity),
                BorderType.Ridge => new RidgeBorder(Color, Width, Opacity),
                BorderType.Solid => new SolidBorder(Color, Width, Opacity),
                BorderType.Dashed => new DashedBorder(Color, Width, Opacity),
                BorderType.Dotted => new DottedBorder(Color, Width, Opacity),
                BorderType.Double => new DoubleBorder(Color, Width, Opacity),
                BorderType.Groove => new GrooveBorder(Color, Width, Opacity),
                BorderType.Outset => new OutsetBorder(Color, Width, Opacity),
                BorderType.RoundDots => new RoundDotsBorder(Color, Width, Opacity),
                BorderType.FixedDashed => new FixedDashedBorder(Color, Width, Opacity),

                _ => throw new NotSupportedException()
            };
        }

        public LazyBorder Copy()
        {
            DeviceRgb color = ColorConverter.CopyDeviceRGB(Color);
            return new LazyBorder(BorderType, color, Width, Opacity);
        }

        ILazyBorder IDeepCloneable<ILazyBorder>.Copy()
        {
            return Copy();
        }

        public bool Equals(LazyBorder? other)
        {
            return other is not null &&
                   BorderType == other.BorderType &&
                   Color.Equals(other.Color) &&
                   Width == other.Width &&
                   Opacity == other.Opacity;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LazyBorder);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BorderType, Color, Width, Opacity);
        }
    }
}
