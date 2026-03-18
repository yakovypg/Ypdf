using System;

namespace Ypdf.Core.FileSystem.Naming;

public class UniqueName : IUniqueName
{
    private readonly Predicate<string> _nameExistingVerifier;

    public UniqueName(Predicate<string> nameExistingVerifier, string? postfix = null)
    {
        _nameExistingVerifier = nameExistingVerifier
            ?? throw new ArgumentNullException(nameof(nameExistingVerifier));

        Postfix = postfix ?? string.Empty;
    }

    public string Postfix { get; }

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
}
