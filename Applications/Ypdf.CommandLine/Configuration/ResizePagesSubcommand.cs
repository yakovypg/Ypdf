using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration;

internal sealed class ResizePagesSubcommand
{
    internal const string Name = "resize";
    internal const string Description = "Resize PDF document pages";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string PageResizingsLongName = "resizing";

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
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    public string OutputPath { get; set; } = string.Empty;

    [MultipleValueOption<PageResizing>(
        longName: PageResizingsLongName,
        shortName: "r",
        description: "page resizings (Pages:W,H -> 1:1920,1080 or 1,3-5:500,500)",
        contextCaptureType: ContextCaptureType.OneOrMore,
        isRequired: true)
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page appearance")]
    public List<PageResizing> PageResizings { get; set; } = [];
}
