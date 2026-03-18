using NetArgumentParser.Informing;
using Ypdf.CommandLine.Configuration;

namespace Ypdf.CommandLine.Execution;

internal interface IToolExecutor
{
    void Execute(YpdfParserConfig config, ParseArgumentsResult parseResult);
}
