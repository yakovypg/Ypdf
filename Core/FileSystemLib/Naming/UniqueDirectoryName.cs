namespace FileSystemLib.Naming
{
    public class UniqueDirectoryName : UniqueName, IEquatable<UniqueDirectoryName?>
    {
        public string WorkingDirectory { get; }

        public UniqueDirectoryName(string workingDirectory)
            : base(t => Directory.Exists(Path.Combine(workingDirectory, t)))
        {
            WorkingDirectory = workingDirectory;
        }

        public bool Equals(UniqueDirectoryName? other)
        {
            return other is not null &&
                   Postfix == other.Postfix &&
                   WorkingDirectory == other.WorkingDirectory;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UniqueDirectoryName);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Postfix, WorkingDirectory);
        }

        public static bool operator ==(UniqueDirectoryName? left, UniqueDirectoryName? right)
        {
            return EqualityComparer<UniqueDirectoryName>.Default.Equals(left, right);
        }

        public static bool operator !=(UniqueDirectoryName? left, UniqueDirectoryName? right)
        {
            return !(left == right);
        }
    }
}
