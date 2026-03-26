using System;
using System.Text;

namespace Ypdf.Core.Extensions;

public static class StringExtensions
{
    private const StringComparison _defaultComparisonType = StringComparison.Ordinal;

#if !NET5_0_OR_GREATER && !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
    public static string Replace(
        this string text,
        string oldValue,
        string? newValue,
        StringComparison comparisonType = _defaultComparisonType)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));
        ExtendedArgumentException.ThrowIfNullOrEmpty(oldValue, nameof(oldValue));

        string replacement = newValue ?? string.Empty;

        int currentPosition = 0;
        int matchIndex = text.IndexOf(oldValue, currentPosition, comparisonType);

        if (matchIndex < 0)
            return text;

        var builder = new StringBuilder(text.Length);

        while (matchIndex >= 0)
        {
            _ = builder
                .Append(text, currentPosition, matchIndex - currentPosition)
                .Append(replacement);

            currentPosition = matchIndex + oldValue.Length;
            matchIndex = text.IndexOf(oldValue, currentPosition, comparisonType);
        }

        return builder
            .Append(text, currentPosition, text.Length - currentPosition)
            .ToString();
    }

    public static bool Contains(
        this string text,
        char value,
        StringComparison comparisonType = _defaultComparisonType)
    {
        return text.Contains($"{value}", comparisonType);
    }

    public static bool Contains(
        this string text,
        string? value,
        StringComparison comparisonType = _defaultComparisonType)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));

        return value is not null
            ? text.IndexOf(value, comparisonType) >= 0
            : false;
    }

    public static int IndexOf(
        this string text,
        char value,
        StringComparison comparisonType = _defaultComparisonType)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));
        return text.IndexOf($"{value}", comparisonType);
    }
#endif

    public static bool StartsWith(
        this string text,
        char value,
        StringComparison comparisonType = _defaultComparisonType)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));
        return text.StartsWith($"{value}", comparisonType);
    }

    public static bool EndsWith(
        this string text,
        char value,
        StringComparison comparisonType = _defaultComparisonType)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));
        return text.EndsWith($"{value}", comparisonType);
    }

    public static string Quoted(this string text, char quoteMark = '"')
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));
        return $"{quoteMark}{text}{quoteMark}";
    }
}
