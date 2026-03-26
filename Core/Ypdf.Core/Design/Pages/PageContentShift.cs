using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Parsers;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageContentShift : IEquatable<PageContentShift>
{
    private const string _expectedStringFormat = "Page:Horizontal,Vertical";

    public PageContentShift(int pageNumber, float horizontal, float vertical)
    {
        DefaultExceptions.ThrowIfLessThan(pageNumber, 1, nameof(pageNumber));

        PageNumber = pageNumber;
        Horizontal = horizontal;
        Vertical = vertical;
    }

    public readonly int PageNumber { get; }
    public readonly float Horizontal { get; }
    public readonly float Vertical { get; }

    public static bool operator ==(PageContentShift left, PageContentShift right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PageContentShift left, PageContentShift right)
    {
        return !(left == right);
    }

    public static PageContentShift Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        string[] rightParts = parts[1].Split(',');

        if (rightParts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        int pageNumber = int.Parse(parts[0], CultureInfo.InvariantCulture);
        int horizontal = int.Parse(rightParts[0], CultureInfo.InvariantCulture);
        int vertical = int.Parse(rightParts[1], CultureInfo.InvariantCulture);

        return new PageContentShift(pageNumber, horizontal, vertical);
    }

    public static PageContentShift[] ParseFromRange(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        IReadOnlyCollection<int> pages = PageRange.Parse(parts[0]).Items;

        return pages
            .Select(t => $"{t}:{parts[1]}")
            .Select(Parse)
            .ToArray();
    }

    public static IList<PageContentShift> ParseMany(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        IList<PageContentShift[]> allShifts =
            AbstractParser.ParseManyToOne(data, ',', ':', ParseFromRange);

        var result = new List<PageContentShift>();

        foreach (PageContentShift[] shifts in allShifts)
            result.AddRange(shifts);

        return result;
    }

    public readonly bool Equals(PageContentShift other)
    {
        return PageNumber == other.PageNumber
            && Horizontal == other.Horizontal
            && Vertical == other.Vertical;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageContentShift other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(PageNumber, Horizontal, Vertical);
    }
}
