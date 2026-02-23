using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddPageNumbersSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForInputPathOption,
        AddRestrictionForPageNumberShiftsOption,
        AddRestrictionForFontPathOption,
        AddRestrictionForFontSizeOption,
        AddRestrictionForFontOpacityOption
    ];

    private static void AddRestrictionForInputPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileAllowedRestrictionForPathOption(subcommand, AddPageNumbersSubcommand.InputPathLongName, PdfExtensions);
    }

    private static void AddRestrictionForPageNumberShiftsOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddCorrectRestrictionForPageContentShiftsOption<List<PageContentShift>>(
            subcommand,
            AddPageNumbersSubcommand.PageNumberShiftsLongName,
            1);
    }

    private static void AddRestrictionForFontPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileAllowedRestrictionForPathOption(subcommand, AddPageNumbersSubcommand.FontPathLongName, FontExtensions);
    }

    private static void AddRestrictionForFontSizeOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddPageNumbersSubcommand.FontSizeLongName, 1, 512);
    }

    private static void AddRestrictionForFontOpacityOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddPageNumbersSubcommand.FontOpacityLongName, 0, 1);
    }
}
