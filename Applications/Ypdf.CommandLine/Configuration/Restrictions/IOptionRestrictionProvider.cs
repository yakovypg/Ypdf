using NetArgumentParser.Subcommands;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal interface IOptionRestrictionProvider
{
    void AddRestrictions(Subcommand subcommand);
}
