namespace FileSystemLib.Paths
{
    public class UniqueName : IUniqueName
    {
        private readonly Predicate<string> _nameExistingVerifier;

        public string Postfix { get; }

        public UniqueName(Predicate<string> nameExistingVerifier, string postfix = "")
        {
            Postfix = postfix;
            _nameExistingVerifier = nameExistingVerifier;
        }

        public string GetNext()
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
    }
}