namespace FileSystemLib.Paths
{
    public class UniqueDirectory : IUniqueDirectory
    {
        public string WorkingDirectory { get; }

        public UniqueDirectory(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;
        }

        public DirectoryInfo Create()
        {
            IUniqueName uniqueNameGenerator = new UniqueDirectoryName(WorkingDirectory);

            string uniqueName = uniqueNameGenerator.GetNext();
            string path = Path.Combine(WorkingDirectory, uniqueName);

            return Directory.CreateDirectory(path);
        }
    }
}
