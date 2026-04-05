using System.Collections.Generic;
using Ypdf.Core.Tools;

namespace Ypdf.Core.Execution;

public interface IToolExecutionParameters
{
    ITool Tool { get; }
    IReadOnlyCollection<string> InputPaths { get; }
    string OutputPath { get; }
}
