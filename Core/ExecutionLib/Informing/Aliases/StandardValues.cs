using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using YpdfLib.Models.Paging;

namespace ExecutionLib.Informing.Aliases
{
    public static class StandardValues
    {
        public static string ColorNames => string.Join('/', _colors.Keys.Select(t => t.ToLower()));
        public static string PageSizeNames => string.Join('/', _pageSizes.Keys.Select(t => t.ToLower()));
        public static string FontFamilyNames => string.Join('/', _fontFamilies.Keys.Select(t => t.ToLower()));
        public static string EncodingNames => string.Join('/', _encodings.Keys.Select(t => t.ToLower()));
        public static string EncryptionAlgorithmNames => string.Join('/', _encryptionAlgorithms.Keys.Select(t => t.ToLower()));
        public static string PageNumberTextPresenterNames => string.Join('/', _pageNumberTextPresenters.Keys.Select(t => t.ToLower()));

        public static IReadOnlyDictionary<string, Color> Colors => _colors;
        public static IReadOnlyDictionary<string, PageSize> PageSizes => _pageSizes;
        public static IReadOnlyDictionary<string, string> FontFamilies => _fontFamilies;
        public static IReadOnlyDictionary<string, string> Encodings => _encodings;
        public static IReadOnlyDictionary<string, int> EncryptionAlgorithms => _encryptionAlgorithms;
        public static IReadOnlyDictionary<string, PageNumberTextPresenter> PageNumberTextPresenters => _pageNumberTextPresenters;

        private static readonly Dictionary<string, Color> _colors = new()
        {
            { "BLACK", ColorConstants.BLACK },
            { "BLUE", ColorConstants.BLUE },
            { "CYAN", ColorConstants.CYAN },
            { "DARK_GRAY", ColorConstants.DARK_GRAY },
            { "GRAY", ColorConstants.GRAY },
            { "GREEN", ColorConstants.GREEN },
            { "LIGHT_GRAY", ColorConstants.LIGHT_GRAY },
            { "MAGENTA", ColorConstants.MAGENTA },
            { "ORANGE", ColorConstants.ORANGE },
            { "PINK", ColorConstants.PINK },
            { "RED", ColorConstants.RED },
            { "WHITE", ColorConstants.WHITE },
            { "YELLOW", ColorConstants.YELLOW }
        };

        private static readonly Dictionary<string, PageSize> _pageSizes = new()
        {
            { "A0", PageSize.A0 },
            { "A1", PageSize.A1 },
            { "A2", PageSize.A2 },
            { "A3", PageSize.A3 },
            { "A4", PageSize.A4 },
            { "A5", PageSize.A5 },
            { "A6", PageSize.A6 },
            { "A7", PageSize.A7 },
            { "A8", PageSize.A8 },
            { "A9", PageSize.A9 },
            { "A10", PageSize.A10 },
            { "B0", PageSize.B0 },
            { "B1", PageSize.B1 },
            { "B2", PageSize.B2 },
            { "B3", PageSize.B3 },
            { "B4", PageSize.B4 },
            { "B5", PageSize.B5 },
            { "B6", PageSize.B6 },
            { "B7", PageSize.B7 },
            { "B8", PageSize.B8 },
            { "B9", PageSize.B9 },
            { "B10", PageSize.B10 },
            { "DEFAULT", PageSize.DEFAULT },
            { "EXECUTIVE", PageSize.EXECUTIVE },
            { "LEDGER", PageSize.LEDGER },
            { "LEGAL", PageSize.LEGAL },
            { "LETTER", PageSize.LETTER },
            { "TABLOID", PageSize.TABLOID }
        };

        private static readonly Dictionary<string, string> _fontFamilies = new()
        {
            { "COURIER", StandardFonts.COURIER },
            { "COURIER_BOLD", StandardFonts.COURIER_BOLD },
            { "COURIER_OBLIQUE", StandardFonts.COURIER_OBLIQUE },
            { "COURIER_BOLDOBLIQUE", StandardFonts.COURIER_BOLDOBLIQUE },
            { "HELVETICA", StandardFonts.HELVETICA },
            { "HELVETICA_BOLD", StandardFonts.HELVETICA_BOLD },
            { "HELVETICA_OBLIQUE", StandardFonts.HELVETICA_OBLIQUE },
            { "HELVETICA_BOLDOBLIQUE", StandardFonts.HELVETICA_BOLDOBLIQUE },
            { "SYMBOL", StandardFonts.SYMBOL },
            { "TIMES_ROMAN", StandardFonts.TIMES_ROMAN },
            { "TIMES_BOLD", StandardFonts.TIMES_BOLD },
            { "TIMES_ITALIC", StandardFonts.TIMES_ITALIC },
            { "TIMES_BOLDITALIC", StandardFonts.TIMES_BOLDITALIC },
            { "ZAPFDINGBATS", StandardFonts.ZAPFDINGBATS }
        };

        private static readonly Dictionary<string, string> _encodings = new()
        {
            { "CP1250", PdfEncodings.CP1250 },
            { "CP1252", PdfEncodings.CP1252 },
            { "CP1253", PdfEncodings.CP1253 },
            { "CP1257", PdfEncodings.CP1257 },
            { "IDENTITY_H", PdfEncodings.IDENTITY_H },
            { "IDENTITY_V", PdfEncodings.IDENTITY_V },
            { "MACROMAN", PdfEncodings.MACROMAN },
            { "PDF_DOC_ENCODING", PdfEncodings.PDF_DOC_ENCODING },
            { "SYMBOL", PdfEncodings.SYMBOL },
            { "UNICODE_BIG", PdfEncodings.UNICODE_BIG },
            { "UNICODE_BIG_UNMARKED", PdfEncodings.UNICODE_BIG_UNMARKED },
            { "UTF8", PdfEncodings.UTF8 },
            { "WINANSI", PdfEncodings.WINANSI },
            { "ZAPFDINGBATS", PdfEncodings.ZAPFDINGBATS }
        };

        private static readonly Dictionary<string, int> _encryptionAlgorithms = new()
        {
            { "AES_128", EncryptionConstants.ENCRYPTION_AES_128 },
            { "AES_256", EncryptionConstants.ENCRYPTION_AES_256 },
            { "STANDARD_40", EncryptionConstants.STANDARD_ENCRYPTION_40 },
            { "STANDARD_128", EncryptionConstants.STANDARD_ENCRYPTION_128 }
        };

        private static readonly Dictionary<string, PageNumberTextPresenter> _pageNumberTextPresenters = new()
        {
            { "DEFAULT", PageNumberTextPresenter.DefaultPresenter },
            { "FRACTIONAL", PageNumberTextPresenter.FractionalPresenter },
            { "VERBAL", PageNumberTextPresenter.VerbalPresenter }
        };
    }
}
