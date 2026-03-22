using System.Collections.Generic;
using NetArgumentParser;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Exceptions;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class OptionRestrictionSetter : IOptionRestrictionSetter
{
    internal OptionRestrictionSetter(
        IReadOnlyDictionary<string, IOptionRestrictionProvider> optionRestrictionProviders)
    {
        ExtendedArgumentNullException.ThrowIfNull(optionRestrictionProviders, nameof(optionRestrictionProviders));
        OptionRestrictionProviders = optionRestrictionProviders;
    }

    internal IReadOnlyDictionary<string, IOptionRestrictionProvider> OptionRestrictionProviders { get; }

    public void SetRestrictions(ArgumentParser parser)
    {
        ExtendedArgumentNullException.ThrowIfNull(parser, nameof(parser));

        foreach (Subcommand subcommand in parser.Subcommands)
        {
            bool hasRestrictions = OptionRestrictionProviders.TryGetValue(
                subcommand.Name,
                out IOptionRestrictionProvider? restrictions);

            if (hasRestrictions && restrictions is not null)
                restrictions.AddRestrictions(subcommand);
        }
    }
}
