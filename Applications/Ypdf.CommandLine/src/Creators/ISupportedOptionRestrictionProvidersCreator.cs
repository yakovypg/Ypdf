using System.Collections.Generic;
using Ypdf.CommandLine.Configuration.Restrictions;

namespace Ypdf.CommandLine.Creators;

internal interface ISupportedOptionRestrictionProvidersCreator
{
    Dictionary<string, IOptionRestrictionProvider> Create();
}
