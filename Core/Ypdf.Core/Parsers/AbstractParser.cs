using System;
using System.Collections.Generic;
using System.Linq;

namespace Ypdf.Core.Parsers;

public static class AbstractParser
{
    public static T Parse<T>(string data, Func<string, T> parser)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(data, nameof(data));
        ExtendedArgumentNullException.ThrowIfNull(parser, nameof(parser));

        return parser.Invoke(data);
    }

    public static IList<T> ParseMany<T>(string data, char delimiter, Func<string, T> parser)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(data, nameof(data));
        ExtendedArgumentNullException.ThrowIfNull(parser, nameof(parser));

        return data.Split(delimiter)
            .Select(parser.Invoke)
            .ToList();
    }

    public static IList<T> ParseManyToOne<T>(
        string data,
        char setDelimiter,
        char partDelimiter,
        Func<string, T> parser)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(data, nameof(data));
        ExtendedArgumentNullException.ThrowIfNull(parser, nameof(parser));

        string[] parts = data.Split(partDelimiter);

        if (parts.Length != 2)
        {
            string format = $"x1{setDelimiter}...{setDelimiter}xn{partDelimiter}Y";
            throw new IncorrectDataFormatException(null, data, format);
        }

        string[] setItems = parts[0].Split(setDelimiter);

        return setItems
            .Select(t => $"{t}{partDelimiter}{parts[1]}")
            .Select(parser.Invoke)
            .ToList();
    }
}
