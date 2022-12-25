namespace ExecutionLib.Execution
{
    public class UndefinedParameterException : ApplicationException
    {
        public string ParameterName { get; }

        public UndefinedParameterException(string parameterName, string? message = null, Exception? innerException = null)
            : base(message ?? $"Parameter '{parameterName}' is undefined.", innerException)
        {
            ParameterName = parameterName;
        }
    }
}
