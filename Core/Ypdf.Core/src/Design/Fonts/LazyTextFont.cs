using System;
using System.IO;
using iText.IO.Font;
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
