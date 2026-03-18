using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Fonts;

public class TextFont : ITextFont, IEquatable<TextFont?>
{
    protected TextFont(string name, PdfFont pdfFont)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ExtendedArgumentNullException.ThrowIfNull(pdfFont, nameof(pdfFont));

        Name = name;
        PdfFont = pdfFont;
    }

    public string Name { get; }
    public PdfFont PdfFont { get; }

    public static TextFont Create(string fontFilePath, string encoding = PdfEncodings.IDENTITY_H)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(fontFilePath, nameof(fontFilePath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(encoding, nameof(encoding));

        string name = Path.GetFileNameWithoutExtension(fontFilePath);
        PdfFont pdfFont = PdfFontFactory.CreateFont(fontFilePath, encoding);

        return new TextFont(name, pdfFont);
    }

    public bool Equals(TextFont? other)
    {
        return other is not null
            && Name == other.Name
            && EqualityComparer<PdfFont>.Default.Equals(PdfFont, other.PdfFont);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TextFont);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(Name, PdfFont);
    }

    internal static TextFont Create(string name, PdfFont pdfFont)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ExtendedArgumentNullException.ThrowIfNull(pdfFont, nameof(pdfFont));

        return new TextFont(name, pdfFont);
    }
}
