using iText.Layout.Properties;
using YpdfLib.Models.Design;
using YpdfLib.Models.Design.Fonts;

namespace YpdfLib.Models.Paging
{
    public class TextPageParameters : ITextPageParameters
    {
        public IMargin Margin { get; set; }
        public TextAlignment TextAlignment { get; set; }

        public IFont Font { get; set; }
        public float FontSize { get; set; }

        public TextPageParameters(float fontSize = 12, TextAlignment horizontalAlignment = TextAlignment.LEFT)
            : this(fontSize, horizontalAlignment, new Margin(75.876f), Fonts.FirstRegular)
        {
        }

        public TextPageParameters(float fontSize, TextAlignment horizontalAlignment, IMargin margin, IFont font)
        {
            FontSize = fontSize;
            TextAlignment = horizontalAlignment;
            Margin = margin;
            Font = font;
        }
    }
}
