using Avalonia.Data.Converters;
using Avalonia.Media;
using ExecutionLib.Informing.Aliases;
using System;
using System.Globalization;
using YpdfLib.Infrastructure.Converters;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class ColorToBrushValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not iText.Kernel.Colors.Color pdfColor)
                throw new NotSupportedException();

            (byte r, byte g, byte b) = ColorConverter.ToRgb(pdfColor);

            return new SolidColorBrush()
            {
                Color = Color.FromRgb(r, g, b)
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not SolidColorBrush brush)
                throw new NotSupportedException();
            
            Color color = brush.Color;
            iText.Kernel.Colors.Color pdfColor = new iText.Kernel.Colors.DeviceRgb(color.R, color.G, color.B);

            return pdfColor;
        }
    }
}
