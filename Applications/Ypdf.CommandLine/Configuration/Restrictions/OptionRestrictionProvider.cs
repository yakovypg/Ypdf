using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NetArgumentParser.Options;
using NetArgumentParser.Options.Configuration;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;
using Ypdf.Extensions;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal abstract class OptionRestrictionProvider : IOptionRestrictionProvider
{
    protected static IReadOnlyList<string> PdfExtensions => [".pdf"];
    protected static IReadOnlyList<string> TextExtensions => [".txt"];
    protected static IReadOnlyList<string> FontExtensions => [".ttf"];
    protected static IReadOnlyList<string> ImagesExtensions => [".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff"];

    protected abstract IReadOnlyCollection<Action<Subcommand>> RestrictionProviders { get; }

    public virtual void AddRestrictions(Subcommand subcommand)
    {
        foreach (Action<Subcommand> addRestriction in RestrictionProviders)
        {
            addRestriction.Invoke(subcommand);
        }
    }

    protected static string CreateValueNotSatisfuRestrictionMessage(string optionName, string? reason = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));

        reason = reason is not null
            ? $": {reason}"
            : null;

        return $"Value of '{optionName}' is incorrect{reason}";
    }

    protected static Predicate<ICommonOption> CreateFindOptionPredicate<T>(string optionName)
    {
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));
        return option => option is ValueOption<T> && option.LongName == optionName;
    }

    protected static Predicate<string> CreatePathToFilePredicate(IEnumerable<string>? allowedExtensions = null)
    {
        return path =>
        {
            bool exists = File.Exists(path);

            bool extensionAllowed =
                (allowedExtensions is null) ||
                (Path.HasExtension(path) && allowedExtensions.Contains(Path.GetExtension(path)));

            return exists && extensionAllowed;
        };
    }

    protected static Predicate<T> CreateValueInRangePredicate<T>(double? minValue = null, double? maxValue = null)
        where T : IConvertible
    {
        return value =>
        {
            bool minRestrictionOk =
                minValue is null ||
                value.ToDouble(CultureInfo.CurrentCulture) >= minValue.Value;

            bool maxRestrictionOk =
                maxValue is null ||
                value.ToDouble(CultureInfo.CurrentCulture) <= maxValue.Value;

            return minRestrictionOk && maxRestrictionOk;
        };
    }

    protected static Predicate<T> CreatePagesGreaterThanZeroPredicate<T>()
        where T : IEnumerable<PageRange>
    {
        return ranges => ranges.All(range => range.Items.All(page => page >= 1));
    }

    protected static Predicate<T> CreatePageContentShiftCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageContentShift>
    {
        return shifts => shifts.All(t => t.PageNumber >= minPage);
    }

    protected static Predicate<FloatPoint> CreateFloatPointCorrectPredicate(float minX, float minY)
    {
        return value => value.X >= minX && value.Y >= minY;
    }

    protected static Predicate<string> CreateValueNotEmptyPredicate()
    {
        return value => !string.IsNullOrEmpty(value);
    }

    protected static ValueOption<T> FindOption<T>(Subcommand subcommand, Predicate<ICommonOption> predicate)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        List<ValueOption<T>> foundOptions = [.. subcommand
            .FindOptions(predicate, false)
            .Cast<ValueOption<T>>()];

        return foundOptions.Count == 1
            ? foundOptions[0]
            : throw new ArgumentException("Subcommand is configured incorrectly", nameof(subcommand));
    }

    protected static void AddRestriction<T>(
        Subcommand subcommand,
        Predicate<ICommonOption> findOptionPredicate,
        Predicate<T> isValueAllowed,
        string? valueNotSatisfuRestrictionMessage = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(findOptionPredicate, nameof(findOptionPredicate));
        ExtendedArgumentNullException.ThrowIfNull(isValueAllowed, nameof(isValueAllowed));

        ValueOption<T> inputPathOption = FindOption<T>(subcommand, findOptionPredicate);

        inputPathOption.ValueRestriction = new OptionValueRestriction<T>(
            isValueAllowed,
            valueNotSatisfuRestrictionMessage);
    }

    protected static void AddFileAllowedRestrictionForPathOption(
        Subcommand subcommand,
        string optionName,
        IEnumerable<string> allowedExtensions)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));
        ExtendedArgumentNullException.ThrowIfNull(allowedExtensions, nameof(allowedExtensions));

        string allowedExtensionsPresenter = string.Join("|", allowedExtensions);
        string badValueReason = $"the path must point to a {allowedExtensionsPresenter} file";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionName, badValueReason);

        AddRestriction(
            subcommand,
            CreateFindOptionPredicate<string>(optionName),
            CreatePathToFilePredicate(PdfExtensions),
            badValueMessage);
    }

    protected static void AddInRangeRestrictionForNumberOption<T>(
        Subcommand subcommand,
        string optionName,
        double? minValue = null,
        double? maxValue = null)
        where T : IConvertible
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));

        string badValueReason = $"it must be ";

        if (minValue is not null && maxValue is not null)
            badValueReason += $"in [{minValue};{maxValue}]";
        else if (minValue is not null)
            badValueReason += $">= {minValue}";
        else if (maxValue is not null)
            badValueReason += $"<= {maxValue}";
        else
            badValueReason += $"correct number";

        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionName, badValueReason);

        AddRestriction(
            subcommand,
            CreateFindOptionPredicate<T>(optionName),
            CreateValueInRangePredicate<T>(minValue, maxValue),
            badValueMessage);
    }

    protected static void AddGreaterThanZeroRestrictionForPagesOption<T>(
        Subcommand subcommand,
        string optionName)
        where T : IEnumerable<PageRange>
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));

        string badValueReason = "all pages must be >= 1";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionName, badValueReason);

        AddRestriction(
            subcommand,
            CreateFindOptionPredicate<T>(optionName),
            CreatePagesGreaterThanZeroPredicate<T>(),
            badValueMessage);
    }

    protected static void AddCorrectRestrictionForPageContentShiftsOption<T>(
        Subcommand subcommand,
        string optionName,
        int minPage = 1)
        where T : IEnumerable<PageContentShift>
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionName, badValueReason);

        AddRestriction(
            subcommand,
            CreateFindOptionPredicate<T>(optionName),
            CreatePageContentShiftCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddCorrectRestrictionForFloatPointOption(
        Subcommand subcommand,
        string optionName,
        float minX = 0,
        float minY = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));

        string badValueReason = $"X-coordinates must be >= {minX} and Y-coordinates must be >= {minY}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionName, badValueReason);

        AddRestriction(
            subcommand,
            CreateFindOptionPredicate<FloatPoint>(optionName),
            CreateFloatPointCorrectPredicate(minX, minY),
            badValueMessage);
    }

    protected static void AddNotEmptyRestrictionForStringOption(Subcommand subcommand, string optionName)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionName, nameof(optionName));

        string badValueReason = $"it mustn't be empty";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionName, badValueReason);

        AddRestriction(
            subcommand,
            CreateFindOptionPredicate<string>(optionName),
            CreateValueNotEmptyPredicate(),
            badValueMessage);
    }
}
