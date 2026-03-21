using System;
using System.Collections.Generic;
using System.Linq;

namespace Ypdf.Core.Extensions;

public static class StringExtensions
{
    private const StringComparison _defaultComparisonType = StringComparison.CurrentCulture;

#if !NET5_0_OR_GREATER && !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
#pragma warning disable IDE0060 // Remove unused parameter
    public static string Replace(
        this string text,
        string oldValue,
        string? newValue,
        StringComparison comparisonType = _defaultComparisonType)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));
        ExtendedArgumentNullException.ThrowIfNull(oldValue, nameof(oldValue));

        return text.Replace(oldValue, newValue);
    }
#pragma warning restore IDE0060 // Remove unused parameter

#pragma warning disable IDE0060 // Remove unused parameter
    public static bool Contains<T>(
        this string text,
        T value,
        StringComparison comparisonType = _defaultComparisonType)
    {
        ExtendedArgumentNullException.ThrowIfNull(text, nameof(text));
        ExtendedArgumentNullException.ThrowIfNull(value, nameof(value));

        return text.Contains($"{value}");
    }
#pragma warning restore IDE0060 // Remove unused parameter

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
}
