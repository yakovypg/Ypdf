using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iText.Kernel.Geom;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;
using Ypdf.Core.Parsers;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageCropping : IEquatable<PageCropping>
{
    private const string _expectedStringFormat = "Pages:LowerLeftPoint,UpperRightPoint";

    public PageCropping(int pageNumber, FloatPoint lowerLeft, FloatPoint upperRight)
    {
        DefaultExceptions.ThrowIfLessThan(pageNumber, 1, nameof(pageNumber));

        PageNumber = pageNumber;
        LowerLeft = lowerLeft;
        UpperRight = upperRight;
    }

    public readonly int PageNumber { get; }
    public readonly FloatPoint LowerLeft { get; }
    public readonly FloatPoint UpperRight { get; }

    public readonly Rectangle Box
    {
        get
        {
            var lowerLeft = new Point(LowerLeft.X, LowerLeft.Y);
            var upperRight = new Point(UpperRight.X + LowerLeft.X, UpperRight.Y + LowerLeft.Y);
            var points = new List<Point>() { lowerLeft, upperRight };

            return Rectangle.CalculateBBox(points);
        }
    }

    public static bool operator ==(PageCropping left, PageCropping right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageCropping left, PageCropping right)
    {
        return !(left == right);
    }

    public static PageCropping Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        string[] pointsData = parts[1].Split(',');

        if (pointsData.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        int pageNumber = int.Parse(parts[0], CultureInfo.CurrentCulture);

        FloatPoint lowerLeftPoint = FloatPoint.Parse(pointsData[0]);
        FloatPoint upperRightPoint = FloatPoint.Parse(pointsData[1]);

        return new PageCropping(pageNumber, lowerLeftPoint, upperRightPoint);
    }

    public static IList<PageCropping> ParseFromRange(string data)
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

    public static IList<PageCropping> ParseMany(string data)
    {
        IList<IList<PageCropping>> allShifts = AbstractParser
            .ParseManyToOne(data, ',', ':', ParseFromRange);

        var result = new List<PageCropping>();

        foreach (IList<PageCropping> shifts in allShifts)
        {
            result.AddRange(shifts);
        }

        return result;
    }

    public readonly bool Equals(PageCropping other)
    {
        return PageNumber == other.PageNumber
            && LowerLeft.Equals(other.LowerLeft)
            && UpperRight.Equals(other.UpperRight);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageCropping other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(PageNumber, LowerLeft, UpperRight);
    }
}
