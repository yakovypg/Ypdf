using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration;

internal sealed class RotatePagesSubcommand
{
    internal const string Name = "rotate";
    internal const string Description = "Rotate PDF document pages";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string PageRotationsLongName = "rotation";

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

    [MultipleValueOption<PageRotation>(
        longName: PageRotationsLongName,
        shortName: "r",
        description: "page rotations (Pages:Angle -> 1:-90 or 1,3-5:90)",
        contextCaptureType: ContextCaptureType.OneOrMore,
        isRequired: true)
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page appearance")]
    public List<PageRotation> PageRotations { get; set; } = [];
}
