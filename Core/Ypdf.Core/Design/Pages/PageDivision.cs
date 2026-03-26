using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Parsers;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageDivision : IEquatable<PageDivision>
{
    private const string _expectedStringFormat = "Pages:Orientation,CenterOffset";

    public PageDivision(int pageNumber, PageDivisionOrientation orientation, float centerOffset = 0)
    {
        DefaultExceptions.ThrowIfLessThan(pageNumber, 1, nameof(pageNumber));

        PageNumber = pageNumber;
        Orientation = orientation;
        CenterOffset = centerOffset;
    }

    public readonly int PageNumber { get; }
    public readonly float CenterOffset { get; }
    public readonly PageDivisionOrientation Orientation { get; }

    public static bool operator ==(PageDivision left, PageDivision right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageDivision left, PageDivision right)
    {
        return !(left == right);
    }

    public static PageDivision Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        string[] additionalData = parts[1].Split(',');

        if (additionalData.Length < 1 || additionalData.Length > 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        int pageNumber = int.Parse(parts[0], CultureInfo.InvariantCulture);

        var orientation = (PageDivisionOrientation)Enum.Parse(
            typeof(PageDivisionOrientation),
            additionalData[0],
            true);

        float centerOffset = additionalData.Length == 2
            ? float.Parse(additionalData[1], CultureInfo.InvariantCulture)
            : 0;

        return new PageDivision(pageNumber, orientation, centerOffset);
    }

    public static IList<PageDivision> ParseFromRange(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        IReadOnlyCollection<int> pages = PageRange.Parse(parts[0]).Items;

        return pages
            .Select(t => $"{t}:{parts[1]}")
            .Select(Parse)
            .ToList();
    }

    public static IList<PageDivision> ParseMany(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        IList<IList<PageDivision>> allShifts = AbstractParser.ParseManyToOne(data, ',', ':', ParseFromRange);
        var result = new List<PageDivision>();

        foreach (IList<PageDivision> shifts in allShifts)
        {
            result.AddRange(shifts);
        }

        return result;
    }

    public readonly bool Equals(PageDivision other)
    {
        return PageNumber == other.PageNumber
            && CenterOffset == other.CenterOffset
            && Orientation == other.Orientation;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageDivision other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(PageNumber, CenterOffset, Orientation);
    }
}
