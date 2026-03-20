using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class RemoveWatermarkAnnotationToolCreator : ToolCreator
{
    public RemoveWatermarkAnnotationToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        RemoveWatermarkAnnotationSubcommand subcommand = config.RemoveWatermarkAnnotationSubcommand;

        RemoveWatermarkAnnotationTool tool = new(
            pages: subcommand.Pages.Count > 0 ? subcommand.Pages : null);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
