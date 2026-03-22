using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class RotatePagesSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForPageRotationsOption,
    ];

    private static void AddRestrictionForPageRotationsOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageRotationEnumerableOption<IList<PageRotation>>(
            parserQuantum: parserQuantum,
            optionLongName: RotatePagesSubcommand.PageRotationsLongName,
            minPage: 1);
    }
}
