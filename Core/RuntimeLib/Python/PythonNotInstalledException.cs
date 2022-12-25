namespace RuntimeLib.Python
{
    public class PythonNotInstalledException : ApplicationException
    {
        private const string DEFAULT_MESSAGE = "Python not installed.";

        public string? CurrentVersion { get; }
        public string? RequiredVersion { get; }

        public PythonNotInstalledException(string? message = DEFAULT_MESSAGE, Exception? innerException = null)
            : this(string.Empty, string.Empty, message, innerException)
        {
        }

        public PythonNotInstalledException(string? currentVersion, string? requiredVersion, string? message = DEFAULT_MESSAGE,
            Exception? innerException = null) : base(message, innerException)
        {
            CurrentVersion = currentVersion;
            RequiredVersion = requiredVersion;
        }
    }
}
