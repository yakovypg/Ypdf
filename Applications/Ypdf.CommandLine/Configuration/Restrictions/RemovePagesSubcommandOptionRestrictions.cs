using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class RemovePagesSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPagesOption,
    ];

    private static void AddRestrictionForPagesOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForSplitPartsOption<List<PageRange>>(
            subcommand: subcommand,
            optionLongName: RemovePagesSubcommand.PagesLongName,
            minPage: 1);
    }
}
