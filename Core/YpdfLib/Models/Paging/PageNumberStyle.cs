using iText.Layout.Element;
using iText.Layout.Properties;
using YpdfLib.Models.Design;
using YpdfLib.Models.Design.Fonts;

namespace YpdfLib.Models.Paging
{
    public class PageNumberStyle : IPageNumberStyle, IDeepCloneable<PageNumberStyle>, IEquatable<PageNumberStyle?>
    {
        public IFontInfo FontInfo { get; set; }

        public bool ConsiderLeftPageMargin { get; set; } = true;
        public bool ConsiderTopPageMargin { get; set; } = false;
        public bool ConsiderRightPageMargin { get; set; } = true;
        public bool ConsiderBottomPageMargin { get; set; } = false;

        public TabAlignment HorizontalAlignment { get; set; } = TabAlignment.CENTER;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.BOTTOM;

        public IMargin Margin { get; set; } = new Margin(0);
        public LocationMode LocationMode { get; set; } = LocationMode.DEFAULT;
        public IPageNumberTextPresenter TextPresenter { get; set; } = PageNumberTextPresenter.DefaultPresenter;

        public PageNumberStyle() : this(new FontInfo())
        {
        }

        public PageNumberStyle(IFontInfo fontInfo)
        {
            FontInfo = fontInfo;
        }

        public Text GetStylizedText(string text)
        {
            return new Text(text)
                .SetFontSize(FontInfo.Size)
                .SetFontColor(FontInfo.Color)
                .SetOpacity(FontInfo.Opacity)
                .SetFont(FontInfo.GetPdfFont());
        }

        public PageNumberStyle Copy()
        {
            return new PageNumberStyle()
            {
                FontInfo = FontInfo.Copy(),
                ConsiderLeftPageMargin = ConsiderLeftPageMargin,
                ConsiderTopPageMargin = ConsiderTopPageMargin,
                ConsiderRightPageMargin = ConsiderRightPageMargin,
                ConsiderBottomPageMargin = ConsiderBottomPageMargin,
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment,
                Margin = Margin.Copy(),
                LocationMode = LocationMode,
                TextPresenter = TextPresenter.Copy()
            };
        }

        IPageNumberStyle IDeepCloneable<IPageNumberStyle>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageNumberStyle? other)
        {
            return other is not null &&
                   EqualityComparer<IFontInfo>.Default.Equals(FontInfo, other.FontInfo) &&
                   ConsiderLeftPageMargin == other.ConsiderLeftPageMargin &&
                   ConsiderTopPageMargin == other.ConsiderTopPageMargin &&
                   ConsiderRightPageMargin == other.ConsiderRightPageMargin &&
                   ConsiderBottomPageMargin == other.ConsiderBottomPageMargin &&
                   HorizontalAlignment == other.HorizontalAlignment &&
                   VerticalAlignment == other.VerticalAlignment &&
                   EqualityComparer<IMargin>.Default.Equals(Margin, other.Margin) &&
                   LocationMode == other.LocationMode &&
                   EqualityComparer<IPageNumberTextPresenter>.Default.Equals(TextPresenter, other.TextPresenter);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageNumberStyle);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(FontInfo);
            hash.Add(ConsiderLeftPageMargin);
            hash.Add(ConsiderTopPageMargin);
            hash.Add(ConsiderRightPageMargin);
            hash.Add(ConsiderBottomPageMargin);
            hash.Add(HorizontalAlignment);
            hash.Add(VerticalAlignment);
            hash.Add(Margin);
            hash.Add(LocationMode);
            hash.Add(TextPresenter);

            return hash.ToHashCode();
        }
    }
}
