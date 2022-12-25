namespace FileSystemLib.Paths
{
    public interface IPathCorrection
    {
        string OldPath { get; }
        string? NewPath { get; }

        bool IsPathCorrected { get; }
        bool IsRemainedUnchanged { get; }
    }
}