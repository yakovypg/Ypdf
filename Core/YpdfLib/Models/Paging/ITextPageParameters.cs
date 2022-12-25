using iText.Kernel.Geom;
using iText.Layout.Properties;
using YpdfLib.Models.Design;
using YpdfLib.Models.Design.Fonts;

namespace YpdfLib.Models.Paging
{
    public interface ITextPageParameters : IDeepCloneable<ITextPageParameters>
    {
        IFontInfo FontInfo { get; set; }
        TextAlignment TextAlignment { get; set; }
        IMargin Margin { get; set; }

        PageSize? PageSize { get; set; }
    }
}
