using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class SplitToolCreator : ToolCreator
{
    public SplitToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
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

        ToolExecutionParameters toolExecutionParameters = new(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
