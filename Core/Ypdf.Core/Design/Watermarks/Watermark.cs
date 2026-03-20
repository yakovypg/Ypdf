using System;
using iText.Kernel.Colors;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Geometry;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Watermarks;

public class Watermark : IWatermark, IEquatable<Watermark?>
{
    protected const string DefaultText = "My watermark";
    protected const double DefaultRotationAngleRadians = Math.PI / 3.0;

    static Watermark()
    {
        DefaultFontInfo = new(ColorConstants.BLACK, 72, 0.5f);
    }

    public Watermark(
        string text = DefaultText,
        double rotationAngleRadians = DefaultRotationAngleRadians)
        : this(text, DefaultFontInfo, rotationAngleRadians) { }

    public Watermark(
        string text,
        TextFontInfo fontInfo,
        double rotationAngleRadians = DefaultRotationAngleRadians,
        FloatPoint? lowerLeftPoint = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));

        Text = text;
        RotationAngleRadians = rotationAngleRadians;
        FontInfo = fontInfo;
        LowerLeftPoint = lowerLeftPoint;
    }

    public string Text { get; }
    public double RotationAngleRadians { get; protected set; }

    public TextFontInfo FontInfo { get; init; }
    public FloatPoint? LowerLeftPoint { get; init; }

    protected static TextFontInfo DefaultFontInfo { get; }

    public double GetRotationAngleInDegrees()
    {
        return Angle.RadiansToDegrees(RotationAngleRadians);
    }

    public void SetRotationAngleInDegrees(double degrees)
    {
        RotationAngleRadians = Angle.DegreesToRadians(degrees);
    }

    public bool Equals(Watermark? other)
    {
        return other is not null
            && Text == other.Text
            && RotationAngleRadians == other.RotationAngleRadians
            && FontInfo == other.FontInfo
            && LowerLeftPoint == other.LowerLeftPoint;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Watermark);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(
            Text,
            RotationAngleRadians,
            FontInfo,
            LowerLeftPoint);
    }
}
