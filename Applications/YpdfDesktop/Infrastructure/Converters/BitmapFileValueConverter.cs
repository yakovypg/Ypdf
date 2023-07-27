using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class BitmapFileValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (!targetType.IsAssignableFrom(typeof(Bitmap)) || value is not string path)
                throw new NotSupportedException();
            
            if (string.IsNullOrEmpty(path))
                return null;

            return File.Exists(path)
                ? new Bitmap(path)
                : throw new FileNotFoundException(path);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
