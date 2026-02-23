using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddWatermarkSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForInputPathOption,
        AddRestrictionForPagesOption,
        AddRestrictionForTextOption,
        AddRestrictionForWidthOption,
        AddRestrictionForHeightOption,
        AddRestrictionForLowerLeftPointOption,
        AddRestrictionForBorderOpacityOption,
        AddRestrictionForBorderThicknessOption,
        AddRestrictionForFontPathOption,
        AddRestrictionForFontSizeOption,
        AddRestrictionForFontOpacityOption
    ];

    private static void AddRestrictionForInputPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileAllowedRestrictionForPathOption(subcommand, AddWatermarkSubcommand.InputPathLongName, PdfExtensions);
    }

    private static void AddRestrictionForPagesOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddGreaterThanZeroRestrictionForPagesOption<List<PageRange>>(subcommand, AddWatermarkSubcommand.PagesLongName);
    }

    private static void AddRestrictionForTextOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddNotEmptyRestrictionForStringOption(subcommand, AddWatermarkSubcommand.TextLongName);
    }

    private static void AddRestrictionForWidthOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkSubcommand.WidthLongName, 1, 100_000);
    }

    private static void AddRestrictionForHeightOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkSubcommand.HeightLongName, 1, 100_000);
    }

    private static void AddRestrictionForLowerLeftPointOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddCorrectRestrictionForFloatPointOption(subcommand, AddWatermarkSubcommand.LowerLeftPointLongName, 0, 0);
    }

    private static void AddRestrictionForBorderOpacityOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(
            subcommand,
            AddWatermarkSubcommand.BorderOpacityLongName,
            0,
            1);
    }

    private static void AddRestrictionForBorderThicknessOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddInRangeRestrictionForNumberOption<float>(
            subcommand,
            AddWatermarkSubcommand.BorderThicknessLongName,
            1,
            100_000);
    }

    private static void AddRestrictionForFontPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileAllowedRestrictionForPathOption(subcommand, AddWatermarkSubcommand.FontPathLongName, FontExtensions);
    }

    private static void AddRestrictionForFontSizeOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkSubcommand.FontSizeLongName, 1, 512);
    }

    private static void AddRestrictionForFontOpacityOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkSubcommand.FontOpacityLongName, 0, 1);
    }
}
