using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Fonts;

public readonly struct LazyTextFont : IEquatable<LazyTextFont>
{
    private readonly string _path;
    private readonly string _encoding;

    public LazyTextFont(string path, string encoding = PdfEncodings.IDENTITY_H)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(encoding, nameof(encoding));

        _path = path;
        _encoding = encoding;

        Name = Path.GetFileNameWithoutExtension(path);
    }

    public readonly string Name { get; }

    public static bool operator ==(LazyTextFont left, LazyTextFont right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(LazyTextFont left, LazyTextFont right)
    {
        return !(left == right);
    }

    public readonly TextFont Create()
    {
        return TextFont.Create(_path, _encoding);
    }

    public readonly TextFontInfo CreateTextFontInfo(float size = 12, float opacity = 1)
    {
        return CreateTextFontInfo(ColorConstants.DARK_GRAY, size, opacity);
    }

    public readonly TextFontInfo CreateTextFontInfo(Color color, float size = 12, float opacity = 1)
    {
        return new TextFontInfo(_path, _encoding, color, size, opacity);
    }

    public readonly bool Equals(LazyTextFont other)
    {
        return _path == other._path
            && _encoding == other._encoding;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is LazyTextFont other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(_path, _encoding);
    }
}
