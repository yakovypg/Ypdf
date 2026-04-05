using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class MergeToolCreator : ToolCreator
{
    public MergeToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        MergeSubcommand subcommand = config.MergeSubcommand;
        MergeTool tool = new();

        ToolExecutionParameters toolExecutionParameters = new(
            tool,
            subcommand.InputPaths,
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
