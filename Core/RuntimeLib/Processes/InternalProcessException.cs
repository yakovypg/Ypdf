namespace RuntimeLib.Processes
{
    public class InternalProcessException : ApplicationException
    {
        public int? ExitCode { get; }

        public InternalProcessException(string? message = null, Exception? innerException = null)
            : this(null, message, innerException)
        {
        }

        public InternalProcessException(int? exitCode, string? message = null, Exception? innerException = null)
            : base(message ?? CreateMessage(exitCode), innerException)
        {
            ExitCode = exitCode;
        }

        private static string CreateMessage(int? exitCode = null)
        {
            return exitCode is null
                ? "Process ended with error."
                : $"Process ended with error (exit code: {exitCode}).";
        }
    }
}