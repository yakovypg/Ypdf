using iText.Layout.Properties;

namespace YpdfLib.Models.Design
{
    public class WatermarkTextAllocator : IWatermarkTextAllocator, IDeepCloneable<IWatermarkTextAllocator>, IEquatable<WatermarkTextAllocator?>
    {
        public TextAlignment TextAlignment { get; set; }
        public HorizontalAlignment TextHorizontalAlignment { get; set; }
        public VerticalAlignment TextContainerVerticalAlignment { get; set; }

        public WatermarkTextAllocator(
            TextAlignment textAlignment = TextAlignment.LEFT,
            HorizontalAlignment textHorizontalAlignment = HorizontalAlignment.LEFT,
            VerticalAlignment textContainerVerticalAlignment = VerticalAlignment.MIDDLE)
        {
            TextAlignment = textAlignment;
            TextHorizontalAlignment = textHorizontalAlignment;
            TextContainerVerticalAlignment = textContainerVerticalAlignment;
        }

        public WatermarkTextAllocator Copy()
        {
            return new WatermarkTextAllocator(
                TextAlignment,
                TextHorizontalAlignment,
                TextContainerVerticalAlignment
            );
        }

        IWatermarkTextAllocator IDeepCloneable<IWatermarkTextAllocator>.Copy()
        {
            return Copy();
        }

        public bool Equals(WatermarkTextAllocator? other)
        {
            return other is not null &&
                   TextAlignment == other.TextAlignment &&
                   TextHorizontalAlignment == other.TextHorizontalAlignment &&
                   TextContainerVerticalAlignment == other.TextContainerVerticalAlignment;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WatermarkTextAllocator);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TextAlignment,
                TextHorizontalAlignment, TextContainerVerticalAlignment);
        }
    }
}
