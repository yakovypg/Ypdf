using iText.Kernel.Colors;

namespace YpdfLib.Infrastructure.Converters
{
    public static class ColorConverter
    {
        public static (byte R, byte G, byte B) ToRgb(Color color)
        {
            float[] colorConfig = color.GetColorValue();

            if (colorConfig.Length != 3)
                throw new NotSupportedException();

            byte r = (byte)(colorConfig[0] * byte.MaxValue);
            byte g = (byte)(colorConfig[1] * byte.MaxValue);
            byte b = (byte)(colorConfig[2] * byte.MaxValue);

            return (R: r, G: g, B: b);
        }

        public static DeviceRgb FromRgb(byte r, byte g, byte b)
        {
            return new DeviceRgb(r, g, b);
        }

        public static DeviceRgb CopyDeviceRGB(DeviceRgb rgb)
        {
            (byte r, byte g, byte b) = ToRgb(rgb);
            return new DeviceRgb(r, g, b);
        }

        public static Color CopyColor(Color color)
        {
            (byte r, byte g, byte b) = ToRgb(color);
            return new DeviceRgb(r, g, b); 
        }
    }
}
