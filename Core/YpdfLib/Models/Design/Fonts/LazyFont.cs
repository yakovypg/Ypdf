using iText.IO.Font;

namespace YpdfLib.Models.Design.Fonts
{
    internal class LazyFont : ILazyFont
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
    }
}
