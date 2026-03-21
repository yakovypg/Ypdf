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

    protected static Predicate<PageContentShift> CreatePageContentShiftCorrectPredicate(int minPage = 1)
    {
        return t => t.PageNumber >= minPage;
    }

    protected static Predicate<T> CreateAllPageContentShiftsCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageContentShift>
    {
        Predicate<PageContentShift> correctPredicate = CreatePageContentShiftCorrectPredicate(minPage);
        return shifts => shifts.All(t => correctPredicate(t));
    }

    protected static Predicate<FloatPoint> CreateFloatPointCorrectPredicate(float minX = 0, float minY = 0)
    {
        return value => value.X >= minX && value.Y >= minY;
    }

    protected static Predicate<PageCropping> CreatePageCroppingCorrectPredicate(
        float minX = 0,
        float minY = 0,
        int minPage = 1)
    {
        return t =>
            t.LowerLeft.X >= minX &&
            t.LowerLeft.Y >= minY &&
            t.UpperRight.X >= minX &&
            t.UpperRight.Y >= minY &&
            t.PageNumber >= minPage;
    }

    protected static Predicate<T> CreateAllPageCroppingsCorrectPredicate<T>(
        float minX = 0,
        float minY = 0,
        int minPage = 1)
        where T : IEnumerable<PageCropping>
    {
        Predicate<PageCropping> correctPredicate = CreatePageCroppingCorrectPredicate(minX, minY, minPage);
        return croppings => croppings.All(t => correctPredicate(t));
    }

    protected static Predicate<PageDivision> CreatePageDivisionCorrectPredicate(int minPage = 1)
    {
        return t => t.PageNumber >= minPage;
    }

    protected static Predicate<T> CreateAllPageDivisionsCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageDivision>
    {
        Predicate<PageDivision> correctPredicate = CreatePageDivisionCorrectPredicate(minPage);
        return divisions => divisions.All(t => correctPredicate(t));
    }

    protected static Predicate<PageOrder> CreatePageOrderCorrectPredicate(int minPage = 1)
    {
        return pageOrder => pageOrder.Pages.All(t => t >= minPage);
    }

    protected static Predicate<PageRotation> CreatePageRotationCorrectPredicate(int minPage = 1)
    {
        return t => t.PageNumber >= minPage;
    }

    protected static Predicate<T> CreateAllPageRotationsCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageRotation>
    {
        Predicate<PageRotation> correctPredicate = CreatePageRotationCorrectPredicate(minPage);
        return rotations => rotations.All(t => correctPredicate(t));
    }

    protected static Predicate<PageRange> CreatePageRangeCorrectPredicate(int minPage = 1)
    {
        return t => t.Start >= minPage;
    }

    protected static Predicate<T> CreateAllPageRangesCorrectPredicate<T>(int minPage = 1)
        where T : IEnumerable<PageRange>
    {
        Predicate<PageRange> correctPredicate = CreatePageRangeCorrectPredicate(minPage);
        return ranges => ranges.All(t => correctPredicate(t));
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
            : throw new SubcommandConfiguredIncorrectlyException(null, subcommand);
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

    protected static void AddRestrictionForPageContentShiftEnumerableOption<T>(
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
            CreateAllPageContentShiftsCorrectPredicate<T>(minPage),
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

    protected static void AddRestrictionForPageCroppingEnumerableOption<T>(
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
            CreateAllPageCroppingsCorrectPredicate<T>(minX, minY, minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageDivisionEnumerableOption<T>(
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
            CreateAllPageDivisionsCorrectPredicate<T>(minPage),
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

    protected static void AddRestrictionForPageRotationEnumerableOption<T>(
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
            CreateAllPageRotationsCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageRangeEnumerableOption<T>(
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
            CreateAllPageRangesCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForLongMathExpressionOption(
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
