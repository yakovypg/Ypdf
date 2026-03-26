using System.Collections.Generic;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.Core.Design;

namespace Ypdf.CommandLine.Configuration;

internal sealed class ImagesToPdfSubcommand
{
    internal const string Name = "image-to-pdf";
    internal const string Description = "Convert images to the PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string MarginLongName = "margin";
    internal const string PageSizeLongName = "page-size";
    internal const string PageRotationAngleDegreesLongName = "angle";
    internal const string DisableAutoIncreasePageSizeLongName = "disable-autoincrease-size";
    internal const string ImageHorizontalAlignmentLongName = "image-h-alignment";

    [MultipleValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "paths to the input files",
        isRequired: true,
        contextCaptureType: ContextCaptureType.OneOrMore,
        valueRestriction: "file jpg jpeg png bmp gif tiff" +
            "\n?all input paths must point to a .jpg|.jpeg|.png|.bmp|.gif|.tiff file")
    ]
    [OptionGroup("paths", "Paths", "Options for configuring paths")]
    public List<string> InputPaths { get; set; } = [];

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
        description: "image margin (M or H,V or L,T,R,B -> 5 or 3,5 or 3,5,3,5)")
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page appearance")]
    public Margin? Margin { get; set; }

    [ValueOption<PageSize>(
        longName: PageSizeLongName,
        shortName: "s",
        description: "page size (name or (width,height))")
    ]
    [OptionGroup("appearance", "", "")]
    public PageSize? PageSize { get; set; }

    [ValueOption<int>(
        defaultValue: 0,
        longName: PageRotationAngleDegreesLongName,
        shortName: "a",
        description: "page rotation angle in degrees",
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    public int PageRotationAngleDegrees { get; set; }

    [FlagOption(
        longName: DisableAutoIncreasePageSizeLongName,
        shortName: "",
        description: "disable automatic increase of the page size")
    ]
    [OptionGroup("appearance", "", "")]
    public bool DisableAutoIncreasePageSize { get; set; }

    [EnumValueOption<HorizontalAlignment>(
        defaultValue: HorizontalAlignment.CENTER,
        longName: ImageHorizontalAlignmentLongName,
        shortName: "",
        description: "horizontal alignment of the image",
        addChoicesToDescription: true,
        addDefaultValueToDescription: true)
    ]
    [OptionGroup("appearance", "", "")]
    public HorizontalAlignment ImageHorizontalAlignment { get; set; }
}
