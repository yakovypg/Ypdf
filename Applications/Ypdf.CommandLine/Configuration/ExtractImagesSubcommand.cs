using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class ExtractImagesSubcommand
{
    internal const string Name = "extract-images";
    internal const string Description = "Extract images from PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string ExtractedImagesLimitLongName = "limit";

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
        description: "path to the output directory",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;

    [ValueOption<int>(
        defaultValue: 0,
        longName: ExtractedImagesLimitLongName,
        shortName: "l",
        description: "maximum number of images that will be extracted. Zero indicates no limitation",
        addDefaultValueToDescription: true,
        valueRestriction: "min 0\n?extracted images limit must be non-negative")
    ]
    [OptionGroup("paths", "", "")]
    internal int ExtractedImagesLimit { get; set; }
}
