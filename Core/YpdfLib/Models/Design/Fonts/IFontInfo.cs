using iText.Kernel.Colors;
using iText.Kernel.Font;

namespace YpdfLib.Models.Design.Fonts
{
    public interface IFontInfo : IDeepCloneable<IFontInfo>
    {
        string? Path { get; set; }
        string Encoding { get; set; }
        float Size { get; set; }
        float Opacity { get; set; }
        string Family { get; set; }
        Color Color { get; set; }

        IFont GetFont();
        IFont GetFontOrDefault();
        ILazyFont GetLazyFont();
        ILazyFont? GetLazyFontOrNull();
        PdfFont GetPdfFont();
    }
}
