using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class MovePageSubcommand
{
    internal const string Name = "move-page";
    internal const string Description = "Move PDF document page";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string SourcePageNumberLongName = "from";
    internal const string DestinationPageNumberLongName = "to";

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

    [ValueOption<int>(
        longName: SourcePageNumberLongName,
        shortName: "f",
        description: "source page number",
        isRequired: true,
        valueRestriction: "min 1\n?source page number must be >= 1")
    ]
    internal int SourcePageNumber { get; set; }

    [ValueOption<int>(
        longName: DestinationPageNumberLongName,
        shortName: "t",
        description: "destination page number",
        isRequired: true,
        valueRestriction: "min 1\n?destination page number must be >= 1")
    ]
    internal int DestinationPageNumber { get; set; }
}
