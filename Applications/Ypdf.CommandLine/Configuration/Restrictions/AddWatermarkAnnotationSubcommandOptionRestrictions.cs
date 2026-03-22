using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddWatermarkAnnotationSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForPagesOption,
        AddRestrictionForLowerLeftPointOption
    ];

    private static void AddRestrictionForPagesOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageRangeEnumerableOption<IList<PageRange>>(
            parserQuantum: parserQuantum,
            optionLongName: AddWatermarkAnnotationSubcommand.PagesLongName,
            minPage: 1);
    }

    private static void AddRestrictionForLowerLeftPointOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForFloatPointOption(
            parserQuantum: parserQuantum,
            optionLongName: AddWatermarkAnnotationSubcommand.LowerLeftPointLongName,
            minX: 0,
            minY: 0);
    }
}
