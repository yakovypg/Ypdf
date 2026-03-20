using System;
using System.Collections.Generic;
using System.Linq;
using NetArgumentParser.Options;
using NetArgumentParser.Options.Configuration;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal abstract class OptionRestrictionProvider : IOptionRestrictionProvider
{
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

    protected static Predicate<T> CreatePageContentShiftCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageContentShift>
    {
        return shifts => shifts.All(t => t.PageNumber >= minPage);
    }

    protected static Predicate<FloatPoint> CreateFloatPointCorrectPredicate(float minX, float minY)
    {
        return value => value.X >= minX && value.Y >= minY;
    }

    protected static Predicate<T> CreatePageCroppingCorrectPredicate<T>(float minX, float minY, int minPage = 1)
        where T : IEnumerable<PageCropping>
    {
        return croppings => croppings.All(t =>
            t.LowerLeft.X >= minX &&
            t.LowerLeft.Y >= minY &&
            t.UpperRight.X >= minX &&
            t.UpperRight.Y >= minY &&
            t.PageNumber >= minPage);
    }

    protected static Predicate<T> CreatePageDivisionCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageDivision>
    {
        return divisions => divisions.All(t => t.PageNumber >= minPage);
    }

    protected static Predicate<PageOrder> CreatePageOrderCorrectPredicate(int minPage = 1)
    {
        return pageOrder => pageOrder.Pages.All(t => t >= minPage);
    }

    protected static Predicate<T> CreatePageRotationCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageRotation>
    {
        return rotations => rotations.All(t => t.PageNumber >= minPage);
    }

    protected static Predicate<T> CreatePageRangeCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageRange>
    {
        return ranges => ranges.All(t => t.Start >= minPage);
    }

    protected static Predicate<MathExpression> CreateLongMathExpressionCorrectPredicate()
    {
        return expression =>
        {
            try
            {
                _ = expression.CalculateLong();
                return true;
            }
            catch
            {
                return false;
            }
        };
    }

    protected static IValueOption<T> FindOption<T>(Subcommand subcommand, string longName)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(longName, nameof(longName));

        bool found = subcommand.FindFirstValueOptionByLongName(longName, false, out IValueOption<T>? foundOption);

        return found && foundOption is not null
            ? foundOption
            : throw new ArgumentException("Subcommand is configured incorrectly", nameof(subcommand));
    }

    protected static void AddRestriction<T>(
        Subcommand subcommand,
        string optionLongName,
        Predicate<T> isValueAllowed,
        string? valueNotSatisfuRestrictionMessage = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));
        ExtendedArgumentNullException.ThrowIfNull(isValueAllowed, nameof(isValueAllowed));

        IValueOption<T> inputPathOption = FindOption<T>(subcommand, optionLongName);

        inputPathOption.ValueRestriction = new OptionValueRestriction<T>(
            isValueAllowed,
            valueNotSatisfuRestrictionMessage);
    }

    protected static void AddRestrictionForPageContentShiftsOption<T>(
        Subcommand subcommand,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageContentShift>
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreatePageContentShiftCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForFloatPointOption(
        Subcommand subcommand,
        string optionLongName,
        float minX = 0,
        float minY = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"X-coordinates must be >= {minX} and Y-coordinates must be >= {minY}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreateFloatPointCorrectPredicate(minX, minY),
            badValueMessage);
    }

    protected static void AddRestrictionForPageCroppingsOption<T>(
        Subcommand subcommand,
        string optionLongName,
        float minX = 0,
        float minY = 0,
        int minPage = 1)
        where T : IEnumerable<PageCropping>
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}, " +
            $"X-coordinates of all points must be >= {minX}, " +
            $"Y-coordinates of all points must be >= {minY}";

        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreatePageCroppingCorrectPredicate<T>(minX, minY, minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageDivisionsOption<T>(
        Subcommand subcommand,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageDivision>
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreatePageDivisionCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageOrderOption(
        Subcommand subcommand,
        string optionLongName,
        int minPage = 1)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreatePageOrderCorrectPredicate(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageRotationsOption<T>(
        Subcommand subcommand,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageRotation>
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreatePageRotationCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForSplitPartsOption<T>(
        Subcommand subcommand,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageRange>
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreatePageRangeCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForSplitPartSizeExpressionOption(
        Subcommand subcommand,
        string optionLongName)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = "expression must be correct";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            subcommand,
            optionLongName,
            CreateLongMathExpressionCorrectPredicate(),
            badValueMessage);
    }
}
