using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class DividePagesSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForPageDivisionsOption
    ];

    private static void AddRestrictionForPageDivisionsOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageDivisionEnumerableOption<IList<PageDivision>>(
            parserQuantum: parserQuantum,
            optionLongName: DividePagesSubcommand.PageDivisionsLongName,
            minPage: 1);
    }
}
