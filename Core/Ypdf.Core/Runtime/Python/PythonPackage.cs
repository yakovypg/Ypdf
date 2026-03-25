namespace Ypdf.Core.Runtime.Python;

public record struct PythonPackage(string Name, string? Version = null)
{
    public readonly string FullName => !string.IsNullOrEmpty(Version)
        ? $"{Name}=={Version}"
        : Name;
}
