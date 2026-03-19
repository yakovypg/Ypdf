using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class CheckCompressionCapabilitySubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders => [];
}
