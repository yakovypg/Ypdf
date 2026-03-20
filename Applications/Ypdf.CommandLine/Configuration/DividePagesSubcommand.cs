using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration;

internal sealed class DividePagesSubcommand
{
    internal const string Name = "divide";
    internal const string Description = "Divide PDF document pages";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string PageDivisionsLongName = "division";

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

    [MultipleValueOption<PageCropping>(
        longName: PageDivisionsLongName,
        shortName: "d",
        description: "page divisions (Pages:Orientation,CenterOffset -> 1:horizontal or 1,3-5:vertical,10)",
        contextCaptureType: ContextCaptureType.OneOrMore,
        isRequired: true)
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page appearance")]
    internal List<PageDivision> PageDivisions { get; set; } = [];
}
