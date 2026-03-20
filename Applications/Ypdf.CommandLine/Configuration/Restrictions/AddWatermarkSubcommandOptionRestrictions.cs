using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddWatermarkSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPagesOption,
        AddRestrictionForLowerLeftPointOption
    ];

    private static void AddRestrictionForPagesOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForSplitPartsOption<List<PageRange>>(
            subcommand: subcommand,
            optionLongName: AddWatermarkSubcommand.PagesLongName,
            minPage: 1);
    }

    private static void AddRestrictionForLowerLeftPointOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForFloatPointOption(
            subcommand: subcommand,
            optionLongName: AddWatermarkSubcommand.LowerLeftPointLongName,
            minX: 0,
            minY: 0);
    }
}
