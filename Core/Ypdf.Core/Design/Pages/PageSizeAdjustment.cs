using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Parsers;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageSizeAdjustment : IEquatable<PageSizeAdjustment>
{
    private const string _expectedStringFormat = "Pages:L,T,R,B";
    private readonly float[] adjustmentValues;

    public PageSizeAdjustment(
        int pageNumber,
        float left = 0,
        float top = 0,
        float right = 0,
        float bottom = 0)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(pageNumber, nameof(pageNumber));

        PageNumber = pageNumber;
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;

        adjustmentValues = [left, top, right, bottom];
    }

    public readonly int PageNumber { get; }
    public readonly float Left { get; }
    public readonly float Top { get; }
    public readonly float Right { get; }
    public readonly float Bottom { get; }

    public readonly bool IsZero => adjustmentValues.All(t => t == 0);
    public readonly bool IsNegative => adjustmentValues.All(t => t < 0);
    public readonly bool IsPositive => adjustmentValues.All(t => t > 0);
    public readonly bool IsNegativeOrZero => adjustmentValues.All(t => t <= 0);
    public readonly bool IsPositiveOrZero => adjustmentValues.All(t => t >= 0);

    public static bool operator ==(PageSizeAdjustment left, PageSizeAdjustment right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageSizeAdjustment left, PageSizeAdjustment right)
    {
        return !(left == right);
    }

    public static PageSizeAdjustment Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        int pageNumber = int.Parse(parts[0], CultureInfo.InvariantCulture);

        string[] ajustementParts = parts[1].Split(',');

        if (ajustementParts.Length != 4)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        float left = float.Parse(ajustementParts[0], CultureInfo.InvariantCulture);
        float top = float.Parse(ajustementParts[1], CultureInfo.InvariantCulture);
        float right = float.Parse(ajustementParts[2], CultureInfo.InvariantCulture);
        float bottom = float.Parse(ajustementParts[3], CultureInfo.InvariantCulture);

        return new PageSizeAdjustment(pageNumber, left, top, right, bottom);
    }

    public static IList<PageSizeAdjustment> ParseFromRange(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        IReadOnlyCollection<int> pages = PageRange.Parse(parts[0]).Items;

        return [.. pages
            .Select(t => $"{t}:{parts[1]}")
            .Select(Parse)];
    }

    public static IList<PageSizeAdjustment> ParseMany(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        IList<IList<PageSizeAdjustment>> allAjustements = AbstractParser
            .ParseManyToOne(data, ',', ':', ParseFromRange);

        var result = new List<PageSizeAdjustment>();

        foreach (IList<PageSizeAdjustment> ajustements in allAjustements)
        {
            result.AddRange(ajustements);
        }

        return result;
    }

    public readonly bool Equals(PageSizeAdjustment other)
    {
        return PageNumber == other.PageNumber
            && Left == other.Left
            && Top == other.Top
            && Right == other.Right
            && Bottom == other.Bottom;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageSizeAdjustment other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(PageNumber, Left, Top, Right, Bottom);
    }
}
