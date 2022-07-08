using iText.Layout.Properties;
using YpdfLib.Models.Design;
using YpdfLib.Models.Design.Fonts;

namespace YpdfLib.Models.Paging
{
    public interface ITextPageParameters
    {
        IMargin Margin { get; set; }
        TextAlignment TextAlignment { get; set; }

        IFont Font { get; set; }
        float FontSize { get; set; }
    }
}
