using iText.Layout.Borders;
using iText.Kernel.Colors;

namespace YpdfLib.Models.Design
{
    public interface ILazyBorder : ILazyType<Border>, IDeepCloneable<ILazyBorder>
    {
        BorderType BorderType { get; }
        DeviceRgb Color { get; }
        
        float Thickness { get; }
        float Opacity { get; }
    }
}
