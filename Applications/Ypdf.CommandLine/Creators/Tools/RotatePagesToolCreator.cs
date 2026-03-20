using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class RotatePagesToolCreator : ToolCreator
{
    public RotatePagesToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        RotatePagesSubcommand subcommand = config.RotatePagesSubcommand;
        RotateTool tool = new(subcommand.PageRotations);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
