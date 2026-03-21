using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class CheckCompressionCapabilitySubcommand
{
    internal const string Name = "can-compress";
    internal const string Description = "Check if PDF document can be compressed";

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
    public string InputPath { get; set; } = string.Empty;

    [ValueOption<string>(
        defaultValue: "",
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file [default=\"\"]")
    ]
    [OptionGroup("paths", "", "")]
    public string OutputPath { get; set; } = string.Empty;
}
