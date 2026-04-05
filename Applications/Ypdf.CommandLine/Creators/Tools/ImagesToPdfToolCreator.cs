using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class ImagesToPdfToolCreator : ToolCreator
{
    public ImagesToPdfToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        ImagesToPdfSubcommand subcommand = config.ImagesToPdfSubcommand;

        var imagePageParameters = new ImagePageParameters(
            subcommand.Margin,
            subcommand.PageSize,
            subcommand.PageRotationAngleDegrees,
            !subcommand.DisableAutoIncreasePageSize,
            subcommand.ImageHorizontalAlignment);

        var tool = new ImageToPdfTool(imagePageParameters);

        var toolExecutionParameters = new ToolExecutionParameters(
            tool,
            subcommand.InputPaths,
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
