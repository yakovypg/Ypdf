namespace FileSystemLib.Naming
{
    public class UniqueFile : UniqueFileName, IUniqueFile, IEquatable<UniqueFile?>
    {
        public UniqueFile(string extension, string workingDirectory) : base(extension, workingDirectory)
        {
        }

        public override string GetNext()
        {
            string uniqueFileName = base.GetNext();
            return Path.Combine(WorkingDirectory, uniqueFileName);
        }

        public FileInfo Create()
        {
            string path = GetNext();
            File.Create(path).Close();

            return new FileInfo(path);
        }

        public bool Equals(UniqueFile? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Postfix == other.Postfix &&
                   Extension == other.Extension &&
                   WorkingDirectory == other.WorkingDirectory;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UniqueFile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Postfix, Extension, WorkingDirectory);
        }

        public static bool operator ==(UniqueFile? left, UniqueFile? right)
        {
            return EqualityComparer<UniqueFile>.Default.Equals(left, right);
        }

        public static bool operator !=(UniqueFile? left, UniqueFile? right)
        {
            return !(left == right);
        }
    }
}
