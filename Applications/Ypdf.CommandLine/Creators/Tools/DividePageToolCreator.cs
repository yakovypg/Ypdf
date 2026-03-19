using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class DividePageToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        DividePageSubcommand subcommand = config.DividePageSubcommand;

        var tool = new DividePageTool(subcommand.PageDivisions);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
