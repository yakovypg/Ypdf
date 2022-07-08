using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Ypdf.Converters.Configuration
{
    public class PageNumberStyle
    {
        public float FontSize { get; set; } = 12;
        public string FontFamily { get; set; } = StandardFonts.TIMES_ROMAN;
        public Color FontColor { get; set; } = ColorConstants.DARK_GRAY;

        public bool ConsiderLeftPageMargin { get; set; } = true;
        public bool ConsiderTopPageMargin { get; set; } = false;
        public bool ConsiderRightPageMargin { get; set; } = true;
        public bool ConsiderBottomPageMargin { get; set; } = false;

        public TabAlignment HorizontalAlignment { get; set; } = TabAlignment.CENTER;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.BOTTOM;

        public Margin Margin { get; set; } = new Margin(0);
        public PageNumberTextPresenter TextPresenter { get; set; } = PageNumberTextPresenter.DefaultPresenter;

        public PdfFont Font => PdfFontFactory.CreateFont(FontFamily);

        public PageNumberStyle(float fontSize = 12)
        {
            FontSize = fontSize;
        }

        public Text GetStylizedText(string text)
        {
            return new Text(text)
                .SetFont(Font)
                .SetFontSize(FontSize)
                .SetFontColor(FontColor);
        }
    }
}
