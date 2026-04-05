using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Converters;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design.Borders;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Design.Watermarks;
using Ypdf.Core.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class AddWatermarkToolCreator : ToolCreator
{
    public AddWatermarkToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        AddWatermarkSubcommand subcommand = config.AddWatermarkSubcommand;

        LazyBorder? border = subcommand.BorderType is not null
            ? new LazyBorder(
                subcommand.BorderType.Value,
                ColorConverter.ToDeviceRgb(subcommand.BorderColor),
                subcommand.BorderThickness,
                subcommand.BorderOpacity)
            : null;

        var textAllocator = new WatermarkTextAllocator(
            subcommand.TextAlignment,
            subcommand.TextHorizontalAlignment,
            subcommand.TextContainerVerticalAlignment);

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

        var watermark = new IndelibleWatermark(
            width: subcommand.Width,
            height: subcommand.Height,
            text: subcommand.Text,
            fontInfo: fontInfo,
            lowerLeftPoint: subcommand.LowerLeftPoint,
            textAllocator: textAllocator,
            border: border);

        watermark.SetRotationAngleInDegrees(subcommand.RotationAngleDegrees);

        var tool = new AddIndelibleWatermarkTool(
            watermark: watermark,
            pages: subcommand.Pages.Count > 0 ? subcommand.Pages : null);

        var toolExecutionParameters = new ToolExecutionParameters(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
