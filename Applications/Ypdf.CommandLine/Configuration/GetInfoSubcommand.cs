using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class GetInfoSubcommand
{
    internal const string Name = "info";
    internal const string Description = "Get info about PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string MaxPageSizesToPrintLongName = "limit-page-sizes";

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
        description: "path to the output file")
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;

    [ValueOption<int>(
        defaultValue: 0,
        longName: MaxPageSizesToPrintLongName,
        shortName: "l",
        description: "maximum number of page sizes that will be printed. Zero indicates no limitation",
        addDefaultValueToDescription: true,
        valueRestriction: "min 0\n?maximum number of page sizes must be non-negative")
    ]
    [OptionGroup("limitations", "Limitations", "Options to configure print limitations")]
    internal int MaxPageSizesToPrint { get; set; }
}
