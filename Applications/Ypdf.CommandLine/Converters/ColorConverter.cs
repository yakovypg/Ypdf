using System;
using System.Linq;
using iText.Kernel.Colors;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core;
using Ypdf.Core.Extensions;

namespace Ypdf.CommandLine.Converters;

internal static class ColorConverter
{
    private const string _expectedStringFormatRgb = "(R,G,B)";
    private const string _expectedStringFormat = $"Name or {_expectedStringFormatRgb}";

    internal static Color ParseByName(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        return data.ToUpperInvariant() switch
        {
            nameof(ColorConstants.BLACK) => ColorConstants.BLACK,
            nameof(ColorConstants.BLUE) => ColorConstants.BLUE,
            nameof(ColorConstants.CYAN) => ColorConstants.CYAN,
            nameof(ColorConstants.DARK_GRAY) => ColorConstants.DARK_GRAY,
            nameof(ColorConstants.GRAY) => ColorConstants.GRAY,
            nameof(ColorConstants.GREEN) => ColorConstants.GREEN,
            nameof(ColorConstants.LIGHT_GRAY) => ColorConstants.LIGHT_GRAY,
            nameof(ColorConstants.MAGENTA) => ColorConstants.MAGENTA,
            nameof(ColorConstants.ORANGE) => ColorConstants.ORANGE,
            nameof(ColorConstants.PINK) => ColorConstants.PINK,
            nameof(ColorConstants.RED) => ColorConstants.RED,
            nameof(ColorConstants.WHITE) => ColorConstants.WHITE,
            nameof(ColorConstants.YELLOW) => ColorConstants.YELLOW,

            _ => throw new ArgumentOutOfRangeException(nameof(data), data)
        };
    }

    internal static Color ParseByRgb(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        if (data.StartsWith('('))
            data = data.Substring(1);

        if (data.EndsWith(')'))
            data = data.Remove(data.Length - 1);

        byte[] rgb = [.. data.Split(',').Select(byte.Parse)];

        if (rgb.Length != 3)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormatRgb);

        return new DeviceRgb(rgb[0], rgb[1], rgb[2]);
    }

    internal static Color Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        try
        {
            return ParseByName(data);
        }
        catch
        {
        }

        try
        {
            return ParseByRgb(data);
        }
        catch
        {
        }

        throw new IncorrectDataFormatException(null, data, _expectedStringFormat);
    }

    internal static DeviceRgb ToDeviceRgb(Color color)
    {
        if (color is DeviceRgb deviceRgb)
            return deviceRgb;

        float[] components = color.GetColorValue();

        return components.Length switch
        {
            1 => new DeviceRgb(components[0], components[0], components[0]),
            3 => new DeviceRgb(components[0], components[1], components[2]),

            _ => throw new NotSupportedException("Color space not supported.")
        };
    }
}
