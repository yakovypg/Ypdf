using System;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public class ImagePageParameters : PageParameters, IImagePageParameters, IEquatable<ImagePageParameters?>
{
    private readonly PageSize? _size;

    public ImagePageParameters(
        Margin? margin = null,
        PageSize? pageSize = null,
        double rotationAngleDegrees = 0,
        bool autoIncreaseSize = true,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.CENTER)
        : base(margin, pageSize)
    {
        RotationAngleDegrees = rotationAngleDegrees;
        AutoIncreaseSize = autoIncreaseSize;
        HorizontalAlignment = horizontalAlignment;
    }

    public double RotationAngleDegrees { get; init; }
    public bool AutoIncreaseSize { get; init; }
    public HorizontalAlignment HorizontalAlignment { get; init; }

    public override PageSize? Size
    {
        get
        {
            if (_size is null || !AutoIncreaseSize)
                return _size;

            float horizontalIncrease = Margin?.HorizontalSum ?? 0;
            float verticalIncrease = Margin?.VerticalSum ?? 0;

            if (horizontalIncrease == 0 && verticalIncrease == 0)
                return _size;

            float width = _size.GetWidth() + horizontalIncrease;
            float height = _size.GetHeight() + verticalIncrease;

            return new PageSize(width, height);
        }
        init
        {
            _size = value;
        }
    }

    public void ApplyStyleToImage(Image image)
    {
        ExtendedArgumentNullException.ThrowIfNull(image, nameof(image));

        image.SetAutoScale(true);
        image.SetRotationAngle(RotationAngleDegrees);
        image.SetHorizontalAlignment(HorizontalAlignment);
    }

    public bool Equals(ImagePageParameters? other)
    {
        return other is not null
            && base.Equals(other)
            && RotationAngleDegrees == other.RotationAngleDegrees
            && AutoIncreaseSize == other.AutoIncreaseSize
            && HorizontalAlignment == other.HorizontalAlignment;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ImagePageParameters);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(
            base.GetHashCode(),
            RotationAngleDegrees,
            AutoIncreaseSize,
            HorizontalAlignment);
    }
}
