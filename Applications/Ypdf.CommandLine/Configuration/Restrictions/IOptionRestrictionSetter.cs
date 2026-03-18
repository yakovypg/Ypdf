using NetArgumentParser;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal interface IOptionRestrictionSetter
{
    void SetRestrictions(ArgumentParser parser);
}
