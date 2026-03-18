using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ypdf.Core;

public static class DefaultExceptions
{
    public static void ThrowIfContainsNotAllowedItem<T>(
        IEnumerable<T> items,
        Predicate<T> isItemAllowedPredicate,
        string? paramName = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(items, nameof(items));
        ExtendedArgumentNullException.ThrowIfNull(isItemAllowedPredicate, nameof(isItemAllowedPredicate));

        int index = 0;

        foreach (T item in items)
        {
            if (!isItemAllowedPredicate.Invoke(item))
                ThrowItemNotAllowed(item, $"{paramName}[{index}]");

            index++;
        }
    }

    public static void ThrowIfContainsNotUniqueItems<T>(
        IEnumerable<T> items,
        string? paramName = null)
    {
        List<T> itemsList = new(items);
        IEnumerable<T> distinctItems = itemsList.Distinct();

        if (itemsList.Count != distinctItems.Count())
            ThrowItemsNotUnique(paramName);
    }

    public static void ThrowIfFileNotExists(string? path, string? paramName = null)
    {
        if (!File.Exists(path))
            ThrowFileNotExists(path, paramName);
    }

    public static void ThrowIfDirectoryNotExists(string? path, string? paramName = null)
    {
        if (path?.Length > 0 && !Directory.Exists(path))
            ThrowDirectoryNotExists(path, paramName);
    }

    public static void ThrowIfContainsNotExistingFile(
        IEnumerable<string> paths,
        string? paramName = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(paths, nameof(paths));

        int index = 0;

        foreach (string item in paths)
        {
            ThrowIfFileNotExists(item, $"{paramName}[{index}]");
            index++;
        }
    }

    public static void ThrowIfZero(double value, string? paramName = null)
    {
        if (value == 0)
            ThrowZero(value, paramName);
    }

    public static void ThrowIfNegative(double value, string? paramName = null)
    {
        if (value < 0)
            ThrowNegative(value, paramName);
    }

    public static void ThrowIfNegativeOrZero(double value, string? paramName = null)
    {
        if (value <= 0)
            ThrowNegativeOrZero(value, paramName);
    }

    public static void ThrowIfEqual<T>(T value, T other, string? paramName = null)
        where T : IEquatable<T>
    {
        if (EqualityComparer<T>.Default.Equals(value, other))
            ThrowEqual(value, other, paramName);
    }

    public static void ThrowIfNotEqual<T>(T value, T other, string? paramName = null)
        where T : IEquatable<T>
    {
        if (!EqualityComparer<T>.Default.Equals(value, other))
            ThrowNotEqual(value, other, paramName);
    }

    public static void ThrowIfGreaterThan<T>(T value, T other, string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) > 0)
            ThrowGreater(value, other, paramName);
    }

    public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) >= 0)
            ThrowGreaterEqual(value, other, paramName);
    }

    public static void ThrowIfLessThan<T>(T value, T other, string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) < 0)
            ThrowLess(value, other, paramName);
    }

    public static void ThrowIfLessThanOrEqual<T>(T value, T other, string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) <= 0)
            ThrowLessEqual(value, other, paramName);
    }

    public static void ThrowIfNotBetween<T>(T value, T leftEdge, T rightEdge, string? paramName = null)
        where T : IComparable<T>
    {
        ThrowIfLessThan(value, leftEdge, paramName);
        ThrowIfGreaterThan(value, rightEdge, paramName);
    }

    private static void ThrowZero<T>(T value, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be a non-zero value.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowNegative<T>(T value, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be a non-negative value.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowNegativeOrZero<T>(T value, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be a non-negative and non-zero value.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowGreater<T>(T value, T other, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be less than or equal to {other}.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowGreaterEqual<T>(T value, T other, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be less than {other}.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowLess<T>(T value, T other, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be greater than or equal to {other}.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowLessEqual<T>(T value, T other, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be greater than {other}.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowEqual<T>(T value, T other, string? paramName)
    {
        string message = $"{paramName} ('{value}') must be equal to {other}.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowNotEqual<T>(T value, T other, string? paramName)
    {
        string message = $"{paramName} ('{value}') must not be equal to {other}.";
        throw new ArgumentOutOfRangeException(paramName, value, message);
    }

    private static void ThrowItemNotAllowed<T>(T item, string? paramName)
    {
        string message = $"{paramName} ('{item}') isn't allowed.";
        throw new ArgumentOutOfRangeException(paramName, item, message);
    }

    private static void ThrowItemsNotUnique(string? paramName)
    {
        string message = $"{paramName} has repeated items.";
        throw new ArgumentException(message, paramName);
    }

    private static void ThrowFileNotExists(string? path, string? paramName)
    {
        string message = $"{paramName} ('{path}') doesn't exists.";
        throw new FileNotFoundException(message);
    }

    private static void ThrowDirectoryNotExists(string? path, string? paramName)
    {
        string message = $"{paramName} ('{path}') doesn't exists.";
        throw new DirectoryNotFoundException(message);
    }
}
