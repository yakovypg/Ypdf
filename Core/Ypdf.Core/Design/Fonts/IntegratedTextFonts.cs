using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Design.Fonts;

public class IntegratedTextFonts
{
    private const string _regularFontNamePart = "regular";
    private const string _boldFontNamePart = "bold";

    public IntegratedTextFonts(string fontsDirectoryPath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(fontsDirectoryPath, nameof(fontsDirectoryPath));
        Fonts = LoadFonts(fontsDirectoryPath);
    }

    public IReadOnlyDictionary<string, LazyTextFont> Fonts { get; }

    public bool GetFirstRegularFontOrDefault(out TextFont font)
    {
        (TextFont recievedFont, bool isDefault) = GetFontOrDefault(
            _regularFontNamePart,
            StandardFonts.TIMES_ROMAN);

        font = recievedFont;
        return !isDefault;
    }

    public bool GetFirstBoldFontOrDefault(out TextFont font)
    {
        (TextFont recievedFont, bool isDefault) = GetFontOrDefault(
            _boldFontNamePart,
            StandardFonts.TIMES_BOLD);

        font = recievedFont;
        return !isDefault;
    }

    private static Dictionary<string, LazyTextFont> LoadFonts(string fontsDirectoryPath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(fontsDirectoryPath, nameof(fontsDirectoryPath));

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

    private (TextFont Font, bool IsDefault) GetFontOrDefault(string namePart, string defaultFontFamily)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(namePart, nameof(namePart));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(defaultFontFamily, nameof(defaultFontFamily));

        if (TryGetFont(namePart, out TextFont? font) && font is not null)
            return (font, false);

        PdfFont standardFont = PdfFontFactory.CreateFont(defaultFontFamily);
        TextFont defaultFont = TextFont.Create(defaultFontFamily, standardFont);

        return (defaultFont, true);
    }

    private bool TryGetFont(string namePart, out TextFont? font)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(namePart, nameof(namePart));

        try
        {
            KeyValuePair<string, LazyTextFont> lazyFontPair = Fonts.First(t =>
            {
                return t.Key.Contains(namePart, StringComparison.OrdinalIgnoreCase);
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
}
