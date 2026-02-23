using System.Collections.Generic;
using System.Linq;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Design.Watermarks;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class AddWatermarkAnnotationToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        AddWatermarkAnnotationSubcommand subcommand = config.AddWatermarkAnnotationSubcommand;

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

        var watermarkAnnotation = new WatermarkAnnotation(
            text: subcommand.Text,
            fontInfo: fontInfo,
            lowerLeftPoint: subcommand.LowerLeftPoint)
        {
            XTranslation = subcommand.XTranslation,
            YTranslation = subcommand.YTranslation
        };

        watermarkAnnotation.SetWidth(subcommand.Width);
        watermarkAnnotation.SetHeight(subcommand.Height);
        watermarkAnnotation.SetRotationAngleInDegrees(subcommand.RotationAngleDegrees);

        IEnumerable<int> pages = subcommand.Pages.SelectMany(t => t.Items);

        var tool = new AddWatermarkAnnotationTool(
            watermark: watermarkAnnotation,
            pages: pages.Any() ? pages : null);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
