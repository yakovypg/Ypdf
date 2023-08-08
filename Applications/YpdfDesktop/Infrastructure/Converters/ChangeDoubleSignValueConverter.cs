using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class ChangeDoubleSignValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not double sourceValue)
                throw new NotSupportedException();

            return -sourceValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
