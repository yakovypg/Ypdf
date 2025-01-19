using iText.Kernel.Font;

namespace Ypdf.Core.Design.Fonts;

public interface ITextFont
{
    string Name { get; }
    PdfFont PdfFont { get; }
}
