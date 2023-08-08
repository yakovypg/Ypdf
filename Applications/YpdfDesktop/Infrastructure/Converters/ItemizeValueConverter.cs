using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class ItemizeValueConverter : IValueConverter
    {
        private const string PREFIX = "• ";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (value is not string valueStr)
                throw new NotSupportedException();

            string prefix = parameter?.ToString() ?? PREFIX;

            return prefix + valueStr;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            if (value is not string valueStr)
                throw new NotSupportedException();

            string prefix = parameter?.ToString() ?? PREFIX;

            return valueStr[prefix.Length..];
        }
    }
}
