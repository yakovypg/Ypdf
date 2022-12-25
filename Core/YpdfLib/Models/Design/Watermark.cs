using YpdfLib.Models.Design.Fonts;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Models.Design
{
    public class Watermark : IWatermark, IDeepCloneable<Watermark>, IEquatable<Watermark?>
    {
        protected const string DEFAULT_TEXT = "My watermark";
        protected const double DEFAULT_ROTATION_ANGLE = Math.PI / 3.0;

        public string Text { get; set; }
        public double RotationAngle { get; set; }

        public IFontInfo FontInfo { get; set; }
        public FloatPoint? LowerLeftPoint { get; set; }

        public Watermark(string text = DEFAULT_TEXT, double rotationAngle = DEFAULT_ROTATION_ANGLE) :
            this(text, rotationAngle, new FontInfo() { Size = 72, Opacity = 0.5f })
        {
        }

        public Watermark(string text, double rotationAngle, IFontInfo fontInfo, FloatPoint? lowerLeftPoint = null)
        {
            Text = text;
            RotationAngle = rotationAngle;
            FontInfo = fontInfo;
            LowerLeftPoint = lowerLeftPoint;
        }

        public double GetRotationAngleInDegrees()
        {
            return Angle.RadiansToDegrees(RotationAngle);
        }

        public void SetRotationAngleInDegrees(double degrees)
        {
            RotationAngle = Angle.DegreesToRadians(degrees);
        }

        public Watermark Copy()
        {
            return new Watermark()
            {
                Text = Text,
                RotationAngle = RotationAngle,
                FontInfo = FontInfo.Copy(),
                LowerLeftPoint = LowerLeftPoint
            };
        }

        public bool Equals(Watermark? other)
        {
            return other is not null &&
                   Text == other.Text &&
                   RotationAngle == other.RotationAngle &&
                   EqualityComparer<IFontInfo>.Default.Equals(FontInfo, other.FontInfo) &&
                   EqualityComparer<FloatPoint?>.Default.Equals(LowerLeftPoint, other.LowerLeftPoint);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Watermark);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, RotationAngle, FontInfo, LowerLeftPoint);
        }
    }
}
