namespace ExecutionLib.Execution
{
    public class NoFileProcessingToolException : ApplicationException
    {
        public NoFileProcessingToolException(string? message = null, Exception? innerException = null)
            : base(message ?? "No file processing tool specified.", innerException)
        {
        }
    }
}
