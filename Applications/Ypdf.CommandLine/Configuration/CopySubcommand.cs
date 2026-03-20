using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class CopySubcommand
{
    internal const string Name = "copy";
    internal const string Description = "Copy PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";

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
}
