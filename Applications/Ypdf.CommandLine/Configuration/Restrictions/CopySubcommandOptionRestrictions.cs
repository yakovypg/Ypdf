using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class CopySubcommandOptionRestrictions : OptionRestrictions
{
    protected override IReadOnlyCollection<Action<ParserQuantum>> ConfigurationProviders => [];
}
