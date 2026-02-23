using System.Collections.Generic;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.CommandLine.Converters;
using Ypdf.Core.Design;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration;

internal sealed class AddPageNumbersSubcommand
{
    internal const string Name = "add-page-numbers";
    internal const string Description = "Add page numbers to PDF document";

    internal const string InputPathLongName = "input-file";
    internal const string OutputPathLongName = "output-file";
    internal const string HorizontalNumberAlignmentLongName = "horizontal-alignment";
    internal const string VerticalNumberLongName = "vertical-alignment";
    internal const string MarginLongName = "margin";
    internal const string LocationModeLongName = "num-location-mode";
    internal const string ConsiderLeftPageMarginLongName = "left-page-margin";
    internal const string ConsiderTopPageMarginLongName = "top-page-margin";
    internal const string ConsiderRightPageMarginLongName = "right-page-margin";
    internal const string ConsiderBottomPageMarginLongName = "bottom-page-margin";
    internal const string TextPresenterLongName = "num-presenter";
    internal const string PageNumberShiftsLongName = "content-shift";
    internal const string FontPathLongName = "font-path";
    internal const string FontEncodingLongName = "font-encoding";
    internal const string FontSizeLongName = "font-size";
    internal const string FontOpacityLongName = "font-opacity";
    internal const string FontFamilyLongName = "font-family";
    internal const string FontColorLongName = "font-color";

    internal const string DefaultTextPresenter = nameof(PageNumberTextPresenter.Default);
    internal const string DefaultMargin = "0";
    internal const string DefaultFontColor = nameof(ColorConstants.BLACK);

    [ValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "path to the input file",
        isRequired: true)
    ]
    [OptionGroup("paths", "Paths", "Options for configuring paths")]
    internal string InputPath { get; set; } = string.Empty;

    [ValueOption<string>(
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;

    [EnumValueOption<TabAlignment>(
        defaultValue: TabAlignment.CENTER,
        longName: HorizontalNumberAlignmentLongName,
        shortName: "H",
        description: "horizontal page number alignment",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page number appearance")]
    internal TabAlignment HorizontalNumberAlignment { get; set; }

    [EnumValueOption<VerticalAlignment>(
        defaultValue: VerticalAlignment.BOTTOM,
        longName: VerticalNumberLongName,
        shortName: "V",
        description: "vertical page number alignment",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    internal VerticalAlignment VerticalNumberAlignment { get; set; }

    [ValueOption<Margin>(
        longName: MarginLongName,
        shortName: "m",
        description: $"page number margin [default={DefaultMargin}] (M or H,V or L,T,R,B)")
    ]
    [OptionGroup("appearance", "", "")]
    internal Margin Margin { get; set; } = Margin.Parse(DefaultMargin);

    [EnumValueOption<LocationMode>(
        defaultValue: LocationMode.WithoutIncrease,
        longName: LocationModeLongName,
        shortName: "l",
        description: "page number location mode",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    internal LocationMode LocationMode { get; set; }

    [FlagOption(
        longName: ConsiderLeftPageMarginLongName,
        shortName: "L",
        description: "indicate whether the left page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    internal bool ConsiderLeftPageMargin { get; set; }

    [FlagOption(
        longName: ConsiderTopPageMarginLongName,
        shortName: "T",
        description: "indicate whether the top page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    internal bool ConsiderTopPageMargin { get; set; }

    [FlagOption(
        longName: ConsiderRightPageMarginLongName,
        shortName: "R",
        description: "indicate whether the right page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    internal bool ConsiderRightPageMargin { get; set; }

    [FlagOption(
        longName: ConsiderBottomPageMarginLongName,
        shortName: "B",
        description: "indicate whether the bottom page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    internal bool ConsiderBottomPageMargin { get; set; }

    [ValueOption<PageNumberTextPresenter>(
        longName: TextPresenterLongName,
        shortName: "P",
        description: $"page number presenter [default={DefaultTextPresenter}]",
        beforeParseChoices: [
            nameof(PageNumberTextPresenter.Default),
            nameof(PageNumberTextPresenter.Fractional),
            nameof(PageNumberTextPresenter.Verbal)
        ],
        addBeforeParseChoicesToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    internal PageNumberTextPresenter TextPresenter { get; set; } = PageNumberTextPresenter.Parse(DefaultTextPresenter);

    [MultipleValueOption<PageContentShift>(
        longName: PageNumberShiftsLongName,
        shortName: "",
        description: "page number shifts [default=[]] (Pages:Horizontal,Vertical -> 1:-50,0 or 1,3-5:10,15)",
        contextCaptureType: ContextCaptureType.ZeroOrMore)
    ]
    [OptionGroup("appearance", "", "")]
    internal List<PageContentShift> PageNumberShifts { get; set; } = [];

    [ValueOption<string>(
        defaultValue: "",
        longName: FontPathLongName,
        shortName: "",
        description: "page number font path [default=\"\"]")
    ]
    [OptionGroup("font", "Font", "Options for configuring page number font")]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(AddPageNumbersSubcommand)}.FontConfig",
        "font config",
        $"{nameof(FontPath)} cannot be used with the {nameof(FontFamily)}")
    ]
    internal string FontPath { get; set; } = string.Empty;

    [ValueOption<string>(
        defaultValue: PdfEncodings.IDENTITY_H,
        longName: FontEncodingLongName,
        shortName: "",
        description: "page number font encoding",
        choices:
        [
            PdfEncodings.CP1250,
            PdfEncodings.CP1252,
            PdfEncodings.CP1253,
            PdfEncodings.CP1257,
            PdfEncodings.IDENTITY_H,
            PdfEncodings.IDENTITY_V,
            PdfEncodings.MACROMAN,
            PdfEncodings.PDF_DOC_ENCODING,
            PdfEncodings.SYMBOL,
            PdfEncodings.UNICODE_BIG,
            PdfEncodings.UNICODE_BIG_UNMARKED,
            PdfEncodings.UTF8,
            PdfEncodings.WINANSI,
            PdfEncodings.ZAPFDINGBATS
        ],
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("font", "", "")]
    internal string FontEncoding { get; set; } = string.Empty;

    [ValueOption<float>(
        defaultValue: 24f,
        longName: FontSizeLongName,
        shortName: "",
        description: "page number font size",
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("font", "", "")]
    internal float FontSize { get; set; }

    [ValueOption<float>(
        defaultValue: 1f,
        longName: FontOpacityLongName,
        shortName: "",
        description: "page number font opacity",
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("font", "", "")]
    internal float FontOpacity { get; set; }

    [ValueOption<string>(
        defaultValue: StandardFonts.TIMES_ROMAN,
        longName: FontFamilyLongName,
        shortName: "",
        description: "page number font family",
        choices:
        [
            StandardFonts.COURIER,
            StandardFonts.COURIER_BOLD,
            StandardFonts.COURIER_OBLIQUE,
            StandardFonts.COURIER_BOLDOBLIQUE,
            StandardFonts.HELVETICA,
            StandardFonts.HELVETICA_BOLD,
            StandardFonts.HELVETICA_OBLIQUE,
            StandardFonts.HELVETICA_BOLDOBLIQUE,
            StandardFonts.SYMBOL,
            StandardFonts.TIMES_ROMAN,
            StandardFonts.TIMES_BOLD,
            StandardFonts.TIMES_ITALIC,
            StandardFonts.TIMES_BOLDITALIC,
            StandardFonts.ZAPFDINGBATS
        ],
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("font", "", "")]
    [MutuallyExclusiveOptionGroup($"{nameof(AddPageNumbersSubcommand)}.FontConfig", "", "")]
    internal string FontFamily { get; set; } = string.Empty;

    [ValueOption<Color>(
        longName: FontColorLongName,
        shortName: "c",
        description: $"page number font color [default={DefaultFontColor}] (name or (r,g,b))",
        beforeParseChoices:
        [
            nameof(ColorConstants.BLACK),
            nameof(ColorConstants.BLUE),
            nameof(ColorConstants.CYAN),
            nameof(ColorConstants.DARK_GRAY),
            nameof(ColorConstants.GRAY),
            nameof(ColorConstants.GREEN),
            nameof(ColorConstants.LIGHT_GRAY),
            nameof(ColorConstants.MAGENTA),
            nameof(ColorConstants.ORANGE),
            nameof(ColorConstants.PINK),
            nameof(ColorConstants.RED),
            nameof(ColorConstants.WHITE),
            nameof(ColorConstants.YELLOW)
        ],
        addBeforeParseChoicesToDescription: true)
    ]
    [OptionGroup("font", "", "")]
    internal Color FontColor { get; set; } = ColorConverter.Parse(DefaultFontColor);
}
