using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class CopyToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        CopySubcommand subcommand = config.CopySubcommand;

        var tool = new CopyTool();

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
