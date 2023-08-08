using iText.Layout.Borders;
using iText.Kernel.Colors;

namespace YpdfLib.Models.Design
{
    public interface ILazyBorder : ILazyType<Border>, IDeepCloneable<ILazyBorder>
    {
        BorderType BorderType { get; }
        DeviceRgb Color { get; }
        
        float Width { get; }
        float Opacity { get; }
    }
}
