using iText.IO.Font;
using iText.Kernel.Font;

namespace YpdfLib.Models.Design.Fonts
{
    public class Font : IFont, IEquatable<Font?>
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

        public bool Equals(Font? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   EqualityComparer<PdfFont>.Default.Equals(PdfFont, other.PdfFont);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Font);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, PdfFont);
        }
    }
}
