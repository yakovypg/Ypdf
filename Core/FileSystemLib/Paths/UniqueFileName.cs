namespace FileSystemLib.Paths
{
    public class UniqueFileName : UniqueName
    {
        public string Extension { get; }
        public string WorkingDirectory { get; }

        public UniqueFileName(string extension, string workingDirectory) 
            : base(t => File.Exists(Path.Combine(workingDirectory, t)), $".{extension}")
        {
            Extension = extension;
            WorkingDirectory = workingDirectory;
        }
    }
}
