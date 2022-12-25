using iText.IO.Font;

namespace YpdfLib.Models.Design.Fonts
{
    public class LazyFont : ILazyFont, IDeepCloneable<LazyFont>, IEquatable<LazyFont?>
    {
        private readonly string _path;
        private readonly string _encoding;

        public string Name { get; }

        public LazyFont(string path, string encoding = PdfEncodings.IDENTITY_H)
        {
            _path = path;
            _encoding = encoding;

            Name = Path.GetFileNameWithoutExtension(path);
        }

        public Font GetFont()
        {
            return Font.Create(_path, _encoding);
        }

        public LazyFont Copy()
        {
            return new LazyFont(_path, _encoding);
        }

        ILazyFont IDeepCloneable<ILazyFont>.Copy()
        {
            return Copy();
        }

        public bool Equals(LazyFont? other)
        {
            return other is not null &&
                   _path == other._path &&
                   _encoding == other._encoding &&
                   Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LazyFont);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_path, _encoding, Name);
        }
    }
}
