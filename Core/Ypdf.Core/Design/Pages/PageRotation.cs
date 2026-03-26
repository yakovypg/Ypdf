using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Parsers;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageRotation : IEquatable<PageRotation>
{
    private const string _expectedStringFormat = "Pages:Angle";

    public PageRotation(int pageNumber, int angleDegrees)
    {
        DefaultExceptions.ThrowIfLessThan(pageNumber, 1, nameof(pageNumber));

        PageNumber = pageNumber;
        AngleDegrees = angleDegrees;
    }

    public readonly int PageNumber { get; }
    public readonly int AngleDegrees { get; }

    public static bool operator ==(PageRotation left, PageRotation right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageRotation left, PageRotation right)
    {
        return !(left == right);
    }

    public static PageRotation Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        int pageNumber = int.Parse(parts[0], CultureInfo.InvariantCulture);
        int angleDegrees = int.Parse(parts[1], CultureInfo.InvariantCulture);

        return new PageRotation(pageNumber, angleDegrees);
    }

    public static IList<PageRotation> ParseFromRange(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        IReadOnlyCollection<int> pages = PageRange.Parse(parts[0]).Items;

        return pages
            .Select(t => $"{t}:{parts[1]}")
            .Select(t => Parse(t))
            .ToList();
    }

    public static IList<PageRotation> ParseMany(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        IList<IList<PageRotation>> allShifts = AbstractParser
            .ParseManyToOne(data, ',', ':', ParseFromRange);

        var result = new List<PageRotation>();

        foreach (IList<PageRotation> shifts in allShifts)
        {
            result.AddRange(shifts);
        }

        return result;
    }

    public readonly bool Equals(PageRotation other)
    {
        return PageNumber == other.PageNumber
            && AngleDegrees == other.AngleDegrees;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageRotation other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(PageNumber, AngleDegrees);
    }
}
