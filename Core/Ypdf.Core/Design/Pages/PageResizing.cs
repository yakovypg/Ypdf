using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Parsers;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageResizing : IEquatable<PageResizing>
{
    private const string _expectedStringFormat = "Pages:W,H";

    public PageResizing(int pageNumber, float width, float height)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(pageNumber, nameof(pageNumber));
        DefaultExceptions.ThrowIfNegativeOrZero(width, nameof(width));
        DefaultExceptions.ThrowIfNegativeOrZero(height, nameof(height));

        PageNumber = pageNumber;
        Width = width;
        Height = height;
    }

    public readonly int PageNumber { get; }
    public readonly float Width { get; }
    public readonly float Height { get; }

    public static bool operator ==(PageResizing left, PageResizing right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageResizing left, PageResizing right)
    {
        return !(left == right);
    }

    public static PageResizing Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] parts = data.Split(':');

        if (parts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        int pageNumber = int.Parse(parts[0], CultureInfo.InvariantCulture);

        string[] sizeParts = parts[1].Split(',');

        if (sizeParts.Length != 2)
            throw new IncorrectDataFormatException(null, data, _expectedStringFormat);

        float width = float.Parse(sizeParts[0], CultureInfo.InvariantCulture);
        float height = float.Parse(sizeParts[1], CultureInfo.InvariantCulture);

        return new PageResizing(pageNumber, width, height);
    }

    public static IList<PageResizing> ParseFromRange(string data)
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

    public static IList<PageResizing> ParseMany(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        IList<IList<PageResizing>> allResizings = AbstractParser
            .ParseManyToOne(data, ',', ':', ParseFromRange);

        var result = new List<PageResizing>();

        foreach (IList<PageResizing> resizings in allResizings)
        {
            result.AddRange(resizings);
        }

        return result;
    }

    public readonly bool Equals(PageResizing other)
    {
        return PageNumber == other.PageNumber
            && Width == other.Width
            && Height == other.Height;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageResizing other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(PageNumber, Width, Height);
    }
}
