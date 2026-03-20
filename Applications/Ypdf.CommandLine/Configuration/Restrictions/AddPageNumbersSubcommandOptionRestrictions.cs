using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddPageNumbersSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPageNumberShiftsOption
    ];

    private static void AddRestrictionForPageNumberShiftsOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForPageContentShiftEnumerableOption<List<PageContentShift>>(
            subcommand: subcommand,
            optionLongName: AddPageNumbersSubcommand.PageNumberShiftsLongName,
            minPage: 1);
    }
}
