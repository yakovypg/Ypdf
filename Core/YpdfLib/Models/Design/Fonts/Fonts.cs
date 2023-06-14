using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace YpdfLib.Models.Design.Fonts
{
    public static class Fonts
    {
        private static readonly Dictionary<string, ILazyFont> _integratedFonts;
        public static IReadOnlyDictionary<string, ILazyFont> IntegratedFonts => _integratedFonts;

        public static Font FirstRegular => _integratedFonts.Keys.Any(t => t.ToLower().Contains("regular"))
            ? _integratedFonts.First(t => t.Key.ToLower().Contains("regular")).Value.GetFont()
            : Font.Create("TIMES_ROMAN", PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));

        public static Font FirstBold => _integratedFonts.Keys.Any(t => t.ToLower().Contains("bold"))
            ? _integratedFonts.First(t => t.Key.ToLower().Contains("bold")).Value.GetFont()
            : Font.Create("TIMES_BOLD", PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD));

        static Fonts()
        {
            _integratedFonts = new Dictionary<string, ILazyFont>();
            TryLoadFonts();
        }

        private static bool TryLoadFonts()
        {
            string fontsRootDir = SharedConfig.Directories.Fonts;

            if (!Directory.Exists(fontsRootDir))
                return false;

            try
            {
                var fontsDirInfo = new DirectoryInfo(fontsRootDir);
                var fontDirs = fontsDirInfo.GetDirectories();

                foreach (var fontDir in fontDirs)
                {
                    foreach (var fontFile in fontDir.GetFiles())
                    {
                        var font = new LazyFont(fontFile.FullName);
                        _integratedFonts.Add(font.Name, font);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
