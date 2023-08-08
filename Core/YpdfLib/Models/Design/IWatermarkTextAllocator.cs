using iText.Layout.Properties;

namespace YpdfLib.Models.Design
{
    public interface IWatermarkTextAllocator : IDeepCloneable<IWatermarkTextAllocator>
    {
        TextAlignment TextAlignment { get; set; }
        HorizontalAlignment TextHorizontalAlignment { get; set; }
        VerticalAlignment TextContainerVerticalAlignment { get; set; }
    }
}
