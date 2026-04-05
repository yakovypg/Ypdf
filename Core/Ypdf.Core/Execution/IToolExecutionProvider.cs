namespace Ypdf.Core.Execution;

public interface IToolExecutionProvider
{
    IToolExecutionParameters ExecutionParameters { get; }
    void ExecuteTool();
}
