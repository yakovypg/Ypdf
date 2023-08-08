using iText.Kernel.Colors;
using YpdfLib.Infrastructure.Converters;
using YpdfLib.Models;
using YpdfLib.Models.Design;

namespace ExecutionLib.Configuration.Models
{
    public class WatermarkBorderConfig : IWatermarkBorderConfig, IDeepCloneable<WatermarkBorderConfig>
    {
        public bool IsConfigured { get; protected set; }
        
        private BorderType _borderType = BorderType.Dashed;
        public BorderType BorderType
        {
            get => _borderType;
            set
            {
                _borderType = value;
                IsConfigured = true;
            }
        }

        private Color _color = new DeviceRgb(0, 0, 0);
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                IsConfigured = true;
            }
        }

        private float _width = 5;
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                IsConfigured = true;
            }
        }

        private float _opacity = 1;
        public float Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                IsConfigured = true;
            }
        }

        public LazyBorder Create()
        {
            if (!IsConfigured)
                throw new NotConfiguredException(nameof(WatermarkBorderConfig));
            
            DeviceRgb rgb = ColorConverter.ToDeviceRgb(Color);
            return new LazyBorder(BorderType, rgb, Width, Opacity);
        }

        public WatermarkBorderConfig Copy()
        {
            return new WatermarkBorderConfig()
            {
                IsConfigured = IsConfigured,
                BorderType = BorderType,
                Color = Color,
                Width = Width,
                Opacity = Opacity
            };
        }

        IWatermarkBorderConfig IDeepCloneable<IWatermarkBorderConfig>.Copy()
        {
            return Copy();
        }

        public bool Equals(WatermarkBorderConfig? other)
        {
            return other is not null &&
                   IsConfigured == other.IsConfigured &&
                   BorderType == other.BorderType &&
                   Color.Equals(other.Color) &&
                   Width == other.Width &&
                   Opacity == other.Opacity;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WatermarkBorderConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsConfigured, BorderType, Color, Width, Opacity);
        }
    }
}
