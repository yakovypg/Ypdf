namespace FileSystemLib.Paths
{
    public class UniqueDirectoryName : UniqueName
    {
        public string WorkingDirectory { get; }

        public UniqueDirectoryName(string workingDirectory)
            : base(t => Directory.Exists(Path.Combine(workingDirectory, t)))
        {
            WorkingDirectory = workingDirectory;
        }
    }
}
