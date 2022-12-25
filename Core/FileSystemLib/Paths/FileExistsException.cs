namespace FileSystemLib.Paths
{
    public class FileExistsException : ApplicationException
    {
        public string Path { get; }

        public FileExistsException(string path, string? message = null, Exception? innerException = null)
            : base(message ?? $"File {path} already exists.", innerException)
        {
            Path = path;
        }
    }
}
