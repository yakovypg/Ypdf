using FileSystemLib.Paths;
using YpdfLib.Models;

namespace ExecutionLib.Configuration
{
    public class PathsConfig : IPathsConfig, IDeepCloneable<PathsConfig>, IEquatable<PathsConfig?>
    {
        public string? InputPath { get; set; }
        public string? OutputPath { get; set; }
        public string OutputDirectory { get; set; }
        public string? FileSearchPattern { get; set; }

        public IList<string> FilePaths { get; private set; }
        public IList<string> DirectoriesForGettingFiles { get; private set; }

        public string[] AllInputFiles => InputPath is null
            ? FilePaths.ToArray()
            : (new string[] { InputPath }).Concat(FilePaths).ToArray();

        public PathsConfig(string? inputPath = null, string? outputPath = null,
            string outputDirectory = "", string? fileSearchPattern = null)
        {
            InputPath = inputPath;
            OutputPath = outputPath;
            OutputDirectory = outputDirectory;
            FileSearchPattern = fileSearchPattern;

            FilePaths = new List<string>();
            DirectoriesForGettingFiles = new List<string>();
        }

        public void PreparePaths()
        {
            PrepareDirectories();
            PrepareFiles();
        }

        public PathsConfig Copy()
        {
            return new PathsConfig(InputPath, OutputPath, OutputDirectory)
            {
                FileSearchPattern = FileSearchPattern,
                FilePaths = new List<string>(FilePaths),
                DirectoriesForGettingFiles = new List<string>(DirectoriesForGettingFiles)
            };
        }

        IPathsConfig IDeepCloneable<IPathsConfig>.Copy()
        {
            return Copy();
        }

        public bool Equals(PathsConfig? other)
        {
            return other is not null &&
                   InputPath == other.InputPath &&
                   OutputPath == other.OutputPath &&
                   OutputDirectory == other.OutputDirectory &&
                   FileSearchPattern == other.FileSearchPattern &&
                   EqualityComparer<IList<string>>.Default.Equals(FilePaths, other.FilePaths) &&
                   EqualityComparer<IList<string>>.Default.Equals(DirectoriesForGettingFiles, other.DirectoriesForGettingFiles);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PathsConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InputPath, OutputPath, OutputDirectory, FileSearchPattern,
                FilePaths, DirectoriesForGettingFiles);
        }

        private void PrepareFiles()
        {
            GetFilesFromSpecifiedDirectories();
        }

        private void PrepareDirectories()
        {
            if (OutputDirectory is not null)
                DirectoryPreparer.PrepareOutputDirectories(OutputDirectory);

            if (OutputPath is not null)
                DirectoryPreparer.PrepareFileDirectories(OutputPath);
        }

        private void GetFilesFromSpecifiedDirectories()
        {
            foreach (string dir in DirectoriesForGettingFiles)
            {
                string[] files = FileSearchPattern is not null
                    ? Directory.GetFiles(dir, FileSearchPattern)
                    : Directory.GetFiles(dir);

                foreach (var file in files)
                    FilePaths.Add(file);
            }
        }
    }
}
