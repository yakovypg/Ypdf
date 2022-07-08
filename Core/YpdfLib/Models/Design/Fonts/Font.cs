using iText.IO.Font;
using iText.Kernel.Font;

namespace YpdfLib.Models.Design.Fonts
{
    public class Font : IFont
    {
        public string Name { get; }
        public PdfFont PdfFont { get; }

        protected Font(string name, PdfFont pdfFont)
        {
            Name = name;
            PdfFont = pdfFont;
        }

        internal static Font Create(string name, PdfFont pdfFont)
        {
            return new Font(name, pdfFont);
        }

        public static Font Create(string fontFilePath, string encoding = PdfEncodings.IDENTITY_H)
        {
            string name = Path.GetFileNameWithoutExtension(fontFilePath);
            PdfFont pdfFont = PdfFontFactory.CreateFont(fontFilePath, encoding);

            return new Font(name, pdfFont);
        }
    }
}
