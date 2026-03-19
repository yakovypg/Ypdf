using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class CompressSubcommand
{
    internal const string Name = "compress";
    internal const string Description = "Compress PDF document";

    internal const string InputPathLongName = "input-file";
    internal const string OutputPathLongName = "output-file";
    internal const string QualityFactorLongName = "quality";
    internal const string SizeFactorLongName = "size";
    internal const string ExtensionLongName = "extension";
    internal const string DisableCompressionCapabilityCheckLongName = "disable-compression-validity-check";

    [ValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "path to the input file",
        isRequired: true,
        valueRestriction: "file pdf\n?input path must point to a .pdf file")
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

    [ValueOption<float>(
        defaultValue: 0.75f,
        longName: QualityFactorLongName,
        shortName: "q",
        description: "extracted image quality factor",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 0 1\n?extracted image quality factor must be in [0; 1]")
    ]
    [OptionGroup("compression", "Compression", "Options for configuring compression")]
    internal float QualityFactor { get; set; }

    [ValueOption<float>(
        defaultValue: 1.0f,
        longName: SizeFactorLongName,
        shortName: "s",
        description: "extracted image size factor",
        addDefaultValueToDescription: true,
        valueRestriction: "inrange 0 1\n?extracted image size factor must be in [0; 1]")
    ]
    [OptionGroup("compression", "", "")]
    internal float SizeFactor { get; set; }

    [ValueOption<string>(
        defaultValue: "jpg",
        longName: ExtensionLongName,
        shortName: "e",
        description: "extracted image extension",
        choices: ["jpg", "jpeg", "png", "bmp", "gif", "tiff"],
        addDefaultValueToDescription: true,
        addChoicesToDescription: true)
    ]
    [OptionGroup("compression", "", "")]
    internal string Extension { get; set; } = string.Empty;

    [FlagOption(
        longName: DisableCompressionCapabilityCheckLongName,
        shortName: "",
        description: "disable compression capability check")
    ]
    [OptionGroup("compression", "", "")]
    internal bool DisableCompressionCapabilityCheck { get; set; }
}
