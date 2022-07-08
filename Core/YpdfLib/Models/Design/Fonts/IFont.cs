using iText.Kernel.Font;

namespace YpdfLib.Models.Design.Fonts
{
    public interface IFont
    {
        string Name { get; }
        PdfFont PdfFont { get; }
    }
}
