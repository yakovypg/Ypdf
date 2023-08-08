using Avalonia.Data.Converters;
using ExecutionLib.Informing.Aliases;
using iText.Kernel.Colors;
using System;
using System.Globalization;
using System.Linq;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class ColorValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not Color color)
                throw new NotSupportedException();

            return StandardValues.Colors.FirstOrDefault(t => t.Value == color).Key;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not string key)
                throw new NotSupportedException();
            
            _ = StandardValues.Colors.TryGetValue(key, out Color? color);
            return color;
        }

        public object? Convert(object? value)
        {
            return Convert(value, typeof(object), null, CultureInfo.InvariantCulture);
        }

        public object? ConvertBack(object? value)
        {
            return ConvertBack(value, typeof(object), null, CultureInfo.InvariantCulture);
        }
    }
}
