using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using Ypdf.Extensions;
using Ypdf.Paths;

namespace Ypdf.Core.Design.Fonts;

public static class IntegratedTextFonts
{
    static IntegratedTextFonts()
    {
        Fonts = LoadFonts();
    }

    public static IReadOnlyDictionary<string, LazyTextFont> Fonts { get; }

    public static TextFont FirstRegular => GetFontOrDefault("regular", StandardFonts.TIMES_ROMAN);
    public static TextFont FirstBold => GetFontOrDefault("bold", StandardFonts.TIMES_BOLD);

    private static TextFont GetFontOrDefault(string namePart, string defaultFontFamily)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(namePart, nameof(namePart));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(defaultFontFamily, nameof(defaultFontFamily));

        if (TryGetFont(namePart, out TextFont? font) && font is not null)
            return font;

        PdfFont standardFont = PdfFontFactory.CreateFont(defaultFontFamily);
        return TextFont.Create(defaultFontFamily, standardFont);
    }

    private static bool TryGetFont(string namePart, out TextFont? font)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(namePart, nameof(namePart));

        try
        {
            KeyValuePair<string, LazyTextFont> lazyFontPair = Fonts.First(t =>
            {
                string keyInLowerCase = t.Key.ToLower(CultureInfo.CurrentCulture);
                return keyInLowerCase.Contains(namePart, StringComparison.CurrentCulture);
            });

            font = lazyFontPair.Value.Create();
            return true;
        }
        catch
        {
            font = null;
            return false;
        }
    }

    private static Dictionary<string, LazyTextFont> LoadFonts()
    {
        string fontsDirectoryPath = Directories.Fonts;
        var fonts = new Dictionary<string, LazyTextFont>();

        if (!Directory.Exists(fontsDirectoryPath))
            return fonts;

        DirectoryInfo[] fontDirectories;

        try
        {
            var fontsDirectoryInfo = new DirectoryInfo(fontsDirectoryPath);
            fontDirectories = fontsDirectoryInfo.GetDirectories();
        }
        catch
        {
            return fonts;
        }

        foreach (DirectoryInfo fontDirectory in fontDirectories)
        {
            FileInfo[] fontFiles = [];

            try
            {
                fontFiles = fontDirectory.GetFiles();
            }
            catch
            {
            }

            foreach (FileInfo fontFile in fontFiles)
            {
                var font = new LazyTextFont(fontFile.FullName);
                fonts.Add(font.Name, font);
            }
        }

        return fonts;
    }
}
