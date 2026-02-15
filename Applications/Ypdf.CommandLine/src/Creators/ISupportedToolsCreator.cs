using System.Collections.Generic;

namespace Ypdf.CommandLine.Creators;

internal interface ISupportedToolsCreator
{
    Dictionary<string, IToolCreator> Create();
}
