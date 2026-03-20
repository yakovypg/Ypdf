using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration;

internal sealed class RemovePagesSubcommand
{
    internal const string Name = "remove-pages";
    internal const string Description = "Remove pages from PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string PagesLongName = "pages";

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

    [MultipleValueOption<PageRange>(
        longName: PagesLongName,
        shortName: "p",
        description: "page number or page range (N or S-E -> 1 or 3-5)",
        contextCaptureType: ContextCaptureType.OneOrMore,
        isRequired: true)
    ]
    [OptionGroup("paging", "Paging", "Options for configuring paging")]
    internal List<PageRange> Pages { get; set; } = [];
}
