using System.Linq;
using Ypdf.Core.Tools;

namespace Ypdf.Core.Execution;

public class ToolExecutionProvider : IToolExecutionProvider
{
    public ToolExecutionProvider(IToolExecutionParameters executionParameters)
    {
        ExtendedArgumentNullException.ThrowIfNull(executionParameters, nameof(executionParameters));
        ExecutionParameters = executionParameters;
    }

    public IToolExecutionParameters ExecutionParameters { get; }

    public void ExecuteTool()
    {
        if (ExecutionParameters.Tool is IMultipleInputTool multipleInputTool &&
            !ExecutionParameters.ForceUseAsSigleInputTool)
        {
            multipleInputTool.Execute(
                ExecutionParameters.InputPaths,
                ExecutionParameters.OutputPath);
        }
        else
        {
            ExecutionParameters.Tool.Execute(
                ExecutionParameters.InputPaths.FirstOrDefault() ?? string.Empty,
                ExecutionParameters.OutputPath);
        }
    }
}
