using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class ReorderPagesSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForPageOrderOption,
    ];

    private static void AddRestrictionForPageOrderOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageOrderOption(
            parserQuantum: parserQuantum,
            optionLongName: ReorderPagesSubcommand.PageOrderLongName,
            minPage: 1);
    }
}
