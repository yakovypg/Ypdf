using System.Collections.Generic;
using NetArgumentParser.Attributes;
using NetArgumentParser.Options.Context;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;

namespace Ypdf.CommandLine.Configuration;

internal sealed class SplitSubcommand
{
    internal const string Name = "split";
    internal const string Description = "Split PDF document";

    internal const string InputPathLongName = "input-file";
    internal const string OutputPathLongName = "output-directory";
    internal const string SplitPartsLongName = "parts";
    internal const string SplitPartSizeExpressionLongName = "part-size";

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
        description: "path to the output directory",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;

    [MultipleValueOption<PageRange>(
        longName: SplitPartsLongName,
        shortName: "p",
        description: "split parts represented by page number or page range (N or S-E -> 1 or 3-5)",
        contextCaptureType: ContextCaptureType.OneOrMore,
        valueRestriction: "min 1\n?all pages must be >= 1")
    ]
    [OptionGroup("paging", "Paging", "Options for configuring paging")]
    internal List<PageRange> SplitParts { get; set; } = [];

    [ValueOption<MathExpression>(
        longName: SplitPartSizeExpressionLongName,
        shortName: "s",
        description: "expression that describes the size of each split part in bytes. " +
            "It can be either a number or a an expression (1024 or (1+3)*1024)")
    ]
    [OptionGroup("paths", "", "")]
    internal MathExpression? SplitPartSizeExpression { get; set; }
}
