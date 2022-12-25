namespace FileSystemLib.Paths
{
    public class PathCorrection : IPathCorrection, IEquatable<PathCorrection?>
    {
        public string OldPath { get; }
        public string? NewPath { get; }

        public bool IsPathCorrected => NewPath is not null;
        public bool IsRemainedUnchanged => OldPath == NewPath;

        public PathCorrection(string oldPath, string? newPath)
        {
            OldPath = oldPath;
            NewPath = newPath;
        }

        public bool Equals(PathCorrection? other)
        {
            return other is not null &&
                   OldPath == other.OldPath &&
                   NewPath == other.NewPath;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PathCorrection);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OldPath, NewPath);
        }

        public static bool operator ==(PathCorrection? left, PathCorrection? right)
        {
            return EqualityComparer<PathCorrection>.Default.Equals(left, right);
        }

        public static bool operator !=(PathCorrection? left, PathCorrection? right)
        {
            return !(left == right);
        }
    }
}
