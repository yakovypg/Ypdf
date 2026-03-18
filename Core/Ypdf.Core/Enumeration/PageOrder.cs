using System;
using System.Collections.Generic;
using System.Linq;
using Ypdf.Core.Parsers;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Enumeration;

public readonly struct PageOrder : IEquatable<PageOrder>
{
    public PageOrder(IEnumerable<int> pages)
    {
        ExtendedArgumentNullException.ThrowIfNull(pages, nameof(pages));
        DefaultExceptions.ThrowIfContainsNotAllowedItem(pages, t => t < 1, nameof(pages));
        DefaultExceptions.ThrowIfContainsNotUniqueItems(pages, nameof(pages));

        Pages = pages.ToArray();
    }

    public PageOrder(IEnumerable<int> pages, int pagesInResultOrder)
    {
        ExtendedArgumentNullException.ThrowIfNull(pages, nameof(pages));

        DefaultExceptions.ThrowIfNegativeOrZero(
            pagesInResultOrder,
            nameof(pagesInResultOrder));

        Pages = CreatePageOrder(pages, pagesInResultOrder)
            .ToArray();
    }

    public readonly IReadOnlyCollection<int> Pages { get; }

    public static bool operator ==(PageOrder left, PageOrder right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageOrder left, PageOrder right)
    {
        return !(left == right);
    }

    public static PageOrder Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        IList<PageRange> pageRanges = AbstractParser.ParseMany(data, ',', PageRange.Parse);
        IList<int> pages = PageRange.GetAllItems(pageRanges);

        return new PageOrder(pages);
    }

    public readonly bool Equals(PageOrder other)
    {
        return Pages.SequenceEqual(other.Pages);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageOrder other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(Pages);
    }

    private static IEnumerable<int> CreatePageOrder(
        IEnumerable<int> pages,
        int pagesInResultOrder)
    {
        ExtendedArgumentNullException.ThrowIfNull(pages, nameof(pages));

        DefaultExceptions.ThrowIfNegativeOrZero(
            pagesInResultOrder,
            nameof(pagesInResultOrder));

        if (pages.Count() > pagesInResultOrder)
            return pages.Take(pagesInResultOrder);

        IEnumerable<int> allPages = Enumerable.Range(1, pagesInResultOrder);
        IEnumerable<int> remainingPages = allPages.Except(pages);

        return pages.Concat(remainingPages);
    }
}
