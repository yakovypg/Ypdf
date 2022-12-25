using iText.Layout.Element;
using iText.Layout.Properties;
using YpdfLib.Models.Design;
using YpdfLib.Models.Design.Fonts;

namespace YpdfLib.Models.Paging
{
    public interface IPageNumberStyle : IDeepCloneable<IPageNumberStyle>
    {
        IFontInfo FontInfo { get; set; }

        bool ConsiderLeftPageMargin { get; set; }
        bool ConsiderTopPageMargin { get; set; }
        bool ConsiderRightPageMargin { get; set; }
        bool ConsiderBottomPageMargin { get; set; }

        TabAlignment HorizontalAlignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }

        IMargin Margin { get; set; }
        LocationMode LocationMode { get; set; }
        IPageNumberTextPresenter TextPresenter { get; set; }

        Text GetStylizedText(string text);
    }
}
