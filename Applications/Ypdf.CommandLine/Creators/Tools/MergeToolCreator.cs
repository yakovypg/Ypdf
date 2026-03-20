using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
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

        return new ToolExecutionProvider(
            tool,
            subcommand.InputPaths,
            subcommand.OutputPath);
    }
}
