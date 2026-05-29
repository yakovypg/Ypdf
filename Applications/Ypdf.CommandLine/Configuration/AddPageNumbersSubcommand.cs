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
    internal const string Description = "Add page numbers to the PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string HorizontalNumberAlignmentLongName = "horizontal-alignment";
    internal const string VerticalNumberLongName = "vertical-alignment";
    internal const string MarginLongName = "margin";
    internal const string ConsiderLeftPageMarginLongName = "left-page-margin";
    internal const string ConsiderTopPageMarginLongName = "top-page-margin";
    internal const string ConsiderRightPageMarginLongName = "right-page-margin";
    internal const string ConsiderBottomPageMarginLongName = "bottom-page-margin";
    internal const string TextPresenterLongName = "num-presenter";
    internal const string PageNumberShiftsLongName = "content-shift";
    internal const string IncreasePageModeLongName = "increase-page-mode";
    internal const string GeneralPageSizeAdjustmentLongName = "page-size-adjustment";
    internal const string FillColorLongName = "fill-color";
    internal const string FontPathLongName = "font-path";
    internal const string FontEncodingLongName = "font-encoding";
    internal const string FontSizeLongName = "font-size";
    internal const string FontOpacityLongName = "font-opacity";
    internal const string FontFamilyLongName = "font-family";
    internal const string FontColorLongName = "font-color";

    internal const string DefaultTextPresenter = nameof(PageNumberTextPresenter.Default);
    internal const string DefaultMargin = "0";
    internal const string DefaultFillColor = nameof(ColorConstants.WHITE);
    internal const string DefaultFontColor = nameof(ColorConstants.BLACK);

    [ValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "path to the input file",
        isRequired: true,
        valueRestriction: "file pdf\n?input path must point to a .pdf file")
    ]
    [OptionGroup("paths", "Paths", "Options for configuring paths")]
    public string InputPath { get; set; } = string.Empty;

    [ValueOption<string>(
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    public string OutputPath { get; set; } = string.Empty;

    [EnumValueOption<TabAlignment>(
        defaultValue: TabAlignment.CENTER,
        longName: HorizontalNumberAlignmentLongName,
        shortName: "H",
        description: "horizontal page number alignment",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page number appearance")]
    public TabAlignment HorizontalNumberAlignment { get; set; }

    [EnumValueOption<VerticalAlignment>(
        defaultValue: VerticalAlignment.BOTTOM,
        longName: VerticalNumberLongName,
        shortName: "V",
        description: "vertical page number alignment",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    public VerticalAlignment VerticalNumberAlignment { get; set; }

    [ValueOption<Margin>(
        longName: MarginLongName,
        shortName: "m",
        description: $"page number margin [default={DefaultMargin}] (M or H,V or L,T,R,B -> 5 or 3,5 or 3,5,3,5)")
    ]
    [OptionGroup("appearance", "", "")]
    public Margin Margin { get; set; } = Margin.Parse(DefaultMargin);

    [FlagOption(
        longName: ConsiderLeftPageMarginLongName,
        shortName: "L",
        description: "indicate whether the left page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    public bool ConsiderLeftPageMargin { get; set; }

    [FlagOption(
        longName: ConsiderTopPageMarginLongName,
        shortName: "T",
        description: "indicate whether the top page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    public bool ConsiderTopPageMargin { get; set; }

    [FlagOption(
        longName: ConsiderRightPageMarginLongName,
        shortName: "R",
        description: "indicate whether the right page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    public bool ConsiderRightPageMargin { get; set; }

    [FlagOption(
        longName: ConsiderBottomPageMarginLongName,
        shortName: "B",
        description: "indicate whether the bottom page margin is considered")
    ]
    [OptionGroup("appearance", "", "")]
    public bool ConsiderBottomPageMargin { get; set; }

    [ValueOption<PageNumberTextPresenter>(
        longName: TextPresenterLongName,
        shortName: "P",
        description: $"page number presenter [default={DefaultTextPresenter}]",
        addBeforeParseChoicesToDescription: true,
        ignoreCaseInChoices: true,
        beforeParseChoices:
        [
            nameof(PageNumberTextPresenter.Default),
            nameof(PageNumberTextPresenter.Fractional),
            nameof(PageNumberTextPresenter.Verbal)
        ])
    ]
    [OptionGroup("appearance", "", "")]
    public PageNumberTextPresenter TextPresenter { get; set; } = PageNumberTextPresenter.Parse(DefaultTextPresenter);

    [MultipleValueOption<PageContentShift>(
        longName: PageNumberShiftsLongName,
        shortName: "",
        description: "page number shifts [default=[]] (Pages:Horizontal,Vertical -> 1:-50,0 or 1,3-5:10,15)",
        contextCaptureType: ContextCaptureType.OneOrMore)
    ]
    [OptionGroup("appearance", "", "")]
    public List<PageContentShift> PageNumberShifts { get; set; } = [];

    [EnumValueOption<IncreasePageMode>(
        defaultValue: IncreasePageMode.WithoutIncrease,
        longName: IncreasePageModeLongName,
        shortName: "l",
        description: "increase page mode",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    public IncreasePageMode IncreasePageMode { get; set; }

    [ValueOption<float>(
        defaultValue: 40f,
        longName: GeneralPageSizeAdjustmentLongName,
        shortName: "",
        description: "page size adjustment for all pages",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 1 100000\n?general page size adjustment must be in [1; 100000]")
    ]
    [OptionGroup("appearance", "", "")]
    public float GeneralPageSizeAdjustment { get; set; }

    [ValueOption<Color>(
        longName: FillColorLongName,
        shortName: "",
        description:
            $"Color that will fill the area after increasing the page size [default={DefaultFontColor}] (name or (r,g,b)). " +
            $"Supported names: {ColorConverter.SupportedColorNames}")
    ]
    [OptionGroup("appearance", "", "")]
    public Color FillColor { get; set; } = ColorConverter.Parse(DefaultFillColor);

    [ValueOption<string>(
        defaultValue: "",
        longName: FontPathLongName,
        shortName: "",
        description: "page number font path [default=\"\"]",
        valueRestriction: "file ttf\n|| empty\n?font path must point to a .ttf file")
    ]
    [OptionGroup("font", "Font", "Options for configuring page number font")]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(AddPageNumbersSubcommand)}.{nameof(FontPath)}-{nameof(FontFamily)}",
        "Font Path",
        $"{nameof(FontPath)} cannot be used with the {nameof(FontFamily)}")
    ]
    public string FontPath { get; set; } = string.Empty;

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
        ignoreCaseInChoices: false,
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("font", "", "")]
    public string FontEncoding { get; set; } = string.Empty;

    [ValueOption<float>(
        defaultValue: 24f,
        longName: FontSizeLongName,
        shortName: "",
        description: "page number font size",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 1 512\n?font size must be in [1; 512]")
    ]
    [OptionGroup("font", "", "")]
    public float FontSize { get; set; }

    [ValueOption<float>(
        defaultValue: 1f,
        longName: FontOpacityLongName,
        shortName: "",
        description: "page number font opacity",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 0 1\n?font opacity must be in [0; 1]")
    ]
    [OptionGroup("font", "", "")]
    public float FontOpacity { get; set; }

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
        ignoreCaseInChoices: false,
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("font", "", "")]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(AddPageNumbersSubcommand)}.{nameof(FontPath)}-{nameof(FontFamily)}",
        "",
        "")
    ]
    public string FontFamily { get; set; } = string.Empty;

    [ValueOption<Color>(
        longName: FontColorLongName,
        shortName: "c",
        description:
            $"page number font color [default={DefaultFontColor}] (name or (r,g,b)). " +
            $"Supported names: {ColorConverter.SupportedColorNames}")
    ]
    [OptionGroup("font", "", "")]
    public Color FontColor { get; set; } = ColorConverter.Parse(DefaultFontColor);
}
