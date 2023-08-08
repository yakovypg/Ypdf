using iText.Layout.Borders;
using iText.Kernel.Colors;
using YpdfLib.Infrastructure.Converters;

namespace YpdfLib.Models.Design
{
    public class LazyBorder : ILazyBorder, IDeepCloneable<LazyBorder>, IEquatable<LazyBorder?>
    {
        public BorderType BorderType { get; }
        public DeviceRgb Color { get; }

        public float Thickness { get; }
        public float Opacity { get; }

        public LazyBorder(BorderType borderType) : this(borderType, new DeviceRgb(0, 0, 0), 5, 1)
        {
        }
        
        public LazyBorder(BorderType borderType, DeviceRgb color, float width = 5, float opacity = 1)
        {
            BorderType = borderType;
            Color = color;
            Thickness = width;
            Opacity = opacity;
        }
        
        public Border Create()
        {
            return BorderType switch
            {
                BorderType.Inset => new InsetBorder(Color, Thickness, Opacity),
                BorderType.Ridge => new RidgeBorder(Color, Thickness, Opacity),
                BorderType.Solid => new SolidBorder(Color, Thickness, Opacity),
                BorderType.Dashed => new DashedBorder(Color, Thickness, Opacity),
                BorderType.Dotted => new DottedBorder(Color, Thickness, Opacity),
                BorderType.Double => new DoubleBorder(Color, Thickness, Opacity),
                BorderType.Groove => new GrooveBorder(Color, Thickness, Opacity),
                BorderType.Outset => new OutsetBorder(Color, Thickness, Opacity),
                BorderType.RoundDots => new RoundDotsBorder(Color, Thickness, Opacity),
                BorderType.FixedDashed => new FixedDashedBorder(Color, Thickness, Opacity),

                _ => throw new NotSupportedException()
            };
        }

        public LazyBorder Copy()
        {
            DeviceRgb color = ColorConverter.CopyDeviceRGB(Color);
            return new LazyBorder(BorderType, color, Thickness, Opacity);
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
                   Thickness == other.Thickness &&
                   Opacity == other.Opacity;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LazyBorder);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BorderType, Color, Thickness, Opacity);
        }
    }
}
