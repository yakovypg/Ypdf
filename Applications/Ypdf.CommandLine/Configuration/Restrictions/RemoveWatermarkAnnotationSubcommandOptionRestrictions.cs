using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class RemoveWatermarkAnnotationSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForPagesOption
    ];

    private static void AddRestrictionForPagesOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageRangeEnumerableOption<IList<PageRange>>(
            parserQuantum: parserQuantum,
            optionLongName: RemoveWatermarkAnnotationSubcommand.PagesLongName,
            minPage: 1);
    }
}
