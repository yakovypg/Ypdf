namespace FileSystemLib.Paths
{
    public interface IPathMulticorrection : IPathCorrection
    {
        string[] Corrections { get; }

        void Correct(string[] preferredExtensions);
        void Correct(string newPath);
        void Correct(int correctionIndex);
    }
}
