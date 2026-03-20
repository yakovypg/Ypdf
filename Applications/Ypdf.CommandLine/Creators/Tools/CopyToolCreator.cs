using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class CopyToolCreator : ToolCreator
{
    public CopyToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        CopySubcommand subcommand = config.CopySubcommand;
        CopyTool tool = new();

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
