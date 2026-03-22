using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class TextToPdfToolCreator : ToolCreator
{
    public TextToPdfToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        TextToPdfSubcommand subcommand = config.TextToPdfSubcommand;

        var fontInfo = string.IsNullOrEmpty(subcommand.FontPath)
            ? new TextFontInfo(
                subcommand.FontFamily,
                subcommand.FontColor,
                subcommand.FontSize,
                subcommand.FontOpacity)
            : new TextFontInfo(
                subcommand.FontPath,
                subcommand.FontEncoding,
                subcommand.FontColor,
                subcommand.FontSize,
                subcommand.FontOpacity);

        var textPageParameters = new TextPageParameters(
            subcommand.Margin,
            subcommand.PageSize,
            subcommand.TextAlignment,
            fontInfo);

        var tool = new TextToPdfTool(textPageParameters);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
