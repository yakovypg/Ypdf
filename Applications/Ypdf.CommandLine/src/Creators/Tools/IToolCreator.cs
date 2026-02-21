using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;

namespace Ypdf.CommandLine.Creators.Tools;

internal interface IToolCreator
{
    IToolExecutionProvider Create(YpdfParserConfig config);
}
