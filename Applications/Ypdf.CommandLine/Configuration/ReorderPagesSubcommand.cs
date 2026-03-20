using NetArgumentParser.Attributes;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration;

internal sealed class ReorderPagesSubcommand
{
    internal const string Name = "reorder-pages";
    internal const string Description = "Reorder PDF document pages";

    internal const string InputPathLongName = "input-file";
    internal const string OutputPathLongName = "output-file";
    internal const string PageOrderLongName = "page-order";

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

    [ValueOption<PageOrder>(
        longName: PageOrderLongName,
        shortName: "p",
        description: "order of the pages (enumerable of N or S-E -> 5,3,2,1,4 or 5,3-1,4)",
        isRequired: true)
    ]
    internal PageOrder PageOrder { get; set; }
}
