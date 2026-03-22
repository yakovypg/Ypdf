using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddPageNumbersSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForPageNumberShiftsOption
    ];

    private static void AddRestrictionForPageNumberShiftsOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageContentShiftEnumerableOption<IList<PageContentShift>>(
            parserQuantum: parserQuantum,
            optionLongName: AddPageNumbersSubcommand.PageNumberShiftsLongName,
            minPage: 1);
    }
}
