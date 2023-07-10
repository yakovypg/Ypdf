using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class IncreaseDoubleValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not double sourceValue)
                throw new NotSupportedException();

            double delta = parameter is not null
                ? System.Convert.ToDouble(parameter)
                : 0;

            return sourceValue + delta;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not double sourceValue)
                throw new NotSupportedException();

            double delta = parameter is not null
                ? System.Convert.ToDouble(parameter)
                : 0;

            return sourceValue - delta;
        }
    }
}
