using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddWatermarkSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForLowerLeftPointOption
    ];

    private static void AddRestrictionForLowerLeftPointOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddRestrictionForFloatPointOption(subcommand, AddWatermarkSubcommand.LowerLeftPointLongName, 0, 0);
    }
}
