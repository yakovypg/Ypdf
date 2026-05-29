using System;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Imaging;

public readonly struct ImageCompression : IEquatable<ImageCompression>
{
    private const float _defaultQualityFactor = 0.75f;
    private const float _defaultSizeFactor = 1.0f;
    private const string _defaultExtension = "jpg";

    public ImageCompression()
        : this(
            _defaultQualityFactor,
            _defaultSizeFactor,
            null,
            null,
            _defaultExtension) { }

    public ImageCompression(
        float qualityFactor,
        float sizeFactor = _defaultSizeFactor,
        string extension = _defaultExtension)
        : this(
            qualityFactor,
            sizeFactor,
            null,
            null,
            extension) { }

    public ImageCompression(
        float qualityFactor,
        int newWidth,
        int newHeight,
        string extension = _defaultExtension)
        : this(
            qualityFactor,
            _defaultSizeFactor,
            newWidth,
            newHeight,
            extension) { }

    private ImageCompression(
        float qualityFactor,
        float sizeFactor,
        int? newWidth,
        int? newHeight,
        string extension)
    {
        DefaultExceptions.ThrowIfNotBetween(qualityFactor, 0, 1, nameof(qualityFactor));
        DefaultExceptions.ThrowIfNotBetween(sizeFactor, 0, 1, nameof(sizeFactor));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(extension, nameof(extension));

        if (newWidth is not null)
            DefaultExceptions.ThrowIfNegativeOrZero(newWidth.Value, nameof(newWidth));

        if (newHeight is not null)
            DefaultExceptions.ThrowIfNegativeOrZero(newHeight.Value, nameof(newHeight));

        QualityFactor = qualityFactor;
        SizeFactor = sizeFactor;
        NewWidth = newWidth;
        NewHeight = newHeight;
        Extension = extension;
    }

    public readonly float QualityFactor { get; }
    public readonly float SizeFactor { get; }
    public readonly int? NewWidth { get; }
    public readonly int? NewHeight { get; }
    public readonly string? Extension { get; }

    public static bool operator ==(ImageCompression left, ImageCompression right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ImageCompression left, ImageCompression right)
    {
        return !(left == right);
    }

    public bool Equals(ImageCompression other)
    {
        return QualityFactor == other.QualityFactor
            && SizeFactor == other.SizeFactor
            && NewWidth == other.NewWidth
            && NewHeight == other.NewHeight
            && Extension == other.Extension;
    }

    public override bool Equals(object? obj)
    {
        return obj is ImageCompression other
            && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(
            QualityFactor,
            SizeFactor,
            NewWidth,
            NewHeight,
            Extension);
    }
}
