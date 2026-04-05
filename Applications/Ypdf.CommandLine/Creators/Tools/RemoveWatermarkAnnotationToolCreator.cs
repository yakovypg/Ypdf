using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Execution;
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

        ToolExecutionParameters toolExecutionParameters = new(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
