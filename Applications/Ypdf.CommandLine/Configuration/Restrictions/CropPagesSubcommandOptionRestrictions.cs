using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class CropPagesSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPageCroppingsOption
    ];

    private static void AddRestrictionForPageCroppingsOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForPageCroppingEnumerableOption<List<PageCropping>>(
            subcommand: subcommand,
            optionLongName: CropPagesSubcommand.PageCroppingsLongName,
            minX: 0,
            minY: 0,
            minPage: 1);
    }
}
