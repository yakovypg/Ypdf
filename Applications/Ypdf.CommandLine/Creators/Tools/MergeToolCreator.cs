using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class MergeToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        MergeSubcommand subcommand = config.MergeSubcommand;
        MergeTool tool = new();

        return new ToolExecutionProvider(
            tool,
            subcommand.InputPaths,
            subcommand.OutputPath);
    }
}
