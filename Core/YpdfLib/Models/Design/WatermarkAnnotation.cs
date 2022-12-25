using iText.Kernel.Geom;
using iText.Kernel.Pdf.Extgstate;
using YpdfLib.Models.Design.Fonts;
using YpdfLib.Models.Geometry;

namespace YpdfLib.Models.Design
{
    public class WatermarkAnnotation : Watermark, IWatermarkAnnotation, IDeepCloneable<WatermarkAnnotation>, IEquatable<WatermarkAnnotation?>
    {
        public float TrimmingRectangleWidth { get; set; } = 300;
        public float TrimmingRectangleHeight { get; set; } = 450;

        public float FormXObjWidth { get; set; } = 300;
        public float FormXObjHeight { get; set; } = 450;
        public float FormXObjXOffset { get; set; } = 0;
        public float FormXObjYOffset { get; set; } = 0;

        public float XTranslation { get; set; } = 50;
        public float YTranslation { get; set; } = 25;

        public PdfExtGState ExtGState => new PdfExtGState().SetFillOpacity(FontInfo.Opacity);
        public Rectangle FormXObjRectangle => new(FormXObjXOffset, FormXObjYOffset, FormXObjWidth, FormXObjHeight);

        public AffineTransform Transform
        {
            get
            {
                var transform = new AffineTransform();
                transform.Translate(XTranslation, YTranslation);
                transform.Rotate(RotationAngle);

                return transform;
            }
        }

        public WatermarkAnnotation(string text = DEFAULT_TEXT, double rotationAngle = DEFAULT_ROTATION_ANGLE) : base(text, rotationAngle)
        {
        }

        public WatermarkAnnotation(string text, double rotationAngle, IFontInfo fontInfo, FloatPoint? lowerLeftPoint = null) :
            base(text, rotationAngle, fontInfo, lowerLeftPoint)
        {
        }

        public void SetWidth(float witdh)
        {
            FormXObjWidth = TrimmingRectangleWidth = witdh;
        }

        public void SetHeight(float height)
        {
            FormXObjHeight = TrimmingRectangleHeight = height;
        }

        public FloatPoint GetCenterredLowerLeftPoint(Rectangle pageSize)
        {
            float pageWidth = pageSize.GetWidth();
            float pageHeight = pageSize.GetHeight();

            float bottomLeftX = pageWidth / 2 - TrimmingRectangleWidth / 2;
            float bottomLeftY = pageHeight / 2 - TrimmingRectangleHeight / 2;

            return new FloatPoint(bottomLeftX, bottomLeftY);
        }

        public Rectangle GetTrimmingRectangle(float bottomLeftX, float bottomLeftY)
        {
            return new Rectangle(bottomLeftX, bottomLeftY, TrimmingRectangleWidth, TrimmingRectangleHeight);
        }

        public IIndelibleWatermark ToIndelibleWatermark()
        {
            return new IndelibleWatermark(TrimmingRectangleWidth, TrimmingRectangleHeight)
            {
                Text = Text,
                RotationAngle = RotationAngle,
                FontInfo = FontInfo.Copy(),
                LowerLeftPoint = LowerLeftPoint
            };
        }

        public new WatermarkAnnotation Copy()
        {
            return new WatermarkAnnotation()
            {
                TrimmingRectangleWidth = TrimmingRectangleWidth,
                TrimmingRectangleHeight = TrimmingRectangleHeight,
                FormXObjWidth = FormXObjWidth,
                FormXObjHeight = FormXObjHeight,
                FormXObjXOffset = FormXObjXOffset,
                FormXObjYOffset = FormXObjYOffset,
                XTranslation = XTranslation,
                YTranslation = YTranslation,

                Text = Text,
                RotationAngle = RotationAngle,
                FontInfo = FontInfo.Copy(),
                LowerLeftPoint = LowerLeftPoint
            };
        }

        IWatermarkAnnotation IDeepCloneable<IWatermarkAnnotation>.Copy()
        {
            return Copy();
        }

        public bool Equals(WatermarkAnnotation? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Text == other.Text &&
                   RotationAngle == other.RotationAngle &&
                   EqualityComparer<IFontInfo>.Default.Equals(FontInfo, other.FontInfo) &&
                   EqualityComparer<FloatPoint?>.Default.Equals(LowerLeftPoint, other.LowerLeftPoint) &&
                   TrimmingRectangleWidth == other.TrimmingRectangleWidth &&
                   TrimmingRectangleHeight == other.TrimmingRectangleHeight &&
                   FormXObjWidth == other.FormXObjWidth &&
                   FormXObjHeight == other.FormXObjHeight &&
                   FormXObjXOffset == other.FormXObjXOffset &&
                   FormXObjYOffset == other.FormXObjYOffset &&
                   XTranslation == other.XTranslation &&
                   YTranslation == other.YTranslation;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WatermarkAnnotation);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(base.GetHashCode());
            hash.Add(Text);
            hash.Add(RotationAngle);
            hash.Add(FontInfo);
            hash.Add(LowerLeftPoint);
            hash.Add(TrimmingRectangleWidth);
            hash.Add(TrimmingRectangleHeight);
            hash.Add(FormXObjWidth);
            hash.Add(FormXObjHeight);
            hash.Add(FormXObjXOffset);
            hash.Add(FormXObjYOffset);
            hash.Add(XTranslation);
            hash.Add(YTranslation);

            return hash.ToHashCode();
        }
    }
}
