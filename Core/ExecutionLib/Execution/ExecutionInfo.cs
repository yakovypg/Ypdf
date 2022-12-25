using ExecutionLib.Configuration;

namespace ExecutionLib.Execution
{
    public class ExecutionInfo : IEquatable<ExecutionInfo?>, IExecutionInfo
    {
        public string? PdfTool { get; }
        public Action<YpdfConfig>? Executor { get; }

        public bool CanExecute { get; }
        public Exception? Exception { get; }

        public ExecutionInfo(Exception exception)
        {
            CanExecute = false;
            Exception = exception;
        }

        public ExecutionInfo(string pdfTool, Action<YpdfConfig> executor)
        {
            PdfTool = pdfTool;
            Executor = executor;
            CanExecute = true;
        }

        public bool Equals(ExecutionInfo? other)
        {
            return other is not null &&
                   PdfTool == other.PdfTool &&
                   EqualityComparer<Action<YpdfConfig>?>.Default.Equals(Executor, other.Executor) &&
                   CanExecute == other.CanExecute &&
                   EqualityComparer<Exception?>.Default.Equals(Exception, other.Exception);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ExecutionInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PdfTool, Executor, CanExecute, Exception);
        }

        public static bool operator ==(ExecutionInfo? left, ExecutionInfo? right)
        {
            return EqualityComparer<ExecutionInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(ExecutionInfo? left, ExecutionInfo? right)
        {
            return !(left == right);
        }
    }
}
