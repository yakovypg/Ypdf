using System;

namespace Ypdf.Core.Extraction;

public static class TextExtractors
{
    public static ITextExtractor Simple => new SimpleTextExtractor();
    public static ITextExtractor LocationBased => new LocationBasedTextExtractor();

    public static ITextExtractor Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        return data switch
        {
            nameof(Simple) => Simple,
            nameof(LocationBased) => LocationBased,

            _ => throw new ArgumentOutOfRangeException(nameof(data), data)
        };
    }
}
