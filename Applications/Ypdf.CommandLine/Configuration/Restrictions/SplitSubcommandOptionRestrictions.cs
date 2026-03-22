using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class SplitSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForSplitPartsOption,
        AddRestrictionForSplitPartSizeExpressionOption
    ];

    private static void AddRestrictionForSplitPartsOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageRangeEnumerableOption<IList<PageRange>>(
            parserQuantum: parserQuantum,
            optionLongName: SplitSubcommand.SplitPartsLongName,
            minPage: 1);
    }

    private static void AddRestrictionForSplitPartSizeExpressionOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForLongMathExpressionOption(
            parserQuantum: parserQuantum,
            optionLongName: SplitSubcommand.SplitPartSizeExpressionLongName);
    }
}
