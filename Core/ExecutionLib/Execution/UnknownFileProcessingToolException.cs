namespace ExecutionLib.Execution
{
    public class UnknownFileProcessingToolException : ApplicationException
    {
        public UnknownFileProcessingToolException(string fileProcessingTool, string? message = null, Exception? innerException = null)
            : base(message ?? $"File processing tool {fileProcessingTool} is unknown.", innerException)
        {
        }
    }
}
