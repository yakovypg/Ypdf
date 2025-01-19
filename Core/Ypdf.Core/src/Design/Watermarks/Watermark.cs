using System;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Geometry;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Watermarks;

public class Watermark : IWatermark, IEquatable<Watermark?>
{
    protected const string DefaultText = "My watermark";
    protected const double DefaultRotationAngleInRadians = Math.PI / 3.0;

    static Watermark()
    {
        DefaultFontInfo = new()
        {
            Size = 72,
            Opacity = 0.5f
        };
    }

    public Watermark(
        string text = DefaultText,
        double rotationAngleInRadians = DefaultRotationAngleInRadians)
        : this(text, DefaultFontInfo, rotationAngleInRadians) { }

    public Watermark(
        string text,
        TextFontInfo fontInfo,
        double rotationAngleInRadians = DefaultRotationAngleInRadians,
        FloatPoint? lowerLeftPoint = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));

        Text = text;
        RotationAngleInRadians = rotationAngleInRadians;
        FontInfo = fontInfo;
        LowerLeftPoint = lowerLeftPoint;
    }

    public string Text { get; }
    public double RotationAngleInRadians { get; protected set; }

    public TextFontInfo FontInfo { get; init; }
    public FloatPoint? LowerLeftPoint { get; init; }

    protected static TextFontInfo DefaultFontInfo { get; }

    public double GetRotationAngleInDegrees()
    {
        return Angle.RadiansToDegrees(RotationAngleInRadians);
    }

    public void SetRotationAngleInDegrees(double degrees)
    {
        RotationAngleInRadians = Angle.DegreesToRadians(degrees);
    }

    public bool Equals(Watermark? other)
    {
        return other is not null
            && Text == other.Text
            && RotationAngleInRadians == other.RotationAngleInRadians
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
            RotationAngleInRadians,
            FontInfo,
            LowerLeftPoint);
    }
}
