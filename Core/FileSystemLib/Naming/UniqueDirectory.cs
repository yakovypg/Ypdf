namespace FileSystemLib.Naming
{
    public class UniqueDirectory : UniqueDirectoryName, IUniqueDirectory, IEquatable<UniqueDirectory?>
    {
        public UniqueDirectory(string workingDirectory) : base(workingDirectory)
        {
        }

        public override string GetNext()
        {
            string uniqueDirName = base.GetNext();
            return Path.Combine(WorkingDirectory, uniqueDirName);
        }

        public DirectoryInfo Create()
        {
            string path = GetNext();
            return Directory.CreateDirectory(path);
        }

        public bool Equals(UniqueDirectory? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Postfix == other.Postfix &&
                   WorkingDirectory == other.WorkingDirectory;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UniqueDirectory);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Postfix, WorkingDirectory);
        }

        public static bool operator ==(UniqueDirectory? left, UniqueDirectory? right)
        {
            return EqualityComparer<UniqueDirectory>.Default.Equals(left, right);
        }

        public static bool operator !=(UniqueDirectory? left, UniqueDirectory? right)
        {
            return !(left == right);
        }
    }
}
