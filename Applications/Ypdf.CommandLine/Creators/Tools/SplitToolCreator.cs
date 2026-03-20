using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class SplitToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        SplitSubcommand subcommand = config.SplitSubcommand;
        SplitTool tool;

        if (subcommand.SplitParts.Count > 0)
            tool = new(subcommand.SplitParts);
        else if (subcommand.SplitPartSizeExpression is not null)
            tool = new(subcommand.SplitPartSizeExpression);
        else
            tool = new();

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
