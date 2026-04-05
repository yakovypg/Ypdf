using System;
using System.Collections.Generic;
using Ypdf.Core.Tools;

namespace Ypdf.Core.Execution;

public class ToolExecutionParameters : IToolExecutionParameters
{
    public ToolExecutionParameters(ITool tool, string inputPath, string outputPath)
        : this(
            tool ?? throw new ArgumentNullException(nameof(tool)),
            [inputPath ?? throw new ArgumentNullException(nameof(inputPath))],
            outputPath ?? throw new ArgumentNullException(nameof(outputPath))) { }

    public ToolExecutionParameters(ITool tool, IEnumerable<string> inputPaths, string outputPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(tool, nameof(tool));
        ExtendedArgumentException.ThrowIfNullOrEmpty(inputPaths, nameof(inputPaths));
        ExtendedArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));

        Tool = tool;
        InputPaths = [.. inputPaths];
        OutputPath = outputPath;
    }

    public ITool Tool { get; }
    public IReadOnlyCollection<string> InputPaths { get; }
    public string OutputPath { get; }
}
