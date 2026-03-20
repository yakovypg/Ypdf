using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class SplitSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForSplitPartsOption,
        AddRestrictionForSplitPartSizeExpressionOption
    ];

    private static void AddRestrictionForSplitPartsOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForPageRangeEnumerableOption<List<PageRange>>(
            subcommand: subcommand,
            optionLongName: SplitSubcommand.SplitPartsLongName,
            minPage: 1);
    }

    private static void AddRestrictionForSplitPartSizeExpressionOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForLongMathExpressionOption(
            subcommand: subcommand,
            optionLongName: SplitSubcommand.SplitPartSizeExpressionLongName);
    }
}
