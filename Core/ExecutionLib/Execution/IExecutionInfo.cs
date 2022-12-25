using ExecutionLib.Configuration;

namespace ExecutionLib.Execution
{
    public interface IExecutionInfo
    {
        string? PdfTool { get; }
        Action<YpdfConfig>? Executor { get; }

        bool CanExecute { get; }
        Exception? Exception { get; }
    }
}