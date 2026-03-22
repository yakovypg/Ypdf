using System.Collections.Generic;
using NetArgumentParser.Options;

namespace Ypdf.CommandLine.Creators;

internal interface ISupportedOptionRestrictionProvidersCreator
{
    Dictionary<string, IOptionConfigurationProvider> Create();
}
