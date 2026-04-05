using System.Collections.Generic;
using System.Linq;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Execution;

internal sealed class ToolExecutionProvider : IToolExecutionProvider
{
    internal ToolExecutionProvider(
        ITool tool,
        IReadOnlyCollection<string> inputPaths,
        string outputPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(tool, nameof(tool));
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        ExtendedArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));

        DefaultExceptions.ThrowIfZero(inputPaths.Count, nameof(inputPaths.Count));

        Tool = tool;
        InputPaths = inputPaths;
        OutputPath = outputPath;
    }

    public ITool Tool { get; }
    public IReadOnlyCollection<string> InputPaths { get; }
    public string OutputPath { get; }

    public void ExecuteTool()
    {
        if (Tool is IMultipleInputTool multipleInputTool)
            multipleInputTool.Execute(InputPaths, OutputPath);
        else
            Tool.Execute(InputPaths.First(), OutputPath);
    }
}
