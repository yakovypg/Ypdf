using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;

namespace YpdfLib.Models.Design.Fonts
{
    public class FontInfo : IFontInfo, IDeepCloneable<FontInfo>, IEquatable<FontInfo?>
    {
        public string? Path { get; set; }
        public string Encoding { get; set; }
        public float Size { get; set; }
        public float Opacity { get; set; }
        public string Family { get; set; }
        public Color Color { get; set; }

        public FontInfo(string? path = null, string encoding = PdfEncodings.IDENTITY_H, float size = 12f, string family = StandardFonts.TIMES_ROMAN) :
            this(path, encoding, size, 1f, family, ColorConstants.DARK_GRAY)
        {
        }

        public FontInfo(string? path, string encoding, float size, float opacity, string family, Color color)
        {
            Path = path;
            Encoding = encoding;
            Size = size;
            Opacity = opacity;
            Family = family;
            Color = color;
        }

        public IFont GetFont()
        {
            return !string.IsNullOrEmpty(Path)
                ? Font.Create(Path, Encoding)
                : throw new ArgumentNullException(nameof(Path));
        }

        public IFont GetFontOrDefault()
        {
            return !string.IsNullOrEmpty(Path)
                ? Font.Create(Path, Encoding)
                : Fonts.FirstRegular;
        }

        public ILazyFont GetLazyFont()
        {
            return !string.IsNullOrEmpty(Path)
                ? new LazyFont(Path, Encoding)
                : throw new ArgumentNullException(nameof(Path));
        }

        public ILazyFont? GetLazyFontOrNull()
        {
            return !string.IsNullOrEmpty(Path)
                ? new LazyFont(Path, Encoding)
                : null;
        }

        public PdfFont GetPdfFont()
        {
            return !string.IsNullOrEmpty(Path)
                ? GetFont().PdfFont
                : PdfFontFactory.CreateFont(Family);
        }

        public FontInfo Copy()
        {
            return new FontInfo(Path, Encoding, Size, Opacity, Family, Color);
        }

        IFontInfo IDeepCloneable<IFontInfo>.Copy()
        {
            return Copy();
        }

        public bool Equals(FontInfo? other)
        {
            return other is not null &&
                   Path == other.Path &&
                   Encoding == other.Encoding &&
                   Size == other.Size &&
                   Opacity == other.Opacity &&
                   Family == other.Family &&
                   EqualityComparer<Color>.Default.Equals(Color, other.Color);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FontInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, Encoding, Size, Opacity, Family, Color);
        }
    }
}
