using System.Collections.Generic;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Execution;

internal interface IToolExecutionProvider
{
    ITool Tool { get; }
    IReadOnlyCollection<string> InputPaths { get; }
    string OutputPath { get; }

    void ExecuteTool();
}
