using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class RotatePagesSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForPageRotationsOption,
    ];

    private static void AddRestrictionForPageRotationsOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddRestrictionForPageRotationEnumerableOption<IList<PageRotation>>(
            subcommand: subcommand,
            optionLongName: RotatePagesSubcommand.PageRotationsLongName,
            minPage: 1);
    }
}
