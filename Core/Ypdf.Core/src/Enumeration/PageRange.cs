using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Enumeration;

public readonly struct PageRange : IEquatable<PageRange>
{
    public PageRange(int start)
        : this(start, start) { }

    public PageRange(int start, int end)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(start, nameof(start));
        DefaultExceptions.ThrowIfNegativeOrZero(end, nameof(end));

        Start = start;
        End = end;
    }

    public readonly int Start { get; }
    public readonly int End { get; }

    public readonly int Length => Math.Abs(End - Start) + 1;

    public readonly IReadOnlyCollection<int> Items
    {
        get
        {
            int min = Math.Min(Start, End);
            IEnumerable<int> items = Enumerable.Range(min, Length);

            return Start <= End
                ? items.ToArray()
                : items.Reverse().ToArray();
        }
    }

    public static bool operator ==(PageRange left, PageRange right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageRange left, PageRange right)
    {
        return !(left == right);
    }

    public static IList<int> GetAllItems(IEnumerable<PageRange> ranges)
    {
        ExtendedArgumentNullException.ThrowIfNull(ranges, nameof(ranges));

        var pages = new List<int>();

        foreach (PageRange range in ranges)
        {
            pages.AddRange(range.Items);
        }

        return pages;
    }

    public static PageRange Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split('-');

        if (parts.Length == 1)
        {
            int value = int.Parse(data, CultureInfo.InvariantCulture);
            return new PageRange(value);
        }

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, "S-E");

        int start = int.Parse(parts[0], CultureInfo.InvariantCulture);
        int end = int.Parse(parts[1], CultureInfo.InvariantCulture);

        return new PageRange(start, end);
    }

    public readonly bool IsInRange(int value)
    {
        return value >= Start
            && value <= End;
    }

    public readonly override string ToString()
    {
        return Start != End
            ? $"{Start}-{End}"
            : $"{Start}";
    }

    public readonly bool Equals(PageRange other)
    {
        return Start == other.Start
            && End == other.End;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageRange other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(Start, End);
    }
}
