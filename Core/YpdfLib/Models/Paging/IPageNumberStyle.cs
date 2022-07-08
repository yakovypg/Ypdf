using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Layout.Properties;
using YpdfLib.Models.Design;

namespace YpdfLib.Models.Paging
{
    public interface IPageNumberStyle
    {
        float FontSize { get; set; }
        string FontFamily { get; set; }
        Color FontColor { get; set; }

        bool ConsiderLeftPageMargin { get; set; }
        bool ConsiderTopPageMargin { get; set; }
        bool ConsiderRightPageMargin { get; set; }
        bool ConsiderBottomPageMargin { get; set; }

        TabAlignment HorizontalAlignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }

        IMargin Margin { get; set; }
        IPageNumberTextPresenter TextPresenter { get; set; }

        PdfFont Font { get; }

        Text GetStylizedText(string text);
    }
}
