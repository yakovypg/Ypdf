using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class DividePagesSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPageDivisionsOption
    ];

    private static void AddRestrictionForPageDivisionsOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForPageDivisionEnumerableOption<List<PageDivision>>(
            subcommand: subcommand,
            optionLongName: DividePagesSubcommand.PageDivisionsLongName,
            minPage: 1);
    }
}
