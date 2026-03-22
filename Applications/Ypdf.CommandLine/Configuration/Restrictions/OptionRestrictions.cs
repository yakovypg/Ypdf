using System;
using System.Collections.Generic;
using System.Linq;
using NetArgumentParser.Options;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal abstract class OptionRestrictions : OptionRestrictionProvider
{
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

    protected static void AddRestrictionForPageContentShiftEnumerableOption<T>(
        ParserQuantum parserQuantum,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageContentShift>
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreateAllPageContentShiftsCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForFloatPointOption(
        ParserQuantum parserQuantum,
        string optionLongName,
        float minX = 0,
        float minY = 0)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"X-coordinates must be >= {minX} and Y-coordinates must be >= {minY}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreateFloatPointCorrectPredicate(minX, minY),
            badValueMessage);
    }

    protected static void AddRestrictionForPageCroppingEnumerableOption<T>(
        ParserQuantum parserQuantum,
        string optionLongName,
        float minX = 0,
        float minY = 0,
        int minPage = 1)
        where T : IEnumerable<PageCropping>
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}, " +
            $"X-coordinates of all points must be >= {minX}, " +
            $"Y-coordinates of all points must be >= {minY}";

        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreateAllPageCroppingsCorrectPredicate<T>(minX, minY, minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageDivisionEnumerableOption<T>(
        ParserQuantum parserQuantum,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageDivision>
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreateAllPageDivisionsCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageOrderOption(
        ParserQuantum parserQuantum,
        string optionLongName,
        int minPage = 1)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreatePageOrderCorrectPredicate(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageRotationEnumerableOption<T>(
        ParserQuantum parserQuantum,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageRotation>
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreateAllPageRotationsCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForPageRangeEnumerableOption<T>(
        ParserQuantum parserQuantum,
        string optionLongName,
        int minPage = 1)
        where T : IEnumerable<PageRange>
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = $"all pages must be >= {minPage}";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreateAllPageRangesCorrectPredicate<T>(minPage),
            badValueMessage);
    }

    protected static void AddRestrictionForLongMathExpressionOption(
        ParserQuantum parserQuantum,
        string optionLongName)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));
        ExtendedArgumentNullException.ThrowIfNull(optionLongName, nameof(optionLongName));

        string badValueReason = "expression must be correct";
        string badValueMessage = CreateValueNotSatisfuRestrictionMessage(optionLongName, badValueReason);

        AddRestriction(
            parserQuantum,
            optionLongName,
            CreateLongMathExpressionCorrectPredicate(),
            badValueMessage);
    }
}
