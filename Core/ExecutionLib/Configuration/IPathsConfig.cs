using YpdfLib.Models;

namespace ExecutionLib.Configuration
{
    public interface IPathsConfig : IDeepCloneable<IPathsConfig>
    {
        string? InputPath { get; set; }
        string? OutputPath { get; set; }
        string OutputDirectory { get; set; }
        string? FileSearchPattern { get; set; }

        IList<string> FilePaths { get; }
        IList<string> DirectoriesForGettingFiles { get; }

        string[] AllInputFiles { get; }

        void PreparePaths();
    }
}
