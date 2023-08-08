using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class RoundDoubleValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not double sourceValue)
                throw new NotSupportedException();

            int digits = parameter is not null
                ? System.Convert.ToInt32(parameter)
                : 0;
            
            return Math.Round(sourceValue, digits);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not double sourceValue)
                throw new NotSupportedException();
            
            return sourceValue;
        }
    }
}
