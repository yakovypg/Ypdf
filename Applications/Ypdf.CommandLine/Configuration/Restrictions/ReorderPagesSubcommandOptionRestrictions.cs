using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class ReorderPagesSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPageOrderOption,
    ];

    private static void AddRestrictionForPageOrderOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForPageOrderOption(
            subcommand: subcommand,
            optionLongName: ReorderPagesSubcommand.PageOrderLongName,
            minPage: 1);
    }
}
