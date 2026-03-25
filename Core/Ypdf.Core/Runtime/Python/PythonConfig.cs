namespace Ypdf.Core.Runtime.Python;

public record PythonConfig(
    string? Alias = null,
    bool RequirePython3 = true,
    PythonVirtualEnvironmentConfig? VirtualEnvironment = null);
