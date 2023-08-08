using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class DegreeContentValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string valueStr = value?.ToString() ?? string.Empty;
            string prefix = parameter?.ToString() ?? string.Empty;
            
            return $"{prefix}{valueStr}°";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}