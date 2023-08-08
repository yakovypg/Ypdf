using iText.Layout.Properties;
using iText.Kernel.Geom;
using YpdfLib.Models.Design.Fonts;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Models.Design
{
    public class IndelibleWatermark : Watermark, IIndelibleWatermark, IDeepCloneable<IndelibleWatermark>, IEquatable<IndelibleWatermark?>
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public IWatermarkTextAllocator TextAllocator { get; set; }
        public ILazyBorder? Border { get; set; }

        public IndelibleWatermark(float width, float height, WatermarkTextAllocator? textAllocator = null) : base()
        {
            Width = width;
            Height = height;
            TextAllocator = textAllocator ?? new WatermarkTextAllocator();
        }

        public IndelibleWatermark(float width, float height, string text, double rotationAngle, IFontInfo fontInfo, FloatPoint? lowerLeftPoint = null, WatermarkTextAllocator? textAllocator = null) :
            base(text, rotationAngle, fontInfo, lowerLeftPoint)
        {
            Width = width;
            Height = height;
            TextAllocator = textAllocator ?? new WatermarkTextAllocator();
        }

        public FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize)
        {
            float pageWidth = pageSize.GetWidth();
            float pageHeight = pageSize.GetHeight();

            float bottomLeftX = pageWidth / 2 - Width / 2;
            float bottomLeftY = pageHeight / 2 - Height / 2;

            return new FloatPoint(bottomLeftX, bottomLeftY);
        }

        public new IndelibleWatermark Copy()
        {
            return new IndelibleWatermark(Width, Height)
            {
                Text = Text,
                RotationAngle = RotationAngle,
                FontInfo = FontInfo.Copy(),
                LowerLeftPoint = LowerLeftPoint,
                TextAllocator = TextAllocator.Copy(),
                Border = Border?.Copy(),
            };
        }

        IIndelibleWatermark IDeepCloneable<IIndelibleWatermark>.Copy()
        {
            return Copy();
        }

        public bool Equals(IndelibleWatermark? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Text == other.Text &&
                   RotationAngle == other.RotationAngle &&
                   EqualityComparer<IFontInfo>.Default.Equals(FontInfo, other.FontInfo) &&
                   EqualityComparer<FloatPoint?>.Default.Equals(LowerLeftPoint, other.LowerLeftPoint) &&
                   Width == other.Width &&
                   Height == other.Height &&
                   EqualityComparer<IWatermarkTextAllocator>.Default.Equals(TextAllocator, other.TextAllocator) &&
                   EqualityComparer<ILazyBorder>.Default.Equals(Border, other.Border);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as IndelibleWatermark);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(base.GetHashCode());
            hash.Add(Text);
            hash.Add(RotationAngle);
            hash.Add(FontInfo);
            hash.Add(LowerLeftPoint);
            hash.Add(Width);
            hash.Add(Height);
            hash.Add(TextAllocator);
            hash.Add(Border);

            return hash.ToHashCode();
        }
    }
}
