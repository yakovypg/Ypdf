using System.Collections.Generic;

namespace Ypdf.Core.Runtime.Python;

public record PythonVirtualEnvironmentConfig(string? Name, string Path, IReadOnlyCollection<PythonPackage> Packages);
