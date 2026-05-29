using System;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Extgstate;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Geometry;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Watermarks;

public class WatermarkAnnotation : Watermark, IWatermarkAnnotation, IEquatable<WatermarkAnnotation?>
{
    private float _trimmingRectangleWidth;
    private float _trimmingRectangleHeight;
    private float _formXObjWidth;
    private float _formXObjHeight;

    public WatermarkAnnotation(
        string text = DefaultText,
        double rotationAngleRadians = DefaultRotationAngleRadians)
        : this(text, DefaultFontInfo, rotationAngleRadians) { }

    public WatermarkAnnotation(
        string text,
        TextFontInfo fontInfo,
        double rotationAngleRadians = DefaultRotationAngleRadians,
        FloatPoint? lowerLeftPoint = null)
        : base(text, fontInfo, rotationAngleRadians, lowerLeftPoint)
    {
        TrimmingRectangleWidth = 300;
        TrimmingRectangleHeight = 450;
        FormXObjWidth = 300;
        FormXObjHeight = 450;
        FormXObjXOffset = 0;
        FormXObjYOffset = 0;
        XTranslation = 50;
        YTranslation = 25;
    }

    public float FormXObjXOffset { get; init; }
    public float FormXObjYOffset { get; init; }
    public float XTranslation { get; init; }
    public float YTranslation { get; init; }

    public float TrimmingRectangleWidth
    {
        get => _trimmingRectangleWidth;
        init
        {
            DefaultExceptions.ThrowIfNegativeOrZero(value, nameof(value));
            _trimmingRectangleWidth = value;
        }
    }

    public float TrimmingRectangleHeight
    {
        get => _trimmingRectangleHeight;
        init
        {
            DefaultExceptions.ThrowIfNegativeOrZero(value, nameof(value));
            _trimmingRectangleHeight = value;
        }
    }

    public float FormXObjWidth
    {
        get => _formXObjWidth;
        init
        {
            DefaultExceptions.ThrowIfNegativeOrZero(value, nameof(value));
            _formXObjWidth = value;
        }
    }

    public float FormXObjHeight
    {
        get => _formXObjHeight;
        init
        {
            DefaultExceptions.ThrowIfNegativeOrZero(value, nameof(value));
            _formXObjHeight = value;
        }
    }

    public AffineTransform Transform
    {
        get
        {
            var transform = new AffineTransform();
            transform.Translate(XTranslation, YTranslation);
            transform.Rotate(RotationAngleRadians);

            return transform;
        }
    }

    public PdfExtGState ExtGState => new PdfExtGState().SetFillOpacity(FontInfo.Opacity);
    public Rectangle FormXObjRectangle => new(FormXObjXOffset, FormXObjYOffset, FormXObjWidth, FormXObjHeight);

    public void SetWidth(float witdh)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(witdh, nameof(witdh));
        _formXObjWidth = _trimmingRectangleWidth = witdh;
    }

    public void SetHeight(float height)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(height, nameof(height));
        _formXObjHeight = _trimmingRectangleHeight = height;
    }

    public FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageSize, nameof(pageSize));

        float pageWidth = pageSize.GetWidth();
        float pageHeight = pageSize.GetHeight();

        float bottomLeftX = (pageWidth / 2) - (TrimmingRectangleWidth / 2);
        float bottomLeftY = (pageHeight / 2) - (TrimmingRectangleHeight / 2);

        return new FloatPoint(bottomLeftX, bottomLeftY);
    }

    public Rectangle GetTrimmingRectangle(float bottomLeftX, float bottomLeftY)
    {
        return new Rectangle(
            bottomLeftX,
            bottomLeftY,
            TrimmingRectangleWidth,
            TrimmingRectangleHeight);
    }

    public IIndelibleWatermark ToIndelibleWatermark()
    {
        return new IndelibleWatermark(
            FontInfo,
            TrimmingRectangleWidth,
            TrimmingRectangleHeight,
            Text,
            RotationAngleRadians,
            LowerLeftPoint);
    }

    public bool Equals(WatermarkAnnotation? other)
    {
        return other is not null
            && base.Equals(other)
            && TrimmingRectangleWidth == other.TrimmingRectangleWidth
            && TrimmingRectangleHeight == other.TrimmingRectangleHeight
            && FormXObjWidth == other.FormXObjWidth
            && FormXObjHeight == other.FormXObjHeight
            && FormXObjXOffset == other.FormXObjXOffset
            && FormXObjYOffset == other.FormXObjYOffset
            && XTranslation == other.XTranslation
            && YTranslation == other.YTranslation;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as WatermarkAnnotation);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(
            base.GetHashCode(),
            TrimmingRectangleWidth,
            TrimmingRectangleHeight,
            FormXObjWidth,
            FormXObjHeight,
            FormXObjXOffset,
            FormXObjYOffset,
            XTranslation,
            YTranslation);
    }
}
