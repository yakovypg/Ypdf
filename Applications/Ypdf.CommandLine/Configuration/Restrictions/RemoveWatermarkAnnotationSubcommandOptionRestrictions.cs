using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class RemoveWatermarkAnnotationSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPagesOption
    ];

    private static void AddRestrictionForPagesOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForPageRangeEnumerableOption<IList<PageRange>>(
            subcommand: subcommand,
            optionLongName: RemoveWatermarkAnnotationSubcommand.PagesLongName,
            minPage: 1);
    }
}
