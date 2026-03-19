using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddWatermarkAnnotationSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForLowerLeftPointOption
    ];

    private static void AddRestrictionForLowerLeftPointOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddRestrictionForFloatPointOption(subcommand, AddWatermarkAnnotationSubcommand.LowerLeftPointLongName, 0, 0);
    }
}
