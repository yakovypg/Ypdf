using System;
using System.Collections.Generic;
using System.Globalization;
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

    public static PageNumberTextPresenter Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        string[] supportedValues = [nameof(Default), nameof(Fractional), nameof(Verbal)];
        string joinedSupportedValues = string.Join(", ", supportedValues);

        return data.ToUpperInvariant() switch
        {
            "DEFAULT" => Default,
            "FRACTIONAL" => Fractional,
            "VERBAL" => Verbal,

            _ => throw new IncorrectDataFormatException(null, data, joinedSupportedValues)
        };
    }

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
