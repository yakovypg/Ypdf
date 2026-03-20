using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;

namespace Ypdf.CommandLine.Configuration;

internal sealed class MergeSubcommand
{
    internal const string Name = "merge";
    internal const string Description = "Merge PDF documents";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";

    [MultipleValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "paths to the input files",
        isRequired: true,
        contextCaptureType: ContextCaptureType.OneOrMore,
        valueRestriction: "file pdf\n?input path must point to a .pdf file")
    ]
    [OptionGroup("paths", "Paths", "Options for configuring paths")]
    internal List<string> InputPaths { get; set; } = [];

    [ValueOption<string>(
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;
}
