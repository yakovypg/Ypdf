using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.Core.Design.Pages;

namespace Ypdf.CommandLine.Configuration;

internal sealed class CropPagesSubcommand
{
    internal const string Name = "crop";
    internal const string Description = "Crop PDF document pages";

    internal const string InputPathLongName = "input-file";
    internal const string OutputPathLongName = "output-file";
    internal const string PageCroppingsLongName = "cropping";

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
        longName: PageCroppingsLongName,
        shortName: "c",
        description: "page croppings (Pages:LowerLeftPoint,UpperRightPoint -> 1:(5,5),(40,60) or 1,3-5:(5,5),(40,60))",
        contextCaptureType: ContextCaptureType.OneOrMore,
        isRequired: true)
    ]
    [OptionGroup("appearance", "Appearance", "Options for configuring page appearance")]
    internal List<PageCropping> PageCroppings { get; set; } = [];
}
