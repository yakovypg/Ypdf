using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace YpdfDesktop.Infrastructure.Converters
{
    public class BitmapAssetValueConverter : IValueConverter
    {
        public const string AVALONIA_ASSETS_PATH_PREFIX = "avares://";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            if (!targetType.IsAssignableFrom(typeof(Bitmap)) || value is not string path)
                throw new NotSupportedException();

            if (string.IsNullOrEmpty(path))
                return null;

            if (!path.StartsWith(AVALONIA_ASSETS_PATH_PREFIX))
            {
                string? assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
                path = $"{AVALONIA_ASSETS_PATH_PREFIX}{assemblyName}/{path}";
            }

            IAssetLoader? assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
            
            Uri uri = new(path);
            Stream? assetStream = assetLoader?.Open(uri);

            return assetStream is not null
                ? new Bitmap(assetStream)
                : throw new FileNotFoundException(path);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
