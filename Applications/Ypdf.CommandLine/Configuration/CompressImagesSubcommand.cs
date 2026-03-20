using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;

namespace Ypdf.CommandLine.Configuration;

internal sealed class CompressImagesSubcommand
{
    internal const string Name = "compress-images";
    internal const string Description = "Compress images";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string QualityFactorLongName = "quality";
    internal const string SizeFactorLongName = "size";
    internal const string WidthLongName = "width";
    internal const string HeightLongName = "height";
    internal const string ExtensionLongName = "extension";

    [MultipleValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "paths to the input files",
        isRequired: true,
        contextCaptureType: ContextCaptureType.OneOrMore,
        valueRestriction: "file jpg jpeg png bmp gif tiff" +
            "\n?input paths must point to a .jpg|.jpeg|.png|.bmp|.gif|.tiff file")
    ]
    [OptionGroup("paths", "Paths", "Options for configuring paths")]
    internal List<string> InputPaths { get; set; } = [];

    [ValueOption<string>(
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file or directory",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;

    [ValueOption<float>(
        defaultValue: 0.75f,
        longName: QualityFactorLongName,
        shortName: "q",
        description: "image quality factor",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 0 1\n?image quality factor must be in [0; 1]")
    ]
    [OptionGroup("compression", "Compression", "Options for configuring compression")]
    internal float QualityFactor { get; set; }

    [ValueOption<float>(
        defaultValue: 1.0f,
        longName: SizeFactorLongName,
        shortName: "s",
        description: "image size factor",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 0 1\n?image size factor must be in [0; 1]")
    ]
    [OptionGroup("compression", "", "")]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(CompressImagesSubcommand)}.{nameof(SizeFactor)}-{nameof(Width)}",
        $"{nameof(SizeFactor)}-{nameof(Width)}",
        $"{nameof(SizeFactor)} cannot be used with the {nameof(Width)}")
    ]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(CompressImagesSubcommand)}.{nameof(SizeFactor)}-{nameof(Height)}",
        $"{nameof(SizeFactor)}-{nameof(Height)}",
        $"{nameof(SizeFactor)} cannot be used with the {nameof(Height)}")
    ]
    internal float SizeFactor { get; set; }

    [ValueOption<int>(
        longName: WidthLongName,
        shortName: "w",
        description: "output image width",
        valueRestriction: "inrange 1 100000\n?output image with must be in [1; 100000]")
    ]
    [OptionGroup("compression", "", "")]
    [MutuallyExclusiveOptionGroup($"{nameof(CompressImagesSubcommand)}.{nameof(SizeFactor)}-{nameof(Width)}", "", "")]
    internal int? Width { get; set; }

    [ValueOption<int>(
        longName: HeightLongName,
        shortName: "h",
        description: "output image height",
        valueRestriction: "inrange 1 100000\n?output image height must be in [1; 100000]")
    ]
    [OptionGroup("compression", "", "")]
    [MutuallyExclusiveOptionGroup($"{nameof(CompressImagesSubcommand)}.{nameof(SizeFactor)}-{nameof(Height)}", "", "")]
    internal int? Height { get; set; }

    [ValueOption<string>(
        defaultValue: "jpg",
        longName: ExtensionLongName,
        shortName: "e",
        description: "output image extension for the case where multiple input files are specified",
        choices: ["jpg", "jpeg", "png", "bmp", "gif", "tiff"],
        addDefaultValueToDescription: true,
        addChoicesToDescription: true)
    ]
    [OptionGroup("compression", "", "")]
    internal string Extension { get; set; } = string.Empty;
}
