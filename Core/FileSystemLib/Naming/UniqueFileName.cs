namespace FileSystemLib.Naming
{
    public class UniqueFileName : UniqueName, IEquatable<UniqueFileName?>
    {
        public string Extension { get; }
        public string WorkingDirectory { get; }

        public UniqueFileName(string extension, string workingDirectory)
            : base(t => File.Exists(Path.Combine(workingDirectory, t)), $".{extension}")
        {
            Extension = extension;
            WorkingDirectory = workingDirectory;
        }

        public bool Equals(UniqueFileName? other)
        {
            return other is not null &&
                   Postfix == other.Postfix &&
                   Extension == other.Extension &&
                   WorkingDirectory == other.WorkingDirectory;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UniqueFileName);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Postfix, Extension, WorkingDirectory);
        }

        public static bool operator ==(UniqueFileName? left, UniqueFileName? right)
        {
            return EqualityComparer<UniqueFileName>.Default.Equals(left, right);
        }

        public static bool operator !=(UniqueFileName? left, UniqueFileName? right)
        {
            return !(left == right);
        }
    }
}
