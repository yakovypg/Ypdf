using System.Collections.Generic;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.CommandLine.Converters;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;

namespace Ypdf.CommandLine.Configuration;

internal sealed class AddWatermarkAnnotationSubcommand
{
    internal const string Name = "add-watermark-annotation";
    internal const string Description = "Add watermark annotation to PDF document";

    internal const string InputPathLongName = "input-file";
    internal const string OutputPathLongName = "output-file";
    internal const string PagesLongName = "pages";
    internal const string TextLongName = "text";
    internal const string RotationAngleDegreesLongName = "angle";
    internal const string WidthLongName = "width";
    internal const string HeightLongName = "height";
    internal const string LowerLeftPointLongName = "position";
    internal const string XTranslationLongName = "x-translation";
    internal const string YTranslationLongName = "y-translation";
    internal const string FontPathLongName = "font-path";
    internal const string FontEncodingLongName = "font-encoding";
    internal const string FontSizeLongName = "font-size";
    internal const string FontOpacityLongName = "font-opacity";
    internal const string FontFamilyLongName = "font-family";
    internal const string FontColorLongName = "font-color";

    internal const string DefaultLowerLeftPoint = "(0;0)";
    internal const string DefaultFontColor = nameof(ColorConstants.BLACK);

    [ValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "path to the input file",
        isRequired: true,
        valueRestriction: "fileexists\n&& extension pdf\n?input path must point to a .pdf file")
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

    [MultipleValueOption<PageRange>(
        longName: PagesLongName,
        shortName: "p",
        description: "page number or page range (N or S-E -> 1 or 3-5)",
        contextCaptureType: ContextCaptureType.OneOrMore,
        isRequired: true,
        valueRestriction: "min 1\n?all pages must be >= 1")
    ]
    internal List<PageRange> Pages { get; set; } = [];

    [ValueOption<string>(
        longName: TextLongName,
        shortName: "t",
        description: "watermark annotation text",
        isRequired: true,
        valueRestriction: "!empty\n?text mustn't be empty")
    ]
    internal string Text { get; set; } = string.Empty;

    [ValueOption<int>(
        defaultValue: 0,
        longName: RotationAngleDegreesLongName,
        shortName: "a",
        description: "rotation angle in degrees",
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring watermark annotation appearance")]
    internal int RotationAngleDegrees { get; set; }

    [ValueOption<float>(
        defaultValue: 300f,
        longName: WidthLongName,
        shortName: "w",
        description: "watermark annotation object width",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 1 100000\n?with must be in [1; 100000]")
    ]
    internal float Width { get; set; }

    [ValueOption<float>(
        defaultValue: 450f,
        longName: HeightLongName,
        shortName: "h",
        description: "watermark annotation object height",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 1 100000\n?height must be in [1; 100000]")
    ]
    internal float Height { get; set; }

    [ValueOption<FloatPoint>(
        longName: LowerLeftPointLongName,
        shortName: "P",
        description: $"lower left point of the watermark annotation [default={DefaultLowerLeftPoint}] ((X;Y) -> (100;250))")
    ]
    internal FloatPoint LowerLeftPoint { get; set; } = FloatPoint.Parse(DefaultLowerLeftPoint);

    [ValueOption<float>(
        defaultValue: 50f,
        longName: XTranslationLongName,
        shortName: "",
        description: "shift of the origin along the X-axis for the watermark annotation",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange -100000 100000\n?X-translation must be in [-100000; 100000]")
    ]
    internal float XTranslation { get; set; }

    [ValueOption<float>(
        defaultValue: 25f,
        longName: YTranslationLongName,
        shortName: "",
        description: "shift of the origin along the Y-axis for the watermark annotation",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange -100000 100000\n?Y-translation must be in [-100000; 100000]")
    ]
    internal float YTranslation { get; set; }

    [ValueOption<string>(
        defaultValue: "",
        longName: FontPathLongName,
        shortName: "",
        description: "font path [default=\"\"]",
        valueRestriction: "fileexists\n&& extension ttf\n|| empty\n?font path must point to a .ttf file")
    ]
    [OptionGroup("font", "Font", "Options for configuring watermark annotation font")]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(AddWatermarkAnnotationSubcommand)}.FontConfig",
        "font config",
        $"{nameof(FontPath)} cannot be used with the {nameof(FontFamily)}")
    ]
    internal string FontPath { get; set; } = string.Empty;

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
    internal string FontEncoding { get; set; } = string.Empty;

    [ValueOption<float>(
        defaultValue: 24f,
        longName: FontSizeLongName,
        shortName: "",
        description: "font size",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 1 512\n?font size must be in [1; 512]")
    ]
    [OptionGroup("font", "", "")]
    internal float FontSize { get; set; }

    [ValueOption<float>(
        defaultValue: 1f,
        longName: FontOpacityLongName,
        shortName: "",
        description: "font opacity",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 0 1\n?font opacity must be in [0; 1]")
    ]
    [OptionGroup("font", "", "")]
    internal float FontOpacity { get; set; }

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
    [MutuallyExclusiveOptionGroup($"{nameof(AddWatermarkAnnotationSubcommand)}.FontConfig", "", "")]
    internal string FontFamily { get; set; } = string.Empty;

    [ValueOption<Color>(
        longName: FontColorLongName,
        shortName: "c",
        description: $"font color [default={DefaultFontColor}] (name or (r,g,b))",
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
