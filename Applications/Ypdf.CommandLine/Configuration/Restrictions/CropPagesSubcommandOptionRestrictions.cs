using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class CropPagesSubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders =>
    [
        AddRestrictionForPageCroppingsOption
    ];

    private static void AddRestrictionForPageCroppingsOption(ParserQuantum parserQuantum)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserQuantum, nameof(parserQuantum));

        AddRestrictionForPageCroppingEnumerableOption<IList<PageCropping>>(
            parserQuantum: parserQuantum,
            optionLongName: CropPagesSubcommand.PageCroppingsLongName,
            minX: 0,
            minY: 0,
            minPage: 1);
    }
}
