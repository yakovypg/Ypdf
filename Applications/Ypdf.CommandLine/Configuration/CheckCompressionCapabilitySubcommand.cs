using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class CheckCompressionCapabilitySubcommand
{
    internal const string Name = "compress";
    internal const string Description = "Compress PDF document";

    internal const string InputPathLongName = "input-file";
    internal const string OutputPathLongName = "output-file";

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
        defaultValue: "",
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file [default=\"\"]")
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;
}
