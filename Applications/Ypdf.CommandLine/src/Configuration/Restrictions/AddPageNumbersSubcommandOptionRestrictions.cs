using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddPageNumbersSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForInputPathOption,
        AddRestrictionForFontPathOption,
        AddRestrictionForFontSizeOption,
        AddRestrictionForFontOpacityOption
    ];

    private static void AddRestrictionForInputPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileRestrictionForPathOption(subcommand, AddPageNumbersSubcommand.InputPathLongName, PdfExtensions);
    }

    private static void AddRestrictionForFontPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileRestrictionForPathOption(subcommand, AddPageNumbersSubcommand.FontPathLongName, FontExtensions);
    }

    private static void AddRestrictionForFontSizeOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForValueOption<float>(subcommand, AddPageNumbersSubcommand.FontSizeLongName, 1, 512);
    }

    private static void AddRestrictionForFontOpacityOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForValueOption<float>(subcommand, AddPageNumbersSubcommand.FontOpacityLongName, 0, 1);
    }
}
