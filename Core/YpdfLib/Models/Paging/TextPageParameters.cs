using iText.Kernel.Geom;
using iText.Layout.Properties;
using YpdfLib.Models.Design;
using YpdfLib.Models.Design.Fonts;

namespace YpdfLib.Models.Paging
{
    public class TextPageParameters : ITextPageParameters, IDeepCloneable<TextPageParameters>, IEquatable<TextPageParameters?>
    {
        public IFontInfo FontInfo { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public IMargin Margin { get; set; }

        public PageSize? PageSize { get; set; }

        public TextPageParameters(TextAlignment horizontalAlignment = TextAlignment.LEFT)
            : this(new FontInfo(), horizontalAlignment, new Margin(75.876f))
        {
        }

        public TextPageParameters(IFontInfo fontInfo, TextAlignment textAlignment, IMargin margin, PageSize? pageSize = null)
        {
            FontInfo = fontInfo;
            TextAlignment = textAlignment;
            Margin = margin;
            PageSize = pageSize;
        }

        public TextPageParameters Copy()
        {
            return new TextPageParameters()
            {
                FontInfo = FontInfo.Copy(),
                TextAlignment = TextAlignment,
                Margin = Margin,
                PageSize = PageSize
            };
        }

        ITextPageParameters IDeepCloneable<ITextPageParameters>.Copy()
        {
            return Copy();
        }

        public bool Equals(TextPageParameters? other)
        {
            return other is not null &&
                   EqualityComparer<IFontInfo>.Default.Equals(FontInfo, other.FontInfo) &&
                   TextAlignment == other.TextAlignment &&
                   EqualityComparer<IMargin>.Default.Equals(Margin, other.Margin) &&
                   EqualityComparer<PageSize?>.Default.Equals(PageSize, other.PageSize);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TextPageParameters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FontInfo, TextAlignment, Margin, PageSize);
        }
    }
}
