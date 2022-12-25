namespace FileSystemLib.Paths
{
    public class IncorrectPathException : ApplicationException
    {
        public string Path { get; }

        public IncorrectPathException(string path, string? message = null, Exception? innerException = null)
            : base(message ?? $"Path '{path}' is incorrect.", innerException)
        {
            Path = path;
        }
    }
}
