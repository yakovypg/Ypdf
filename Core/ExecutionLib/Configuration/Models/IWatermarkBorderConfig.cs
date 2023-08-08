using iText.Kernel.Colors;
using YpdfLib.Models;
using YpdfLib.Models.Design;

namespace ExecutionLib.Configuration.Models
{
    public interface IWatermarkBorderConfig : ILazyType<LazyBorder>, IDeepCloneable<IWatermarkBorderConfig>
    {
        bool IsConfigured { get; }
        
        BorderType BorderType { get; set; }
        Color Color { get; set; }

        float Thickness { get; set; }
        float Opacity { get; set; }
    }
}
