using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Design;
using Ypdf.Core.Design.Fonts;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class AddPageNumbersToolCreator : ToolCreator
{
    public AddPageNumbersToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        AddPageNumbersSubcommand subcommand = config.AddPageNumbersSubcommand;

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

        var pageNumberStyle = new PageNumberStyle()
        {
            FontInfo = fontInfo,
            HorizontalAlignment = subcommand.HorizontalNumberAlignment,
            VerticalAlignment = subcommand.VerticalNumberAlignment,
            Margin = subcommand.Margin,
            ConsiderLeftPageMargin = subcommand.ConsiderLeftPageMargin,
            ConsiderTopPageMargin = subcommand.ConsiderTopPageMargin,
            ConsiderRightPageMargin = subcommand.ConsiderRightPageMargin,
            ConsiderBottomPageMargin = subcommand.ConsiderBottomPageMargin,
            TextPresenter = subcommand.TextPresenter
        };

        var addPageNumbersTool = new AddPageNumbersTool(pageNumberStyle, subcommand.PageNumberShifts);

        var increasePageSizeTool = new IncreasePageSizeTool(
            subcommand.GeneralPageSizeAdjustment,
            subcommand.FillColor,
            subcommand.IncreasePageMode);

        ITool tool = subcommand.IncreasePageMode != IncreasePageMode.WithoutIncrease
            ? new ToolsPipeline([increasePageSizeTool, addPageNumbersTool])
            : addPageNumbersTool;

        ToolExecutionParameters toolExecutionParameters = new(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
