using System.Collections.Generic;
using Ypdf.CommandLine.Creators.Tools;

namespace Ypdf.CommandLine.Creators;

internal interface ISupportedToolsCreator
{
    Dictionary<string, IToolCreator> Create();
}
