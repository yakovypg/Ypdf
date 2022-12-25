namespace FileSystemLib.Naming
{
    public class UniqueName : IUniqueName, IEquatable<UniqueName?>
    {
        private readonly Predicate<string> _nameExistingVerifier;

        public string Postfix { get; }

        public UniqueName(Predicate<string> nameExistingVerifier, string postfix = "")
        {
            Postfix = postfix;
            _nameExistingVerifier = nameExistingVerifier;
        }

        public virtual string GetNext()
        {
            string name;

            do
            {
                Guid guid = Guid.NewGuid();
                name = $"{guid}{Postfix}";
            }
            while (_nameExistingVerifier.Invoke(name));

            return name;
        }

        public bool Equals(UniqueName? other)
        {
            return other is not null &&
                   EqualityComparer<Predicate<string>>.Default.Equals(_nameExistingVerifier, other._nameExistingVerifier) &&
                   Postfix == other.Postfix;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UniqueName);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_nameExistingVerifier, Postfix);
        }

        public static bool operator ==(UniqueName? left, UniqueName? right)
        {
            return EqualityComparer<UniqueName>.Default.Equals(left, right);
        }

        public static bool operator !=(UniqueName? left, UniqueName? right)
        {
            return !(left == right);
        }
    }
}