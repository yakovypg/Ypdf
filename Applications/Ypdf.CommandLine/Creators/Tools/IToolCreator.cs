using Ypdf.CommandLine.Configuration;
using Ypdf.Core.Execution;

namespace Ypdf.CommandLine.Creators.Tools;

internal interface IToolCreator
{
    IToolExecutionProvider Create(YpdfParserConfig config);
}
