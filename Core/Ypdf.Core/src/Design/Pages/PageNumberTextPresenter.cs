using System;
using System.Collections.Generic;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public class PageNumberTextPresenter : IPageNumberTextPresenter, IEquatable<PageNumberTextPresenter?>
{
    public PageNumberTextPresenter(Func<int, int, string>? converter = null)
    {
        Converter = converter ?? Default.Converter;
    }

    public static PageNumberTextPresenter Default => new((pageNum, numOfPages) => $"{pageNum}");
    public static PageNumberTextPresenter Fractional => new((pageNum, numOfPages) => $"{pageNum}/{numOfPages}");
    public static PageNumberTextPresenter Verbal => new((pageNum, numOfPages) => $"page {pageNum} of {numOfPages}");

    public Func<int, int, string> Converter { get; }

    public string GetText(int pageNum, int numOfPages)
    {
        return Converter.Invoke(pageNum, numOfPages);
    }

    public bool Equals(PageNumberTextPresenter? other)
    {
        return other is not null
            && EqualityComparer<Func<int, int, string>>.Default.Equals(Converter, other.Converter);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PageNumberTextPresenter);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(Converter);
    }
}
