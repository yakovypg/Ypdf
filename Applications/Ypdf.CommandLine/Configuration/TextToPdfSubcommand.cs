using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using NetArgumentParser.Attributes;
using Ypdf.CommandLine.Converters;
using Ypdf.Core.Design;

namespace Ypdf.CommandLine.Configuration;

internal sealed class TextToPdfSubcommand
{
    internal const string Name = "text-to-pdf";
    internal const string Description = "Convert text to the PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string MarginLongName = "margin";
    internal const string PageSizeLongName = "size";
    internal const string TextAlignmentLongName = "text-alignment";
    internal const string FontPathLongName = "font-path";
    internal const string FontEncodingLongName = "font-encoding";
    internal const string FontSizeLongName = "font-size";
    internal const string FontOpacityLongName = "font-opacity";
    internal const string FontFamilyLongName = "font-family";
    internal const string FontColorLongName = "font-color";

    internal const string DefaultMargin = "76";
    internal const string DefaultPageSize = nameof(PageSize.LETTER);
    internal const string DefaultFontColor = nameof(ColorConstants.BLACK);

    [ValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "path to the input file",
        isRequired: true,
        valueRestriction: "fileexists\n?input path must point to an existing file")
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

    [ValueOption<Margin>(
        longName: MarginLongName,
        shortName: "m",
        description: $"text margin [default={DefaultMargin}] (M or H,V or L,T,R,B -> 5 or 3,5 or 3,5,3,5)")
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page appearance")]
    public Margin Margin { get; set; } = Margin.Parse(DefaultMargin);

    [ValueOption<PageSize>(
        longName: PageSizeLongName,
        shortName: "s",
        description: $"page size [default={DefaultPageSize}] (name or (width,height))")
    ]
    [OptionGroup("appearance", "", "")]
    public PageSize PageSize { get; set; } = PageSizeConverter.Parse(DefaultPageSize);

    [EnumValueOption<TextAlignment>(
        defaultValue: TextAlignment.LEFT,
        longName: TextAlignmentLongName,
        shortName: "a",
        description: "alignment of the text",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    public TextAlignment TextAlignment { get; set; }

    [ValueOption<string>(
        defaultValue: "",
        longName: FontPathLongName,
        shortName: "",
        description: "font path [default=\"\"]",
        valueRestriction: "file ttf\n|| empty\n?font path must point to a .ttf file")
    ]
    [OptionGroup("font", "Font", "Options for configuring watermark annotation font")]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(AddWatermarkAnnotationSubcommand)}.{nameof(FontPath)}-{nameof(FontFamily)}",
        "Font Path",
        $"{nameof(FontPath)} cannot be used with the {nameof(FontFamily)}")
    ]
    public string FontPath { get; set; } = string.Empty;

    [ValueOption<string>(
        defaultValue: PdfEncodings.IDENTITY_H,
        longName: FontEncodingLongName,
        shortName: "",
        description: "font encoding",
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
    public string FontEncoding { get; set; } = string.Empty;

    [ValueOption<float>(
        defaultValue: 24f,
        longName: FontSizeLongName,
        shortName: "",
        description: "font size",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 1 512\n?font size must be in [1; 512]")
    ]
    [OptionGroup("font", "", "")]
    public float FontSize { get; set; }

    [ValueOption<float>(
        defaultValue: 1f,
        longName: FontOpacityLongName,
        shortName: "",
        description: "font opacity",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 0 1\n?font opacity must be in [0; 1]")
    ]
    [OptionGroup("font", "", "")]
    public float FontOpacity { get; set; }

    [ValueOption<string>(
        defaultValue: StandardFonts.TIMES_ROMAN,
        longName: FontFamilyLongName,
        shortName: "",
        description: "font family",
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
    [MutuallyExclusiveOptionGroup(
        $"{nameof(AddWatermarkAnnotationSubcommand)}.{nameof(FontPath)}-{nameof(FontFamily)}",
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
