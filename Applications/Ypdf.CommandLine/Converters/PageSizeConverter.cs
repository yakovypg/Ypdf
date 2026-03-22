using System;
using System.Globalization;
using System.Linq;
using iText.Kernel.Geom;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core;
using Ypdf.Core.Extensions;

namespace Ypdf.CommandLine.Converters;

internal static class PageSizeConverter
{
    private const string _expectedStringFormatDimensions = "(Width,Height)";
    private const string _expectedStringFormat = $"Name or {_expectedStringFormatDimensions}";

    internal static PageSize ParseByName(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        return data.ToUpper(CultureInfo.CurrentCulture) switch
        {
            nameof(PageSize.A0) => PageSize.A0,
            nameof(PageSize.A1) => PageSize.A1,
            nameof(PageSize.A2) => PageSize.A2,
            nameof(PageSize.A3) => PageSize.A3,
            nameof(PageSize.A4) => PageSize.A4,
            nameof(PageSize.A5) => PageSize.A5,
            nameof(PageSize.A6) => PageSize.A6,
            nameof(PageSize.A7) => PageSize.A7,
            nameof(PageSize.A8) => PageSize.A8,
            nameof(PageSize.A9) => PageSize.A9,
            nameof(PageSize.A10) => PageSize.A10,
            nameof(PageSize.B0) => PageSize.B0,
            nameof(PageSize.B1) => PageSize.B1,
            nameof(PageSize.B2) => PageSize.B2,
            nameof(PageSize.B3) => PageSize.B3,
            nameof(PageSize.B4) => PageSize.B4,
            nameof(PageSize.B5) => PageSize.B5,
            nameof(PageSize.B6) => PageSize.B6,
            nameof(PageSize.B7) => PageSize.B7,
            nameof(PageSize.B8) => PageSize.B8,
            nameof(PageSize.B9) => PageSize.B9,
            nameof(PageSize.B10) => PageSize.B10,
            nameof(PageSize.EXECUTIVE) => PageSize.EXECUTIVE,
            nameof(PageSize.LEDGER) => PageSize.LEDGER,
            nameof(PageSize.LEGAL) => PageSize.LEGAL,
            nameof(PageSize.LETTER) => PageSize.LETTER,
            nameof(PageSize.TABLOID) => PageSize.TABLOID,

            _ => throw new ArgumentOutOfRangeException(nameof(data), data)
        };
    }

    internal static PageSize ParseByDimensions(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        if (data.StartsWith('('))
            data = data.Substring(1);

        if (data.EndsWith(')'))
            data = data.Remove(data.Length - 1);

        float[] dimensions = [.. data.Split(',').Select(float.Parse)];

        if (dimensions.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormatDimensions);

        return new PageSize(dimensions[0], dimensions[1]);
    }

    internal static PageSize Parse(string data)
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
            return ParseByDimensions(data);
        }
        catch
        {
        }

        throw new IncorrectDataFormatException(null, data, _expectedStringFormat);
    }
}
